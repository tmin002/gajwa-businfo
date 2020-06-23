using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.IO;
using System.Net;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Threading;


namespace gajwa_businfo
{
    /// <summary>
    /// FoodInfoPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FoodInfoPage : Page
    {

        private static IWebDriver driver;
        private List<string> foodList = new List<string>();
        private delegate void de();

        public FoodInfoPage() //todo: 급식 이미지 가져오는거 더 좋은 방법으로 바꾸기. null값 진짜값 구분할수 있게끔.
        {
            InitializeComponent();
            TitleLabel.Content = DateTime.Now.ToString("M월 dd일 dddd 급식"); //시스템 로캘이 반드시 한국어야한다 !!!!

            var f = new FirefoxOptions();
            f.AddArgument("-headless");
            var s = FirefoxDriverService.CreateDefaultService();
            s.HideCommandPromptWindow = true;
            driver = new FirefoxDriver(s, f);

            //이미지 가져오기
            Thread t = new Thread(UpdateFoodImage);
            t.Start();

            //식단 리스트 가져오기
            driver.Url = "http://gajwa-h.hs.kr/";

            foreach (IWebElement i in driver.FindElements(By.TagName("pre")))
            {
                if (i.Text.Contains("/")) //찾았으면 여기한번하고 끝내기
                {
                    string rawLine = i.Text.Replace('/','\n');
                    StringReader sr = new StringReader(rawLine);

                    while (sr.Peek() >= 0)
                    {
                        foodList.Add(sr.ReadLine());
                    }

                    
                }
            }

            foreach (string i in foodList) d.write(i);


            for (int i=0; i<6 - foodList.Count; i++) //6은 최대 표시가능한 반찬 수
            {
                foodList[foodList.Count + i] = "";
            }

            Label1.Content = foodList[0];
            Label2.Content = foodList[1];
            Label3.Content = foodList[2];
            Label4.Content = foodList[3];
            Label5.Content = foodList[4];
            Label6.Content = foodList[5];



        }

        private void UpdateFoodImage()
        {
            this.Dispatcher.Invoke(new de(() =>
            FoodImage.Background = new ImageBrush(new BitmapImage(new Uri($"http://gajwa-h.hs.kr/_File/up_file/gajwa-h.hs.kr/meal/{DateTime.Now.ToString("yyMMdd")}_2.jpg")))));

            Thread.Sleep(1000);
        }

        
    }
}
