using SharpLexer;
using System;
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

        static string NormalizeFileName( string filename )
        {
            var curr = Directory.GetCurrentDirectory().ToLower() + "\\";
            filename = filename.ToLower();

            string final;

            if ( Path.IsPathRooted( filename ) )
            {
                if ( filename.IndexOf(curr) == 0 )
                {
                    final = filename.Substring(curr.Length);
                }
                else
                {
                    throw new Exception("file should under PHOTONPATH");
                }
            }
            else
            {
                final = filename;
            }

            return final.Replace('\\', '/');
        }

        static void ImportFile(Package pkg, Parser parser, string filename)
        {
            var content = System.IO.File.ReadAllText(filename);

            SourceFile srcfile = new SourceFile(content, NormalizeFileName( filename ));

            pkg.AddSource(srcfile);

            parser.Import(srcfile);            
        }

        public static void Import(Executable exe, string packageName,  string packageFileName, ImportMode mode)
        {
            var parser = new Parser();
            parser.Exe = exe;

            var pkg = exe.AddPackage(packageName, parser.PackageScope);
            
            parser.PackageScope.RelatePackage = packageName;

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

            var initPos = TokenPos.Init;
            initPos.SourceName = packageName;

            var cs = new CommandSet(pkg.Name, initPos, parser.PackageScope.RegCount, true);

            pkg.AddProcedure(cs);

            var param = new CompileParameter();

            param.Pkg = pkg;
            param.CS = cs;

            // 遍历AST,生成代码
            parser.Chunk.Compile(param);

            param.Pkg.AST = parser.Chunk;
            
            cs.Add(new Command(Opcode.EXIT).SetCodePos(parser.CurrTokenPos));
        }



        public static Executable Compile(string filename)
        {            
            var exe = new Executable( );

            Compile(exe, filename);

            return exe;
        }

        public static void Compile(Executable exe, string filename)
        {            
            Import(exe, "main", filename, ImportMode.File); 
        
            for( int i = 0;i<exe.PackageCount;i++)
            {
                var pkg = exe.GetPackage(i);
                pkg.ResolveNode();
            }
        }

    }
}
