using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Photon
{
    public interface IPhoSerializable
    {
        void Serialize(BinarySerializer serializer);
    }

    public class BinarySerializer
    {
        BinaryWriter _writer;
        BinaryReader _reader;

        public BinarySerializer(Stream s, bool loading)
        {
            if (loading)
            {
                _reader = new BinaryReader(s);
            }
            else
            {
                _writer = new BinaryWriter(s);
            }

        }

        public bool IsLoading
        {
            get { return _writer == null; }
        }

        public Stream Stream
        {
            get { return _writer.BaseStream; }
        }

        public BinarySerializer Serialize(ref int data)
        {
            if (IsLoading)
            {
                data = _reader.ReadInt32();
            }
            else
            {
                _writer.Write(data);
            }

            return this;
        }

        public BinarySerializer Serialize(ref string data)
        {
            if (IsLoading)
            {
                data = _reader.ReadString();
            }
            else
            {
                _writer.Write(data);
            }

            return this;
        }

        // 序列化对象
        public BinarySerializer Serialize<T>(ref T data) where T : IPhoSerializable
        {
            if (IsLoading)
            {
                string name = string.Empty;
                this.Serialize(ref name);

                data = (T)Activator.CreateInstance(Assembly.GetExecutingAssembly().FullName, name).Unwrap();
            }
            else
            {                
                string name = data.GetType().FullName;
                this.Serialize(ref name);
            }

            data.Serialize(this);

            return this;
        }

        // 列表
        public BinarySerializer Serialize<T>(ref List<T> listdata) where T : IPhoSerializable
        {
            if (listdata == null)
            {
                listdata = new List<T>();
            }

            int size = 0;

            if (IsLoading)
            {
                Serialize(ref size);

                for (int i = 0; i < size; i++)
                {
                    T v = default(T);
                    Serialize(ref v);                    
                    listdata.Add(v);
                }
            }
            else
            {
                size = listdata.Count;
                Serialize(ref size);

                for (int i = 0; i < listdata.Count;i++ )
                {
                    var data = listdata[i];
                    Serialize(ref data);
                }
            }

            return this;
        }
    }

}
