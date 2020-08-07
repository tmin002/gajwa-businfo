using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Media;
using System.Threading;
using System.Windows.Input;
using System.Diagnostics;
using System.Net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Windows;

namespace gajwa_businfo
{

    public static class base_
    {
        //winpe support
        public static bool RUNNING_ON_WINPE = Environment.GetEnvironmentVariable("windir").Contains("X:");
        public static string WINPE_BROWSER_BINARY_LOCATION = Environment.GetEnvironmentVariable("programfiles") + "/GoogleChrome/Chrome.exe";

        //misc
        public static string PWD
           // = RUNNING_ON_WINPE ? Environment.GetEnvironmentVariable("systemdrive") + "/gajwa-businfo" : Directory.GetCurrentDirectory();
            = RUNNING_ON_WINPE ? "C:/gajwa-businfo" : Directory.GetCurrentDirectory();
        public static string ERR_PROGRAM_LOCATION 
         = RUNNING_ON_WINPE ? "X:/gajwa-businfo/gajwa-businfo-err.exe" : PWD + "/gajwa-businfo-err.exe";
        public static string CONFIG_SAVE_TITLE_STRING
        {
            get { return $"#Gajwa-Businfo Standard Config Form\n#Saved at {DateTime.Now.ToString()}"; }
        }
        public static bool SETTINGS_WINDOW_KEY_ACTIVE_STATE
        {
            get { return Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.LeftShift); }
        }
        public static string[] NETWORK_CHECK_PING_DOMAIN_LIST = new string[] { "http://gajwa-h.hs.kr/", "http://m.gbis.go.kr/" }; //index0=gajwahs index1=gbus
        public static int NETWORK_CHECK_TIMEOUT_SEC = 20; //초반에 네트워크 startup_script 실행되고 나서 약 5초후 네트워크 연결되는데, 그 기다릴 딜레이 시간 설정.

        //designer
        public static string DESIGNER_BLUE = "#81D4FA";
        public static string DESIGNER_RED = "#EF9A9A";
        public static string DESIGNER_YELLOW = "#FBC02D";

        //businfo
        public static int BUS057_AVERAGE_TERM_SECONDS = 0;
        public static int BUS_UPDATE_TERM = 5000; //단위는 밀리세컨드
        public static int BUS_FIRST_START_WAIT_TIME = 3000;
        public static int BUSINFO_LOADING_WAIT_TIME = 300;
        public static string BUS057_AVERAGE_FILE_NAME = PWD + "/057-average.txt";
        public static string BUS_SHOW_LIST_FILE_NAME = PWD + "/buslist.txt";
        public static List<string> BUS_SHOW_LIST = new List<string>();

        //foodinfo
        public static string FOODINFO_TMP_FILE = PWD + "/foodinfo_tmp_file";

        //schedule
        public static int SCHEDULE_WATCHER_DELAY = 1000;
        public static int SCHEDULE_WATCHER_EVENT_DECLARE_DELAY = 1000;
        public static List<Schedule> DEFAULT_SCHEDULES = new List<Schedule>() { };
        public static List<Schedule> SPECIAL_SCHEDULES = new List<Schedule>() { };
        public static ConfigContainer SCHEDULES_CONFIGCONTANER = new ConfigContainer();
        public static string SCHEDULES_FILE_LOCATION = PWD + "/schedule.txt";
        public static Schedule TODAY_SCHEDULE
        {
            get 
            {

                bool special = false;
                foreach (Schedule i in base_.SPECIAL_SCHEDULES)
                {
                    if (DateTime.Now.ToString("yyyy-MM-dd") == i.Date)
                    {
                        special = true;
                        return i;
                       // break;
                    }
                }

                if (!special) foreach (Schedule i in base_.DEFAULT_SCHEDULES)
                    {
                        if (DateTime.Now.DayOfWeek.ToString().ToUpper() == i.DayOfWeek.ToUpper())
                        {
                            return i;
                            //break;
                        }
                    }

                return null;
            }
        }

        



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

