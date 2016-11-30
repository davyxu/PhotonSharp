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

        Dictionary<int, Procedure> _methods = new Dictionary<int, Procedure>();

        Dictionary<int, string> _memberVar = new Dictionary<int, string>();

        Executable _exe;
        ObjectName _name;

        internal ClassType(Executable exe, ObjectName name)
        {
            _exe = exe;
            _name = name;
        }

        internal bool HasCtor
        {
            get { return false; }
        }

        internal void AddMethod( int nameKey, Procedure proc )
        {           
            _methods.Add(nameKey, proc);
        }

        internal void AddMemeber( int nameKey, string name )
        {
            _memberVar.Add(nameKey, name);
        }

        public override string ToString()
        {
            return string.Format("{0}", _name);
        }
    }
}
