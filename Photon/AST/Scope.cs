using System.Collections.Generic;
using System.Collections;

namespace Photon.AST
{
    public class ScopeMeta
    {
        public string Name;
        public Node Decl;
        public int Slot;

        public Scope Parent;
    }

    public class Scope
    {
        Scope _outter;
        string _name;
        Dictionary<string, ScopeMeta> _objects = new Dictionary<string, ScopeMeta>();

        public Scope( Scope outter, string name )
        {
            _outter = outter;
            _name = name;
        }

        public Scope Outter
        {
            get { return _outter; }
        }

        public ScopeMeta Lookup( string name ) 
        {
           ScopeMeta ret;
           if ( _objects.TryGetValue( name, out ret ) )
           {
               return ret;
           }

           return null;
        }

        public int AllocatedReg
        {
            get { return _objects.Count; }
        }

        public void Insert( ScopeMeta data )
        {
            data.Slot = _objects.Count;
            data.Parent = this;

            _objects.Add(data.Name, data);
        }
    }
}
