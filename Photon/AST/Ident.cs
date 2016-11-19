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
            // 故地重游, 再次拨叫
            if ( ScopeInfo == null )
            {
                ScopeInfo = BaseScope.FindSymbolOutter(Name);
            }

            GenCode(param, 2);
        }

        internal override void Compile(CompileParameter param)
        {
            // 占位
            _cmdGen = param.CS.Add(new Command(Opcode.NOP))
                .SetComment(Name)
                .SetCodePos(DefinePos);

            GenCode(param, 1);

            if ( _cmdGen.Op == Opcode.NOP && !param.IsNodeInNextPass(this) )
            {
                throw new ParseException("code not resolve", DefinePos);
            }
        }


        void GenCode(CompileParameter param, int pass)
        {            
            // 赋值
            if (param.LHS)
            {
                // TODO 左值在下方被定义
                if( ScopeInfo == null )
                {
                    throw new ParseException(string.Format("undeclared symbol {0}", Name), DefinePos);
                }

                if (ScopeInfo.IsGlobal)
                {
                    _cmdGen.Op = Opcode.SETG;

                    _cmdGen.DataA = param.Pkg.ID;
                    _cmdGen.DataB = ScopeInfo.RegIndex;
                }
                else if ( UpValue )
                {
                    _cmdGen.Op = Opcode.SETU;

                    
                    _cmdGen.DataA = ScopeInfo.RegIndex;
                }
                else
                {
                    _cmdGen.Op = Opcode.SETR;

                    _cmdGen.DataA = ScopeInfo.RegIndex;
                }


                
            }
            else
            {
                // 取值
                if ( ScopeInfo == null )
                {
                    if (pass == 1)
                    {
                        param.NextPassToResolve(this);
                    }
                    else
                    {
                        throw new ParseException(string.Format("undeclared symbol {0}", Name), DefinePos);
                    }
                }
                else
                {
                    switch (ScopeInfo.Usage)    
                    {
                        case SymbolUsage.Delegate:
                        case SymbolUsage.Func:
                            {
                                _cmdGen.Op = Opcode.LOADF;

                                if (!ResolveFuncEntry(param.Pkg, Name, _cmdGen))
                                {
                                    // 不是本package, 或者函数在本package后面定义, 延迟到下次pass进行解析

                                    if (pass == 1)
                                    {
                                        param.NextPassToResolve(this);
                                    }
                                    else
                                    {
                                        throw new ParseException(string.Format("unsolved function name {0}", Name), DefinePos);
                                    }

                                    
                                }

                            }
                            break;
                        case SymbolUsage.Variable:
                        case SymbolUsage.Parameter:
                            {
                                // 将自己视为变量

                                if (ScopeInfo.IsGlobal)
                                {
                                    _cmdGen.Op = Opcode.LOADG;
                                    _cmdGen.DataA = param.Pkg.ID;
                                    _cmdGen.DataB = ScopeInfo.RegIndex;
                                }
                                else if (UpValue)
                                {
                                    _cmdGen.Op = Opcode.LOADU;
                                    _cmdGen.DataA = ScopeInfo.RegIndex;
                                }
                                else
                                {
                                    _cmdGen.Op = Opcode.LOADR;
                                    _cmdGen.DataA = ScopeInfo.RegIndex;
                                }

                                
                            }
                            break;
                        default:
                            throw new ParseException("Unknown usage", DefinePos);
                    }
                }
      
                
            }
         
        }
    }
}
