using SharpLexer;
using System.Text;

namespace Photon
{

    public class Command
    {
        public Opcode Op;

        public int DataA;
        public int DataB;

        int _dataCount;

        string _comment;

        TokenPos _pos;

        public Command(Opcode op)
        {
            Op = op;
            _dataCount = 0;
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
            _dataCount = 1;
        }

        public Command(Opcode op, int dataA, int dataB)
        {
            Op = op;
            DataA = dataA;
            DataB = dataB;
            _dataCount = 2;
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
            

            if (_dataCount >= 1 )
            {
                sb.Append(DataA);
                sb.Append(" ");
            }

            if (_dataCount >= 2)
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
