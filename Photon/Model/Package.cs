using System.Collections.Generic;

namespace Photon
{
    public class File
    {
        SourceFile _source;
    }


    public class Package
    {
        string _name;

        int _id;

        List<File> _file = new List<File>();

        // 所有函数执行体
        List<CommandSet> _cmdset = new List<CommandSet>();

        // 常量表
        ConstantSet _constSet = new ConstantSet();

        Executable _parent;

        internal Executable Exe
        {
            get { return _parent; }
        }

        internal ConstantSet Constants
        {
            get { return _constSet; }
        }

        internal int ID
        {
            get { return _id; }
        }


        public string Name
        {
            get { return _name; }
        }

        internal Package( int id, string name, Executable exe )
        {
            _parent = exe;
            _name = name;
            _id = id;
        }

        public override string ToString()
        {
            return _name;
        }

        internal int AddCmdSet(CommandSet f)
        {
            f.Pkg = this;

            _cmdset.Add(f);

            f.ID = _cmdset.Count - 1;

            return f.ID;
        }

        public CommandSet GetCmdSet(int index)
        {
            return _cmdset[index];
        }

        internal void DebugPrint( SourceFile source )
        {
            foreach (var cs in _cmdset)
            {
                cs.DebugPrint(source);
            }
        }
    }
}
