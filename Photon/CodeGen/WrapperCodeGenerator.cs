using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Photon
{
    public class WrapperCodeGenerator
    {
        public void GenerateClass(Type cls, string namespaceName, string filename)
        {
            var genPkg = new WrapperGenPackage();

            CollectClassInfo(genPkg, cls, filename);

            var content = Print(genPkg, namespaceName);

            Directory.CreateDirectory(Path.GetDirectoryName(filename));            
            System.IO.File.WriteAllText(filename, content, System.Text.Encoding.UTF8);
        }

        void CollectClassInfo(WrapperGenPackage genPkg, Type cls, string filename)
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

        string Print( WrapperGenPackage genPkg, string namespaceName )
        {
            var codeGen = new CodeGenerator();

            codeGen.PrintLine("using Photon;");
            codeGen.PrintLine("using System;");
            codeGen.PrintLine();

            codeGen.PrintLine("namespace ", namespaceName );
            codeGen.PrintLine("{");
            codeGen.In();

            foreach( var c in genPkg.Classes )
            {
                codeGen.PrintLine("[NativeWrapperClass(typeof(", c.Name, "))]");
                codeGen.PrintLine("public class ", c.Name, "Wrapper");
                codeGen.PrintLine("{");
                codeGen.In();

                foreach( var m in c.Methods )
                {
                    codeGen.PrintLine("[NativeEntry(NativeEntryType.ClassMethod)]");
                    codeGen.PrintLine("public static int ", m.Name,"( VMachine vm )");
                    codeGen.PrintLine("{");
                    codeGen.In();
                    codeGen.PrintLine("var phoClassIns = vm.DataStack.GetNativeInstance<",c.Name,">(0);");
                    codeGen.PrintLine();

                    // 获取输入参数
                    int argIndex =1;
                    foreach( var p in m.InputParameters)
                    {
                        codeGen.PrintLine("var ", p.Name, " = vm.DataStack.Get", p.KindString,"(", argIndex, ");");                        

                        argIndex++;
                    }

                    if ( argIndex > 1)
                    {
                        codeGen.PrintLine();
                    }

                    // 声明输出参数
                    argIndex = 1;
                    foreach (var p in m.OutputParameters)
                    {
                        codeGen.PrintLine(p.NativeKindString, " ", p.Name, ";");                        

                        argIndex++;
                    }

                    // 调用本体函数
                    codeGen.BeginLine();

                    if ( m.RetParameter.Kind != ValueKind.Nil)
                    {
                        codeGen.Print("var phoRetArg = ");
                    }


                    
                    codeGen.Print("phoClassIns.", m.Name, "( ");

                    // 传入输入参数
                    argIndex = 1;
                    foreach (var p in m.InputParameters)
                    {
                        if ( argIndex > 1 )
                        {
                            codeGen.Print(", ");
                        }

                        codeGen.Print(p.Name );

                        argIndex++;
                    }

                    // 传入输出参数
                    foreach (var p in m.OutputParameters)
                    {
                        if (argIndex > 1)
                        {
                            codeGen.Print(", ");
                        }

                        codeGen.Print( "out ",p.Name );

                        argIndex++;
                    }

                    codeGen.Print(" );\n");

                    codeGen.EndLine();

                    // 栈推入输出参数
                    foreach (var p in m.OutputParameters)
                    {                        
                        codeGen.PrintLine("vm.DataStack.Push", p.KindString, "( ",p.Name," );");

                        argIndex++;
                    }

                    int totalRet = m.OutputParameters.Count;

                    // 栈推入返回参数
                    if (m.RetParameter.Kind != ValueKind.Nil)
                    {
                        codeGen.PrintLine("vm.DataStack.Push", m.RetParameter.KindString, "( phoRetArg );");
                        totalRet++;
                    }

                    
                    

                    // 返回返回值数量
                    codeGen.PrintLine();

                    codeGen.PrintLine("return ", totalRet, ";");
                    

                    codeGen.Out();
                    codeGen.PrintLine("}");
                    codeGen.PrintLine();
                }

                codeGen.Out();
                codeGen.PrintLine("}");
            }

            


            codeGen.Out();
            codeGen.PrintLine("}");


            return codeGen.ToString();
        }
    }
}
