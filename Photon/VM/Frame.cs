using Photon.OpCode;

namespace Photon.VM
{
    struct Frame
    {
        public int PC;
        public int RegBase;
        public CommandSet CmdSet;
        public void Reset(CommandSet cs, int regbase)
        {
            PC = 0;
            CmdSet = cs;
            RegBase = regbase;
        }

        public override string ToString()
        {
            return string.Format("pc:{0}", PC);
        }
    }
}