        public static bool IsNetworkAvailable(string pingurl = "https://google.com")
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead(pingurl))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public static void DriverNavigate(ChromeDriver driver, string url) //todo: 아직 driver 관련 설정 제어 못해서 driver 관련 함수 호출전마다 이렇게할수밖에 없음.
        {

            var pingUrl = "https://google.com";

            if (url.Contains("gbis.go.kr")) pingUrl = "http://m.gbis.go.kr/";
            if (url.Contains("gajwa-h.hs.kr")) pingUrl = "http://gajwa-h.hs.kr/";

            if (IsNetworkAvailable(pingUrl))
            {
                driver.Url = url;
            }
            else
            {
                NetworkLossBehavior();
            }
            
        }

        public static void NetworkLossBehavior()
        {
            d.write("Network Loss detected, terminating application");
            Environment.Exit(1);
        }

        public static Brush HexColorToSolidBrush(string hex)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
        }

        public static void RebootComputer()
        {
            if (base_.RUNNING_ON_WINPE)
            {
                Process.Start("wpeutil", "reboot");
            }
            else
            {
                Process.Start("shutdown", "-r -t 0");
            }
        }

        public static string ConvertEnglishToKoreanDayString(string day) //0~6
        {

            var raw = day.ToUpper();

            switch (raw)
            {
                case "MONDAY":
                    return "월요일";
                case "TUESDAY":
                    return "화요일";
                case "WEDNESDAY":
                    return "수요일";
                case "THURSDAY":
                    return "목요일";
                case "FRIDAY":
                    return "금요일";
                case "SATURDAY":
                    return "토요일";
                case "SUNDAY":
                    return "일요일";
                default:
                    return null;
            }

        }

        public static ChromeDriver GetChromeDriver()
        {
            ChromeOptions options = new ChromeOptions();

            if (base_.RUNNING_ON_WINPE)
            {
                options.BinaryLocation = base_.WINPE_BROWSER_BINARY_LOCATION;
            }

            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            options.AddArgument("headless");

            return new ChromeDriver(service, options);
        }

        public static void LoadSchedules()
        {
            ConfigContainer config = ConfigLoader.LoadConfigFromFile(base_.SCHEDULES_FILE_LOCATION);
            base_.SCHEDULES_CONFIGCONTANER = config;

            foreach (ConfigGroup i in config.Items)
            {

                Schedule sc = new Schedule();

                //아이템 추가
                List<string> AllItems = new List<string>() {"active", "tvon", "tvoff", "clock", "food", "bus", "firstbelltime", "reboot" };
                List<string> reachableItems = new List<string>();

                foreach (string j in AllItems)
                {
                    if (i.FindItem(j) != null) reachableItems.Add(j);
                }


                foreach (string j in reachableItems)
                {
                    switch (j)
                    {

                        case "active":
                            sc.Enable
                                = i.FindItem("active") == "true"; //사실 이부분이 좀 걸리긴 하는데 뭐 괜찮겠지. 
                            break;

                        case "tvon":
                            sc.ScreenOnTime = Schedule.ParseStringToTime(i.FindItem("tvon")); break;

                        case "tvoff":
                            sc.ScreenOffTime = Schedule.ParseStringToTime(i.FindItem("tvoff")); break;

                        case "clock":
                            sc.ClockScreenOnTime = Schedule.ParseStringToTime(i.FindItem("clock").Split('-')[0]);
                            sc.ClockScreenOffTime = Schedule.ParseStringToTime(i.FindItem("clock").Split('-')[1]);
                            break;

                        case "food":
                            sc.FoodScreenOnTime = Schedule.ParseStringToTime(i.FindItem("food").Split('-')[0]);
                            sc.FoodScreenOffTime = Schedule.ParseStringToTime(i.FindItem("food").Split('-')[1]);
                            break;

                        case "bus":
                            sc.BusScreenOnTime = Schedule.ParseStringToTime(i.FindItem("bus").Split('-')[0]);
                            sc.BusScreenOffTime = Schedule.ParseStringToTime(i.FindItem("bus").Split('-')[1]);
                            break;

                        case "firstbelltime":
                            sc.BellRingTime = Schedule.ParseStringToTime(i.FindItem("firstbelltime")); break;

                        case "reboot":
                            sc.RebootTime = Schedule.ParseStringToTime(i.FindItem("reboot")); break;

                    }
                }

                //분류 & 추가

               if (i.GroupName.Contains("custom"))
                {
                    sc.Date = i.GroupName.Replace("custom_", "");
                    base_.SPECIAL_SCHEDULES.Add(sc);
                }
               else
                {
                    sc.DayOfWeek = i.GroupName;
                    base_.DEFAULT_SCHEDULES.Add(sc);
                }

            }


        }


    }


}







namespace gajwa_businfo.src { } //visual studio가 컴파일할때 이거 없으면 오류뿜음. gajwa_businfo.src 네임스페이스가 단 하나라도 있어야됨.
