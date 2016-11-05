using System.Collections.Generic;
using System.Diagnostics;

namespace Photon.OpCode
{
    public class ConstantSet
    {
        List<DataValue> _cset = new List<DataValue>();

        public int Add(DataValue inc)
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

        public DataValue Get(int index)
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
                Debug.WriteLine("C[{0}]={1}", index, c.GetDesc());
                index++;
            }

            Debug.WriteLine("");
        }
    }
}
