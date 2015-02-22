namespace Photon.Scanner
{

    public class UnknownMatcher : TokenMatcher
    {
        public override Token Match(Tokenizer tz)
        {
            int beginIndex = tz.Index;
            tz.Consume();
            return new Token(TokenType.Unknown, tz.Source.Substring( beginIndex, 1) );
        }
    }
}
