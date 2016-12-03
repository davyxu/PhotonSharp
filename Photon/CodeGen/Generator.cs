using System;
using System.IO;

namespace Photon
{
    public class WrapperCodeGenerator
    {
        public void GenerateClass(Type cls, string namespaceName, string filename)
        {
            var genPkg = new WrapperGenPackage();

            WrapperCodeCollector.CollectClassInfo(genPkg, cls, filename);

            var content = WrapperCodePrinter.Print(genPkg, namespaceName);

            Directory.CreateDirectory(Path.GetDirectoryName(filename));
            System.IO.File.WriteAllText(filename, content, System.Text.Encoding.UTF8);
        }

    }
}
