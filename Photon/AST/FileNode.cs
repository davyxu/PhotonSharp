using System.Collections.Generic;

namespace Photon
{
    internal class FileNode : Node
    {
        public List<ImportStmt> Imports = new List<ImportStmt>();

        public BlockStmt Block;

        public FileNode(BlockStmt block, List<ImportStmt> imports)
        {
            Block = block;
            Imports = imports;

            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            foreach (var im in Imports)
            {
                yield return im;
            }

            yield return Block;

        }

        public override string ToString()
        {
            return "File";
        }

        internal override void Compile(CompileParameter param)
        {
            foreach (var im in Imports)
            {
                im.Compile(param);
            }

            Block.Compile(param);

        }
    }
}
