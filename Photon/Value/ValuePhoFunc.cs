using SharpLexer;
using System.Collections.Generic;
using System.Diagnostics;

namespace Photon
{

    class ValuePhoFunc : ValueFunc
    {
        List<Command> _cmds = new List<Command>();

        int _regCount;

        Scope _scope;

        TokenPos _defpos;

        internal ValuePhoFunc(ObjectName name, TokenPos codepos, int regCount, Scope s)
            : base(name)
        {            
            _regCount = regCount;
            _scope = s;
            _defpos = codepos;            
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
            return string.Format("{0} {1}", Name, _defpos);
        }

        internal int CurrCmdID
        {
            get { return _cmds.Count; }
        }

        internal override bool Invoke(VMachine vm, int argCount, int receiverCount, ValueClosure closure)
        {
            // 更换当前上下文
            vm.EnterFrame(this);

            vm.CurrFrame.ReceiverCount = receiverCount;
            vm.CurrFrame.Closure = closure;

            // 将栈转为被调用函数的寄存器
            for (int i = 0; i < argCount; i++)
            {
                var arg = vm.DataStack.Get(-i - 1);
                vm.LocalReg.Set(argCount - i - 1 + vm.RegBase, arg);
            }

            // 清空栈
            vm.DataStack.PopMulti(argCount);

            // 记录当前的数据栈位置
            vm.CurrFrame.DataStackBase = vm.DataStack.Count;

            return false;
        }

        public void DebugPrint( Executable exe)
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
                        Debug.WriteLine("");
                    }
                    
                    currLine = c.CodePos.Line;

                    // 显示源码
                    Debug.WriteLine("{0}|{1}", currLine, exe.QuerySourceLine(c.CodePos));
                }

                // 显示汇编
                Debug.WriteLine("{0,2}| {1}", index, c.ToString());
                index++;
            }

            Debug.WriteLine("");
        }
    }


}
