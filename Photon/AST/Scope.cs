using SharpLexer;
using System.Collections.Generic;
using System.Diagnostics;

namespace Photon.AST
{

    public enum ScopeType
    {
        Global,
        Function,
        Block,
        For,
        Closure,
    }

    public class Scope
    {
        Scope _outter;
        ScopeType _type;
        TokenPos _defpos;

        Dictionary<string, Symbol> _symbolByName = new Dictionary<string, Symbol>();

        List<Scope> _child = new List<Scope>();

        public Scope(Scope outter, ScopeType type, TokenPos pos )
        {
            if (outter != null )
            {
                outter._child.Add(this);
            }
            
            _outter = outter;

            _type = type;

            _defpos = pos;
        }

        public ScopeType Type
        {
            get { return this._type; }
        }

        public Scope Outter
        {
            get { return _outter; }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", _type.ToString(), _defpos );
        }

        public Symbol FindSymbol( string name ) 
        {
           Symbol ret;
           if ( _symbolByName.TryGetValue( name, out ret ) )
           {
               return ret;
           }

           return null;
        }

        public int SymbolCount
        {
            get{                

                return _symbolByName.Count;
            }
           
        }


        public int CalcRegBase()
        {
            int regBase = this.SymbolCount;

            Scope s = this;

            // 函数只从自己开始算regbase
            // 其他作用域向上查到函数

            if (s.Type != ScopeType.Function && s.Type != ScopeType.Closure)
            {
                do
                {


                    s = s.Outter;

                    if (s == null)
                    {
                        break;
                    }


                    regBase += s.SymbolCount;

                } while (s.Type != ScopeType.Function && s.Type != ScopeType.Closure);
            }


            return regBase;
        }

        public void DebugPrint( string indent )
        {
            Debug.WriteLine(indent + _type.ToString());

            foreach( var kv in _symbolByName )
            {
                Debug.WriteLine(string.Format("{0} {1}", indent,kv.Value ));                
            }


            foreach( var c in _child )
            {
                c.DebugPrint(indent + "\t");
            }
        }

        public void Insert( Symbol data )
        {
            data.RegIndex = CalcRegBase();
            data.Belong = this;

            _symbolByName.Add(data.Name, data);
        }
    }
}
