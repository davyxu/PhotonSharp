using SharpLexer;

namespace Photon
{
    internal class BaseLit : Expr
    {                
        public TokenPos Pos;                

        public BaseLit(TokenPos pos)
        {
            Pos = pos;
        }

        public override string ToString()
        {
            return string.Format("BaseLit {0}", Pos);
        }

        internal override void Compile(CompileParameter param)
        {

        }

      
    }
}
