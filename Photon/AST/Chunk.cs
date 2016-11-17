﻿using System.Collections.Generic;

namespace Photon
{
    public class Chunk : Node
    {
        public BlockStmt Block;

        public Chunk(BlockStmt block)
        {
            Block = block;

            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            return Block.Child();
        }

        public override string ToString()
        {
            return "Chunk";
        }

        internal override void Compile(Package exe, CommandSet cm, bool lhs)
        {
            Block.Compile(exe, cm, lhs);
        
        }
    }
}
