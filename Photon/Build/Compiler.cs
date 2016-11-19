using System.Diagnostics;
using System;
using SharpLexer;

namespace Photon
{
    internal struct CompileParameter
    {
        internal Package Pkg;
        internal CommandSet CS;
        internal bool LHS;

        internal CompileParameter SetLHS(bool lhs)
        {
            LHS = lhs;
            return this;
        }

        internal CompileParameter SetComdSet( CommandSet cs )
        {
            CS = cs;
            return this;
        }

        internal void NextPassToResolve(Node n)
        {
            CompileContext ctx;
            ctx.node = n;
            ctx.parameter = this;

            Pkg.Exe._secondPass.Add(ctx);
        }

        internal bool IsNodeInNextPass( Node n )
        {
            foreach( var c in Pkg.Exe._secondPass )
            {
                if (c.node == n)
                    return true;
            }

            return false;
        }
    }


    struct CompileContext
    {
        internal Node node;
        internal CompileParameter parameter;
    }


    public class Compiler
    {
        public static Executable Compile(SourceFile file)
        {            
            var parser = new CodeParser();

            // 编译生成AST
            var chunk = parser.Parse(file.Source);
            
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
