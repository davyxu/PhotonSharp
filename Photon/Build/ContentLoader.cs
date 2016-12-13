
namespace Photon
{
    public class ContentLoader
    {
        public void AddSource(Package pkg, object parser, string content, string sourceName)
        {
            SourceFile srcfile = new SourceFile(content, sourceName);

            var code = new CodeFile();
           
            code.Source = srcfile;
            code.AST = (parser as Parser).ParseFile(srcfile);

            pkg.AddCode(code);
        }

        public virtual void Load(Package pkg, object parser, string sourceName, ImportMode mode)
        {
            throw new System.NotImplementedException();
        }
    }

}
