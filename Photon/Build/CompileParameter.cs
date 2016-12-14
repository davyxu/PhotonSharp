
namespace Photon
{
    struct CompileContext
    {
        internal Node node;
        internal CompileParameter parameter;
    }


    internal struct CompileParameter
    {
        internal Package Pkg;
        internal Executable Exe;
        internal ValuePhoFunc CS;
        internal ConstantSet Constants;
        internal bool LHS;        

        internal CompileParameter SetLHS(bool lhs)
        {
            var copy = (CompileParameter)this.MemberwiseClone();
            copy.LHS = lhs;
            return copy;
        }

        internal CompileParameter SetCmdSet(ValuePhoFunc cs)
        {
            var copy = (CompileParameter)this.MemberwiseClone();
            copy.CS = cs;
            return copy;
        }

        internal CompileParameter SetPackage(Package pkg)
        {
            var copy = (CompileParameter)this.MemberwiseClone();
            copy.Pkg = pkg;
            return copy;
        }

        internal CompileParameter Clone( )
        {            
            return (CompileParameter)this.MemberwiseClone();
        }

        internal void NextPassToResolve(Node n)
        {
            Pkg.AddSecondPass(n, this);
        }

        internal bool IsNodeInNextPass(Node n)
        {
            return Pkg.ContainSecondPassNode(n);            
        }
    }

}
