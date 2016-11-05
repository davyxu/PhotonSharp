using Photon.Model;

namespace Photon.VM
{
    public class RuntimeFrame
    {
        public int PC;

        public Register Reg;

        public CommandSet CmdSet;

        public int DataStackBase;

        // 结束运行后, 需要恢复数据栈
        public bool RestoreDataStack;


        public RuntimeFrame(CommandSet cs)
        {
            Reg = new Register("reg",10);
            CmdSet = cs; 
        }        

        public override string ToString()
        {
            return string.Format("pc:{0}", PC);
        }
    }
}
