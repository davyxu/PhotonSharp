using System.Collections.Generic;

namespace Photon
{

    public class Package
    {
        string _name;

        int _id;

        // 调试Symbol
        Chunk _chunk;

        // 父级
        Executable _exe;

        // 包的作用域
        Scope _top;

        // 第一次pass无法搞定的node
        List<CompileContext> _secondPass = new List<CompileContext>();


        // 第二次pass
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

        internal bool ContainSecondPassNode( Node n )
        {
            foreach( var c in _secondPass )
            {
                if (c.node == n)
                    return true;
            }

            return false;
        }

        internal void AddSecondPass( Node n, CompileParameter cp )
        {
            CompileContext ctx;
            ctx.node = n;
            ctx.parameter = cp;

            _secondPass.Add(ctx);
        }



        static void PrintAST(Node n, string indent = "")
        {
            Logger.DebugLine(indent + n.ToString());

            foreach (var c in n.Child())
            {
                PrintAST(c, indent + "\t");
            }
        }

        internal void DebugPrint(  )
        {
            if (AST != null )
            {
                // 语法树
                Logger.DebugLine("ast:");
                PrintAST(AST);
                Logger.DebugLine("");
            }
            
            if (_top != null )
            {
                // 符号
                Logger.DebugLine("symbols:");
                _top.DebugPrint("");
                Logger.DebugLine("");
            }

        }
    }
}
