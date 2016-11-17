
namespace PhotonToy
{
    public struct AssemblyLocation
    {
        public int CmdSetID;
        public int Pos;

        public AssemblyLocation(int cmdsetid, int pos)
        {
            CmdSetID = cmdsetid;
            Pos = pos;
        }

        public override bool Equals(object obj)
        {
            var other = (AssemblyLocation)obj;

            return this.CmdSetID == other.CmdSetID && this.Pos == other.Pos;
        }

        public override int GetHashCode()
        {
            return Pos.GetHashCode() + CmdSetID.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", CmdSetID, Pos);
        }
    }
}
