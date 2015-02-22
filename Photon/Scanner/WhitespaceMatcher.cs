

namespace Photon.Scanner
{    
    public class WhitespaceMatcher : TokenMatcher
    {
        public static Token WhiteToken = new Token(TokenType.Whitespace, null);

        bool IsWhiteSpace( char c )
        {
            return c == ' ' || c == '\t' || c == '\r' || c == '\n';
        }

        public override Token Match(Tokenizer tz)
        {
            
            int count = 0;
            for (; ; count++)
            {
                if (!IsWhiteSpace(tz.Peek(count)))
                    break;                
            }

            if (count == 0)
                return null;

            int beginIndex = tz.Index;

            tz.Consume( count );

            return WhiteToken;
        }
    }
}
