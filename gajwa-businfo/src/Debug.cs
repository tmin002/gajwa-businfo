using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.NetworkInformation;

namespace gajwa_businfo
{
    public static class d //debug
    {

        public static bool ENABLE_LOGGING = true;
        public static string LOG_PATH = base_.PWD + "/gajwa-businfo-log.txt";
        public static string LOG_LAST = "";
        public static bool NO_REBOOT = true; //todo//disable when distribute

        public static void write(object txt)
        {
            string text = $"[{DateTime.Now}] {txt}";
            Console.WriteLine(text);

            LOG_LAST = txt.ToString();

            if (ENABLE_LOGGING) File.AppendAllText(LOG_PATH, text + "\n");

        }
    }
}
