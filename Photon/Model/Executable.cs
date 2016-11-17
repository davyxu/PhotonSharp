
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Photon
{

    public class Executable
    {
        // 第一次pass无法搞定的node
        List<Node> _unsolvedNode = new List<Node>();

        List<Package> _packages = new List<Package>();
        
        // 常量表
        ConstantSet _constSet = new ConstantSet();

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


        public ConstantSet Constants
        {
            get { return _constSet; }
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

            // 常量表
            _constSet.DebugPrint( );

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

        internal CommandSet GetCmdSet( int packageID, int cmdSetID )
        {            
            var pkg = _packages[packageID];
            if (pkg == null)
                return null;

            return pkg.GetCmdSet(cmdSetID);
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
