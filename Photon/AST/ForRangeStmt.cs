
using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    // for k, v in x
    internal class ForRangeStmt : Stmt
    {        
        public Ident Key;

        public Ident Value;

        public Expr X;

        public TokenPos InPos;

        public LoopTypes TypeInfo;

        public ForRangeStmt(Ident key, Ident value, Expr x, TokenPos inpos, LoopTypes types)
        {
            Key = key;
            Value = value;
            X = x;            
            InPos = inpos;
            TypeInfo = types;

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

            yield return TypeInfo.Body;
        }

        Ident DelcareIteratorVar( )
        {
            var iter = new Ident(new Token(TypeInfo.Pos, null, "@Iterator"));
            iter.BaseScope = TypeInfo.ScopeInfo;
            iter.Symbol = new Symbol();
            iter.Symbol.Name = iter.Name;
            iter.Symbol.Decl = this;
            iter.Symbol.DefinePos = TypeInfo.Pos;
            iter.Symbol.Usage = SymbolUsage.Variable;
            TypeInfo.ScopeInfo.Insert(iter.Symbol);

            return iter;
        }
        // 手动分配1个iterator变量
        // k, v, iter = ITER( x, iter )
        // 
        internal override void Compile(CompileParameter param)
        {
            var iterVar = DelcareIteratorVar();

            var loopStart = param.CS.CurrCmdID;

            X.Compile(param);

            iterVar.Compile(param);

            var jmpCmd = param.CS.Add(new Command(Opcode.VISIT, -1 )).SetCodePos(TypeInfo.Pos);

            Key.Compile(param.SetLHS(true));

            Value.Compile(param.SetLHS(true));

            iterVar.Compile(param.SetLHS(true));

            TypeInfo.Body.Compile(param.SetLHS(false));


            param.CS.Add(new Command(Opcode.JMP, loopStart))
                .SetCodePos(TypeInfo.Pos);

            // 循环结束
            jmpCmd.DataA = param.CS.CurrCmdID;
        }

    }
}
