
using SharpLexer;
namespace Photon.Model
{
    public class ValueFunc : Value
    {
        int _index = 0;  // 相对于Executable的索引
        TokenPos _pos;

        public ValueFunc( int index, TokenPos pos )
        {
            _index = index;
            _pos = pos;
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
