using SharpLexer;
using System.Collections.Generic;

namespace Photon
{
    public class RuntimeFrame
    {
        public int PC;

        internal ValuePhoFunc Func;

        internal int DataStackBase;

        // 函数返回值接收变量数量
        internal int ReceiverCount;

        internal ValueClosure Closure;

        internal Register Reg = new Register("R", 32);

        public int FuncID
        {
            get { return Func.ID; }
        }
        public string FuncName
        {
            get { return Func.Name.ToString(); }
        }


        public TokenPos FuncDefPos
        {
            get { return Func.DefPos; }
        }

        public string PkgName
        {
            get { return Func.Name.PackageName; }
        }

        public List<Command> Commands
        {
            get { return Func.Commands; }
        }

        public TokenPos CodePos
        {
            get {
                var cmd = GetCurrCommand();
                if (cmd == null)
                    return TokenPos.Invalid;

                return cmd.CodePos;
            }
        }

        internal RuntimeFrame(ValuePhoFunc cs)
        {            
            Func = cs; 
        }

        internal Command GetCurrCommand()
        {
            if (PC >= Func.Commands.Count || PC < 0)
            {
                return null;
            }

            return Func.Commands[PC];
        }

        public string DebugString()
        {
            return string.Format("{0} {1}", CodePos, Func.Name);
        }

        public override string ToString()
        {
            return DebugString();
        }
    }
}
