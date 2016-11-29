
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
        List<Procedure> _proc = new List<Procedure>();

        // 源码
        List<File> _file = new List<File>();


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


        internal Procedure AddProcedure(Procedure f)
        {            
            _proc.Add(f);

            f.ID = _proc.Count - 1;

            return f;
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

        internal Procedure GetProcedure( int procid )
        {
            return _proc[procid];            
        }

        internal Procedure GetProcedureByName( ProcedureName name )
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


        public void RegisterPackage(Type classType)
        {
            // 这个scope是给通过代码导入定义时使用                    
            var pkg = GetPackageByName(classType.Name);
            if ( pkg != null )
            {
                throw new RuntimeException("class package already define in code");
            }

            Scope pkgScope = new Scope(null, ScopeType.Package, TokenPos.Init );

            pkg = AddPackage( classType.Name, pkgScope );

            foreach( var m in classType.GetMembers() )
            {
                var attr = m.GetCustomAttribute<DelegateAttribute>();
                if (attr == null)
                    continue;
                
                var mm = classType.GetMethod(m.Name);
                var dele = mm.CreateDelegate(typeof(DelegateEntry)) as DelegateEntry;
                                

                // 让导入的代码能认识这个函数
                Symbol data = new Symbol();
                data.Name = m.Name;
                data.Decl = null;                
                data.Usage = SymbolUsage.Func;
                pkgScope.Insert(data);

                var proc = pkg.Exe.AddProcedure(new Delegate(new ProcedureName(pkg.Name, m.Name)));
              
                var del = proc as Delegate;
                del.Entry = dele;
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
                var cs = p as CommandSet;
                if (cs != null)
                {
                    Debug.WriteLine(string.Format("cmdset: [{0}] id: {1} regs: {2}", cs, cs.ID, cs.RegCount));
                    cs.DebugPrint(this);
                }
                var del = p as Delegate;
                if (del != null)
                {
                    Debug.WriteLine(string.Format("delegate: [{0}] id: {1}", del.Name, del.ID));
                }
            }
        }
    }
}
