using SharpLexer;
using System.Text;

namespace Photon
{

    public class Command
    {
        public Opcode Op;

        const int MaxDataCount = 2;

        int[] _data = new int[MaxDataCount];
        bool[] _dataUsed = new bool[MaxDataCount];


        string _comment;

        TokenPos _pos;
        
        internal Package Pkg
        {
            get;
            set;
        }

        internal int DataA
        {
            get
            {
                return _data[0];
            }

            set
            {
                _data[0] = value;
                _dataUsed[0] = true;
            }
        }

        internal int DataB
        {
            get
            {
                return _data[1];
            }

            set
            {
                _data[1] = value;
                _dataUsed[1] = true;
            }
        }

        internal int UsedDataCount
        {
            get {

                int ret = 0;
                foreach( var used in _dataUsed )
                {
                    if ( used )
                    {
                        ret++;
                    }
                }

                return ret;

            }
        }

        public Command(Opcode op)
        {
            Op = op;
        }

        public string Comment
        {
            get { return _comment; }            
        }

        public TokenPos CodePos
        {
            get { return _pos; }
        }

        public Command(Opcode op, int data)
        {
            Op = op;
            DataA = data;            
        }

        public Command(Opcode op, int dataA, int dataB)
        {
            Op = op;
            DataA = dataA;
            DataB = dataB;            
        }

        public Command SetComment( string text )
        {
            _comment = text;

            return this;
        }

        public Command SetCodePos(TokenPos pos)
        {
            _pos = pos;
            return this;
        }

        public override string ToString()
        {

            var sb = new StringBuilder();

            sb.Append(Op);
            sb.Append(" ");

            var usedCount = UsedDataCount;


            if (usedCount >= 1)
            {
                sb.Append(DataA);
                sb.Append(" ");
            }

            if (usedCount >= 2)
            {
                sb.Append(DataB);
                sb.Append(" ");
            }
           
            if ( !string.IsNullOrEmpty(Comment) )
            {
                sb.Append("; ");
                sb.Append(Comment);
                sb.Append(" ");
            }

            return sb.ToString();
        }
    }

}
