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

    public enum ScopeType
    {
        Global,
        Function,
    }

    public class Scope
    {
        Scope _outter;
        ScopeType _type;
        int _regbase;
        
        Dictionary<string, ScopeMeta> _objects = new Dictionary<string, ScopeMeta>();

        public Scope(Scope outter, ScopeType type)
        {
            _outter = outter;

            _type = type;
        }

        public int Index { get; set; }

        public Scope Outter
        {
            get { return _outter; }
        }

        public int RegBase
        {
            get { return _regbase; }
        }

        public void BuildRegbase( )
        {
            _regbase = GetRegBase( _outter );
        }

        static int GetRegBase( Scope s )
        {
            if (s != null )
            {
                if (s._type == ScopeType.Function || s._type == ScopeType.Global)
                {
                    return s.AllocatedReg;
                }


                return GetRegBase(s.Outter);
            }

            return 0;
        }

        public override string ToString()
        {
            return _type.ToString();
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

    public class ScopeSet
    {
        List<Scope> _scopeset = new List<Scope>();

        public Scope Add( Scope s )
        {
            s.Index = _scopeset.Count;

            _scopeset.Add(s);

            return s;
        }

        public void BuildRegbase( )
        {
            foreach( var s in _scopeset )
            {
                s.BuildRegbase();
            }
        }

        public Scope Get(int index)
        {
            return _scopeset[index];
        }
    }
}
