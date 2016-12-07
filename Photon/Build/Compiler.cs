using SharpLexer;
using System.IO;

namespace Photon
{
    public enum ImportMode
    {
        Directory,
        File,
    }

    public class Compiler
    {
        public static void GenerateBuildinFiles()
        {
            Array.GenerateWrapper();
            Map.GenerateWrapper();
        }


        internal static void Import(Executable exe, ContentLoader loader, string packageName,  string sourceName, ImportMode mode )
        {
            var parser = new Parser(exe, loader );

            loader.Init(parser, exe);

            var pkg = exe.AddPackage(packageName, parser.PackageScope);

            loader.Load(sourceName, mode);

            var initPos = TokenPos.Init;
            initPos.SourceName = sourceName;

            var cs = new ValuePhoFunc( new ObjectName( pkg.Name, pkg.Name ), initPos, parser.PackageScope.RegCount, parser.PackageScope);

            exe.AddFunc(cs);

            var param = new CompileParameter();

            param.Pkg = pkg;
            param.CS = cs;
            param.Exe = exe;

            // 遍历AST,生成代码
            parser.Chunk.Compile(param);

            param.Pkg.AST = parser.Chunk;
            
            cs.Add(new Command(Opcode.EXIT).SetCodePos(parser.CurrTokenPos));
        }

        public static Executable CompileFile(string filename)
        {
            Executable exe = new Executable();

            Compiler.Compile(exe, new FileLoader(Directory.GetCurrentDirectory()), filename);

            return exe;
        }

        public static void Compile(Executable exe, ContentLoader loader, string filename)
        {            
            Import(exe, loader, "main", filename, ImportMode.File); 
        
            for( int i = 0;i<exe.PackageCount;i++)
            {
                var pkg = exe.GetPackage(i);
                pkg.ResolveNode();
            }
        }

    }
}
