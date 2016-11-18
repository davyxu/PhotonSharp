
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Photon
{

    public class Executable
    {
        // 第一次pass无法搞定的node
        internal List<Node> _secondPassNode = new List<Node>();

        List<Package> _packages = new List<Package>();
       
        // 外部代理函数
        Dictionary<string, ValueDelegate> _delegateByName = new Dictionary<string, ValueDelegate>();

        // 调试Symbol
        Chunk _chunk;

        // 作用域
        Scope _globalScope;

        // 源码
        SourceFile _sourcefile;

        public SourceFile Source
        {
            get { return _sourcefile; }
        }


        internal void ResolveNode( )
        {
            foreach( var n in _secondPassNode )
            {
                n.Resolve();
            }
            
        }

        internal Executable( Chunk ast,Scope s, SourceFile src )
        {
            _chunk = ast;
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
            Debug.WriteLine("ast:");
            PrintAST(_chunk);
            Debug.WriteLine("");

            // 符号
            Debug.WriteLine("symbols:");
            _globalScope.DebugPrint("");
            Debug.WriteLine("");

            foreach (var pkg in _packages)
            {
                // 汇编
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
