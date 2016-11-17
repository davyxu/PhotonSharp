
using System;

namespace Photon
{
    class InstructionAttribute : Attribute
    {
        public Opcode Cmd
        {
            get;
            set;
        }
    }


    class Instruction
    {
        public VMachine vm;

        public virtual bool Execute( Command cmd )
        {
            // true表示下一条指令
            return true;
        }

        public virtual string Print( Command cmd )
        {
            return string.Empty;
        }
    }


}
