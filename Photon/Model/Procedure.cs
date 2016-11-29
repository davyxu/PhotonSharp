
namespace Photon
{
    public struct ProcedureName
    {
        public string PackageName;
        public string EntryName;

        public ProcedureName( string pkg, string entry )
        {
            PackageName = pkg;
            EntryName = entry;
        }

        public override bool Equals(object obj)
        {
            var other = (ProcedureName)obj;

            return PackageName == other.PackageName && EntryName == other.EntryName;
        }

        public override int GetHashCode()
        {
            return PackageName.GetHashCode() + EntryName.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}", PackageName, EntryName);
        }
    }

    // 外部函数和内建函数统称过程
    public class Procedure
    {
        int _id;

        ProcedureName _name;

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


        public ProcedureName Name
        {
            get { return _name; }
        }

        internal Procedure(ProcedureName name)
        {
            _name = name;
        }
    }
}
