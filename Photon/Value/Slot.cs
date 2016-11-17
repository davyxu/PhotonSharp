
namespace Photon
{

    public class Slot
    {
        Value _data = Value.Nil;

        public int ID;

        public Slot(int id)
        {
            ID = id;
        }

        public Value Data
        {
            get { return _data; }
        }

        public void SetData(Value d)
        {
            if (ID == 14)
            {
                int a = 1;
            }

            _data = d;
        }

        public override string ToString()
        {
            return string.Format("{0} ID:{1}", Data.ToString(), ID);
        }
    }

}
