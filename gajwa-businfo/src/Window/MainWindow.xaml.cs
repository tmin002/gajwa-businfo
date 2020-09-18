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
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using System.Threading;
using gajwa_businfo.src;

namespace gajwa_businfo
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private PageType LastPage = PageType.First;
        private enum PageType { Clock, Food, Bus, Blank, First };
        private delegate void de();

        private Dictionary<PageType, Page> Pages = new Dictionary<PageType, Page>()
        {
            { PageType.Clock, null },
            { PageType.Food, null },
            { PageType.Bus, null },
            { PageType.Blank, null },
        };

        private Dictionary<ScheduleManager.ScheduleChangedEvents, PageType> OffEventTypes = new Dictionary<ScheduleManager.ScheduleChangedEvents, PageType>()
        {
            {ScheduleManager.ScheduleChangedEvents.ClockScreenOff, PageType.Clock },
            {ScheduleManager.ScheduleChangedEvents.BusScreenOff, PageType.Bus },
            {ScheduleManager.ScheduleChangedEvents.FoodScreenOff, PageType.Food }
        };

        //private PageType LastPageType = PageType.Blank; 



        public MainWindow()
        {

            InitializeComponent();
            this.Top = 0; this.Left = 0;

            ScheduleManager.StartWatcher();

            ScheduleManager.ScheduleChanged += ScheduleChangedThread; //이벤트 핸들러


        }
        private void BusListPanel_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private void ScheduleChangedThread(ScheduleChangedEventArgs e)
        {

            if (e.EventType == ScheduleManager.ScheduleChangedEvents.Reboot)
            {
                base_.RebootComputer();
                return;
            }
                

            this.Dispatcher.Invoke(new de(() =>
            {
                if (LastPage != PageType.First) Pages[LastPage].Visibility = Visibility.Hidden;

                switch (e.EventType)
                {

                    case ScheduleManager.ScheduleChangedEvents.ScreenOn:
                        ArduinoSerialControl.ToggleTVpower();
                        break;

                    case ScheduleManager.ScheduleChangedEvents.ScreenOff:
                        ArduinoSerialControl.ToggleTVpower();
                        break;

                    case ScheduleManager.ScheduleChangedEvents.ClockScreenOn:
                        Pages[PageType.Clock] = new ClockPage();
                        LastPage = PageType.Clock;
                        this.Content = Pages[PageType.Clock];
                        break;

                    case ScheduleManager.ScheduleChangedEvents.FoodScreenOn:
                        Pages[PageType.Food] = new FoodInfoPage();
                        LastPage = PageType.Food;
                        this.Content = Pages[PageType.Food];

                        break;

                    case ScheduleManager.ScheduleChangedEvents.BusScreenOn:
                        Pages[PageType.Bus] = new BusInfoPage();
                        LastPage = PageType.Bus;
                        this.Content = Pages[PageType.Bus];
                        break;

                    default:

                        if (LastPage == OffEventTypes[e.EventType])
                        {
                            Pages[PageType.Blank] = new BlankPage();
                            LastPage = PageType.Blank;
                            this.Content = Pages[PageType.Blank];
                        }
                        break;


                }
            }));
        }

        private void SettingsWindowLaunch(object sender, KeyEventArgs e)
        {
            if (base_.SETTINGS_WINDOW_KEY_ACTIVE_STATE)
            {
                SettingsWindow sw = new SettingsWindow();
                sw.ShowDialog();
            }
        }

        private void Terminate(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }

}
