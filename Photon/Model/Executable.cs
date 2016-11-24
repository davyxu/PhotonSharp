
using SharpLexer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Photon
{

    public class Executable
    {

        List<Package> _packages = new List<Package>();
       
        // 外部代理函数
        Dictionary<string, ValueDelegate> _delegateByName = new Dictionary<string, ValueDelegate>();


        internal Package AddPackage( string name, Scope top )
        {
            if ( GetPackageByName(name) != null )
            {
                throw new RuntimeExcetion("duplicate register package, name: " + name);
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

        internal Procedure GetProcedure( int packageID, int cmdSetID )
        {            
            var pkg = _packages[packageID];
            if (pkg == null)
                return null;

            return pkg.GetProcedure(cmdSetID);
        }

        internal bool GetProcedureDetail(string name, out Procedure outP, out Package outPkg)
        {
            outP = null;
            outPkg = null;

            foreach( var pkg in _packages )
            {
                outP = pkg.GetProcedureByName(name);
                if (outP != null)
                {
                    outPkg = pkg;
                    return true;
                }
            }
            

            return false;
        }

        internal void AddDelegate( string name, ValueDelegate d )
        {
            _delegateByName.Add(name, d);
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
            foreach (var pkg in _packages)
            {
                foreach (var f in pkg.FileList)
                {
                    if (f.Source.Name == filename)
                    {
                        return f.Source;
                    }
                }
            }

            return null;
        }


        public void RegisterPackage(Type classType)
        {
            // 这个scope是给通过代码导入定义时使用                    
            var pkg = GetPackageByName(classType.Name);
            if ( pkg == null )
            {
                throw new RuntimeExcetion("class package not define in code");
            }

            foreach( var m in classType.GetMembers() )
            {
                var attr = m.GetCustomAttribute<DelegateAttribute>();
                if (attr == null)
                    continue;
                
                var mm = classType.GetMethod(m.Name);
                var dele = mm.CreateDelegate(typeof(DelegateEntry)) as DelegateEntry;

                var proc = pkg.GetProcedureByName(m.Name);
                if ( proc == null )
                {
                    throw new RuntimeExcetion("package function not define in code");
                }

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
        }
    }
}
