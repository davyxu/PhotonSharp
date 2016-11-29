
namespace Photon
{
    public class RuntimePackage
    {
        public Register Reg= new Register("G", 10);

        public ConstantSet Constants;

        string _name;

        public string Name
        {
            get { return _name; }
        }

        internal RuntimePackage( Package pkg )
        {
            _name = pkg.Name;
            Constants = pkg.Constants;
        }
    }
}
