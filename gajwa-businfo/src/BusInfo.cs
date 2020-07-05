using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.IO;
using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace gajwa_businfo
{
    public class BusInfo
    {
        public string busname = "";
        public string bustime = "";
        public string busstatus = "";


        public BusInfo(string name, string time, string status) //얘는 그냥 컨테이너 역할.
        {
            busname = name;
            bustime = time;
            busstatus = status;

        }

        public static BusInfo FindByBusName(string name)
        {
            foreach (BusInfo i in BusInfoList)
            {

                if (i.busname == name) return i;
            }

            return null;
        }

        //staticfff

        public static List<BusInfo> BusInfoList = new List<BusInfo>();
        public static string websiteURL =
        "http://m.gbis.go.kr/search/StationArrivalViaList.do?stationId=219001119";

        private static IWebDriver driver;
        private static bool FirstLaunch = true;

        public static void CloseDriver()
        {
            driver.Quit();
            FirstLaunch = true;
        }

        public static void UpdateBusInfoList(bool Do057only = false)
        {
           
            List<string> busname = new List<string>(); //n번
            List<string> bustime = new List<string>(); //n분, 빈칸이면 차고지 or 정보없음
            List<string> busstatus = new List<string>(); //n번째 정류소 전 or 차고지대기 or 정보없음

            if (FirstLaunch) 
            {

                ChromeOptions options = new ChromeOptions();

                if (base_.RUNNING_ON_WINPE)
                {
                    options.BinaryLocation = base_.WINPE_BROWSER_BINARY_LOCATION;
                }

                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;

                options.AddArgument("headlessㅁ");

                driver = new ChromeDriver(service, options);

                FirstLaunch = false;
            }

            BusInfoList = new List<BusInfo>();
            driver.Url = websiteURL;

            Thread.Sleep(base_.BUSINFO_LOADING_WAIT_TIME);


            foreach (IWebElement i in driver.FindElements(By.ClassName("bus-num-y"))) busname.Add(i.Text);

            foreach (IWebElement j in driver.FindElements(By.ClassName("predict-time")))
            {
                if (j.Text.Contains("잠시후"))
                {
                    bustime.Add("잠시후");
                }
                else
                {
                    bustime.Add(j.Text);
                }

            }

            foreach (IWebElement k in driver.FindElements(By.ClassName("bus-location")))
            {
                if (k.Text.Contains("정류소 전"))
                {
                    busstatus.Add(k.Text.Replace("(", "").Replace("번째 정류소 전)", "") + "전");
                }
                else
                {
                    busstatus.Add(k.Text);
                }

            }


            for (int i = 0; i < busname.Count; i++)
            {

                BusInfoList.Add(new BusInfo(busname[i], bustime[i], busstatus[i]));
                //BusInfoList.Add(new BusInfo("055", "잠시후 도착", "3전"));
            }

            // BusInfo.PrintBusInfoList();
            //driver.Quit();

        }

        public static void PrintBusInfoList()
        {
            foreach (BusInfo i in BusInfoList) d.write($"name={i.busname}, time={i.bustime}, status={i.busstatus}");
        }

    }
}