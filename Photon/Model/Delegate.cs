
namespace Photon
{

    class Delegate : Procedure
    {
        public DelegateEntry Entry;

        internal Delegate( string name)
            : base( name )
        {
            
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }

}
