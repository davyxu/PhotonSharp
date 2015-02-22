using System;

namespace Photon.Scanner
{
    public class NumeralMatcher : TokenMatcher
    {
        public override Token Match(Tokenizer tz)
        {

            if (!Char.IsDigit(tz.Current))
                return null;

            int beginIndex = tz.Index;


            do
            {
                tz.Consume();

            } while (char.IsDigit(tz.Current) || tz.Current == '.');


            return new Token(TokenType.Number, tz.Source.Substring( beginIndex, tz.Index - beginIndex) );
        }

    }
}
