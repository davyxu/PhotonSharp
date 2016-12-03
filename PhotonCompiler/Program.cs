using Photon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace PhotonCompiler
{

    partial class Program
    {
        static void Help( )
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("pho command [arguments] pho files...");
            Console.WriteLine("The commands are:");
            Console.WriteLine("     compile     compile packages and dependencies");
            Console.WriteLine("     run         compile and run");
        }

        static int Main(string[] args)
        {
            if ( args.Length < 2 )
            {
                Help();
                return 1;
            }

            var mode = args[0];
            var fs = new FlagSet(args, 1);
            bool needRun = false;

            switch( mode )
            {
                case "compile":
                    {
                        needRun = false;
                    }
                    break;
                case "run":
                    {
                        needRun = true;

                    }
                    break;
                default:
                    Help();
                    return 1;
            }
            int ret = 0;

            if ( Debugger.IsAttached )
            {
                Run(fs.Args, needRun, fs.BoolOption("-l"));
            }
            else
            {
                try
                {
                    Run(fs.Args, needRun, fs.BoolOption("-l"));
                }
                catch (Exception ex )
                {
                    Console.WriteLine(ex.ToString());
                    ret = 1;
                }
            }

            return ret;
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
