using SharpLexer;

namespace Photon
{


    public class Ident : Expr
    {
        Token _token;

        public Symbol ScopeInfo;

        public bool UpValue; // 闭包中捕获的变量


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

        internal override void Compile(Package pkg, CommandSet cm, bool lhs)
        {
            Command cmd = null;

            if (lhs)
            {
                if( ScopeInfo == null )
                {
                    throw new ParseException(string.Format("undeclared symbol {0}", Name), DefinePos);
                }

                if (ScopeInfo.IsGlobal)
                {
                    cmd = cm.Add(new Command(Opcode.SETG, ScopeInfo.RegIndex));
                }
                else if ( UpValue )
                {
                    cmd = cm.Add(new Command(Opcode.SETU, ScopeInfo.RegIndex));
                }
                else
                {
                    cmd = cm.Add(new Command(Opcode.SETR, ScopeInfo.RegIndex));
                }
                
            }
            else
            {
                if ( ScopeInfo == null )
                {
                    // 将自己视为字符串( 在处理selector和index指令时, 将key视为字符串, 后面没有用到这段代码)

                    var c = new ValueString(_token.Value);

                    var ci = pkg.Constants.Add( c );

                    cmd = cm.Add(new Command(Opcode.LOADC, ci));
                }
                else
                {
                    // 将自己视为变量

                    if (ScopeInfo.IsGlobal)
                    {
                        cmd = cm.Add(new Command(Opcode.LOADG, ScopeInfo.RegIndex));
                    }
                    else if ( UpValue )
                    {
                        cmd = cm.Add(new Command(Opcode.LOADU, ScopeInfo.RegIndex));
                    }
                    else
                    {
                        cmd = cm.Add(new Command(Opcode.LOADR, ScopeInfo.RegIndex));
                    }
                }
            }


            cmd.SetComment(Name).SetCodePos(DefinePos);
        }
    }
}
