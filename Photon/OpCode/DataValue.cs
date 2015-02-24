
namespace Photon.OpCode
{
    public class DataValue
    {
        public virtual bool Equal( DataValue other )
        {
            return false;
        }

        public virtual string GetDesc()
        {
            return "unknown";
        }
    }


    public class NumberValue : DataValue
    {
        float _number = 0;

        public NumberValue( float number )
        {
            _number = number;
        }

        public float Number
        {
            get { return _number; }
        }

        public override bool Equal(DataValue other)
        {
            var otherT = other as NumberValue;
            if (otherT == null)
                return false;

            return otherT._number == _number;
        }

        public override string ToString()
        {
            return _number.ToString();
        }

        public override string GetDesc()
        {
            return _number.ToString() + "|number";
        }
    }

    public class FuncValue : DataValue
    {
        int _index = 0;  // 相对于Executable的索引

        public FuncValue( int index )
        {
            _index = index;
        }

        public int Index
        {
            get { return _index;  }
        }

        public override bool Equal(DataValue other)
        {
            var otherT = other as FuncValue;
            if (otherT == null)
                return false;

            return otherT._index == _index;
        }

        public override string ToString()
        {
            return _index.ToString();
        }

        public override string GetDesc()
        {
            return _index.ToString() + "|func";
        }
    }

 
}
