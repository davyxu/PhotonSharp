
namespace Photon.Model
{
    public class ValueFunc : Value
    {
        int _index = 0;  // 相对于Executable的索引

        public ValueFunc( int index )
        {
            _index = index;
        }

        public int Index
        {
            get { return _index;  }
        }

        public override bool Equal(Value other)
        {
            var otherT = other as ValueFunc;
            if (otherT == null)
                return false;

            return otherT._index == _index;
        }


        public override string ToString()
        {
            return _index.ToString() + "(func)";
        }
    }

 
}
