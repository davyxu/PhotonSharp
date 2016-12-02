

using SharpLexer;
namespace Photon
{
    public class RuntimeFrame
    {
        public int PC;

        internal ValuePhoFunc CmdSet;

        internal int DataStackBase;

        // 结束运行后, 需要恢复数据栈
        internal bool RestoreDataStack;

        internal ValueClosure Closure;

        internal Register Reg = new Register("R", 10);

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
            CmdSet = cs; 
        }

        internal Command GetCurrCommand()
        {
            if (PC >= CmdSet.Commands.Count || PC < 0)
            {
                return null;
            }

            return CmdSet.Commands[PC];
        }

        public string DebugString()
        {
            return string.Format("{0} {1}", CmdSet.CodePos, CmdSet.Name );
        }

        public override string ToString()
        {
            return DebugString();
        }
    }
}
