using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    // a.b   x=a  selector=b
    internal class SelectorExpr : Expr
    {
        public Expr X;
        public Ident Selector;
        public TokenPos DotPos;
        
        public SelectorExpr(Expr x, Ident sel, TokenPos pos)
        {
            X = x;
            Selector = sel;
            DotPos = pos;
            BuildRelation();

        }

        public override IEnumerable<Node> Child()
        {
            yield return X;

            yield return Selector;
        }

        public override string ToString()
        {
            return "SelectorExpr";
        }

        internal int CompileSelfParameter(CompileParameter param)
        {
            var xident = X as Ident;
            if (xident != null)
            {
                if (xident.Symbol == null)
                {
                    throw new CompileException("undefined symbol: " + xident.Name, DotPos);
                }


                switch( xident.Symbol.Usage )
                {
                    case SymbolUsage.Parameter:
                    case SymbolUsage.Variable:
                        {
                            X.Compile(param.SetLHS(false));
                            return 1;
                        }                     
                }
            }

            return 0;
        }

        internal override void Compile(CompileParameter param)
        {
            var xident = X as Ident;
            if ( xident != null )
            {
                if ( xident.Symbol == null )
                {
                    throw new CompileException("undefined symbol: " + xident.Name, DotPos);
                }
                
                switch( xident.Symbol.Usage )
                {
                        // 包.函数名
                    case SymbolUsage.Package:
                        {
                            var pkg = param.Exe.GetPackageByName(xident.Name);

                            if (pkg == null)
                            {
                                throw new CompileException("package not found: " + xident.Name, DotPos);
                            }

                            // Ident直接出代码
                            Selector.Compile(param.SetLHS(false).SetPackage(pkg));
                        }
                        break;
                    // 实例.函数名  转换为  函数名( 实例, p2...)
                    // 类成员访问
                    case SymbolUsage.Parameter:
                    case SymbolUsage.Variable:
                    case SymbolUsage.SelfParameter:
                        {
                            
                            X.Compile(param.SetLHS(false));

                            
                            var ci = param.Pkg.Constants.AddString(Selector.Name);

                            Opcode cm = param.LHS ? Opcode.SETM : Opcode.LOADM;

                            // 无法推导X类型, 所以这里只能用动态方法直接加载,或设置
                            param.CS.Add(new Command(cm, ci))
                                .SetCodePos(DotPos).SetComment(Selector.Name);
                        }
                        break;
                    default:
                        throw new CompileException("unknown symbol usage", DotPos);
                }

            }
            else
            {
                // 动态表达式, 需要用指令解析
                X.Compile(param);                

                var ci = param.Pkg.Constants.AddString(Selector.Name);

                param.CS.Add(new Command(Opcode.SEL, ci))
                    .SetCodePos(DotPos).SetComment(Selector.Name);
            }

           
        }
    }
}
