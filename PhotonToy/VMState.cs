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

        public VMState(VMachine vm)
        {
            Location.CmdSetID = vm.CurrFrame.CmdSet.ID;
            Location.Pos = vm.CurrFrame.PC;

            for (int i = 0; i < vm.LocalReg.Count; i++)
            {
                Register.Add(vm.LocalReg.Get(i).ToString());
            }

            for (int i = 0; i < vm.DataStack.Count; i++)
            {
                DataStack.Add(vm.DataStack.Get(i).ToString());
            }

            var arr = vm.CallStack.ToArray();

            for (int i = 0; i < arr.Length; i++)
            {
                CallStack.Add(arr[i].ToString());
            }
        }
    }

}
