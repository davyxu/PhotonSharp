using System.Collections.Generic;
using System.Collections;

namespace Photon.AST
{
    public class ScopeMeta
    {
        public string Name;
        public Node Decl;
    }

    public class Scope
    {
        Scope _outter;
        Dictionary<string, ScopeMeta> _objects = new Dictionary<string, ScopeMeta>();

        public Scope( Scope outter )
        {
            _outter = outter;
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

        public void Insert( ScopeMeta data )
        {
            _objects.Add(data.Name, data);
        }
    }
}
