using MarkSerializer;
using SharpLexer;
using System.Text;

namespace Photon
{

    public class Command : IMarkSerializable
    {
        internal Opcode Op;

        const int MaxDataCount = 2;

        int[] _data = new int[MaxDataCount];
        bool[] _dataUsed = new bool[MaxDataCount];
        
        string _comment;

        TokenPos _pos;

        // 类名/函数名
        internal ObjectName EntryName;

        public void Serialize(BinarySerializer ser)
        {
            ser.Serialize(ref Op);
            ser.Serialize(ref _data);
            ser.Serialize(ref _dataUsed);
            ser.Serialize(ref _comment);
            ser.Serialize(ref EntryName);
        }


        internal int DataA
        {
            get
            {
                if (_dataUsed[0])
                    return _data[0];
                else
                    return -1;
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
                if (_dataUsed[1])
                    return _data[1];
                else
                    return -1;
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

        internal Command(Opcode op)
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

        public Command()
        {

        }

        internal Command(Opcode op, int data)
        {
            Op = op;
            DataA = data;            
        }

        internal Command(Opcode op, int dataA, int dataB)
        {
            Op = op;
            DataA = dataA;
            DataB = dataB;            
        }


        internal Command SetComment( string text )
        {
            _comment = text;

            return this;
        }

        internal Command SetCodePos(TokenPos pos)
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
