using MarkSerializer;
using System.Collections.Generic;

namespace Photon
{
    class ValueArrayIterator : Value
    {
        IEnumerator<Value> _data;
        int index;

        public ValueArrayIterator( Array arr )
        {
            _data = arr.Raw.GetEnumerator();
            index = 0;
            _data.MoveNext();
        }

        public void Next( )
        {
            _data.MoveNext();
            index++;
        }

        // k, v, iter = ITER( x, iter )
        public bool Iterate(DataStack ds)
        {
            if (_data.Current == null)
                return false;

            ds.Push(this); 
            ds.Push(_data.Current);
            ds.Push(new ValueInteger32(index));

            return true;
        }

        public override void Serialize(BinarySerializer ser)
        {
            throw new RuntimeException("Can not serialize iterator");
        }

        public IEnumerator<Value> RawValue
        {
            get { return _data; }
        }

        internal override object Raw
        {
            get { return _data; }
        }        

        public override bool Equals(object other)
        {
            var otherT = other as ValueArrayIterator;
            if (otherT == null)
                return false;

            return otherT._data.Equals(_data);
        }

        public override int GetHashCode()
        {
            return _data.GetHashCode();
        }

        public override string DebugString()
        {
            return string.Format("'{0}' (arr iterator)", _data);
        }

        public override ValueKind Kind
        {
            get { return ValueKind.ArrayIterator; }
        }
    }



 
}
