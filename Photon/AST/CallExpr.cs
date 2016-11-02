using System.Collections.Generic;
using Photon.Scanner;
using Photon.OpCode;

namespace Photon.AST
{

    public class CallExpr : Expr
    {
        public Expr Func;
        public List<Expr> Args;
        public Scope S;

        public CallExpr(Expr f, List<Expr> args, Scope s )
        {
            Func = f;
            Args = args;
            S = s;
        }

        public override IEnumerable<Node> Child()
        {
            yield return Func;

            foreach( var e in Args)
            {
                yield return e;
            }
        }

        public override string ToString()
        {
            return "CallExpr";
        }

        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {
            Func.Compile(exe, cm, false);

            foreach (var arg in Args)
            {
                arg.Compile(exe, cm, false);                
            }

            cm.Add(new Command(Opcode.Call, Args.Count));
        }
    }
}
