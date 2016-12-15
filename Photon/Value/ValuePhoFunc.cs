using SharpLexer;
using System.Collections.Generic;

namespace Photon
{

    class ValuePhoFunc : ValueFunc
    {
        List<Command> _cmds = new List<Command>();

        internal static ValuePhoFunc Empty = new ValuePhoFunc();

        int _regCount;

        Scope _scope;

        TokenPos _defpos;

        // 序列化用, 不要删除
        public ValuePhoFunc()
        {

        }

        internal ValuePhoFunc(ObjectName name, TokenPos codepos, int regCount, Scope s)
            : base(name)
        {            
            _regCount = regCount;
            _scope = s;
            _defpos = codepos;            
        }


        public override void Serialize(BinarySerializer ser)
        {
            base.Serialize(ser);

            ser
                .Serialize(ref _regCount)
                .Serialize(ref _cmds);
        }

        internal Scope Scope
        {
            get { return _scope; }
        }

        public int RegCount
        {
            get { return _regCount; }
        }

        internal TokenPos DefPos
        {
            get { return _defpos; }
        }

        public List<Command> Commands
        {
            get { return _cmds; }
        }

        internal Command Add(Command c)
        {            
            _cmds.Add(c);            
            return c;
        }

        public override string DebugString()
        {
            return string.Format("{0} {1} id: {2} regs: {3}", Name, _defpos, ID, RegCount);
        }

        internal int CurrCmdID
        {
            get { return _cmds.Count; }
        }

        public override bool Equals(object other)
        {
            var otherT = other as ValuePhoFunc;
            if (otherT == null)
                return false;

            return otherT._cmds.Equals( _cmds );
        }

        public override int GetHashCode()
        {
            return _cmds.GetHashCode();
        }

        internal override bool Invoke(VMachine vm, int argCount, int receiverCount, ValueClosure closure)
        {
            // 更换当前上下文
            vm.EnterFrame(this);

            vm.CurrFrame.ReceiverCount = receiverCount;
            vm.CurrFrame.Closure = closure;

            vm.MoveArgStack2Local(argCount);

            // 清空栈
            vm.DataStack.PopMulti(argCount);

            // 记录当前的数据栈位置
            vm.CurrFrame.DataStackBase = vm.DataStack.Count;

            return false;
        }

        internal override void DebugPrint( Executable exe)
        {            
            int index = 0;

            int currLine = 0;

            foreach (var c in _cmds)
            {                
                // 到新的源码行
                if (c.CodePos.Line > currLine )
                {
                    // 每个源码下的汇编成为一块, 块与块之间空行
                    if ( currLine != 0 )
                    {
                        Logger.DebugLine("");
                    }
                    
                    currLine = c.CodePos.Line;

                    // 显示源码
                    Logger.DebugLine("{0}|{1}", currLine, exe.QuerySourceLine(c.CodePos));
                }

                // 显示汇编
                Logger.DebugLine("{0,2}| {1}", index, c.ToString());
                index++;
            }

            Logger.DebugLine("");
        }
    }


}
