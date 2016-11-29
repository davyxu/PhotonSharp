
namespace Photon
{

    class Delegate : Procedure
    {
        public DelegateEntry Entry;

        internal Delegate(ProcedureName name)
            : base( name )
        {
            
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }

}
