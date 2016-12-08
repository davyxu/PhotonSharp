using System.Collections.Generic;

namespace Photon
{
    public class SourceFile
    {
        List<string> _sourceLine = new List<string>();

        string _src;

        string _name;

        public string Source
        {
            get { return _src; }
        }

        public string Name
        {
            get { return _name; }
        }

        public List<string> Lines
        {
            get { return _sourceLine; }
        }

        public SourceFile( string src, string name )
        {
            _src = src;
            _name = name;

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

            var final =line - 1;

            if (final < 0 || final >= _sourceLine.Count)
                return string.Empty;

            return _sourceLine[line - 1];
        }



        public void DebugPrint( )
        {
            Logger.DebugLine("source:");
            var lineCount = 0;
            foreach( var line in _sourceLine )
            {
                lineCount++;
                Logger.DebugLine(string.Format("{0} {1}", lineCount, line));
            }

            Logger.DebugLine("");
        }
    }
}
