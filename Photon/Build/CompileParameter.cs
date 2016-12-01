
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
        internal CommandSet CS;
        internal bool LHS;

        internal CompileParameter SetLHS(bool lhs)
        {
            var copy = (CompileParameter)this.MemberwiseClone();
            copy.LHS = lhs;
            return copy;
        }

        internal CompileParameter SetComdSet(CommandSet cs)
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
