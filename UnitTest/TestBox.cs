﻿using System;
using Photon;
using System.IO;

namespace UnitTest
{
    class TestBox
    {
        Executable _exe = new Executable();
        VMachine _vm = new VMachine();        

        string _caseName;

        public static bool SerializeTest = true;

        public static bool ShowDebugInfo = false;

        public VMachine VM
        {
            get { return _vm; }
        }

        public Executable Exe
        {
            get { return _exe; }
        }
        public TestBox()
        {
            _exe.RegisterBuiltinPackage();
        }

        public TestBox Compile( string caseName, string src )
        {
            _caseName = caseName;

            Logger.DebugLine(string.Format("################### {0} ###################", _caseName));

            Compiler.Compile(_exe, new FileLoader(Directory.GetCurrentDirectory()), src);

            if (ShowDebugInfo)
            {
                _exe.DebugPrint();            
            }
            

            return this;
        }

        static Executable SerTestExecuable(Executable inExe)
        {
            MemoryStream stream = new MemoryStream();

            Executable.Serialize(stream, ref inExe, false );            

            stream.Position = 0;

            Executable newExe = new Executable();

            Executable.Serialize(stream, ref newExe, true);

            if (_registerCallback != null)
            {
                _registerCallback(newExe);
            }

            // 反序列化注册顺序和序列化顺序相反
            newExe.RegisterBuiltinPackage();

            if (ShowDebugInfo)
            {
                newExe.DebugPrint();        
            }
            

            return newExe;
        }

        public TestBox Run( )
        {
            Logger.DebugLine(string.Format(">>>>>>>>>Start {0}", _caseName));
            _vm.ShowDebugInfo = ShowDebugInfo;

            if (SerializeTest)
            {
                var newExe = SerTestExecuable(_exe);

                _vm.Execute(newExe);
            }
            else
            {
                _vm.Execute(_exe);
            }
            

            Logger.DebugLine(string.Format(">>>>>>>>>End {0}", _caseName));

            if ( ShowDebugInfo )
            {
                _vm.DataStack.DebugPrint();

                _vm.LocalReg.DebugPrint();

                var G = _vm.GetRuntimePackageByName("main").Reg;

                G.DebugPrint();
            }
            
            return this;
        }



        public TestBox CompileFile(string filename)
        {
            return Compile(filename, filename);
        }

        public TestBox RunFile( string filename )
        {                        
            CompileFile(filename );

            Run();

            CheckStackClear();

            return this;
        }

        public delegate void RegisterFuncCallback( Executable exe);

        static RegisterFuncCallback _registerCallback;

        public TestBox RegisterRunFile(RegisterFuncCallback callback, string filename  )
        {
            callback(_exe);
            _registerCallback = callback;

            CompileFile(filename);

            Run();

            CheckStackClear();

            return this;
        }

        public RuntimePackage MainPackage
        {
            get { return _vm.GetRuntimePackageByName("main"); }
        }

        public void Error( string info )
        {
            Logger.DebugLine("[{0}] failed, {1}", _caseName, info);
            throw new Exception(info);
        }

        public TestBox CheckStackClear()
        {
            if (_vm.DataStack.Count != 0)
            {
                Error("Stack not clear");
            }

            return this;
        }

        public TestBox CheckGlobalVarMatchKind( string name, ValueKind kind )
        {
            var symbolKind = MainPackage.GetVarKind(name);
            if (symbolKind != kind)
            {
                Error("CheckGlobalSymbolMatchKind failed, name: " + name);
            }

            return this;
        }

        public TestBox CheckGlobalVarMatchValue(string name, object value)
        {
            if (!MainPackage.EqualsValue(name, value ))
            {
                Error("CheckGlobalSymbolMatchValue failed, name: " + name);
            }

            return this;
        }

    }
}
