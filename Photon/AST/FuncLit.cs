
using System.Collections.Generic;

namespace Photon
{
    // 匿名函数
    internal class FuncLit : Expr
    {
        public FuncType TypeInfo;

        public BlockStmt Body;

        public List<Ident> RefUpvalues = new List<Ident>();

        public FuncLit(BlockStmt body, FuncType ft )
        {
            TypeInfo = ft;
            Body = body;

            BuildRelation();
        }


        public override string ToString()
        {
            return string.Format("FuncLit {0}", TypeInfo.ToString());
        }

        public override IEnumerable<Node> Child()
        {
            return Body.Child();
        }


        internal override void Compile(CompileParameter param)
        {
            var newset = new CommandSet("closure", TypeInfo.FuncPos, TypeInfo.ScopeInfo.CalcUsedReg(), false);

            var proc = param.Pkg.AddProcedure(newset);

            param.CS.Add(new Command(Opcode.CLOSURE, param.Pkg.ID, proc.ID)).SetCodePos(TypeInfo.FuncPos);
            
            Body.Compile(param.SetLHS(false).SetComdSet(newset));

            // 找到这一层闭包用的upvalues集合(引用上面函数作用域的)
            var upvalues = new Dictionary<UpvaluePair, Ident>();
            FindUsedUpvalue(Body, upvalues );

            // 捕获的目标变量的引用建立后, 在闭包的upvalue里的索引就无所谓了, 只要跟用的时候索引对应上就OK
            foreach( var pr in upvalues )
            {
                int regIndex = CalcRefUpvalueRegIndex(TypeInfo.ScopeInfo, pr.Value.Symbol.RegBelong, pr.Value);

                // 对需要引用上层作用域变量的Upvalue, 放入指令进行引用
                param.CS.Add(new Command(Opcode.LINKU, regIndex))
                    .SetComment( pr.Value.ToString() )
                    .SetCodePos(TypeInfo.FuncPos);                
            }

            foreach (var pr in RefUpvalues)
            {
                // 对需要引用上层作用域变量的Upvalue, 放入指令进行引用
                 param.CS.Add(new Command(Opcode.LINKU, pr.Symbol.RegIndex))
                    .SetComment(pr.ToString())
                    .SetCodePos(TypeInfo.FuncPos);
            }


            newset.InputValueCount = TypeInfo.Params.Count;

            newset.OutputValueCount = FuncType.CalcReturnValueCount( Body.Child() );
            
        }

        FuncLit Scope2FuncLitNode( Scope s )
        {
            Node n = Parent;

            while( !(n is FuncLit) )
            {
                n = n.Parent;
            }

            return n as FuncLit;
        }

        int CalcRefUpvalueRegIndex( Scope baseScope, Scope regScope, Ident var )
        {
            int regIndex = 0;

            Scope s = baseScope;

            int level = 0;

            while( regScope != s )
            {
                if ( s != baseScope )
                {
                    var fl = Scope2FuncLitNode(s);
                    if ( fl != null )
                    {
                        // 给这一层增加一个引用变量
                        fl.RefUpvalues.Add(var);

                        // 只有在最近的上一层才可以计算寄存器索引
                        if (level == -1 )
                        {
                            regIndex = fl.TypeInfo.ScopeInfo.RegCount;
                        }
                        
                    }
                }

                s = s.Outter;
                level--;
            }

            return regIndex;
        }

        void FindUsedUpvalue(Node n, Dictionary<UpvaluePair, Ident> upvalues)
        {
            var fn = n as FuncLit;
            if (fn != null)
            {
                int a = 1;
            }

            var usedVar = n as Ident;
            if (usedVar != null && usedVar.UpValue)
            {
                var key = new UpvaluePair( usedVar );
                if (!upvalues.ContainsKey(key))
                {
                    upvalues.Add(key, usedVar);
                }
                
            }

            // 如果也是闭包声明, 跳过
            if (n is FuncLit)
                return;

            foreach( var c in n.Child() )
            {                
                FindUsedUpvalue(c, upvalues);
            }
        }


    }

    // 作为引用的Upvalue, 必须保证在同作用域下的唯一
    class UpvaluePair
    {
        Ident _id;

        public UpvaluePair(Ident id)
        {
            _id = id;
        }

        public override bool Equals(object obj)
        {
            var otherid = (UpvaluePair)obj;

            return _id.Symbol == otherid._id.Symbol &&
                _id.Name == otherid._id.Name;
        }

        public override int GetHashCode()
        {
            return _id.Symbol.GetHashCode() + _id.Name.GetHashCode();
        }
    }
}
