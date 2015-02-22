using System;

namespace Photon.Scanner
{
    public class IdentifierMatcher : TokenMatcher
    {
        public override Token Match(Tokenizer tz)
        {

            if (!( Char.IsLetter(tz.Current) || tz.Current == '_' ))
                return null;

            int beginIndex = tz.Index;


            do
            {
                tz.Consume();

            } while (char.IsLetterOrDigit(tz.Current) || tz.Current == '_');


            return new Token( TokenType.Identifier, tz.Source.Substring( beginIndex, tz.Index - beginIndex) );
        }

    }
}
