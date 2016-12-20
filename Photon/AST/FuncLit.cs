
using System.Collections.Generic;

namespace Photon
{
    // 匿名函数
    internal class FuncLit : Expr
    {
        public FuncType TypeInfo;

        public BlockStmt Body;

        public List<Ident> UpValues = new List<Ident>();

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

        // foo  var a
        // closure var b   间接UpValue a
        // closure2 var c  直接UpValue a, b

        // LINKU和LOADU对应
        // 引用模式分为2种
        // 1. 引用最近的上一层作用域,  LINKU第一个参数=0(当前执行体不是闭包时)
        // 1. 引用上N层作用域,  LINKU第一个参数=1 (当前执行体为闭包时)
        // 
        // UpValue索引计算
        // 不使用被引用的变量在它归属作用域分配的寄存器索引
        // 直接引用并使用的UpValue

        internal override void Compile(CompileParameter param)
        {
            var newset = new ValuePhoFunc(param.Pkg.GenClosureName(), TypeInfo.FuncPos, TypeInfo.ScopeInfo.CalcUsedReg(), TypeInfo.ScopeInfo);

            var proc = param.Exe.AddFunc(newset);

            var closureCmd = new Command(Opcode.CLOSURE).SetCodePos(TypeInfo.FuncPos);
            closureCmd.FuncEntryName = newset.Name;

            param.CS.Add(closureCmd);

            var funcParam = param.SetLHS(false).SetCmdSet(newset);

            // 深度遍历, 所以最终引用层会先被遍历到
            Body.Compile(funcParam);
            
            // 找到这一层闭包用的upvalues集合(引用上面函数作用域的)            
            FindUsedUpvalue(Body );

            // 给每一个upvalue添加引用的upvalue
            foreach (var uv in UpValues)
            {                
                AddRefUpvalueInNode(TypeInfo.ScopeInfo, uv.Symbol.RegBelong, uv );
            }

            int ThisLevelIndex = 0;

            foreach (var uv in UpValues)
            {
                var cmd = param.CS.Add(new Command(Opcode.LINKU, -1, -1 ))
                  .SetComment(uv.ToString())
                  .SetCodePos(TypeInfo.FuncPos);


                // 引用的上一层的upvalue的索引, 对应指令引用LocalReg的Index
                int UplevelIndex = GetRegIndex(TypeInfo.ScopeInfo, uv.Name);
                if ( UplevelIndex != -1 )
                {
                    cmd.DataA = 0;
                    cmd.DataB = UplevelIndex;
                }
                else
                {
                    // 引用上层自己的Upvalue(间接引用)
                    UplevelIndex = GetUpvalueIndex(TypeInfo.ScopeInfo, uv.Name);

                    cmd.DataA = 1;
                    cmd.DataB = UplevelIndex;
                }
                

                // LOADU的对应的Upvalue的索引, 修改下
                uv.CmdGen.DataA = ThisLevelIndex;

                ThisLevelIndex++;
            }

            TypeInfo.GenDefaultRet(Body.Child(), funcParam);

            newset.InputValueCount = TypeInfo.Params.Count;

            newset.OutputValueCount = FuncType.CalcReturnValueCount( Body.Child() );
            
        }


        int GetUpvalueIndex( Scope baseScope, string name )
        {
            Scope s = baseScope.Outter;
            FuncLit fl = Scope2FuncLitNode(s, Parent);
            
            for (int i = 0; i < fl.UpValues.Count;i++ )
            {
                if (fl.UpValues[i].Name == name)
                    return i;
            }

            return -1;
        }

        int GetRegIndex( Scope baseScope, string name )
        {
            Scope s = baseScope.Outter;
            var symbol = s.FindSymbol(name);
            if (symbol == null)
                return -1;

            return symbol.RegIndex;
        }

        void AddRefUpvalueInNode( Scope baseScope, Scope regScope, Ident var )
        {                        
            Scope s = baseScope.Outter;

            int level = 0;

            while( regScope != s )
            {

                var fl = Scope2FuncLitNode(s, Parent);
                if ( fl != null )
                {
                    // 给这一层增加一个引用变量
                    fl.UpValues.Add(var);       
                }

                s = s.Outter;
                level--;
            }            
        }

        void FindUsedUpvalue(Node n )
        {
            var usedVar = n as Ident;
            if (usedVar != null && usedVar.UpValue)
            {                
                if (!UpValues.Contains(usedVar))
                {
                    UpValues.Add(usedVar);
                }
                
            }

            // 如果也是闭包声明, 跳过
            if (n is FuncLit)
                return;

            foreach( var c in n.Child() )
            {                
                FindUsedUpvalue(c );
            }
        }

        static FuncLit Scope2FuncLitNode(Scope s, Node parent)
        {
            Node n = parent;

            while (!(n is FuncLit))
            {
                n = n.Parent;
            }

            return n as FuncLit;
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
