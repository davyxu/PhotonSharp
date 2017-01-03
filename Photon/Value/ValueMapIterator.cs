using MarkSerializer;
using System.Collections.Generic;

namespace Photon
{
    class ValueMapIterator : ValueIterator
    {
        IEnumerator<KeyValuePair<Value, Value>> _data;
        static KeyValuePair<Value, Value> nullKV = new KeyValuePair<Value, Value>();

        public ValueMapIterator(Map arr)
        {
            _data = arr.Raw.GetEnumerator();
            _data.MoveNext();
        }

        internal override void Next()
        {
            _data.MoveNext();
        }

        // k, v, iter = ITER( x, iter )
        internal override bool Iterate(DataStack ds)
        {
            if (_data.Current.Equals(nullKV))
                return false;

            ds.Push(this);
            var kv = _data.Current;
            ds.Push(kv.Value);
            ds.Push(kv.Key);            

            return true;
        }

        public IEnumerator<KeyValuePair<Value, Value>> RawValue
        {
            get { return _data; }
        }

        internal override object Raw
        {
            get { return _data; }
        }        

        public override bool Equals(object other)
        {
            var otherT = other as ValueMapIterator;
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
            return string.Format("'{0}' (map iterator)", _data.Current);
        }

        public override ValueKind Kind
        {
            get { return ValueKind.MapIterator; }
        }
    }



 
}
