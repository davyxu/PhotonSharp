using Photon;
using System.Collections.Generic;
namespace PhotonCompiler
{

    partial class Program
    {
        class Animal
        {
            public Animal( )
            {

            }
        }

        class Cat :Animal
        {
            public Cat( )
            {

            }
        }

        static void Main(string[] args)
        {
            new Cat();

            if ( args.Length < 2 )
            {
                return;
            }

            var mode = args[0];
            
            switch( mode )
            {
                case "unittest":
                    {
                        TestCase();
                    }
                    break;
                case "compile":
                    {
                        var fs = new FlagSet(args, 1);
                        Run(fs.Args, false, fs.BoolOption("-l"));
                    }
                    break;
                case "run":
                    {
                        var fs = new FlagSet(args, 1);

                        Run(fs.Args, true, fs.BoolOption("-l"));
                    }
                    break;
            }
            
        }

        static void Run( List<string> files, bool run, bool debugInfo )
        {
            if (files.Count == 0)
                return;            

            var exe = Compiler.Compile(files[0]);

            if ( debugInfo )
            {
                exe.DebugPrint();      
            }

            if ( run)
            {
                var vm = new VMachine();

                vm.ShowDebugInfo = debugInfo;

                vm.Run(exe );
            }


            
        }
    }
}
