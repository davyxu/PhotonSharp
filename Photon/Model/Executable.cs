using SharpLexer;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;

namespace Photon
{

    public class File
    {
        public SourceFile Source;
    }


    public class Executable
    {

        List<Package> _packages = new List<Package>();

        // 所有函数执行体
        List<ValueFunc> _func = new List<ValueFunc>();

        // 源码
        List<File> _file = new List<File>();

        // 类
        List<ValueClassType> _classType = new List<ValueClassType>();
        Dictionary<Type, ValueClassType> _classTypeByNativeType = new Dictionary<Type, ValueClassType>();

        // 常量表
        ConstantSet _constSet = new ConstantSet();

        public Executable()
        {
            Array.Register(this);
            Map.Register(this);
        }

        public List<File> FileList
        {
            get { return _file; }
        }

        internal ConstantSet Constants
        {
            get { return _constSet; }
        }

        internal string QuerySourceLine(TokenPos pos)
        {
            foreach (var f in _file)
            {
                if (f.Source.Name == pos.SourceName)
                {
                    return f.Source.GetLine(pos.Line);
                }
            }

            return string.Empty;
        }

        internal void AddSource(SourceFile source)
        {
            var f = new File();
            f.Source = source;
            _file.Add(f);
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
                _classTypeByNativeType.Add(nativeClass.Raw, c);
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

        internal Package AddPackage( string name, Scope top )
        {
            if ( GetPackageByName(name) != null )
            {
                throw new RuntimeException("duplicate register package, name: " + name);
            }

            var pkg = new Package( _packages.Count,  name, this, top);

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

            foreach (var f in FileList)
            {
                if (f.Source.Name == filename)
                {
                    return f.Source;
                }
            }

            return null;
        }

        public void RegisterNativeClass(Assembly ass, string className, string pkgNameRegTo)
        {
            RegisterNativeClass(ass.GetType(className), pkgNameRegTo);
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
                Scope pkgScope = new Scope(null, ScopeType.Package, TokenPos.Init);

                pkg = AddPackage(pkgNameRegTo, pkgScope);
            }

            Type instClass;
            var classAttr = classType.GetCustomAttribute<NativeWrapperClassAttribute>();
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
                Debug.WriteLine(string.Format("============= {0} id: {1} =============", pkg.Name, pkg.ID));
                pkg.DebugPrint();
            }

            Debug.WriteLine("");

            if (Constants.Count > 0)
            {
                // 常量
                Constants.DebugPrint();
            }


            // 汇编
            foreach (var p in _func)
            {
                var cs = p as ValuePhoFunc;
                if (cs != null)
                {
                    Debug.WriteLine(string.Format("{0} id: {1} regs: {2}", cs, cs.ID, cs.RegCount));
                    cs.DebugPrint(this);
                }
                var del = p as ValueNativeFunc;
                if (del != null)
                {
                    Debug.WriteLine(string.Format("{0} id: {1}", del.Name, del.ID));
                }
            }
        }
    }
}
