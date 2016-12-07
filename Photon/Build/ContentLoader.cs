
namespace Photon
{
    public class ContentLoader
    {
        internal Parser _parser;
        internal Executable _exe;

        internal void Init(Parser parser, Executable exe)
        {
            _parser = parser;
            _exe = exe;
        }

        public void AddSource(string content, string sourceName)
        {
            if (_parser == null || _exe == null)
                return;

            SourceFile srcfile = new SourceFile(content, sourceName);

            _exe.AddSource(srcfile);

            _parser.Parse(srcfile);
        }

        public virtual void Load(string sourceName, ImportMode mode)
        {
            throw new System.NotImplementedException();
        }
    }

}
