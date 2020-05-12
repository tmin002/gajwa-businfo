using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.IO;
using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Threading;

namespace gajwa_businfo
{
    public class BusInfo
    {
        public string busname = "";
        public string bustime = "";
        public string busstatus = "";


        public BusInfo(string name, string time, string status)
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
        private static IWebDriver driver = new FirefoxDriver();

        public static void UpdateBusInfoList()
        {
           
            List<string> busname = new List<string>(); //n번
            List<string> bustime = new List<string>(); //n분, 빈칸이면 차고지 or 정보없음
            List<string> busstatus = new List<string>(); //n번째 정류소 전 or 차고지대기 or 정보없음


            BusInfoList = new List<BusInfo>();
            driver.Url = websiteURL;
            Thread.Sleep(300);


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
            }

           // BusInfo.PrintBusInfoList();


        }

        public static void PrintBusInfoList()
        {
            foreach (BusInfo i in BusInfoList) d.write($"name={i.busname}, time={i.bustime}, status={i.busstatus}");
        }

    }
}