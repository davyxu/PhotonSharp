using System.Collections.Generic;
using System.Diagnostics;

namespace Photon.OpCode
{

    public class Command
    {
        public Opcode Op;

        public int Data;

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
            Data = data;
            _dataCount = 1;
        }

        public override string ToString()
        {
            if (_dataCount == 1 )
            {
                return string.Format("{0} {1} ; {2}", Op.ToString(), Data.ToString(), Comment );
            }

            return string.Format("{0} ; {1}", Op.ToString(), Comment);
            
        }
    }


    public class CommandSet
    {
        List<Command> _cmds = new List<Command>();

        string _name;
        public CommandSet( string name )
        {
            _name = name;
        }

        public string Name
        {
            get { return _name;  }
        }

        public List<Command> Commands
        {
            get { return _cmds; }
        }

        public Command Add(Command c)
        {
            _cmds.Add(c);
            return c;
        }

        public override string ToString()
        {
            return _name;
        }

        public int CurrGenIndex
        {
            get { return _cmds.Count; }
        }

        public void DebugPrint( )
        {
            int index = 1;
            foreach( var c in _cmds )
            {
                Debug.WriteLine("{0} {1}: {2}", _name, index, c.ToString());
                index++;
            }
        }
    }
}
