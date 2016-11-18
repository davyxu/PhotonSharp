using SharpLexer;

namespace Photon
{


    public class Ident : Expr
    {
        Token _token;

        internal Symbol ScopeInfo; // 这个为空, 说明未找到定义

        internal Scope BaseScope;   // 所在的Scope

        internal bool UpValue; // 闭包中捕获的变量

        Command _cmdGen;

        public Ident(Token t)
        {
            _token = t;
        }

        public TokenPos DefinePos
        {
            get { return _token.Pos; }
        }

        public string Name
        {
            get { return _token.Value; }
        }

        public override string ToString()
        {
            if (ScopeInfo != null)
            {
                if ( UpValue )
                    return string.Format("{0} R{1} (UpValue)", Name, ScopeInfo.RegIndex);   
                else
                    return string.Format("{0} R{1}", Name, ScopeInfo.RegIndex);   
            }

            return Name;

        }

        static bool ResolveFuncEntry(Package pkg, string name, Command cmd)
        {

            // 优先在本包找
            var cs = pkg.FindProcedureByName(name) as CommandSet;
            if (cs != null)
            {
                cmd.DataA = pkg.ID;
                cmd.DataB = cs.ID;

                return true;
            }

            Procedure outP;
            Package outPkg;

            if (pkg.Exe.FindCmdSetInPackage(name, out outP, out outPkg))
            {
                var outCS = outP as CommandSet;

                cmd.DataA = outPkg.ID;
                cmd.DataB = outCS.ID;

                return true;
            }

            return false;
        }

        internal override void Resolve(CompileParameter param)
        {
            if (ScopeInfo == null )
            {

            }
            else
            {
                if (!ResolveFuncEntry(param.Pkg, Name, _cmdGen))
                {
                    throw new ParseException("unsolved function entry", DefinePos);
                }
            }
            
        }


        internal override void Compile(CompileParameter param)
        {            

            // 赋值
            if (param.LHS)
            {
                if( ScopeInfo == null )
                {
                    throw new ParseException(string.Format("undeclared symbol {0}", Name), DefinePos);
                }

                if (ScopeInfo.IsGlobal)
                {
                    _cmdGen = param.CS.Add(new Command(Opcode.SETG, ScopeInfo.RegIndex));
                }
                else if ( UpValue )
                {
                    _cmdGen = param.CS.Add(new Command(Opcode.SETU, ScopeInfo.RegIndex));
                }
                else
                {
                    _cmdGen = param.CS.Add(new Command(Opcode.SETR, ScopeInfo.RegIndex));
                }
                
            }
            else
            {
                // 取值


                if ( ScopeInfo == null )
                {
                    _cmdGen = param.CS.Add(new Command(Opcode.NOP, 0, 0)).SetCodePos(DefinePos);

                    param.NextPassToResolve(this);
                    //throw new ParseException("Not Implement", DefinePos);
                    // 将自己视为字符串( 在处理selector和index指令时, 将key视为字符串, 后面没有用到这段代码)

                    //var c = new ValueString(_token.Value);

                    //var ci = param.Pkg.Constants.Add(c);

                    //_cmdGen = param.CS.Add(new Command(Opcode.LOADC, ci));
                }
                else
                {
                    switch (ScopeInfo.Usage)    
                    {
                        case SymbolUsage.Delegate:
                        case SymbolUsage.Func:
                            {

                                _cmdGen = param.CS.Add(new Command(Opcode.LOADF, 0, 0)).SetCodePos(DefinePos);

                                if (!ResolveFuncEntry(param.Pkg, Name, _cmdGen))
                                {
                                    // 不是本package, 或者函数在本package后面定义, 延迟到下次pass进行解析

                                    param.NextPassToResolve(this);                                    
                                }

                            }
                            break;
                        case SymbolUsage.Variable:
                        case SymbolUsage.Parameter:
                            {
                                // 将自己视为变量

                                if (ScopeInfo.IsGlobal)
                                {
                                    _cmdGen = param.CS.Add(new Command(Opcode.LOADG, ScopeInfo.RegIndex));
                                }
                                else if (UpValue)
                                {
                                    _cmdGen = param.CS.Add(new Command(Opcode.LOADU, ScopeInfo.RegIndex));
                                }
                                else
                                {
                                    _cmdGen = param.CS.Add(new Command(Opcode.LOADR, ScopeInfo.RegIndex));
                                }
                            }
                            break;
                        default:
                            throw new ParseException("Unknown usage", DefinePos);
                    }
                }
      
                
            }


            _cmdGen.SetComment(Name).SetCodePos(DefinePos);
        }
    }
}
