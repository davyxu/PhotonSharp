using System;
using System.IO;

namespace Photon
{

    public class StringSerializer : TypeSerializer
    {
        public override bool Match(Type ft)
        {
            return ft == typeof(string);
        }

        public override void Serialize(BinarySerializer2 ser, object ins)
        {
            if (string.IsNullOrEmpty((string)ins))
            {
                ser.Writer.Write(string.Empty);
            }
            else
            {
                ser.Writer.Write((string)ins);
            }
        }


        public override object Deserialize(BinaryReader reader)
        {
            return reader.ReadString();
        }
    }

}
