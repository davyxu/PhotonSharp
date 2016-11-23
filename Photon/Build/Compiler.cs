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

        static void ImportFile(Package pkg, Parser parser, string filename)
        {
            var content = System.IO.File.ReadAllText(filename);

            SourceFile srcfile = new SourceFile(content, Path.GetFileName(filename));

            pkg.AddSource(srcfile);

            parser.Import(srcfile);            
        }

        public static void Import(Executable exe,string packageName,  string packageFileName, ImportMode mode)
        {
            var parser = new Parser();
            parser.Exe = exe;

            var pkg = exe.AddPackage(packageName, parser.PackageScope);

            if ( mode == ImportMode.Directory)
            {
                var files = Directory.GetFiles(packageFileName, "*.pho", SearchOption.TopDirectoryOnly);

                foreach (var filename in files)
                {
                    ImportFile(pkg, parser, filename);
                }
            }
            else
            {                
                ImportFile(pkg, parser, packageFileName);
            }

            var cs = new CommandSet(pkg.Name, TokenPos.Init, parser.PackageScope.RegCount, true);

            pkg.AddProcedure(cs);

            var param = new CompileParameter();

            param.Pkg = pkg;
            param.CS = cs;

            // 遍历AST,生成代码
            parser.Chunk.Compile(param);

            param.Pkg.AST = parser.Chunk;
            
            cs.Add(new Command(Opcode.EXIT));

            pkg.ResolveNode();
        }



        public static Executable Compile(string filename)
        {            
            var exe = new Executable( );

            Import(exe, "main", filename, ImportMode.File);

            return exe;
        }

    }
}
