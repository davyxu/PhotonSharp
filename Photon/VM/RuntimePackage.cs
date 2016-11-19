
namespace Photon
{
    public class RuntimePackage
    {
        public Register Reg= new Register("G", 10);

        string _name;
        public string Name
        {
            get { return _name; }
        }

        internal RuntimePackage( string name )
        {
            _name = name;
        }
    }
}
