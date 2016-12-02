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
            Location.CmdSet = vm.CurrFrame.CmdSet;
            Location.Pos = vm.CurrFrame.PC;
            RegPackage = regpkg;

            
            Location.FileName = vm.CurrFrame.CodePos.SourceName;                        

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
