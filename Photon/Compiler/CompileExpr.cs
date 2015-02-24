using System;
using Photon.AST;
using Photon.OpCode;
using Photon.Scanner;


namespace Photon.Compiler
{
    public partial class Compiler
    {

        bool CompileExpr(CommandSet cm, Node n, bool lhs)
        {

            if (n is BinaryExpr)
            {
                var v = n as BinaryExpr;
                CompileNode(cm, v.X, lhs);
                CompileNode(cm, v.Y, lhs);

                switch (v.Op)
                {
                    case TokenType.Add:
                        cm.Add(new Command(Opcode.Add));
                        break;
                    case TokenType.Mul:
                        cm.Add(new Command(Opcode.Mul));
                        break;
                    case TokenType.Sub:
                        cm.Add(new Command(Opcode.Sub));
                        break;
                    case TokenType.Div:
                        cm.Add(new Command(Opcode.Div));
                        break;
                    case TokenType.GreatThan:
                        cm.Add(new Command(Opcode.GT));
                        break;
                    case TokenType.GreatEqual:
                        cm.Add(new Command(Opcode.GE));
                        break;
                    case TokenType.LessThan:
                        cm.Add(new Command(Opcode.LT));
                        break;
                    case TokenType.LessEqual:
                        cm.Add(new Command(Opcode.LE));
                        break;
                    case TokenType.Equal:
                        cm.Add(new Command(Opcode.EQ));
                        break;
                    case TokenType.NotEqual:
                        cm.Add(new Command(Opcode.NE));
                        break;
                }

                return true;
            }

            if (n is BasicLit)
            {
                var v = n as BasicLit;

                var c = Lit2Const(v);
                var ci = _exe.Constants.Add(c);

                cm.Add(new Command(Opcode.LoadC, ci)).Comment = c.GetDesc();

                return true;
            }

            if (n is Ident)
            {
                var v = n as Ident;

                var scopeIndex = v.ScopeInfo.Parent.Index;

                if (lhs)
                {
                    cm.Add(new Command(Opcode.SetR, v.ScopeInfo.Slot, scopeIndex)).Comment = v.Name;
                }
                else
                {
                    cm.Add(new Command(Opcode.LoadR, v.ScopeInfo.Slot, scopeIndex)).Comment = v.Name;
                }

                return true;
            }

            if (n is CallExpr)
            {
                var v = n as CallExpr;
                CompileNode(cm, v.Func, false);

                foreach (var arg in v.Args)
                {
                    CompileNode(cm, arg, false);
                }

                cm.Add(new Command(Opcode.Call, v.Args.Count));
                return true;
            }

            return false;
        }
    }
}
