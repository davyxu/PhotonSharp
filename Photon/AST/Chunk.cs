using System.Collections.Generic;

namespace Photon
{
    public class Chunk : Node
    {
        // 每个block是一个文件
        public List<BlockStmt> BlockList = new List<BlockStmt>();

        public void Add(BlockStmt block)
        {
            BlockList.Add( block );

            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            foreach( var b in BlockList )
            {
                yield return b;
            }
            
        }

        public override string ToString()
        {
            return "Chunk";
        }

        internal override void Compile(CompileParameter param)
        {
            foreach (var b in BlockList)
            {
                b.Compile(param);
            }            
        
        }
    }
}
