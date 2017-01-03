using MarkSerializer;

namespace Photon
{
    class ValueIterator : Value
    {
        internal virtual void Next( )
        {

        }

        internal virtual bool Iterate(DataStack ds)
        {
            return false;
        }

        public override void Serialize(BinarySerializer ser)
        {
            throw new RuntimeException("Can not serialize iterator");
        }
    }
}
