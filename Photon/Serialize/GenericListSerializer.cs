using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Photon
{

    public class GenericListSerializer : TypeSerializer
    {
        public override bool Match(Type ft)
        {
            return ft.GetGenericTypeDefinition() == typeof(List<>);
        }

        public override void Serialize(BinarySerializer2 ser, object ins)
        {
            ser.Serialize(typeof(int), (ins as ICollection).Count);

            foreach (var listItem in ins as IEnumerable)
            {
                ser.Serialize(listItem.GetType(), listItem);
            }
        }

        public override object Deserialize(BinaryReader reader)
        {
            return reader.ReadString();
        }
    }

}
