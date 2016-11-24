using SharpLexer;
using System.Text;

namespace Photon
{
    internal class Ident : Expr
    {
        Token _token;

        internal Symbol Symbol; // 这个为空, 说明未找到定义

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
            if (Symbol != null)
            {
                var sb = new StringBuilder();

                if (Symbol.RegIndex != -1 )
                {
                    sb.Append(Symbol);
                }
                else
                {
                    sb.Append(Name);
                }

                if ( UpValue )
                {
                    sb.Append(" (UpValue)");
                }

                return sb.ToString();
            }

            return Name;

        }

        static bool ResolveFuncEntry(Package pkg, string name, Command cmd)
        {

            // 优先在本包找
            var proc = pkg.GetProcedureByName(name);
            if (proc != null)
            {
                cmd.DataA = pkg.ID;
                cmd.DataB = proc.ID;

                return true;
            }

            Procedure outP= pkg.Exe.GetProcedureByName(name );            

            if (outP != null )
            {                
                cmd.DataA = outP.Pkg.ID;
                cmd.DataB = outP.ID;

                return true;
            }

            return false;
        }

        internal override void Resolve(CompileParameter param)
        {
            // 故地重游, 再次拨叫
            if ( Symbol == null )
            {
                Symbol = BaseScope.FindSymbolOutter(Name);
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
                if( Symbol == null )
                {
                    throw new ParseException(string.Format("undeclared symbol {0}", Name), DefinePos);
                }

                if (Symbol.IsGlobal)
                {
                    _cmdGen.Op = Opcode.SETG;

                    _cmdGen.DataA = param.Pkg.ID;
                    _cmdGen.DataB = Symbol.RegIndex;
                }
                else if ( UpValue )
                {
                    _cmdGen.Op = Opcode.SETU;

                    
                    _cmdGen.DataA = Symbol.RegIndex;
                }
                else
                {
                    _cmdGen.Op = Opcode.SETR;

                    _cmdGen.DataA = Symbol.RegIndex;
                }
 
            }
            else
            {
                // 取值
                if ( Symbol == null )
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
                    switch (Symbol.Usage)    
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

                                if (Symbol.IsGlobal)
                                {
                                    _cmdGen.Op = Opcode.LOADG;
                                    _cmdGen.DataA = param.Pkg.ID;
                                    _cmdGen.DataB = Symbol.RegIndex;
                                }
                                else if (UpValue)
                                {
                                    _cmdGen.Op = Opcode.LOADU;
                                    _cmdGen.DataA = Symbol.RegIndex;
                                }
                                else
                                {
                                    _cmdGen.Op = Opcode.LOADR;
                                    _cmdGen.DataA = Symbol.RegIndex;
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
