using System;
using System.Collections.Generic;
using System.IO;

namespace Photon
{
    public class TypeSerializer
    {
        public virtual bool Match(Type ft)
        {
            return false;
        }
        public virtual void Serialize(BinarySerializer2 ser, object ins)
        {
            
        }

        public virtual object Deserialize(BinaryReader reader)
        {
            return null;
        }
    }

    public class BinarySerializer2
    {
        List<TypeSerializer> _ser = new List<TypeSerializer>();

        BinaryWriter _writer;


        internal BinaryWriter Writer
        {
            get { return _writer; }
        }

        public BinarySerializer2(Stream s)
        {
            _writer = new BinaryWriter(s);

            Register(new Integer32Serializer());
        }

        public void Register(TypeSerializer ins)
        {
            _ser.Add(ins);
        }

        TypeSerializer MatchType(Type ft)
        {
            foreach (var ts in _ser)
            {
                if (ts.Match(ft))
                {
                    return ts;
                }
            }

            return null;
        }

        public void Serialize(Type ft, object ins)
        {
            var ts = MatchType(ft);
            if ( ts == null )
            {
                throw new Exception("Serialize failed, unknown type " + ft.ToString());
            }

            ts.Serialize(this, ins);
        }
    }

}
