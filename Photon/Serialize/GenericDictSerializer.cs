using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Photon
{

    public class GenericDictSerializer : TypeSerializer
    {
        public override bool Match(Type ft)
        {
            return ft.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }

        public override void Serialize(BinarySerializer2 ser, object ins)
        {
            ser.Serialize(typeof(int), (ins as ICollection).Count);

            foreach (DictionaryEntry dictItem in ins as IDictionary)
            {
                ser.Serialize(dictItem.Key.GetType(), dictItem.Key);
                ser.Serialize(dictItem.Value.GetType(), dictItem.Value);
            }
        }

        public override object Deserialize(BinaryReader reader)
        {
            return reader.ReadString();
        }
    }

}
