using SharpLexer;
using System.IO;

namespace Photon
{

    public class Compiler
    {

        //public static void Import( Parser parser, string packageName )
        //{
        //    var param = new CompileParameter();

        //    param.Pkg = exe.AddPackage(packageName);

        //    param.CS = new CommandSet(packageName, TokenPos.Init, parser.PackageScope.RegCount, true);

        //    param.Pkg.AddProcedure(param.CS);

        //    var files = Directory.GetFiles(packageName, "*.pho", SearchOption.TopDirectoryOnly);

        //    foreach( var filename in files )
        //    {
        //        var content = System.IO.File.ReadAllText(packageName);

        //        SourceFile file = new SourceFile(content, packageName);

        //        var chunk = parser.Import(file);

        //        // 遍历AST,生成代码
        //        chunk.Compile(param);
        //    }
        //}



        public static Executable Compile(SourceFile file)
        {            
            var parser = new Parser();

            var exe = new Executable(parser.PackageScope, file);



            // 编译生成AST
            var chunk = parser.Import(file);
            
           
            var param = new CompileParameter();

            param.Pkg = exe.AddPackage("main");
            param.Pkg.AST = chunk;

            param.CS = new CommandSet("main", TokenPos.Init, parser.PackageScope.RegCount, true);

            param.Pkg.AddProcedure(param.CS);               

            // 遍历AST,生成代码
            chunk.Compile(param);

            param.CS.Add(new Command(Opcode.EXIT));

            exe.ResolveNode();

            //exe.RegisterDelegate("array", (vm) =>
            //{
            //    vm.DataStack.Push(new ValueArray());

            //    return 1;
            //});


            return exe;
        }

    }
}
