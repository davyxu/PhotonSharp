using System;

namespace Photon
{
    class ParserException : Exception
    {
        public ParserException( string msg )
            : base( msg )
        {

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
