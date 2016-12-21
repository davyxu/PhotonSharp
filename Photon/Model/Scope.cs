using MarkSerializer;
using SharpLexer;
using System;
using System.Collections.Generic;

namespace Photon
{

    enum ScopeType
    {
        None,
        Package,
        Function,
        Block,
        For,
        Closure,
        Class,
    }

    class Scope : IMarkSerializable
    {
        Scope _outter;
        
        ScopeType _type;
        
        TokenPos _defpos;
       
        public string ClassName;

        Dictionary<string, Symbol> _symbolByName = new Dictionary<string, Symbol>();

        Dictionary<string, Symbol> _regByName = new Dictionary<string, Symbol>();

        List<Scope> _child = new List<Scope>();


        public void Serialize(BinarySerializer ser)
        {
            ser.Serialize(ref _type);
            ser.Serialize(ref _defpos);            
            ser.Serialize(ref _regByName);
            ser.Serialize(ref _symbolByName);// 只是为了调试
            ser.Serialize(ref _child);
        }

        internal List<Scope> Child
        {
            get { return _child; }
        }

        internal TokenPos CodePos
        {
            get { return _defpos; }
            set { _defpos = value; }
        }

        public Scope()
        {

        }

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
            return string.Format("{0} reg: {1} symbol: {2}  {3}", _type.ToString(), _regByName.Count, _symbolByName.Count, _defpos );
        }

        internal Symbol FindRegister(string name)
        {
            Symbol ret;
            if (_regByName.TryGetValue(name, out ret))
            {
                return ret;
            }

            return null;
        }

        internal Symbol FindRegisterByIndex(int regIndex )
        {
            foreach (var reg in _regByName)
            {
                if (reg.Value.RegIndex == regIndex)
                    return reg.Value;
            }

            return null;
        }

        internal Symbol FindSymbol(string name) 
        {
           Symbol ret;
           if ( _symbolByName.TryGetValue( name, out ret ) )
           {
               return ret;
           }

           return null;
        }

        internal Symbol FindSymbolOutter( string name )
        {
            Scope s = this;
            while (s != null)
            {
                var symbol = s.FindSymbol(name);
                if (symbol != null)
                {
                    return symbol;
                }

                s = s.Outter;
            }

            return null;
        }

        public int RegCount
        {
            get{return _regByName.Count; }
           
        }

        public int CalcUsedReg()
        {
            return RawCalcUsedReg( this );
        }

        // 计算某个作用域使用到的重叠寄存器量( 方向向下 )
        int RawCalcUsedReg( Scope s  )
        {
            int maxReg = 0;

            foreach( var c in s._child )
            {
                if (c.Type == ScopeType.Function || c.Type == ScopeType.Closure)
                {
                    continue;
                }

                // 递归同层取最大的
                maxReg = Math.Max(maxReg, RawCalcUsedReg(c));
            }

            return maxReg + s.RegCount;
        }

        // 计算symbol应该分配的寄存器base( 方向向上 )
        public int CalcRegBase()
        {
            int regBase = this.RegCount;

            Scope s = this;

            // 函数只从自己开始算regbase
            // 其他作用域向上查到函数

            while (s.Type != ScopeType.Function && s.Type != ScopeType.Closure)
            {
                s = s.Outter;

                if (s == null)
                {
                    break;
                }

                regBase += s.RegCount;
            }

            return regBase;
        }

        public void DebugPrint( string indent )
        {
            if (string.IsNullOrEmpty(ClassName))
            {
                Logger.DebugLine(indent + _type.ToString());
            }
            else
            {
                Logger.DebugLine(string.Format("{0}{1} '{2}'",indent, _type.ToString(), ClassName));
            }
            

            foreach( var kv in _symbolByName )
            {
                Logger.DebugLine(string.Format("{0} {1}", indent,kv.Value ));                
            }


            foreach( var c in _child )
            {
                c.DebugPrint(indent + "\t");
            }
        }
        

        static bool NeedAllocReg( SymbolUsage usage )
        {
            switch( usage )
            {
                case SymbolUsage.Parameter:
                case SymbolUsage.SelfParameter:
                case SymbolUsage.Variable:
                    return true;
            }

            return false;
        }

        // 找到能分配寄存器的scope
        static Scope FoundRegAllocableScope( Scope s )
        {
            Scope regBound = s;
            while (regBound.Type != ScopeType.Package &&
                 regBound.Type != ScopeType.Function &&
                 regBound.Type != ScopeType.Closure)
            {

                regBound = regBound.Outter;
            }

            return regBound;
        }

        internal void Insert(Symbol symbol)
        {

            if (NeedAllocReg(symbol.Usage))
            {
                var regBound = FoundRegAllocableScope(this);

                symbol.RegIndex = regBound.RegCount;

                symbol.RegBelong = regBound;

                regBound._regByName.Add(symbol.Name, symbol);
            }
            else
            {
                symbol.RegIndex = -1;
            }
            
            

            _symbolByName.Add(symbol.Name, symbol);
        }
    }
}
