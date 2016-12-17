using System;
using System.IO;

namespace Photon
{

    public class Integer32Serializer : TypeSerializer
    {
        public override bool Match(Type ft)
        {
            return ft == typeof(int);
        }

        public override void Serialize(BinarySerializer2 ser, object ins)
        {
            ser.Writer.Write((int)ins);
        }

        public override object Deserialize(BinaryReader reader)
        {
            return reader.ReadInt32();
        }
    }

}
