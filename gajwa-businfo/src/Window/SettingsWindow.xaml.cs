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
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Threading;
using gajwa_businfo.src;

namespace gajwa_businfo
{
    /// <summary>
    /// 
    /// SettingsWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SettingsWindow : Window
    {

        private delegate void de();
        private bool LogUpdateExit = false;
        public SettingsWindow()
        {
            InitializeComponent();

            Task.Factory.StartNew(() =>
            {
                while (!LogUpdateExit)
                {
                    Thread.Sleep(100);
                    Dispatcher.Invoke(new de(() => {

                        activelog.Content = d.LOG_LAST;
                        timelabel.Content = DateTime.Now.ToString("yyyy-MM-dd ddd HH:mm:ss");

                        }));

                }
            });


        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;

                case Key.D0:
                    base_.RebootComputer();
                    break;

                case Key.D1:
                    base_.ShutdownComputer();
                    break;

                case Key.D2:
                    Process.Start("notepad", base_.SCHEDULES_FILE_LOCATION);
                    break;

                case Key.D3:
                    Process.Start("cmd");
                    break;

                case Key.D4:

                    int h = DateTime.Now.Hour;
                    int m = DateTime.Now.Minute;

                    ScheduleManager.ManuallyUpdateScheduleKeys(new Schedule()
                    { 
                    
                        ScreenOnTime = new int[] { h, m+1 },
                        ScreenOffTime = new int[] { h, m+5 },
                        ClockScreenOnTime = new int[] { h, m+1 },
                        ClockScreenOffTime = new int[] { h, m },
                        FoodScreenOnTime = new int[] { h, m+2 },
                        FoodScreenOffTime = new int[] { h, m+4 },
                        BusScreenOnTime = new int[] { h, m+4 },
                        BusScreenOffTime = new int[] { h, m+5 },
                        RebootTime = new int[] { h, m+2 }

                    });

                    d.write("Schedule reset due to function test.");
                    break;

                case Key.D5:
                    Process.Start(base_.PWD + "/gajwa-businfo.exe");
                    Environment.Exit(0);
                    break;

                case Key.D6:
                    ArduinoSerialControl.ToggleTVpower();
                    break;

                case Key.D7:
                    MessageBox.Show("WIP");
                    break;

            }


        }

        private void TerminateThread(object sender, EventArgs e)
        {
            LogUpdateExit = true;
        }
    }
}
