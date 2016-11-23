
using SharpLexer;
namespace Photon
{
    class ValueFunc : Value
    {
        protected Procedure _proc;        

        internal ValueFunc( Procedure p)
        {
            _proc = p;            
        }

        internal Procedure Proc
        {
            get { return _proc; }
        }

        internal override bool Equal(Value other)
        {
            var otherT = other as ValueFunc;
            if (otherT == null)
                return false;

            return otherT._proc == _proc;
        }

        public override string DebugString()
        {
            return string.Format("{0} (proc)", _proc.ToString());
        }

        public override string ToString()
        {
            return DebugString();
        }

        public override ValueType GetValueType()
        {
            return ValueType.Func;
        }
    }

 
}
