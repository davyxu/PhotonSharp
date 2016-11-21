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

        // 调试Symbol
        Chunk _chunk;

        List<File> _file = new List<File>();

        // 所有函数执行体
        List<Procedure> _proc = new List<Procedure>();

        // 常量表
        ConstantSet _constSet = new ConstantSet();

        Executable _parent;

        internal Chunk AST
        {
            get { return _chunk; }
            set { _chunk = value; }
        }

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

        internal int AddProcedure(Procedure f)
        {
            f.Pkg = this;

            _proc.Add(f);

            f.ID = _proc.Count - 1;

            return f.ID;
        }

        public Procedure GetProcedure(int index)
        {
            return _proc[index];
        }

        internal Procedure FindProcedureByName(string name)
        {
            foreach (var cs in _proc)
            {
                if (cs.Name == name)
                    return cs;
            }

            return null;
        }

        internal void DebugPrint( SourceFile source )
        {
            foreach (var p in _proc)
            {
                var cs = p as CommandSet;
                if ( cs != null )
                {
                    cs.DebugPrint(source);
                }
                
            }
        }
    }
}
