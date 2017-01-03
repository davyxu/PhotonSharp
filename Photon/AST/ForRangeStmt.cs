
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    // for k, v in x
    internal class ForRangeStmt : LoopStmt
    {        
        public Ident Key;

        public Ident Value;

        public Expr X;

        public TokenPos InPos;

        public ForRangeStmt(Ident key, Ident value, Expr x, TokenPos inpos, TokenPos defpos, Scope s, BlockStmt body)
        {
            Key = key;
            Value = value;
            X = x;            
            InPos = inpos;
            Pos = defpos;
            ScopeInfo = s;
            Body = body;

            BuildRelation();
        }


        public override string ToString()
        {
            return "ForRangeStmt";
        }

        public override IEnumerable<Node> Child()
        {
            if (Key != null)
            {
                yield return Key;
            }
            
            if (Value != null)
            {
                yield return Value;
            }


            yield return X;

            yield return Body;
        }

        Ident DelcareIteratorVar( )
        {
            var iter = new Ident(new Token(Pos, null, "@Iterator"));
            iter.BaseScope = ScopeInfo;
            iter.Symbol = new Symbol();
            iter.Symbol.Name = iter.Name;
            iter.Symbol.Decl = this;
            iter.Symbol.DefinePos = Pos;
            iter.Symbol.Usage = SymbolUsage.Variable;
            ScopeInfo.Insert(iter.Symbol);

            return iter;
        }
        // 手动分配1个iterator变量
        // k, v, iter = ITER( x, iter )
        // 
        internal override void Compile(CompileParameter param)
        {
            var iterVar = DelcareIteratorVar();            

            param.CS.Add(new Command(Opcode.INITR, iterVar.Symbol.RegIndex))
                .SetCodePos(Pos)
                .SetComment("init iterator");


            LoopBeginCmdID = param.CS.CurrCmdID;

            X.Compile(param);

            iterVar.Compile(param);

            var jmpCmd = param.CS.Add(new Command(Opcode.VISIT, -1 ))
                .SetCodePos(Pos)
                .SetComment("for kv");

            Key.Compile(param.SetLHS(true));

            Value.Compile(param.SetLHS(true));

            iterVar.Compile(param.SetLHS(true));

            Body.Compile(param.SetLHS(false));


            param.CS.Add(new Command(Opcode.JMP, LoopBeginCmdID))
                .SetCodePos(Pos)
                .SetComment("for kv loop");

            // 循环结束
            LoopEndCmdID = param.CS.CurrCmdID;
            jmpCmd.DataA = LoopEndCmdID;
        }

    }
}
