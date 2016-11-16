using SharpLexer;
using System.Collections.Generic;
using System.Diagnostics;

namespace Photon.Model
{
    public class CommandSet
    {
        List<Command> _cmds = new List<Command>();

        int _id;

        string _name;

        int _regCount;

        bool _isGlobal;

        TokenPos _codePos;

        public CommandSet( string name, TokenPos codepos, int regCount, bool isGlobal )
        {
            _name = name;
            _regCount = regCount;
            _isGlobal = isGlobal;
            _codePos = codepos;
        }

        public int ID
        {
            get { return _id; }
            internal set { _id = value; }
        }

        public bool IsGlobal
        {
            get { return _isGlobal; }
        }

        public string Name
        {
            get { return _name;  }
        }

        public int RegCount
        {
            get { return _regCount; }
        }

        public List<Command> Commands
        {
            get { return _cmds; }
        }

        public Command Add(Command c)
        {
            _cmds.Add(c);
            return c;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", _name, _codePos);
        }

        public int CurrGenIndex
        {
            get { return _cmds.Count; }
        }

        // 传入参数数量
        public int InputValueCount { get; set; }

        // 返回值数量
        public int OutputValueCount { get; set; }


        public void DebugPrint(SourceFile file)
        {
            Debug.WriteLine("[{0}] locals: {1}", _name, RegCount);

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
                    Debug.WriteLine("{0}|{1}", currLine, file.GetLine(currLine));
                }

                // 显示汇编
                Debug.WriteLine("{0,2}| {1}", index, c.ToString());
                index++;
            }

            Debug.WriteLine("");
        }
    }


}
