using SharpLexer;
using System.Collections.Generic;
using System.Diagnostics;

namespace Photon
{
    // 外部函数和内建函数统称过程
    public class Procedure
    {
        int _id;

        string _name;

        public int ID
        {
            get { return _id; }
            internal set { _id = value; }
        }

        internal Package Pkg
        {
            get;
            set;
        }


        public string Name
        {
            get { return _name; }
        }

        internal Procedure( string name )
        {
            _name = name;
        }
    }


    class Delegate : Procedure
    {
        internal Delegate( string name )
            : base( name )
        {

        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }


    public class CommandSet : Procedure
    {
        List<Command> _cmds = new List<Command>();

        int _regCount;

        bool _isGlobal;

        TokenPos _codePos;

        internal CommandSet(string name, TokenPos codepos, int regCount, bool isGlobal)
            : base(name)
        {            
            _regCount = regCount;
            _isGlobal = isGlobal;
            _codePos = codepos;            
        }

        internal bool IsGlobal
        {
            get { return _isGlobal; }
        }

        public int RegCount
        {
            get { return _regCount; }
        }

        public List<Command> Commands
        {
            get { return _cmds; }
        }

        internal Command Add(Command c)
        {
            c.Pkg = Pkg;            
            _cmds.Add(c);            
            return c;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Name, _codePos);
        }

        internal int CurrCmdID
        {
            get { return _cmds.Count; }
        }

        // 传入参数数量
        internal int InputValueCount { get; set; }

        // 返回值数量
        internal int OutputValueCount { get; set; }


        public void DebugPrint( )
        {
            Debug.WriteLine("[{0}] locals: {1}", Name, RegCount);

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
                    Debug.WriteLine("{0}|{1}", currLine, Pkg.QuerySourceLine(c.CodePos));
                }

                // 显示汇编
                Debug.WriteLine("{0,2}| {1}", index, c.ToString());
                index++;
            }

            Debug.WriteLine("");
        }
    }


}
