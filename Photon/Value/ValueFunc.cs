
using SharpLexer;
namespace Photon
{
    class ValueFunc : Value
    {
        internal int ID;

        ObjectName _name;
        public ObjectName Name
        {
            get { return _name; }
        }

        // 传入参数数量
        internal int InputValueCount { get; set; }

        // 返回值数量
        internal int OutputValueCount { get; set; }

        public ValueFunc()
        {

        }

        internal ValueFunc(ObjectName name)
        {
            _name = name;
        }

        public override void Serialize(BinarySerializer serializer)
        {
            serializer
                .Serialize(ref ID)
                .Serialize(ref _name);
        }

        internal virtual bool Invoke(VMachine vm, int argCount, int receiverCount, ValueClosure closure)
        {
            return false;
        }

        internal virtual void DebugPrint(Executable exe )
        {

        }
        
        public override ValueKind Kind
        {
            get { return ValueKind.Func; }
        }
    }

 
}
