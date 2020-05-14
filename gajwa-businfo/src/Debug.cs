using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace gajwa_businfo
{
    public static class d //debug
    {

        public static bool ENABLE_LOGGING = false;
        public static string LOG_PATH = "/home/tmin002/Desktop/log.txt";
        public static void write(object txt)
        {
            string text = $"[{DateTime.Now.ToString()}] {txt.ToString()}";
            Console.WriteLine(text);

            if (ENABLE_LOGGING) File.AppendAllText(LOG_PATH, text + "\n");

        }
    }
}
