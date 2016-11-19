

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
            return string.Format("pc:{0}", PC);
        }
    }
}
