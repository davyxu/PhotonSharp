
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Photon
{

    public class Executable
    {
        List<CommandSet> _cmdset = new List<CommandSet>();
        Dictionary<string, ValueDelegate> _delegateByName = new Dictionary<string, ValueDelegate>();
        ConstantSet _constSet = new ConstantSet();

        // 调试Symbol
        Chunk _chunk;

        Scope _globalScope;

        SourceFile _sourcefile;

        public SourceFile Source
        {
            get { return _sourcefile; }
        }

        public List<CommandSet> CmdSet
        {
            get { return _cmdset; }
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

            // 汇编
            foreach( var cs in _cmdset )
            {
                cs.DebugPrint(_sourcefile);
            }

            Debug.WriteLine("");
        }

        public int AddCmdSet(CommandSet f)
        {
            _cmdset.Add(f);

            f.ID = _cmdset.Count - 1;

            return f.ID;
        }

        public CommandSet GetCmdSet(int index )
        {
            return _cmdset[index];
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
