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

        private static ChromeDriver driver;
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
            driver = base_.GetChromeDriver();

            //식단 리스트 가져오기
            base_.DriverNavigate(driver, "http://gajwa-h.hs.kr/");

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

            //이미지 가져오기
            Thread UpdateFoodImageThread = new Thread(UpdateFoodImage);
            UpdateFoodImageThread.Start();

        }

        private void UpdateFoodImage()
        {
            while (!stopupdate)
            {
                try {

                    if (File.Exists(base_.FOODINFO_TMP_FILE)) File.Delete(base_.FOODINFO_TMP_FILE);

                    var driver2 = base_.GetChromeDriver();
                    base_.DriverNavigate(driver2, $"http://gajwa-h.hs.kr/_File/up_file/gajwa-h.hs.kr/meal/{DateTime.Now.ToString("yyyyMMdd")}_2.jpg");
                    var imgelement = driver2.FindElementsByTagName("img")[0];
                    var imgelementattr = imgelement.GetAttribute("src");

                    if (imgelementattr.Contains("error"))
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    else
                    {

                        driver2.Manage().Window.Size = new System.Drawing.Size(959, 721);

                        Screenshot ss = ((ITakesScreenshot)driver2).GetScreenshot();
                        ss.SaveAsFile(base_.FOODINFO_TMP_FILE,
                        ScreenshotImageFormat.Png); //이게 사용중이어서 foodinfopage 재실행시 그림 못받아옴. todo

                    }


                    this.Dispatcher.Invoke(new de(() =>
                    {

                        FoodImage.Background = new ImageBrush(new BitmapImage(new Uri(base_.FOODINFO_TMP_FILE)));

                    }
                    ));

                    driver2.Quit();
                    break; //여기까지 온거는 성공한거임.

                }
                catch (Exception ex)
                {
                    d.write($"FoodInfoPage: {ex.Message}");
                    break;
                }

            }
            stopupdate = false;
        }

        
    }
}
