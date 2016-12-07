
using System.Collections.Generic;
namespace Photon
{
    class RuntimeContext
    {
        public DataStack DataStack;

        public RuntimeFrame CurrFrame;

        public Stack<RuntimeFrame> CallStack = new Stack<RuntimeFrame>();

        public List<RuntimePackage> Package = new List<RuntimePackage>();
    }
}
