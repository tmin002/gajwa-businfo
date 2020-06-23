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
    public partial class ClockPage : Page
    {

        private bool escape = false;
        private int bellringsec = 20 * 3600 + 32 * 60 + 0;
        private delegate void de();

        public ClockPage()
        {
            InitializeComponent();
            Thread clockThread = new Thread(clockUpdate);
            clockThread.Start();


        }

        private void clockUpdate()
        {
            while (!escape)
            {

                this.Dispatcher.Invoke(new de(() => clockLabel.Content = DateTime.Now.ToString("H:mm:ss"))); 

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
                    this.Dispatcher.Invoke(new de(() => leftLabel.Content = "-" + min + ":" + sec));
                }
                else
                {
                    this.Dispatcher.Invoke(new de(() => leftLabel.Content = "+" + min + ":" + sec));
                }

                Thread.Sleep(300);
            }

        }

        private void StopCounting() => escape = true;
    }
}
