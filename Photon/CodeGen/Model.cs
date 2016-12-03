using System;
using System.Collections.Generic;
using System.Reflection;

namespace Photon
{

    class WrapperGenParameter
    {
        public string Name;
        public ValueKind Kind;
        public string KindString;
        public string NativeKindString;

        internal static WrapperGenParameter FromParameterInfo(ParameterInfo pi)
        {
            var genParam = new WrapperGenParameter();
            genParam.Name = pi.Name;

            if (!genParam.SetKindByType(pi.ParameterType))
            {
                throw new RuntimeException("Unknown parameter type: " + pi.ToString());
            }

            return genParam;
        }

        internal bool SetKindByType(Type t )
        {
            // out类型, 要转下
            if ( t.IsByRef )
            {
                t = t.GetElementType();
            }

            NativeKindString = t.Name;

            if (t == typeof(Int32))
            {
                Kind = ValueKind.Number;
                KindString = "Integer32";
            }
            else if ( t == typeof(float))
            {
                Kind = ValueKind.Number;
                KindString = "Float32";
            }
            else if (t == typeof(string))
            {
                Kind = ValueKind.String;
                KindString = "String";
            }
            else if ( t == typeof(void))
            {
                Kind = ValueKind.Nil;
                KindString = "Nil";
            }
            else if ( t.IsClass )
            {
                Kind = ValueKind.NativeClassInstance;
            }
            else
            {
                return false;
            }

            return true;

        }
    }

    enum WrapperFuncMode
    {
        PackageFunc,
        ClassMethod,
    }

    class WrapperGenFunc
    {
        public string Name;
        public WrapperFuncMode Mode;
        public List<WrapperGenParameter> InputParameters = new List<WrapperGenParameter>();
        public List<WrapperGenParameter> OutputParameters = new List<WrapperGenParameter>();

        public WrapperGenParameter RetParameter;
       
    }

    class WrapperGenClass
    {
        public string Name;
        public List<WrapperGenFunc> Methods = new List<WrapperGenFunc>();
    }


    class WrapperGenPackage
    {
        public string Name;

        public List<WrapperGenClass> Classes = new List<WrapperGenClass>();

        public List<WrapperGenFunc> PackageFuncs = new List<WrapperGenFunc>();
    }
}
