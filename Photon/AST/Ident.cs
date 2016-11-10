
using Photon.Model;
using SharpLexer;
namespace Photon.AST
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

        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {            

            if (lhs)
            {
                if (ScopeInfo.IsGlobal)
                {
                    cm.Add(new Command(Opcode.SetG, ScopeInfo.RegIndex)).Comment = Name;
                }
                else if ( UpValue )
                {
                    cm.Add(new Command(Opcode.SetU, ScopeInfo.RegIndex)).Comment = Name;
                }
                else
                {
                    cm.Add(new Command(Opcode.SetR, ScopeInfo.RegIndex)).Comment = Name;
                }
                
            }
            else
            {
                if ( ScopeInfo == null )
                {
                    // 将自己视为字符串( 在处理selector和index指令时, 将key视为字符串, 后面没有用到这段代码)

                    var c = new ValueString(_token.Value);

                    var ci = exe.Constants.Add( c );

                    cm.Add(new Command(Opcode.LoadC, ci)).Comment = c.ToString();
                }
                else
                {
                    // 将自己视为变量

                    if (ScopeInfo.IsGlobal)
                    {
                        cm.Add(new Command(Opcode.LoadG, ScopeInfo.RegIndex)).Comment = Name;
                    }
                    else if ( UpValue )
                    {
                        cm.Add(new Command(Opcode.LoadU, ScopeInfo.RegIndex)).Comment = Name;
                    }
                    else
                    {
                        cm.Add(new Command(Opcode.LoadR, ScopeInfo.RegIndex)).Comment = Name;
                    }
                }
            }
        }
    }
}
