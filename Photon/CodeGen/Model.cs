using System;
using System.Collections.Generic;
using System.Reflection;

namespace Photon
{

    class WrapperGenParameter
    {
        public string Name;
        public string TypeString;
        public string NativeTypeString;

        internal static WrapperGenParameter FromParameterInfo(ParameterInfo pi)
        {
            var genParam = new WrapperGenParameter();
            genParam.Name = pi.Name;

            Type t;
            // out类型, 要转下
            if (pi.ParameterType.IsByRef)
            {
                t = pi.ParameterType.GetElementType();
            }
            else
            {
                t = pi.ParameterType;
            }

            genParam.NativeTypeString = t.Name;

            genParam.TypeString = Convertor.NativeTypeToTypeName(t);

            if (string.IsNullOrEmpty(genParam.TypeString))
            {
                throw new RuntimeException("Unknown parameter type: " + pi.ToString());
            }

            return genParam;
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

    class WrapperGenProperty
    {
        public string Name;
        public string TypeString;
        public bool CanRead;
        public bool CanWrite;
    }

    class WrapperGenClass
    {
        public string Name;
        public string NativePackageName;
        public List<WrapperGenFunc> Methods = new List<WrapperGenFunc>();
        public List<WrapperGenProperty> Properties = new List<WrapperGenProperty>();

        public string FullName
        {
            get { return NativePackageName + "." + Name; }
        }
    }


    class WrapperGenPackage
    {
        public string Name;

        public List<WrapperGenClass> Classes = new List<WrapperGenClass>();

        public List<WrapperGenFunc> PackageFuncs = new List<WrapperGenFunc>();
    }
}
