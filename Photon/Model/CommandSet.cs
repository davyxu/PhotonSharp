using System.Collections.Generic;
using System.Diagnostics;

namespace Photon.Model
{
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

        // 传入参数数量
        public int InputValueCount { get; set; }

        // 返回值数量
        public int OutputValueCount { get; set; }
        

        public void DebugPrint( )
        {
            Debug.WriteLine( _name);

            int index = 0;
            foreach( var c in _cmds )
            {
                Debug.WriteLine("{0,2}| {1}", index, c.ToString());
                index++;
            }

            Debug.WriteLine("");
        }
    }


}
