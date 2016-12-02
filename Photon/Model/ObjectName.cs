
namespace Photon
{
    internal struct ObjectName
    {
        public string PackageName;
        public string EntryName;

        internal static readonly  ObjectName Empty = new ObjectName(string.Empty, string.Empty);

        internal ObjectName(string pkg, string entry)
        {
            PackageName = pkg;
            EntryName = entry;
        }

        public override bool Equals(object obj)
        {
            var other = (ObjectName)obj;

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
}
