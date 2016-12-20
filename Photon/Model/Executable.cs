using MarkSerializer;
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

        // 类        
        Dictionary<ObjectName, ValueClassType> _classByName = new Dictionary<ObjectName, ValueClassType>();

        // native类索引
        Dictionary<Type, ValueClassType> _classTypeByNativeType = new Dictionary<Type, ValueClassType>();

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
            foreach (var kv in _classByName)
            {
                int linkid = arr.Count;
                kv.Value.ID = linkid;
                arr.Add(kv.Value);
            }

            return arr;
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
                if ( pkg.InitEntry != null )
                {
                    pkg.InitEntry.LinkCommandEntry(this);
                }
            }
        }


        public void Serialize(BinarySerializer ser)
        {
            ser.Serialize<ConstantSet>(_constSet);
            ser.Serialize<Dictionary<ObjectName, ValueFunc>>(_phoFuncByName);
            ser.Serialize<List<Package>>(_packages);
        }

        public void Deserialize(BinaryDeserializer ser)
        {
            _constSet = ser.Deserialize<ConstantSet>();
            _phoFuncByName = ser.Deserialize<Dictionary<ObjectName, ValueFunc>>();
            _packages = ser.Deserialize<List<Package>>();
        }

        public void RegisterBuiltinPackage()
        {
            Array.Register(this);
            Map.Register(this);
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
            if ( nativeClass != null )
            {
                _classTypeByNativeType.Add(nativeClass.RawValue, c);
            }

            
            _classByName.Add(c.Name, c);            

            return c;
        }



        internal ValueClassType GetClassTypeByName(ObjectName name)
        {
            ValueClassType clsType;
            if ( _classByName.TryGetValue( name, out clsType) )
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

            _packages.Add(pkg);

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
                pkg = AddPackage(new Package(pkgNameRegTo) );
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

        static bool typeReady;

        static void PrepareBinaryTypes()
        {
            if (typeReady)
            {
                return;
            }

            BinaryTypeSet.Register(new TokenPosSerializer());

            typeReady = true;
        }

        public static void Serialize(Executable exe, Stream stream )
        {
            PrepareBinaryTypes();

            var bs = new BinarySerializer(stream);
            bs.Serialize<Executable>(exe);
        }

        public static Executable Deserialize(Stream stream)
        {
            PrepareBinaryTypes();

            var bs = new BinaryDeserializer(stream);
            return bs.Deserialize<Executable>();
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
