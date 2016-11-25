using SharpLexer;
using System.Collections.Generic;
using System.Diagnostics;

namespace Photon
{
    public class File
    {
        public SourceFile Source;
    }


    public class Package
    {
        string _name;

        int _id;

        // 调试Symbol
        Chunk _chunk;

        List<File> _file = new List<File>();

        // 所有函数执行体
        List<Procedure> _proc = new List<Procedure>();

        // 常量表
        ConstantSet _constSet = new ConstantSet();

        Executable _exe;

        Scope _top;

        // 第一次pass无法搞定的node
        internal List<CompileContext> _secondPass = new List<CompileContext>();

        internal void ResolveNode()
        {
            foreach (var ctx in _secondPass)
            {
                ctx.node.Resolve(ctx.parameter);
            }

        }

        internal Chunk AST
        {
            get { return _chunk; }
            set { _chunk = value; }
        }

        internal Executable Exe
        {
            get { return _exe; }
        }

        internal ConstantSet Constants
        {
            get { return _constSet; }
        }

        internal int ID
        {
            get { return _id; }
        }


        public string Name
        {
            get { return _name; }
        }

        internal Scope TopScope
        {
            get { return _top; }
        }

        public List<Procedure> ProcedureList
        {
            get { return _proc; }
        }

        public List<File> FileList
        {
            get { return _file; }
        }

        internal Package( int id, string name, Executable exe, Scope s )
        {
            _exe = exe;
            _name = name;
            _id = id;
            _top = s;
        }

        public override string ToString()
        {
            return _name;
        }

        internal void AddSource( SourceFile source )
        {
            var f = new File();
            f.Source = source;
            _file.Add( f);
        }

        internal Procedure AddProcedure(Procedure f)
        {
            f.Pkg = this;

            _proc.Add(f);

            f.ID = _proc.Count - 1;

            return f;
        }

        public Procedure GetProcedure(int index)
        {
            return _proc[index];
        }

        internal Procedure GetProcedureByName(string name)
        {
            foreach (var cs in _proc)
            {
                if (cs.Name == name)
                    return cs;
            }

            return null;
        }
        static void PrintAST(Node n, string indent = "")
        {
            Debug.WriteLine(indent + n.ToString());

            foreach (var c in n.Child())
            {
                PrintAST(c, indent + "\t");
            }
        }

        internal string QuerySourceLine(TokenPos pos)
        {
            foreach( var f in _file)
            {
                if ( f.Source.Name == pos.SourceName )
                {
                    return f.Source.GetLine(pos.Line);
                }
            }

            return string.Empty;
        }




        internal void DebugPrint(  )
        {
            if (AST != null )
            {
                // 语法树
                Debug.WriteLine("ast:");
                PrintAST(AST);
                Debug.WriteLine("");
            }
            


            if (_top != null )
            {
                // 符号
                Debug.WriteLine("symbols:");
                _top.DebugPrint("");
                Debug.WriteLine("");
            }


            if (Constants.Count >0 )
            {
                // 常量
                Constants.DebugPrint();
            }
            
            
            // 汇编
            foreach (var p in _proc)
            {
                var cs = p as CommandSet;
                if (cs != null)
                {
                    Debug.WriteLine(string.Format("cmdset: [{0}] id: {1} regs: {2}", cs,cs.ID, cs.RegCount));
                    cs.DebugPrint();
                }
                var del = p as Delegate;
                if ( del != null )
                {
                    Debug.WriteLine(string.Format("delegate: [{0}] id: {1}", del.Name, del.ID));                    
                }
            }
        }
    }
}
