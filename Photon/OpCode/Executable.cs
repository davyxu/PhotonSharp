using Photon.AST;
using System.Collections.Generic;

namespace Photon.OpCode
{

    public class Executable
    {
        List<CommandSet> _cmdset = new List<CommandSet>();
        ConstantSet _constSet = new ConstantSet();
        ScopeSet _scopeSet;

        public List<CommandSet> CmdSet
        {
            get { return _cmdset; }
        }

        public ConstantSet Constants
        {
            get { return _constSet; }
        }

        public ScopeSet ScopeInfoSet
        {
            get { return _scopeSet; }
            set { _scopeSet = value; }
        }

        
        public void DebugPrint( )
        {
            _constSet.DebugPrint(  );

            foreach( var cs in _cmdset )
            {
                cs.DebugPrint();
            }
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
