using SharpLexer;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace Photon
{
    public partial class Executable
    {
        // 常量表
        [PhoSerialize]
        ConstantSet _constSet = new ConstantSet();

        
        // 所有函数执行体
        [PhoSerialize]
        List<ValueFunc> _func = new List<ValueFunc>();

        
        [PhoSerialize]
        List<Package> _packages = new List<Package>();

        // 类
        List<ValueClassType> _classType = new List<ValueClassType>();

        
        Dictionary<Type, ValueClassType> _classTypeByNativeType = new Dictionary<Type, ValueClassType>();




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
            _func.Add(f);

            f.ID = _func.Count - 1;

            return f;
        }

        internal ValueClassType AddClassType(ValueClassType c)
        {
            var nativeClass = c as ValueNativeClassType;
            if ( nativeClass != null )
            {
                _classTypeByNativeType.Add(nativeClass.RawValue, c);
            }

            _classType.Add(c);

            c.ID = _classType.Count - 1;

            return c;
        }

        internal ValueClassType GetClassType(int cid)
        {
            return _classType[cid];
        }

        internal ValueClassType GetClassTypeByName(ObjectName name)
        {
            foreach( var c in _classType )
            {
                if (c.Name.Equals( name))
                    return c;
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

        internal ValueFunc GetFunc( int funcid )
        {
            return _func[funcid];            
        }

        internal ValueFunc GetFuncByName(ObjectName name)
        {           
            foreach( var func in _func )
            {
                if ( func.Name.Equals( name ) )
                {
                    return func;
                }

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
            foreach (var p in _func)
            {
                Logger.DebugLine(p.DebugString());
                p.DebugPrint(this);
            }
        }
    }
}
