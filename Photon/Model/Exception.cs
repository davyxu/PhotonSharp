using SharpLexer;
using System;

namespace Photon
{
    public class ParseException : Exception
    {
        public TokenPos Pos;

        public ParseException(string msg, TokenPos pos)
            : base(msg)
        {
            Pos = pos;
        }

        public override string ToString()
        {
            return string.Format("{0} at line {1}", this.Message, Pos.Line);
        }
    }

    class RuntimeExcetion : Exception
    {
        public RuntimeExcetion(string msg)
            : base( msg )
        {

        }
    }
}
