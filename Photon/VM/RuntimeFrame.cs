

namespace Photon
{
    public class RuntimeFrame
    {
        public int PC;

        public CommandSet CmdSet;

        internal int DataStackBase;

        // 结束运行后, 需要恢复数据栈
        internal bool RestoreDataStack;

        internal ValueClosure Closure;

        internal Register Reg = new Register("R", 10);

        public RuntimeFrame(CommandSet cs)
        {            
            CmdSet = cs; 
        }        

        public string DebugString()
        {
            Command cmd = CmdSet.Commands[PC];
            if ( cmd != null )
            {
                return string.Format("{0}", cmd.CodePos);
            }

            return string.Format("{0}", CmdSet.Name);
        }

        public override string ToString()
        {
            return DebugString();
        }
    }
}
