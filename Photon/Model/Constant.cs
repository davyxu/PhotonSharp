using System.Collections.Generic;
using System.Diagnostics;

namespace Photon.Model
{
    public class ConstantSet
    {
        List<Value> _cset = new List<Value>();

        public int Add(Value inc)
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

        public Value Get(int index)
        {
            return _cset[index];
        }
        public string ValueToString(int index)
        {
            var v = Get(index);
            if (v == null)
            {
                return "null";
            }

            return v.ToString();
        }

        public void DebugPrint( )
        {
            Debug.WriteLine("constant:");

            int index = 0;
            foreach (var c in _cset)
            {
                Debug.WriteLine("C[{0}]={1}", index, c.ToString());
                index++;
            }

            Debug.WriteLine("");
        }
    }
}
