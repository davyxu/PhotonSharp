
using Photon;
namespace PhotonToy
{
    public struct AssemblyLocation
    {
        public CommandSet CmdSet;
        public int Pos;
        public string FileName;

        public AssemblyLocation(CommandSet cmdset, int pos, string filename)
        {
            CmdSet = cmdset;
            Pos = pos;
            FileName = filename;
        }

        public override bool Equals(object obj)
        {
            var other = (AssemblyLocation)obj;

            return this.CmdSet == other.CmdSet && 
                this.Pos == other.Pos &&
                this.FileName == other.FileName;
        }

        public override int GetHashCode()
        {
            return Pos.GetHashCode() + CmdSet.GetHashCode() + FileName.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", CmdSet.Name, Pos, FileName);
        }
    }
}
