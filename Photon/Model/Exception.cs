using System;

namespace Photon.Model
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
