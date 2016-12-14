using SharpLexer;
using System;

namespace Photon
{
    public class CompileException : Exception
    {
        public TokenPos Pos;

        public CompileException(string msg, TokenPos pos)
            : base(msg)
        {
            Pos = pos;
        }

        public override string ToString()
        {
            return string.Format("{0} at line {1}", this.Message, Pos.Line);
        }
    }

    public class RuntimeException : Exception
    {
        public RuntimeException(string msg)
            : base( msg )
        {

        }
    }
}
