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
using OpenQA.Selenium.Chrome;
using System.Threading;


namespace gajwa_businfo
{
    /// <summary>
    /// FoodInfoPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FoodInfoPage : Page
    {

        private static IWebDriver driver;
        private List<string> foodList = new List<string>() { "", "", "", "", "", "" };
        private delegate void de();
        private bool stopupdate = false;

        public FoodInfoPage() //todo: 급식 이미지 가져오는거 더 좋은 방법으로 바꾸기. null값 진짜값 구분할수 있게끔.
        {

            this.IsVisibleChanged += (object sender, DependencyPropertyChangedEventArgs e) => //sender, e는 사실상 사용되지 않는다.
            {
                if (this.Visibility == Visibility.Hidden) stopupdate = true;
            };


            InitializeComponent();
            TitleLabel.Content = DateTime.Now.ToString($"M월 dd일 {base_.ConvertEnglishToKoreanDayString(DateTime.Now.DayOfWeek.ToString())} 급식"); 

            ChromeOptions options = new ChromeOptions();

            if (base_.RUNNING_ON_WINPE)
            {
                options.BinaryLocation = base_.WINPE_BROWSER_BINARY_LOCATION;
            }

            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            options.AddArgument("headlessㅁ");

            driver = new ChromeDriver(service, options);

            //이미지 가져오기
            Thread t = new Thread(UpdateFoodImage);
            t.Start();

            //식단 리스트 가져오기
            driver.Url = "http://gajwa-h.hs.kr/";

            var w_cnt = 0;
            foreach (IWebElement i in driver.FindElements(By.TagName("pre")))
            {
                if (i.Text.Contains("/")) //찾았으면 여기한번하고 끝내기
                {
                    string rawLine = i.Text.Replace('/','\n');
                    StringReader sr = new StringReader(rawLine);

                    while (sr.Peek() >= 0)
                    {
                        foodList[w_cnt] = sr.ReadLine();
                        w_cnt++;
                    }

                    
                }

            }

            driver.Quit(); //한번 끝나면 새 페이지 객체 선언 필요.

            List<Label> Labels = new List<Label>() {Label1, Label2, Label3, Label4, Label5, Label6 };

            for(int i=0; i<6; i++)
            {
                if (foodList[i] != "") Labels[i].Content = foodList[i];
            }



        }

        private void UpdateFoodImage()
        {
            while (!stopupdate)
            {
                this.Dispatcher.Invoke(new de(() =>
                FoodImage.Background = new ImageBrush(new BitmapImage(new Uri($"http://gajwa-h.hs.kr/_File/up_file/gajwa-h.hs.kr/meal/{DateTime.Now.ToString("yyyyMMdd")}_2.jpg")))));
                Thread.Sleep(1000);
            }
            stopupdate = false;
        }

        
    }
}
