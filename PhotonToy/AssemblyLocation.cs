using Photon;
using SharpLexer;
using System.Collections.Generic;

namespace PhotonToy
{
    public struct AssemblyLocation
    {
        public int FuncID;
        public string FuncName;
        public TokenPos FuncDefPos;

        public int PC;// 汇编位置
        public TokenPos CodePos;//当前代码运行位置
        public List<Command> Commands;


        public bool IsDiff( AssemblyLocation other )
        {
            return this.FuncID != other.FuncID ||               
                this.CodePos.SourceName != other.CodePos.SourceName;
        }

        public override bool Equals(object obj)
        {
            var other = (AssemblyLocation)obj;

            return this.FuncID == other.FuncID && 
                this.PC == other.PC &&
                this.CodePos.SourceName == other.CodePos.SourceName;
        }

        public override int GetHashCode()
        {
            return PC.GetHashCode() + FuncID.GetHashCode() + CodePos.SourceName.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", FuncName, PC, CodePos.SourceName);
        }
    }
}
