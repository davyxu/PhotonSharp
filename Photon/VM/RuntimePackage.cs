
namespace Photon
{
    public partial class RuntimePackage
    {
        public Register Reg = new Register("G", 10);        

        Package _pkg;

        public string Name
        {
            get { return _pkg.Name; }
        }

        public override string ToString()
        {
            return _pkg.Name;
        }

        internal RuntimePackage( Package pkg )
        {
            _pkg = pkg;
            Reg.AttachScope(pkg.TopScope);
        }

        internal Value GetRegisterValue(string name)
        {
            if (_pkg == null)
                return Value.Nil;

            var symbol = _pkg.TopScope.FindRegister(name);
            if (symbol == null)
                return Value.Nil;

            return Reg.Get(symbol.RegIndex) as Value;
        }

        void SetRegisterValue(string name, object v)
        {
            if (_pkg == null)
                return;

            var symbol = _pkg.TopScope.FindRegister(name);
            if (symbol == null)
                return;

            Reg.Set(symbol.RegIndex, v as Value);
        }


    }
}
