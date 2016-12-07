using System;
using System.Reflection;

namespace Photon
{
    class WrapperCodeCollector
    {
        internal static T GetCustomAttribute<T>(PropertyInfo pi) where T : class
        {
            object[] objs = pi.GetCustomAttributes(typeof(T), false);
            if (objs.Length > 0)
                return (T)objs[0];

            return null;
        }

        internal static T GetCustomAttribute<T>(MethodInfo mi) where T : class
        {            
            object[] objs = mi.GetCustomAttributes(typeof(T), false);
            if (objs.Length > 0)
                return (T)objs[0];

            return null;
        }

        internal static void CollectClassInfo(WrapperGenPackage genPkg, Type cls, string filename)
        {
            if ( !cls.IsClass)
            {
                throw new RuntimeException("Require class type");
            }

            var genClass = new WrapperGenClass();
            genClass.Name = cls.Name;
            genClass.NativePackageName = cls.Namespace;
            genPkg.Classes.Add(genClass);


            foreach (var propInfo in cls.GetProperties())
            {
                // 必须是本类自己的成员
                if (propInfo.DeclaringType != cls)
                    continue;

                if (GetCustomAttribute<NoGenWrapperAttribute>(propInfo) != null)
                {
                    continue;
                }


                var prop = new WrapperGenProperty();
                prop.Name = propInfo.Name;
                prop.TypeString = Convertor.NativeTypeToTypeName(propInfo.PropertyType);
                prop.CanRead = propInfo.CanRead;
                prop.CanWrite = propInfo.CanWrite;
                genClass.Properties.Add(prop);
            }


            foreach (var methodInfo in cls.GetMethods())
            {
                // 必须是本类自己的成员
                if (methodInfo.DeclaringType != cls)
                    continue;

                // 掠过属性生成的函数
                if (methodInfo.IsSpecialName)
                    continue;

                var attr = GetCustomAttribute<NativeEntryAttribute>(methodInfo);
                // 有标签的掠过， 无需生成
                if (attr != null)
                    continue;

                // 不生成wrapper
                if (GetCustomAttribute<NoGenWrapperAttribute>(methodInfo) != null)
                {
                    continue;
                }
                

                var genFunc = new WrapperGenFunc( );
                genFunc.Name = methodInfo.Name;

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
