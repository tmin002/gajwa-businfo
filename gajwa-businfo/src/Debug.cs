using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace gajwa_businfo
{
    public static class d //debug
    {

        public static bool ENABLE_LOGGING = true;
        public static string LOG_PATH = Environment.GetEnvironmentVariable("systemdrive") + "/gajwa-businfo-log.txt";
        public static void write(object txt)
        {
            string text = $"[{DateTime.Now.ToString()}] {txt.ToString()}";
            Console.WriteLine(text);

            if (ENABLE_LOGGING) File.AppendAllText(LOG_PATH, text + "\n");

        }
    }
}
