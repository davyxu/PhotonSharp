using SharpLexer;

namespace Photon
{

    public class Compiler
    {
        public static Executable Compile(SourceFile file)
        {            
            var parser = new CodeParser();

            // 编译生成AST
            var chunk = parser.Parse(file);
            
            var exe = new Executable(chunk, parser.GlobalScope, file );

            var param = new CompileParameter();

            param.Pkg = exe.AddPackage("main");

            param.CS = new CommandSet("global", TokenPos.Init, parser.GlobalScope.RegCount, true);

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
