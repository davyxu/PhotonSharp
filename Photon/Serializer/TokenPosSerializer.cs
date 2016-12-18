using MarkSerializer;
using SharpLexer;
using System;

namespace Photon
{

    public class TokenPosSerializer : BinaryTypeSerializer
    {
        public override bool Match(Type ft)
        {
            return ft == typeof(TokenPos);
        }

        public override void Serialize(BinarySerializer ser, object ins)
        {
            var tp = (TokenPos)ins;
            ser.Serialize<int>(tp.Col);
            ser.Serialize<int>(tp.Line);
            ser.Serialize<string>(tp.SourceName);
        }

        public override object Deserialize(BinaryDeserializer ser, Type ft)
        {
            TokenPos tp;
            tp.Col = ser.Deserialize<int>();
            tp.Line = ser.Deserialize<int>();
            tp.SourceName = ser.Deserialize<string>();

            return tp;
        }
    }

}
