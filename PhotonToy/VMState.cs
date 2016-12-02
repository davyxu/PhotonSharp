using Photon;
using System.Collections.Generic;

namespace PhotonToy
{
    class VMState
    {
        public AssemblyLocation Location;
        public List<string> Register = new List<string>();
        public List<string> DataStack = new List<string>();
        public List<string> CallStack = new List<string>();
        public string RegPackage;        

        public VMState(VMachine vm, string regpkg)
        {
            Location.FuncID = vm.CurrFrame.FuncID;
            Location.FuncName = vm.CurrFrame.FuncName;
            Location.PC = vm.CurrFrame.PC;
            Location.Commands = vm.CurrFrame.Commands;
            Location.FuncDefPos = vm.CurrFrame.FuncDefPos;
            RegPackage = regpkg;


            Location.CodePos = vm.CurrFrame.CodePos;

            if (string.IsNullOrEmpty(regpkg ))
            {
                for (int i = 0; i < vm.LocalReg.Count; i++)
                {
                    Register.Add(vm.LocalReg.DebugString(i));
                }
            }
            else
            {
                var pkg = vm.GetRuntimePackageByName(regpkg);
                for (int i = 0; i < pkg.Reg.Count; i++)
                {
                    Register.Add(pkg.Reg.DebugString(i));
                }
            }
            

            for (int i = 0; i < vm.DataStack.Count; i++)
            {
                DataStack.Add(vm.DataStack.DebugString(i));
            }

            var arr = vm.CallStack.ToArray();

            for (int i = 0; i < arr.Length; i++)
            {
                CallStack.Add(arr[i].DebugString());
            }
        }
    }

}
