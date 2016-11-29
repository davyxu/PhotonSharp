
using System;
using System.Reflection;

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


    class InstructionSet
    {
        // 指令集
        Instruction[] _instruction = new Instruction[(int)Opcode.MAX];

        internal InstructionSet(VMachine vm)
        {
            var ass = Assembly.GetExecutingAssembly();

            foreach (var t in ass.GetTypes())
            {
                var att = t.GetCustomAttribute<InstructionAttribute>();
                if (att == null)
                    continue;

                var cmd = Activator.CreateInstance(t) as Instruction;
                cmd.vm = vm;
                _instruction[(int)att.Cmd] = cmd;
            }
        }

        internal string InstructToString(Command cmd)
        {
            var inc = _instruction[(int)cmd.Op];

            if (inc == null)
            {
                return string.Empty;
            }

            return inc.Print(cmd);
        }

        internal bool ExecCode(Command cmd)
        {
            var inc = _instruction[(int)cmd.Op];

            if (inc == null)
            {
                throw new RuntimeException("invalid instruction");
            }


            return inc.Execute(cmd);
        }
    }


}
