using Photon.AST;
using System.Collections.Generic;
using System.Diagnostics;

namespace Photon.Model
{

    public class Executable
    {
        List<CommandSet> _cmdset = new List<CommandSet>();
        Dictionary<string, ValueDelegate> _delegateByName = new Dictionary<string, ValueDelegate>();
        ConstantSet _constSet = new ConstantSet();

        public List<CommandSet> CmdSet
        {
            get { return _cmdset; }
        }

        public Dictionary<string, ValueDelegate> DelegateMap
        {
            get { return _delegateByName; }
        }

        public ConstantSet Constants
        {
            get { return _constSet; }
        }

        public void DebugPrint( )
        {
            _constSet.DebugPrint(  );

            foreach( var cs in _cmdset )
            {
                cs.DebugPrint();
            }
            Debug.WriteLine("");
        }

        public int AddCmdSet(CommandSet f)
        {
            _cmdset.Add(f);

            return _cmdset.Count - 1;
        }

        public CommandSet GetCmdSet(int index )
        {
            return _cmdset[index];
        }
        
        
    }
}
