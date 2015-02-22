using System.Collections.Generic;

namespace Photon.Scanner
{
    public class Lexer
    {
        IList<TokenMatcher> _tokenmatcher = new List<TokenMatcher>();
        IEnumerator<Token> _tokeniter;

        public void AddMatcher(TokenMatcher matcher)
        {
            _tokenmatcher.Add(matcher);
        }

        public override string ToString()
        {
            if (_tokeniter != null)
                return Peek().ToString();

            return base.ToString();
        }

        public IEnumerable<Token> Tokenize(string source)
        {
            var tz = new Tokenizer(source);

            while( !tz.EOF() )
            {

                foreach (var matcher in _tokenmatcher)
                {
                    var token = matcher.Match(tz);
                    if (token == null)
                    {
                        continue;
                    }

                    // 跳过已经parse部分, 不返回外部
                    if (matcher.IsIgnored)
                        break;


                    yield return token;

                    break;
                }

            }


            yield return new Token(TokenType.EOF, null );
        }

        public void Start( string src )
        {
            _tokeniter = Tokenize(src).GetEnumerator();

            _tokeniter.MoveNext();
        }

        public Token Read( )
        {
            var tk = _tokeniter.Current;

            _tokeniter.MoveNext();

            return tk;
        }

        public Token Peek( )
        {
            return _tokeniter.Current;
        }
    }
}
