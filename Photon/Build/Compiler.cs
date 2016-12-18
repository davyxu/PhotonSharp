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
        public static void GenerateBuiltinFiles()
        {
            Array.GenerateWrapper();
            Map.GenerateWrapper();
        }


        internal static void Import(Executable exe, ContentLoader loader, string packageName,  string sourceName, ImportMode mode )
        {
            var pkg = new Package(packageName);

            var parser = new Parser(exe, loader, pkg.ScopeMgr);            

            loader.Load(pkg, parser, sourceName, mode);

            exe.AddPackage(pkg);

            var initPos = TokenPos.Init;
            initPos.SourceName = sourceName;

            // 全局入口( 不进入函数列表, 只在Package上保存 )
            var cs = new ValuePhoFunc(new ObjectName(pkg.Name, "@init"), initPos, pkg.PackageScope.RegCount, pkg.PackageScope);
            pkg.InitEntry = cs;

            var param = new CompileParameter();

            param.Pkg = pkg;
            param.CS = cs;
            param.Exe = exe;
            param.Constants = exe.Constants;

            pkg.Compile(param);
            
            cs.Add(new Command(Opcode.EXIT).SetCodePos(parser.CurrTokenPos));
        }

        public static Executable CompileFile(string filename)
        {
            Executable exe = new Executable();
            exe.RegisterBuiltinPackage();

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
