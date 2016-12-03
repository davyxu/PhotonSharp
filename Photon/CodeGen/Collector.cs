using System;
using System.Reflection;

namespace Photon
{
    class WrapperCodeCollector
    {
        internal static void CollectClassInfo(WrapperGenPackage genPkg, Type cls, string filename)
        {
            if ( !cls.IsClass)
            {
                throw new RuntimeException("Require class type");
            }

            var genClass = new WrapperGenClass();
            genClass.Name = cls.Name;
            genPkg.Classes.Add(genClass);

            foreach (var m in cls.GetMembers())
            {
                // 必须是本类自己的成员
                if (m.DeclaringType != cls)
                    continue;

                var attr = m.GetCustomAttribute<NativeEntryAttribute>();
                if (attr != null)
                    continue;

                var methodInfo = cls.GetMethod(m.Name);
                if (methodInfo == null)
                    continue;

                var genFunc = new WrapperGenFunc( );
                genFunc.Name = m.Name;

                if ( methodInfo.IsStatic )
                {
                    genFunc.Mode = WrapperFuncMode.PackageFunc;
                }
                else
                {
                    genFunc.Mode = WrapperFuncMode.ClassMethod;
                }

                genFunc.RetParameter = WrapperGenParameter.FromParameterInfo(methodInfo.ReturnParameter);

                foreach( var param in methodInfo.GetParameters() )
                {
                    var genParam = WrapperGenParameter.FromParameterInfo(param);

                    if (param.IsOut)
                    {
                        genFunc.OutputParameters.Add(genParam);
                    }
                    else
                    {
                        genFunc.InputParameters.Add(genParam);
                    }
                }


                genClass.Methods.Add(genFunc);

                
            }
        }

    }
}
