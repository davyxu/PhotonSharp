using System;
using System.IO;

namespace Photon
{
    public class WrapperCodeGenerator
    {
        public static void GenerateClass(Type cls, string pkgName, string filename)
        {
            var genPkg = new WrapperGenPackage();
            genPkg.Name = pkgName;
            WrapperCodeCollector.CollectClassInfo(genPkg, cls, filename);

            var content = WrapperCodePrinter.Print(genPkg );

            Directory.CreateDirectory(Path.GetDirectoryName(filename));
            System.IO.File.WriteAllText(filename, content, System.Text.Encoding.UTF8);
        }
    }
}
