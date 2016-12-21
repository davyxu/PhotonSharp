using MarkSerializer;
using SharpLexer;
using System;

namespace Photon
{

    public class TokenPosSerializer : TypeSerializer
    {
        public override bool Match(Type ft)
        {
            return ft == typeof(TokenPos);
        }

        public override bool Serialize(BinarySerializer ser, Type ft, ref object obj)
        {            
            var ins = (TokenPos)obj;

            ser.Serialize(ref ins.Col);
            ser.Serialize(ref ins.Line);
            ser.Serialize(ref ins.SourceName);

            return true;
        }        
    }

}
