using Photon.Model;
using System.Collections.Generic;

namespace Photon.AST
{
    // 匿名函数
    public class FuncLit : Expr
    {
        public FuncType TypeInfo;

        public BlockStmt Body;

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


        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {
            var newset = new CommandSet("closure", TypeInfo.FuncPos, TypeInfo.ScopeInfo.CalcUsedReg(), false);

            var funcIndex = exe.AddCmdSet(newset);

            var c = new ValueFunc(funcIndex, TypeInfo.FuncPos);
            var ci = exe.Constants.Add(c);

            cm.Add(new Command(Opcode.Closure, ci))
                .SetComment(c.ToString())
                .SetCodePos(TypeInfo.FuncPos);

            Body.Compile(exe, newset, false);

            // 找这个闭包用过的
            var upvalues = new Dictionary<UpvaluePair, Ident>();
            FindUsedUpvalue(Body, upvalues );

            // 捕获的目标变量的引用建立后, 在闭包的upvalue里的索引就无所谓了, 只要跟用的时候索引对应上就OK
            int upIndex = 0;
            foreach( var pr in upvalues )
            {
                // 对需要引用上层作用域变量的Upvalue, 放入指令进行引用
                cm.Add(new Command(Opcode.LinkU, upIndex, pr.Value.ScopeInfo.RegIndex))
                    .SetComment( pr.Value.ToString() )
                    .SetCodePos(TypeInfo.FuncPos);

                upIndex++;
            }


            newset.InputValueCount = TypeInfo.Params.Count;

            newset.OutputValueCount = FuncType.CalcReturnValueCount( Body.Child() );
            
        }

        void FindUsedUpvalue(Node n, Dictionary<UpvaluePair, Ident> upvalues)
        {
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

            return _id.ScopeInfo == otherid._id.ScopeInfo &&
                _id.Name == otherid._id.Name;
        }

        public override int GetHashCode()
        {
            return _id.ScopeInfo.GetHashCode() + _id.Name.GetHashCode();
        }
    }
}
