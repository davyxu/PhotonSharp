using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.AST;
using Photon.OpCode;
using Photon.Scanner;

namespace Photon.Compiler
{
    public class Compiler
    {
        Executable _exe = new Executable();

        CommandSet _globalSet;
        CommandSet _currSet;

        public Compiler()
        {
            _currSet = _globalSet = _exe.Add( new CommandSet("global") );
        }


        void Error(string str)
        {
            throw new Exception(str);
        }

        public Executable Walk( Chunk c )
        {
            ResolveNode(_currSet, c.Block, false);

            return _exe;
        }

        void ResolveNode(CommandSet cm, Node n, bool lhs)
        {
            if ( n is BinaryExpr)
            {
                var v = n as BinaryExpr;
                ResolveNode(cm, v.X, lhs);
                ResolveNode(cm, v.Y, lhs);

                switch( v.Op )
                {
                    case TokenType.Add:
                        cm.Add(new Command(Opcode.Add));
                        break;
                }
                
                return;
            }

            if ( n is BasicLit )
            {
                var v = n as BasicLit;

                var c = Lit2Const(v);
                var ci = _exe.Constants.Add(c);

                cm.Add(new Command(Opcode.LoadC, ci)).Comment = c.ToString();
                return;
            }

            if ( n is Ident )
            {
                var v = n as Ident;

                if ( lhs )
                {
                    cm.Add(new Command(Opcode.SetR, v.ScopeInfo.Slot)).Comment = v.Name;
                }
                else
                {
                    cm.Add(new Command(Opcode.LoadR, v.ScopeInfo.Slot)).Comment = v.Name;
                }

                return;
            }

            if ( n is VarDeclare )
            {
                var v = n as VarDeclare;

                ResolveNode(cm, v.Values[0], false);
                ResolveNode(cm, v.Names[0], true);
                return;
            }


            if (n is ReturnStmt)
            {
                var v = n as ReturnStmt;

                ResolveNode(cm, v.Results[0], false);
                cm.Add(new Command(Opcode.Ret));
                return;
            }

            if ( n is CallExpr )
            {
                var v = n as CallExpr;
                ResolveNode(cm, v.Func, false);

                foreach( var arg in v.Args )
                {
                    ResolveNode(cm, arg, false);
                }


                cm.Add(new Command(Opcode.Call));
                return;
            }

            if (n is BlockStmt)
            {
                var v = n as BlockStmt;


                foreach( var b in v.Stmts )
                {
                    ResolveNode(cm, b, false);
                }


                return;
            }

            if (n is FuncDeclare)
            {
                var v = n as FuncDeclare;

                var newset = _exe.Add(new CommandSet(v.Name.Name));

                var c = new Constant(newset);
                var ci = _exe.Constants.Add(c);

                cm.Add(new Command(Opcode.LoadC, ci)).Comment = c.ToString();
                cm.Add(new Command(Opcode.SetR, v.Name.ScopeInfo.Slot)).Comment = v.Name.Name;

                ResolveNode(newset, v.Body, false);

                return;
            }

            Error("unsolved ast node: " + n.ToString());
            
        }


        static Constant Lit2Const( BasicLit lite )
        {
            Constant c = null;

            switch( lite.Type )
            {
                case TokenType.Number:
                    {
                        float v;
                        if ( !float.TryParse( lite.Value, out v) )
                            return null;

                        c = new Constant(v);
                    }
                    break;
            }


            return c;
        }
        
    }
}
