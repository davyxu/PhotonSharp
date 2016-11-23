
using System.Collections.Generic;
using System.Text;

namespace Photon
{
    // 函数前置声明
    internal class DelegateDeclare : Stmt
    {
        public Ident Name;

        public FuncType TypeInfo;

        public DelegateDeclare(Ident name, FuncType ft )
        {
            Name = name;
            TypeInfo = ft;

            BuildRelation();
        }

        public override string ToString()
        {
            return string.Format("DelegateDeclare {0} {1}", Name.Name, TypeInfo.ToString());
        }

        public override IEnumerable<Node> Child()
        {
            yield break;
        }


        internal override void Compile(CompileParameter param)
        {
            var c = new ValueDelegate( );
            var ci = param.Pkg.Constants.Add(c);

            param.Pkg.Exe.AddDelegate(Name.Name, c);
        }
    }
}
