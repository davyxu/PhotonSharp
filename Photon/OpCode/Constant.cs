using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.OpCode
{
    public enum ConstantType
    {
        Number,
        String,
        Func,
    }
  
    public class Constant
    {
        public ConstantType Type;
        
        float _number;
        CommandSet _func;

        public Constant( float number)
        {
            Type = ConstantType.Number;
            _number = number;
        }

        public Constant( CommandSet cs )
        {
            Type = ConstantType.Func;
            _func = cs;
        }

        public bool Equal( Constant other )
        {
            if ( other.Type != Type )
                return false;

            return _number == other._number;
        }

        public override string ToString()
        {
            switch( Type )
            {
                case ConstantType.Number:
                    return _number.ToString();
                case ConstantType.String:
                    return "";
                case ConstantType.Func:
                    return "func:" + _func.Name;
            }

            return "unknown";
        }
    }


    public class ConstantSet
    {
        List<Constant> _cset = new List<Constant>();

        public int Add(Constant inc )
        {
            int index = 0;
            foreach( var c in _cset )
            {
                if (c.Equal(inc))
                    return index;

                index++;
            }

            _cset.Add( inc );

            return _cset.Count - 1;
        }

        public Constant Get( int index )
        {
            return _cset[index];
        }

        public void DebugPrint( )
        {
            int index = 0;
            foreach (var c in _cset)
            {
                Debug.WriteLine("C[{0}]={1}", index, c.ToString());
                index++;
            }
        }
    }
}
