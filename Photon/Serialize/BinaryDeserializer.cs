using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Photon
{
    public class BinaryDeserializer
    {
        BinaryReader _reader;

        public BinaryDeserializer(Stream s)
        {
            _reader = new BinaryReader(s);
        }

        public T DeserializeValue<T>()
        {
            return (T)DeserializeValue(typeof(T));
        }

        public object DeserializeValue(Type ft)
        {
            if (ft == typeof(int))
            {
                return _reader.ReadInt32();
            }
            else if (ft == typeof(string))
            {
                return _reader.ReadString();
            }
            else if (ft.IsGenericType)
            {
                if (ft.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var size = DeserializeValue<int>();

                    var parameterType = ft.GetGenericArguments()[0];

                    var ins = Activator.CreateInstance(ft) as IList;

                    for (int i = 0; i < size; i++)
                    {
                        var v = DeserializeValue(parameterType);
                        ins.Add(v);
                    }

                    return ins;

                }
                else if (ft.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    var size = DeserializeValue<int>();

                    var ins = Activator.CreateInstance(ft) as IDictionary;

                    var keyType = ft.GetGenericArguments()[0];
                    var valueType = ft.GetGenericArguments()[1];

                    for (int i = 0; i < size; i++)
                    {
                        var key = DeserializeValue(keyType);
                        var value = DeserializeValue(valueType);
                        ins.Add(key, value);
                    }

                    return ins;
                }
                else
                {
                    throw new Exception("Deserialize failed, unknown type " + ft.ToString());
                }
            }
            else if (ft.IsArray)
            {
                var size = DeserializeValue<int>();

                var ins = Activator.CreateInstance(ft, size) as System.Array;

                for (int i = 0; i < size; i++)
                {
                    var v = DeserializeValue(ft.GetElementType());
                    ins.SetValue(v, i);
                }

                return ins;
            }
            else if (ft.IsEnum)
            {
                var v = DeserializeValue(typeof(int));
                return Enum.ToObject(ft, v);
            }
            else if (ft.IsClass)
            {
                var className = DeserializeValue<string>();

                var ins = Activator.CreateInstance(Assembly.GetExecutingAssembly().FullName, className).Unwrap();

                int desercount = 0;
                // 只遍历私有成员
                foreach (var mi in ins.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (mi.IsDefined(typeof(PhoSerializeAttribute), false))
                    {
                        var v = DeserializeValue(mi.FieldType);
                        mi.SetValue(ins, v);
                        desercount++;
                    }
                }

                if (desercount == 0)
                {
                    throw new Exception("zero deserialize " + ft.ToString());
                }

                return ins;
            }
            else
            {
                throw new Exception("Deserialize failed, unknown type " + ft.ToString());
            }
        }
    }
}
