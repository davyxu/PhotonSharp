
namespace Photon
{
    // 外部函数和内建函数统称过程
    public class Procedure
    {
        int _id;

        ObjectName _name;

        public int ID
        {
            get { return _id; }
            internal set { _id = value; }
        }

        public Package Pkg
        {
            get;
            set;
        }

        // 传入参数数量
        internal int InputValueCount { get; set; }

        // 返回值数量
        internal int OutputValueCount { get; set; }


        public ObjectName Name
        {
            get { return _name; }
        }

        internal Procedure(ObjectName name)
        {
            _name = name;
        }

        internal virtual bool Invoke(VMachine vm, int argCount, bool balanceStack, ValueClosure closure)
        {
            return false;
        }
    }
}
