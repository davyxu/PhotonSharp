using System.Collections.Generic;
using System.Diagnostics;

namespace Photon
{
    public class SourceFile
    {
        List<string> _sourceLine = new List<string>();

        string _src;

        public string Source
        {
            get { return _src; }
        }

        public List<string> Lines
        {
            get { return _sourceLine; }
        }

        public SourceFile( string src )
        {
            _src = src;

            var lines = src.Split('\r');            
            foreach (var line in lines)
            {
                string trimedLine;
                if (line.Length > 0 && line[0] == '\n')
                {
                    trimedLine = line.Substring(1);
                }
                else
                {
                    trimedLine = line;
                }

                _sourceLine.Add(trimedLine);                                
            }
        }

        public string GetLine( int line )
        {
            if ( line == 0 )
            {
                return string.Empty;
            }

            return _sourceLine[line - 1];
        }



        public void DebugPrint( )
        {
            Debug.WriteLine("source:");
            var lineCount = 0;
            foreach( var line in _sourceLine )
            {
                lineCount++;
                Debug.Print("{0} {1}", lineCount, line);
            }

            Debug.WriteLine("");
        }
    }
}
