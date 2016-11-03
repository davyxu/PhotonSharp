using System.Collections.Generic;
using System.Collections;

namespace Photon.AST
{
    public class Symbol
    {
        public string Name;
        public Node Decl;
        public int RegIndex;

        public Scope Belong;

        public bool IsGlobal
        {
            get
            {
                return Belong.Type == ScopeType.Global;
            }
        }
    }

    public enum ScopeType
    {
        Global,
        Function,
        Block,
        For,
    }

    public class Scope
    {
        Scope outter;
        ScopeType type;        
        
        Dictionary<string, Symbol> symbolByName = new Dictionary<string, Symbol>();

        public Scope(Scope outter, ScopeType type)
        {
            this.outter = outter;

            this.type = type;
        }

        public ScopeType Type
        {
            get { return this.type; }
        }

        public Scope Outter
        {
            get { return outter; }
        }

        public override string ToString()
        {
            return type.ToString();
        }

        public Symbol Lookup( string name ) 
        {
           Symbol ret;
           if ( symbolByName.TryGetValue( name, out ret ) )
           {
               return ret;
           }

           return null;
        }

        public int RegBase
        {
            get{

                if (outter != null)
                {
                    return outter.RegBase;
                }

                return symbolByName.Count;
            }
           
        }


        public void Insert( Symbol data )
        {
            data.RegIndex = symbolByName.Count;
            data.Belong = this;

            symbolByName.Add(data.Name, data);
        }
    }
}
