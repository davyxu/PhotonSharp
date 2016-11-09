
namespace Photon.Model
{

    public class Command
    {
        public Opcode Op;

        public int DataA;
        public int DataB;

        int _dataCount;

        string _comment;

        public Command(Opcode op)
        {
            Op = op;
            _dataCount = 0;
        }

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        public Command(Opcode op, int data)
        {
            Op = op;
            DataA = data;
            _dataCount = 1;
        }

        public Command(Opcode op, int dataA, int dataB)
        {
            Op = op;
            DataA = dataA;
            DataB = dataB;
            _dataCount = 2;
        }

        public override string ToString()
        {
            string CommentData = string.Empty;
            if ( !string.IsNullOrEmpty(Comment) )
            {
                CommentData = "; " + Comment;
            }


            if (_dataCount == 1 )
            {
                return string.Format("{0} {1} {2}", Op.ToString(), DataA.ToString(), CommentData);
            }
            else if (_dataCount == 2)
            {
                return string.Format("{0} {1} {2} {3}", Op.ToString(), DataA.ToString(), DataB.ToString(), CommentData);
            }

            return string.Format("{0} {1}", Op.ToString(), CommentData);
            
        }
    }

}
