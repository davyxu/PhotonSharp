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

        List<File> _file = new List<File>();

        // 所有函数执行体
        List<CommandSet> _cmdset = new List<CommandSet>();

        // 常量表
        ConstantSet _constSet = new ConstantSet();

        // 外部代理函数
        Dictionary<string, ValueDelegate> _delegateByName = new Dictionary<string, ValueDelegate>();

        // 调试Symbol
        Chunk _chunk;

        // 作用域
        Scope _globalScope;


        public string Name
        {
            get { return _name; }
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
