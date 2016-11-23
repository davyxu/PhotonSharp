

namespace Photon
{
    public class RuntimeFrame
    {
        public int PC;

        public CommandSet CmdSet;

        public int DataStackBase;

        // 结束运行后, 需要恢复数据栈
        public bool RestoreDataStack;

        public ValueClosure Closure;

        public RuntimeFrame(CommandSet cs)
        {            
            CmdSet = cs; 
        }        

        public override string ToString()
        {
            Command cmd = CmdSet.Commands[PC];
            if ( cmd != null )
            {
                return string.Format("{0}", cmd.CodePos);
            }

            return string.Format("{0}", CmdSet.Name);

            
        }
    }
}
