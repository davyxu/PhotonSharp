
using MarkSerializer;
namespace Photon
{
    internal struct ObjectName : IMarkSerializable
    {        
        public string PackageName;
        
        public string EntryName;
        
        public string ClassName;

        internal static readonly  ObjectName Empty = new ObjectName(string.Empty, string.Empty, string.Empty);

        public void Serialize(BinarySerializer ser)
        {
            ser.Serialize<string>(PackageName);
            ser.Serialize<string>(EntryName);
            ser.Serialize<string>(ClassName);
        }

        public void Deserialize(BinaryDeserializer ser)
        {
            PackageName = ser.Deserialize<string>();
            EntryName = ser.Deserialize<string>();
            ClassName = ser.Deserialize<string>();
        }


        internal ObjectName(string pkg, string entry)
        {
            PackageName = pkg;
            EntryName = entry;
            ClassName = string.Empty;
        }

        internal ObjectName(string pkg, string className, string entry)
        {
            PackageName = pkg;
            ClassName = className;
            EntryName = entry;
        }

        public override bool Equals(object obj)
        {
            var other = (ObjectName)obj;

            return PackageName == other.PackageName && 
                EntryName == other.EntryName && 
                ClassName == other.ClassName;
        }

        public override int GetHashCode()
        {
            return PackageName.GetHashCode() + EntryName.GetHashCode() + ClassName.GetHashCode();
        }

        public override string ToString()
        {

            if ( string.IsNullOrEmpty(ClassName) )
            {
                return string.Format("{0}.{1}", PackageName, EntryName);
            }

            return string.Format("{0}.{1}.{2}", PackageName, ClassName,EntryName);
        }
    }
}
