
namespace Photon
{
    public class CodeFile
    {
        internal SourceFile Source;

        // 调试Symbol
        internal FileNode AST;

        public override string ToString()
        {
            return Source.Name;
        }
    }

}
