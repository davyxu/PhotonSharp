﻿using MarkSerializer;
using SharpLexer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Photon
{
    public partial class Executable : IMarkSerializable
    {
        // 常量表        
        ConstantSet _constSet = new ConstantSet();

        // 所有包函数, 类成员函数, 闭包
        Dictionary<ObjectName, ValueFunc> _phoFuncByName = new Dictionary<ObjectName, ValueFunc>();

        // 外部函数执行体                
        Dictionary<ObjectName, ValueFunc> _nativeFuncByName = new Dictionary<ObjectName, ValueFunc>();
        
        // 所有包
        List<Package> _packages = new List<Package>();
        List<Package> _nativePackages = new List<Package>();

        // 类        
        Dictionary<ObjectName, ValueClassType> _classByName = new Dictionary<ObjectName, ValueClassType>();
        Dictionary<ObjectName, ValueClassType> _nativeClassByName = new Dictionary<ObjectName, ValueClassType>();

        // native类索引
        Dictionary<Type, ValueClassType> _classTypeByNativeType = new Dictionary<Type, ValueClassType>();

        // 类持久化id生成器
        internal int persistantIDGen = 0;

        internal int GenPersistantID()
        {
            persistantIDGen++;
            return persistantIDGen;
        }

        bool needinit = true;
        public void Serialize(BinarySerializer ser)
        {
            if (needinit)
            {
                // 外部类型注册
                TypeSerializerSet.Instance.Register(new TokenPosSerializer());

                needinit = false;
            }

            ser.Serialize(ref _constSet);
            ser.Serialize(ref _phoFuncByName);
            ser.Serialize(ref _classByName);
            ser.Serialize(ref _packages);

            // 序列化完成时, 还有一些延迟处理的, 例如id转指针
            if (ser.IsLoading)
            {
                foreach (var kv in _classByName)
                {
                    kv.Value.OnSerializeDone(this);
                }
            }
        }

        public void RegisterBuiltinPackage()
        {
            Array.Register(this);
            Map.Register(this);
        }

        // 生成函数列表, 建立id
        internal List<Value> GenEntryList()
        {
            var arr = new List<Value>();

            // 添加所有的函数
            foreach (var kv in _phoFuncByName)
            {
                int linkid = arr.Count;
                kv.Value.ID = linkid;
                arr.Add(kv.Value);
            }

            // 添加所有的函数
            foreach (var kv in _nativeFuncByName)
            {
                int linkid = arr.Count;
                kv.Value.ID = linkid;
                arr.Add(kv.Value);
            }

            // 添加所有的类
            foreach (var kv in SortClassInheritLevel())
            {
                int linkid = arr.Count;                
                kv.ID = linkid;
                arr.Add(kv);
            }

            // 添加所有的类
            foreach (var kv in _nativeClassByName)
            {
                int linkid = arr.Count;
                kv.Value.ID = linkid;
                arr.Add(kv.Value);
            }


            return arr;
        }

        // 按照依赖层次排序, 从被依赖最多的开始
        List<ValueClassType> SortClassInheritLevel()
        {
            var classArr = new List<ValueClassType>();

            foreach (var kv in _classByName)
            {
                classArr.Add(kv.Value);
            }

            classArr.Sort(delegate(ValueClassType a, ValueClassType b)
            {
                return a.GetInheritLevel().CompareTo(b.GetInheritLevel());
            });

            return classArr;
        }


        // 将加载函数时对应的指令中的函数入口与实际的函数连接, 放入数组中供VM使用
        internal void LinkEntryPoint()
        {
            var phoFuncArr = new List<ValuePhoFunc>();

            // 所有脚本函数处理指令中函数入口
            foreach (var kv in _phoFuncByName)
            {
                var phoFunc = kv.Value as ValuePhoFunc;
                phoFunc.LinkCommandEntry(this);
            }

            // 包入口函数处理指令中函数入口
            foreach (var pkg in _packages)
            {
                if (pkg.InitEntry != null)
                {
                    pkg.InitEntry.LinkCommandEntry(this);
                }
            }
        }



        internal ConstantSet Constants
        {
            get { return _constSet; }
        }

        internal string QuerySourceLine(TokenPos pos)
        {
            foreach (var p in _packages)
            {
                foreach (var f in p.FileList)
                {
                    if (f.Source.Name == pos.SourceName)
                    {
                        return f.Source.GetLine(pos.Line);
                    }
                }
            }


            return string.Empty;
        }

        internal ValueFunc AddFunc(ValueFunc f)
        {
            if ( f is ValuePhoFunc )
            {
                _phoFuncByName.Add(f.Name, f);
            }
            else if( f is ValueNativeFunc )
            {
                _nativeFuncByName.Add(f.Name, f);
            }
            else
            {
                throw new Exception("unknown func type " + f.TypeName);
            }
            

            return f;
        }

        internal ValueClassType AddClassType(ValueClassType c)
        {            
            var nativeClass = c as ValueNativeClassType;
            if (nativeClass != null)
            {
                _classTypeByNativeType.Add(nativeClass.RawValue, c);
                _nativeClassByName.Add(c.Name, c);
            }
            else 
            {
                _classByName.Add(c.Name, c);            
            }

            return c;
        }

        internal ValuePhoClassType FindClassByPersistantID(int id)
        {
            foreach (var kv in _classByName)
            {
                if (kv.Value.ID == id)
                    return kv.Value as ValuePhoClassType;
            }

            return null;
        }



        internal ValueClassType GetClassTypeByName(ObjectName name)
        {
            ValueClassType clsType;
            if ( _classByName.TryGetValue( name, out clsType) )
            {
                return clsType;
            }

            if (_nativeClassByName.TryGetValue(name, out clsType))
            {
                return clsType;
            }

            return null;
        }

        // 通过native类的原始类型获取注册好的类型
        internal ValueClassType GetClassTypeByNativeType(Type t )
        {
            ValueClassType ct;
            if (_classTypeByNativeType.TryGetValue( t, out ct ))
            {
                return ct;
            }

            return null;
        }

        internal Package AddPackage( Package pkg )
        {
            if ( GetPackageByName(pkg.Name) != null )
            {
                throw new RuntimeException("duplicate register package, name: " + pkg.Name);
            }            

            pkg.ID = _packages.Count;
            pkg.Exe = this;

            if (pkg.IsNative)
            {
                _nativePackages.Add(pkg);
            }
            else
            {
                _packages.Add(pkg);
            }

            
            return pkg;
        }

        internal Package GetPackageByName(string name)
        {
            foreach (var pkg in _packages)
            {
                if (pkg.Name == name)
                {
                    return pkg;
                }
            }

            foreach (var pkg in _nativePackages)
            {
                if (pkg.Name == name)
                {
                    return pkg;
                }
            }


            return null;
        }

        internal ValueFunc GetFuncByName(ObjectName name)
        {
            ValueFunc f;
            if (_phoFuncByName.TryGetValue(name, out f ))
            {
                return f;
            }

            if (_nativeFuncByName.TryGetValue(name, out f))
            {
                return f;
            }

            return null;
        }

        public Package GetPackage(int pkgid)
        {
            return _packages[pkgid];
        }

        public int PackageCount
        {
            get { return _packages.Count; }
        }

        public SourceFile GetSourceFile(string filename)
        {
            foreach(var p in _packages )
            {
                foreach (var f in p.FileList)
                {
                    if (f.Source.Name == filename)
                    {
                        return f.Source;
                    }
                }
            }

            return null;
        }

        public void RegisterNativeClass(Assembly ass, string className, string pkgNameRegTo)
        {
            RegisterNativeClass(ass.GetType(className), pkgNameRegTo);
        }

        internal static T GetCustomAttribute<T>(Type type) where T : class
        {
            object[] objs = type.GetCustomAttributes(typeof(T), false);
            if (objs.Length > 0)
                return (T)objs[0];

            return null;
        }

        public void RegisterNativeClass( Type classType, string pkgNameRegTo )
        {
            if (!classType.IsClass)
            {
                throw new RuntimeException("Require class type");
            }

            var pkg = GetPackageByName(pkgNameRegTo);
            if (pkg == null)
            {                
                pkg = AddPackage(new Package(pkgNameRegTo, true) );
            }

            Type instClass;
            var classAttr = GetCustomAttribute<NativeWrapperClassAttribute>(classType);
            // 自动生成代码绑定
            if (classAttr != null)
            {
                instClass = classAttr.BindingClass;
            }
            else
            {
                instClass = classType;
            }

            var lanClass = new ValueNativeClassType(pkg, instClass, new ObjectName(pkg.Name, instClass.Name));
            lanClass.BuildMember(instClass.Name, classType);

            // 还需要扫描实例类本体有手动添加的
            if ( instClass != classType )
            {
                lanClass.BuildMember(instClass.Name, instClass);
            }

            AddClassType(lanClass);
        }

        public static void Serialize(Stream stream, ref Executable exe, bool loading )
        {            
            var bs = new BinarySerializer(stream, loading);
            bs.Serialize<Executable>(ref exe);
        }

        public void DebugPrint()
        {
            // 语法树
            foreach (var pkg in _packages)
            {
                Logger.DebugLine(string.Format("============= {0} id: {1} =============", pkg.Name, pkg.ID));

                pkg.PrintAST();

                pkg.PrintSymbols();

                pkg.PrintInitEntry();
            }

            Logger.DebugLine("");

            if (Constants.Count > 0)
            {
                Logger.DebugLine("Constant:");

                // 常量
                Constants.DebugPrint();
            }


            // 汇编
            foreach (var kv in _phoFuncByName)
            {
                Logger.DebugLine(kv.Value.DebugString());
                kv.Value.DebugPrint(this);
            }
        }
    }
}
