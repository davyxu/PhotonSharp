
using SharpLexer;
namespace Photon
{
    public class ValueFunc : Value
    {
        protected int _index = 0;  // 相对于Executable的索引
        protected TokenPos _pos;

        public ValueFunc( int index, TokenPos pos )
        {
            _index = index;
            _pos = pos;
        }

        public ValueFunc( ValueFunc f )
        {
            _index = f._index;
            _pos = f._pos;
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
            return string.Format("{0} (func) {1}", _index.ToString(), _pos);
        }
    }

 
}
