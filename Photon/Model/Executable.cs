using SharpLexer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

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
        List<ValueFunc> _proc = new List<ValueFunc>();

        // 源码
        List<File> _file = new List<File>();

        // 类
        List<ValueClassType> _class = new List<ValueClassType>();

        public List<File> FileList
        {
            get { return _file; }
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
            _proc.Add(f);

            f.ID = _proc.Count - 1;

            return f;
        }

        internal ValueClassType AddClassType(ValueClassType c)
        {
            _class.Add(c);

            c.ID = _class.Count - 1;

            return c;
        }

        internal ValueClassType GetClassType(int cid)
        {
            return _class[cid];
        }

        internal ValueClassType GetClassTypeByName(ObjectName name)
        {
            foreach( var c in _class )
            {
                if (c.Name.Equals( name))
                    return c;
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

        internal ValueFunc GetFunc( int procid )
        {
            return _proc[procid];            
        }

        internal ValueFunc GetFuncByName(ObjectName name)
        {           
            foreach( var pro in _proc )
            {
                if ( pro.Name.Equals( name ) )
                {
                    return pro;
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

        public void RegisterNativeClass( Type classType, string pkgNameRegTo )
        {
            var pkg = GetPackageByName(pkgNameRegTo);
            if (pkg == null)
            {
                Scope pkgScope = new Scope(null, ScopeType.Package, TokenPos.Init);

                pkg = AddPackage(pkgNameRegTo, pkgScope);
            }

            var lanClass = new ValueNativeClassType(this, classType);

            AddClassType(lanClass);

            foreach (var m in classType.GetMembers())
            {
                var attr = m.GetCustomAttribute<DelegateAttribute>();
                if (attr == null)
                    continue;

                var mm = classType.GetMethod(m.Name);

                var dele = mm.CreateDelegate(typeof(NativeDelegate)) as NativeDelegate;

                // TODO 添加自定义属性可以自定义导入后使用的名称

                if ( pkg.TopScope.FindSymbol(m.Name) != null )
                {
                    throw new RuntimeException("name duplicate: " + m.Name);
                }

                // 让导入的代码能认识这个函数
                Symbol data = new Symbol();
                data.Name = m.Name;
                data.Decl = null;
                data.Usage = SymbolUsage.Func;
                pkg.TopScope.Insert(data);

                AddFunc(new ValueNativeFunc(new ObjectName(pkg.Name, m.Name), dele));
            }
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


            // 汇编
            foreach (var p in _proc)
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
