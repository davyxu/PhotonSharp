
namespace Photon
{
    public partial class RuntimePackage
    {
        public Register Reg= new Register("G", 10);        

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

        Value GetRegisterValue(string name)
        {
            var symbol = _pkg.TopScope.FindRegister(name);
            if (symbol == null)
                return Value.Nil;

            return Reg.Get(symbol.RegIndex) as Value;
        }


    }
}
