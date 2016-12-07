
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

        public bool EqualsValue(string name, object v )
        {            
            return GetRegisterValue(name).Equals(Convertor.NativeValueToValue(v));
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

        public void SetVarInteger32(string name, int v) 
        {
            SetRegisterValue(name, Convertor.Integer32ToValue(v) );
        }

        public void SetVarBool(string name, bool v)
        {
            SetRegisterValue(name, Convertor.BoolToValue(v));
        }

        public void SetVarFloat32(string name, int v)
        {
            SetRegisterValue(name, Convertor.Float32ToValue(v));
        }

        public void SetVarString(string name, string v)
        {
            SetRegisterValue(name, Convertor.StringToValue(v));
        }

        public void SetVarNativeInstance<T>(string name, T ins) where T : class
        {
            if ( _pkg == null )
                return;            

            var classType = _pkg.Exe.GetClassTypeByNativeType( typeof(T) ) as ValueNativeClassType;
            if ( classType == null )
            {
                throw new RuntimeException("native class not registed: " + name);
            }

            SetRegisterValue(name, new ValueNativeClassIns(classType, ins ));
        }
    }
}
