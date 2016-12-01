using System.Collections.Generic;

namespace Photon
{
    internal class ClassType
    {
        internal int ID { get; set; }
        
        internal ObjectName Name 
        {
            get { return _name; }
        }

        Dictionary<int, Value> _member = new Dictionary<int, Value>();
        
        Executable _exe;

        ObjectName _name;

        internal ClassType Parent { get; set; }

        internal ClassType(Executable exe, ObjectName name)
        {
            _exe = exe;
            _name = name;
        }


        internal void AddMethod( int nameKey, Procedure proc )
        {
            _member.Add(nameKey, new ValueFunc(proc));
        }

        internal void AddMemeber( int nameKey, string name )
        {
            _member.Add(nameKey, new ValueNil());
        }

        internal bool GetVirtualMember( int nameKey, out Value v )
        {
            v = Value.Nil;

            ClassType ct = this;

            while( ct != null )
            {                
                if (ct._member.TryGetValue(nameKey, out v))
                {
                    return true;
                }

                ct = ct.Parent;
            }

            return false;
        }


        public override string ToString()
        {
            return string.Format("{0}", _name);
        }
    }
}
