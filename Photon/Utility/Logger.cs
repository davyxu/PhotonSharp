using System;
using System.Diagnostics;

namespace Photon
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warnning,
        Error,        
    }

    public class Logger
    {
        public static bool OutputToDebugger = true;

        public static bool OutputToConsole = false;

        static void WriteLine(LogLevel level, string msg)
        {
            if (OutputToDebugger)
            {
                Debug.WriteLine(msg);
            }

            if (OutputToConsole)
            {
                Console.WriteLine(msg);
            }
        }        

        public static void DebugLine(string fmt, params object[] args)
        {
            var text = string.Format(fmt, args);

            WriteLine(LogLevel.Debug, text);           
        }

        public static void InfoLine(string fmt, params object[] args)
        {
            var text = string.Format(fmt, args);

            WriteLine(LogLevel.Info, text);
        }

        public static void WarningLine(string fmt, params object[] args)
        {
            var text = string.Format(fmt, args);

            WriteLine(LogLevel.Warnning, text);
        }

        public static void ErrorLine(string fmt, params object[] args)
        {
            var text = string.Format(fmt, args);

            WriteLine(LogLevel.Error, text);
        }

        
    }
}
