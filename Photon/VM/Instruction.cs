
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
        public virtual bool Execute( VMachine vm, Command cmd )
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

        internal static T GetCustomAttribute<T>(Type type) where T : class
        {
            object[] objs = type.GetCustomAttributes(typeof(T), false);
            if (objs.Length > 0)
                return (T)objs[0];

            return null;
        }

        internal InstructionSet(VMachine vm)
        {
            var ass = Assembly.GetExecutingAssembly();

            foreach (var t in ass.GetTypes())
            {
                var att = GetCustomAttribute<InstructionAttribute>(t);
                if (att == null)
                    continue;

                var cmd = Activator.CreateInstance(t) as Instruction;                
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

        internal bool ExecCode(VMachine vm, Command cmd)
        {
            var inc = _instruction[(int)cmd.Op];

            if (inc == null)
            {
                throw new RuntimeException("invalid instruction");
            }


            return inc.Execute(vm,cmd);
        }
    }


}
