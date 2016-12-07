
namespace Photon
{
    public partial class RuntimePackage
    {
        public int GetVarAsInteger32(string name)
        {
            return Convertor.ValueToInteger32(GetRegisterValue(name));
        }

        public bool GetVarAsBool(string name)
        {
            return Convertor.ValueToBool(GetRegisterValue(name));
        }

        public float GetVarAsFloat32(string name)
        {
            return Convertor.ValueToFloat32(GetRegisterValue(name));
        }

        public string GetVarAsString(string name)
        {
            return Convertor.ValueToString(GetRegisterValue(name));
        }

        public T GetVarAsNativeInstance<T>(string name) where T:class
        {
            return Convertor.ValueToObject(GetRegisterValue(name)) as T;
        }

        public ValueKind GetVarKind(string name)
        {
            return GetRegisterValue(name).Kind;            
        }

        public bool IsVarNil( string name )
        {
            return GetRegisterValue(name) is ValueNil;
        }
    }
}
