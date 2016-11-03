
using Photon.OpCode;
using SharpLexer;
namespace Photon.AST
{
    public class Ident : Expr
    {
        Token _token;

        public Symbol ScopeInfo;

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
                return string.Format("{0} R({1})", Name, ScopeInfo.RegIndex);
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
                else
                {
                    cm.Add(new Command(Opcode.SetR, ScopeInfo.RegIndex)).Comment = Name;
                }
                
            }
            else
            {
                if (ScopeInfo.IsGlobal )
                {
                    cm.Add(new Command(Opcode.LoadG, ScopeInfo.RegIndex)).Comment = Name;
                }
                else
                {
                    cm.Add(new Command(Opcode.LoadR, ScopeInfo.RegIndex)).Comment = Name;
                }
                
            }
        }
    }
}
