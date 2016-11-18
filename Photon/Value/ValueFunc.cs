
using SharpLexer;
namespace Photon
{
    public class ValueFunc : Value
    {
        protected Procedure _proc;
        protected TokenPos _pos;

        internal ValueFunc( Procedure p, TokenPos pos )
        {
            _proc = p;
            _pos = pos;
        }

        internal Procedure Proc
        {
            get { return _proc; }
        }

        public override bool Equal(Value other)
        {
            var otherT = other as ValueFunc;
            if (otherT == null)
                return false;

            return otherT._proc == _proc;
        }


        public override string ToString()
        {
            return string.Format("{0} (proc) {1}", _proc.ToString(), _pos);
        }
    }

 
}
