using System.Collections.Generic;
using System.Collections;
using SharpLexer;
using System.Diagnostics;

namespace Photon.AST
{
    public class Symbol
    {
        public string Name;
        public TokenPos DefinePos;
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

        public override string ToString()
        {
            //if ( IsGlobal )
            //{
            //    return string.Format("{0} G{1}  {2}", Name, RegIndex, DefinePos);
            //}
            //else
            {
                return string.Format("{0} R{1}  {2}", Name, RegIndex, DefinePos);
            }
            
        }
    }

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
        
        Dictionary<string, Symbol> _symbolByName = new Dictionary<string, Symbol>();

        List<Scope> _child = new List<Scope>();

        public Scope(Scope outter, ScopeType type)
        {
            if (outter != null )
            {
                outter._child.Add(this);
            }
            
            _outter = outter;

            _type = type;
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
            return _type.ToString();
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

            if ( s.Type != ScopeType.Function )
            {
                do
                {


                    s = s.Outter;

                    if (s == null)
                    {
                        break;
                    }


                    regBase += s.SymbolCount;

                } while (s.Type != ScopeType.Function);
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
