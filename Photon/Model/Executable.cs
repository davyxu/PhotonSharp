
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Photon
{

    public class Executable
    {
        // 第一次pass无法搞定的node
        internal List<CompileContext> _secondPass = new List<CompileContext>();

        List<Package> _packages = new List<Package>();
       
        // 外部代理函数
        Dictionary<string, ValueDelegate> _delegateByName = new Dictionary<string, ValueDelegate>();



        // 作用域
        Scope _globalScope;

        // 源码
        SourceFile _sourcefile;

        public SourceFile Source
        {
            get { return _sourcefile; }
        }


        internal void ResolveNode()
        {
            foreach( var ctx in _secondPass )
            {
                ctx.node.Resolve(ctx.parameter);
            }
            
        }

        internal Executable( Scope s, SourceFile src )
        {            
            _globalScope = s;
            _sourcefile = src;
        }
        public static void PrintAST(Node n, string indent = "")
        {
            Debug.WriteLine(indent + n.ToString());

            foreach (var c in n.Child())
            {
                PrintAST(c, indent + "\t");
            }
        }


        public void DebugPrint( )
        {
            // 源文件
            _sourcefile.DebugPrint();

            // 语法树
            foreach (var pkg in _packages)
            {
                Debug.WriteLine("ast: {0}", pkg.Name);
                PrintAST(pkg.AST);
                Debug.WriteLine("");

                // 汇编
                pkg.Constants.DebugPrint();

            }

            // 符号
            Debug.WriteLine("symbols:");
            _globalScope.DebugPrint("");
            Debug.WriteLine("");

            // 常量表
            foreach (var pkg in _packages)
            {                
                pkg.Constants.DebugPrint();

            }

            foreach( var pkg in _packages )
            {
                // 汇编
                pkg.DebugPrint(_sourcefile);

            }

            

            Debug.WriteLine("");
        }
        internal Package AddPackage( string name )
        {
            var pkg = new Package( _packages.Count,  name, this);

            _packages.Add(pkg);

            return pkg;

        }
        internal Package GetPackage(int pkgid)
        {
            return _packages[pkgid];
        }

        internal int PackageCount
        {
            get { return _packages.Count; }
        }

        internal Procedure GetProcedure( int packageID, int cmdSetID )
        {            
            var pkg = _packages[packageID];
            if (pkg == null)
                return null;

            return pkg.GetProcedure(cmdSetID);
        }

        internal bool FindCmdSetInPackage(string name, out Procedure outP, out Package outPkg)
        {
            outP = null;
            outPkg = null;

            foreach( var pkg in _packages )
            {
                outP = pkg.FindProcedureByName(name);
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

        public void RegisterDelegate(string name, Func<VMachine, int> callback)
        {
            ValueDelegate v = null;
            if (!_delegateByName.TryGetValue(name, out v))
            {
                Debug.WriteLine("extern func not define in code: " + name);
                return;
            }

            v.Entry = callback;
        }
    }
}
