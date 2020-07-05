using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Threading;
using System.Windows.Shapes;

namespace gajwa_businfo
{
    /// <summary>
    /// ClockPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ClockPage
    {

        private bool escape = false;
        private int bellringsec
            = base_.TODAY_SCHEDULE.BellRingTime[0] * 3600 + base_.TODAY_SCHEDULE.BellRingTime[1] * 60;
        private delegate void de();

        public ClockPage()
        {

            this.IsVisibleChanged += (object sender, DependencyPropertyChangedEventArgs e) => //sender, e는 사실상 사용되지 않는다.
            {
                if (this.Visibility == Visibility.Hidden) StopCounting();
            };


            InitializeComponent();
            Thread clockThread = new Thread(clockUpdate);
            clockThread.Start();

           // this.dis

        }


        private void clockUpdate()
        {
            while (!escape)
            {

                this.Dispatcher.Invoke(new de(() =>
                {
                    DatePanel.TimeLabel.Content = DateTime.Now.ToString("H:mm:ss");
                    DatePanel.DateLabel.Content = DateTime.Now.ToString($"yyyy년 MM월 dd일 {base_.ConvertEnglishToKoreanDayString(DateTime.Now.DayOfWeek.ToString())}");
                }
                )); 


                int tsec = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
                int lsec = bellringsec - tsec;
                bool d = lsec > 0;

                lsec = Math.Abs(lsec);

                string min = Math.Truncate((double)lsec / 60).ToString();
                string sec = (lsec % 60).ToString();

                if (min.Length == 1) min = "0" + min;
                if (sec.Length == 1) sec = "0" + sec;

                if (d)
                {
                    this.Dispatcher.Invoke(new de(() => leftLabel.Content = min + ":" + sec));
                }
                else
                {
                    this.Dispatcher.Invoke(new de(() => leftLabel.Content = min + ":" + sec));
                }

                Thread.Sleep(300);
            }
            escape = false;

        }

        private void StopCounting() => escape = true;
    }
}






public abstract class GeeksForGeeks
{

    // abstract method 'gfg()' 
    public abstract void gfg();

}

// class 'GeeksForGeeks' inherit 
// in child class 'Geek1' 
public partial class Geek1 : GeeksForGeeks
{

    // abstract method 'gfg()'  
    // declare here with  
    // 'override' keyword 
    public override void gfg()
    {
        throw new NotImplementedException();
    }

}