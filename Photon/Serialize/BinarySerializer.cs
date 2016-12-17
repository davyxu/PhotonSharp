using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Photon
{
    // 不生成自动绑定类
    public sealed class PhoSerializeAttribute : Attribute
    {

    }


    public class BinarySerializer
    {
        BinaryWriter _writer;

        public BinarySerializer(Stream s)
        {
            _writer = new BinaryWriter(s);
        }

        public void SerializeValue(Type ft, object ins  )
        {                     
            if ( ft == typeof(int))
            {
                _writer.Write((int)ins);
            }
            else if (ft == typeof(string))
            {
                if ( string.IsNullOrEmpty((string)ins))
                {
                    _writer.Write(string.Empty);
                }
                else
                {
                    _writer.Write((string)ins);
                }
            }            
            else if ( ft.IsGenericType )
            {
                if ( ft.GetGenericTypeDefinition() == typeof(List<>))
                {
                    SerializeValue(typeof(int),(ins as ICollection).Count);                    
                   
                    foreach (var listItem in ins as IEnumerable)
                    {
                        SerializeValue(listItem.GetType(), listItem);
                    }

                }
                else if (ft.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    SerializeValue(typeof(int), (ins as ICollection).Count);
                    
                    foreach (DictionaryEntry dictItem in ins as IDictionary)
                    {
                        SerializeValue(dictItem.Key.GetType(), dictItem.Key);
                        SerializeValue(dictItem.Value.GetType(), dictItem.Value);
                    }
                }
                else
                {
                    throw new Exception("Serialize failed, unknown type " + ft.ToString());
                }
            }
            else if (ft.IsArray )
            {
                var arr = ins as System.Array;

                SerializeValue(typeof(int), arr.Length);

                for (int i = 0; i < arr.Length; i++)
                {
                    var obj = arr.GetValue(i);
                    SerializeValue(ft.GetElementType(), obj);
                }
            }
            else if (ft.IsEnum )
            {
                SerializeValue(typeof(int),Convert.ToInt32(ins));
            }
            else if (ft.IsClass)
            {
                SerializeValue(typeof(string), ft.FullName);

                int serfieldCount = 0;

                // 只遍历私有成员
                foreach (var mi in ft.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (mi.IsDefined(typeof(PhoSerializeAttribute), false))
                    { 
                        SerializeValue(mi.FieldType, mi.GetValue(ins));
                        serfieldCount++;
                    }
                }

                if ( serfieldCount == 0 )
                {
                    throw new Exception("zero serialize " + ft.ToString());
                }
            }
            else
            {
                throw new Exception("Serialize failed, unknown type " + ft.ToString());
            }
        }
    }
}
