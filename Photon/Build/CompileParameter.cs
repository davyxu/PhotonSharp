
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
        internal CommandSet CS;
        internal bool LHS;

        internal CompileParameter SetLHS(bool lhs)
        {
            LHS = lhs;
            return this;
        }

        internal CompileParameter SetComdSet(CommandSet cs)
        {
            CS = cs;
            return this;
        }

        internal void NextPassToResolve(Node n)
        {
            CompileContext ctx;
            ctx.node = n;
            ctx.parameter = this;

            Pkg.Exe._secondPass.Add(ctx);
        }

        internal bool IsNodeInNextPass(Node n)
        {
            foreach (var c in Pkg.Exe._secondPass)
            {
                if (c.node == n)
                    return true;
            }

            return false;
        }
    }

}
