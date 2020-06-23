using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Media;
using System.Threading;


namespace gajwa_businfo
{

    public static class base_
    {


        public static string DESIGNER_BLUE = "#81D4FA";
        public static string DESIGNER_RED = "#EF9A9A";
        public static string DESIGNER_YELLOW = "#FBC02D";


        public static int BUS057_AVERAGE_TERM_SECONDS = 0;
        public static int BUS_UPDATE_TERM = 5000; //단위는 밀리세컨드
        public static int BUS_FIRST_START_WAIT_TIME = 3000;

        public static string PWD = Directory.GetCurrentDirectory();
        public static string BUS057_AVERAGE_FILE_NAME = PWD + "/057-average.txt";
        public static string BUS_SHOW_LIST_FILE_NAME = PWD + "/buslist.txt";

        public static List<string> BUS_SHOW_LIST = new List<string>();

        public static string FOODINFO_TMP_FILE = PWD + "/foodinfo_tmp_file";


        public static void Update057Average(Single elapsed_secs, bool updateOnly = false)
        {
            string raw = File.ReadAllText(base_.BUS057_AVERAGE_FILE_NAME);
            int cnt = Convert.ToInt32(raw.Split(',')[0]) + 1;
            int lastavg = Convert.ToInt32(raw.Split(',')[1]);


            if (!updateOnly)
            {
                int avg = (int)(((cnt - 1) * lastavg + elapsed_secs) / cnt);
                BUS057_AVERAGE_TERM_SECONDS = avg;
                File.WriteAllText(base_.BUS057_AVERAGE_FILE_NAME, cnt.ToString() + "," + avg.ToString());
            }
            else
            {
                BUS057_AVERAGE_TERM_SECONDS = lastavg;
            }
            
        }

        public static void UpdateBusShowList()
        {
            string rawBusList = File.ReadAllText(BUS_SHOW_LIST_FILE_NAME).Replace(",", "\n");
            StringReader reader = new StringReader(rawBusList);

            while (reader.Peek() >= 0)
            {
                string rawline = reader.ReadLine();
                base_.BUS_SHOW_LIST.Add(rawline);
            }
        }

        public static Brush HexColorToSolidBrush(string hex)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
        }

    }

}