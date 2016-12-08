using System;

namespace Photon
{
    public static class Convertor
    {
        #region native类型转值类型

        public static object Integer32ToValue(Int32 v)
        {
            return new ValueInteger32(v);
        }

        public static object Integer64ToValue(Int64 v)
        {
            return new ValueInteger64(v);
        }

        public static object Float32ToValue(float v)
        {
            return new ValueFloat32(v);
        }

        public static object Float64ToValue(double v)
        {
            return new ValueFloat64(v);
        }

        public static object BoolToValue(bool v)
        {
            return new ValueBool(v);
        }

        public static object StringToValue(string s)
        {
            return new ValueString(s);
        }

        internal static Value NativeValueToValue(object v)
        {
            Type vt = v.GetType();

            if (vt == typeof(Int32))
            {
                return new ValueInteger32((Int32)v);
            }
            else if (vt == typeof(Int64))
            {
                return new ValueInteger64((Int64)v);
            }
            else if (vt == typeof(float))
            {
                return new ValueFloat32((float)v);
            }
            else if (vt == typeof(double))
            {
                return new ValueFloat64((double)v);
            }
            else if (vt == typeof(string))
            {
                return new ValueString((string)v);
            }
            else if (vt == typeof(bool))
            {
                return new ValueBool((bool)v);
            }
            
            throw new RuntimeException("Unsupport native type: "+ vt.ToString());            
        }

        #endregion

        #region 值类型转native类型

        public static Int32 ValueToInteger32(object v)
        {
            var real = (v as ValueInteger32);

            if (real == null)
            {
                throw new RuntimeException("Expect 'Integer32' value");
            }

            return real.RawValue;
        }

        public static Int64 ValueToInteger64(object v)
        {
            var real = (v as ValueInteger64);

            if (real == null)
            {
                throw new RuntimeException("Expect 'Integer64' value");
            }

            return real.RawValue;
        }

        public static float ValueToFloat32(object v)
        {
            var real = (v as ValueFloat32);

            if (real == null)
            {
                throw new RuntimeException("Expect 'Float32' value");
            }

            return real.RawValue;
        }

        public static double ValueToFloat64(object v)
        {
            var real = (v as ValueFloat64);

            if (real == null)
            {
                throw new RuntimeException("Expect 'Float64' value");
            }

            return real.RawValue;
        }

        public static bool ValueToBool(object v)
        {
            var real = (v as ValueBool);

            if (real == null)
            {
                throw new RuntimeException("Expect 'Bool' value");
            }

            return real.RawValue;
        }

        public static string ValueToString(object v)
        {
            var real = (v as ValueString);

            if (real == null)
            {
                throw new RuntimeException("Expect 'String' value");
            }

            return real.RawValue;
        }

        public static object ValueToObject(object v)
        {
            var real = (v as ValueObject);

            if (real == null)
            {
                throw new RuntimeException("Expect 'Object' value");
            }

            return real.Raw;
        }

        internal static object ValueToNativeValue( Value v )
        {
            return v.Raw;
        }

        #endregion

        #region 内部转换

        internal static ValueObject CastObject(Value v )
        {
            var real = (v as ValueObject);

            if (real == null)
            {
                throw new RuntimeException("Expect 'Object' value");
            }

            return real;
        }

        internal static ValueFunc CastFunc(Value v)
        {
            var real = (v as ValueFunc);

            if (real == null)
            {
                throw new RuntimeException("Expect 'Func' value");
            }

            return real;
        }

        internal static ValueClosure CastClosure(Value v)
        {
            var real = (v as ValueClosure);

            if (real == null)
            {
                throw new RuntimeException("Expect 'Closure' value");
            }

            return real;
        }

        internal static string NativeTypeToTypeName(Type t)
        {
            if (t == typeof(Int32))
            {
                return "Integer32";
            }
            else if (t == typeof(Int64))
            {
                return "Integer64";
            }
            else if ( t == typeof(float))
            {
                return "Float32";
            }
            else if (t == typeof(double))
            {
                return "Float64";
            }
            else if (t == typeof(string))
            {
                return "String";
            }
            else if (t == typeof(bool))
            {
                return "Bool";
            }
            else if ( t == typeof(void))
            {
                return "Nil";
            }
            else if (t == typeof(Value))
            {
                return "Value";
            }
            
            return string.Empty;
        }

        #endregion
    }
}
