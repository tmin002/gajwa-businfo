using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;


namespace gajwa_businfo
{

    public static class base_
    {

        public static int BUS057_AVERAGE_TERM_SECONDS = 0;
        public static Single BUSINFO_UPDATE_CNT = 0;

        public static string PWD = Directory.GetCurrentDirectory();
        public static string BUS057_AVERAGE_FILE_NAME = PWD + "/057-average.txt";

        public static List<string> BUS_SHOW_LIST = new List<string>();

        public static Thread updateThread; 
        public static Thread clockThread;

        public static bool STOP_EVERYTHING = false;



    }

}