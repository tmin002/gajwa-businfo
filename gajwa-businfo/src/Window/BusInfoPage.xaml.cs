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
namespace gajwa_businfo
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BusInfoPage : Page
    {

        private List<BusListPanel> BusListPanelList = new List<BusListPanel>();
        private Thread BusListPanelsUpdateThread;
        private Thread Bus057UpdateThread;
        private bool Bus057ActiveState = false;
        private int BusThread_RunningState = 0; //0: no thread running, 2: all thread running, 3: all thread stop signal 5: all thread stop
        //한번 멈추면 이 창 객체 재정의 필요.

        private Bus057PanelActiveState Bus057ActivePanel = new Bus057PanelActiveState()
        { Visibility = Visibility.Hidden, Margin = new Thickness(70,200, 0, 0),
            HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top
        };

        private Bus057PanelNormalState Bus057NormalPanel = new Bus057PanelNormalState() 
        { Visibility = Visibility.Hidden, Margin = new Thickness(70,200, 0, 0) ,
            HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top
        };

        private string LastBus057time = "";
        private int Bus057DetectionCount = 0;

        private delegate void de();

        public BusInfoPage()
        {
            InitializeComponent();

            this.IsVisibleChanged += (object sender, DependencyPropertyChangedEventArgs e) => //sender, e는 사실상 사용되지 않는다.
            {
                if (this.Visibility == Visibility.Hidden) StopUpdate();
            };

            //업데이트
            BusInfo.UpdateBusInfoList();
           // base_.Update057Average(0, true); //app.xaml에서 담당.
           // base_.UpdateBusShowList();

            //Bus057Panel 2개 추가

            MainGrid.Children.Add(Bus057ActivePanel);
            MainGrid.Children.Add(Bus057NormalPanel);

            Bus057PanelNormalMode();


            //4개의 BusListPanel

            int bus_list_panel_add_cnt = 0;
            foreach (string i in base_.BUS_SHOW_LIST)
            {
                var fb = BusInfo.FindByBusName(i);

                if (fb != null)
                {
                    BusListPanel b = new BusListPanel();
                    b.SetPanelBusInfo(fb);
                    MainGrid.Children.Add(b);

                    //todo: 4개 이상 버스 있을시 전환효과.

                    b.Margin = new Thickness(

                        this.Width - b.Width, //left, 여기서부터 시계방향으로 돌아감
                        TopPanel.Height + bus_list_panel_add_cnt * b.Height,
                        0,
                        this.Height - (TopPanel.Height + bus_list_panel_add_cnt * b.Height + b.Height)

                        );



                    BusListPanelList.Add(b);
                    bus_list_panel_add_cnt += 1;

                }
            }

            //업데이트 쓰레드 설정
            BusListPanelsUpdateThread = new Thread(UpdateBusListPanels);
            BusListPanelsUpdateThread.Start();

            Thread.Sleep(base_.BUS_FIRST_START_WAIT_TIME);

            Bus057UpdateThread = new Thread(Update057Text);
            Bus057UpdateThread.Start();


        }

        private void UpdateBusListPanels() //외부 쓰레드에서 호출되는 함수
        {
            BusThread_RunningState += 1;

            while (BusThread_RunningState < 3)
            {
                BusInfo.UpdateBusInfoList();

                foreach (BusListPanel b in BusListPanelList)
                {
                    this.Dispatcher.Invoke(new de(() => b.SetPanelBusInfo(BusInfo.FindByBusName(b.BusName.Content.ToString()))));
                }

                Update057();

                Thread.Sleep(base_.BUS_UPDATE_TERM);
            }

            BusThread_RunningState += 1;
            d.write($"UpdateBusListPanels() Termination, BusThread_RunningState={BusThread_RunningState}");

        }

        private void Update057() //외부 쓰레드에서 호출되는 함수, UpdateBusLIstPanels()로부터 호출받음.
        {

            //UpdateBusInfoList() 했다고 가정
            //BusInfo.PrintBusInfoList();
            var bsinfo = BusInfo.FindByBusName("057");

            if (bsinfo != null)
            {


                if (!bsinfo.bustime.Contains("잠시 후") && LastBus057time.Contains("잠시 후")) //버스 도착 순간
                {
                    d.write("[057watcher] 057 arrived");
                    d.write("[057watcher] Starting new term counter");

                    Bus057TermCountThread.StartCount();

                    this.Dispatcher.Invoke(new de(Bus057PanelNormalMode));
                    Bus057DetectionCount += 1;

                }

                if (bsinfo.bustime.Contains("잠시 후"))
                {
                    this.Dispatcher.Invoke(new de(Bus057PanelActiveMode));

                    if (bsinfo.bustime.Contains("잠시 후") && !LastBus057time.Contains("잠시 후") && Bus057TermCountThread.Started)
                    {
                        base_.Update057Average(Bus057TermCountThread.Count + 60 * 2);
                        d.write($"[057watcher] term counter stopped, term={Bus057TermCountThread.Count + 60 * 2}sec, saving result.");
                        Bus057TermCountThread.StopCount();


                    }
                }

                LastBus057time = bsinfo.bustime;

            }
            else
            {
                d.write("[057watcher] null string received, ignoring");
            }

        }

        private void Bus057PanelActiveMode()
        {

            Bus057ActiveState = true;

            Bus057NormalPanel.Visibility = Visibility.Hidden;
            Bus057ActivePanel.Visibility = Visibility.Visible;
            LastArrivalSec.Visibility = Visibility.Visible;
            LastArrivalSec2.Visibility = Visibility.Hidden;

            LastArrivalSec1.Content = "현재 5단지앞 정류장 경유중";
            LastArrivalSec.Content = "도착까지 한정거장 전";
        }

        private void Bus057PanelNormalMode()
        {

            Bus057ActiveState = false;

            Bus057NormalPanel.Visibility = Visibility.Visible;
            Bus057ActivePanel.Visibility = Visibility.Hidden;

            LastArrivalSec2.Visibility = Visibility.Visible;

            LastArrivalSec1.Content = "마지막 출발로부터 ";
            LastArrivalSec2.Content = "경과";
        }

        private void Update057Text() //외부 쓰레드에서 실행됨
        {
            BusThread_RunningState += 1;

            while (BusThread_RunningState < 3)
            { 

                if (Bus057ActiveState)
                {
                    Thread.Sleep(500);
                    continue;
                }

                string min = Math.Truncate(Bus057TermCountThread.Count / 60).ToString();
                string sec = (Bus057TermCountThread.Count % 60).ToString();

                if (min.Length == 1) min = "0" + min;
                if (sec.Length == 1) sec = "0" + sec;


                this.Dispatcher.Invoke(new de(() =>
                {

                    switch (Bus057DetectionCount)
                    {
                        case 0:
                            LastArrivalSec.Visibility = Visibility.Hidden;
                            LastArrivalSec1.Visibility = Visibility.Visible;
                            LastArrivalSec2.Visibility = Visibility.Hidden;

                            LastArrivalSec1.Content = "출발정보 수집중";
                            break;

                        case 1:
                            LastArrivalSec.Visibility = Visibility.Visible;
                            LastArrivalSec1.Visibility = Visibility.Visible;
                            LastArrivalSec2.Visibility = Visibility.Visible;

                            LastArrivalSec.Content = min + ":" + sec;
                            break;

                        default:
                            LastArrivalSec.Content = min + ":" + sec;
                            break;
                    }

                    Average057Term.Content = "약 " + Math.Round((double)base_.BUS057_AVERAGE_TERM_SECONDS / 60, 2).ToString() + "분";


                }));

                Thread.Sleep(500);

            }

            BusThread_RunningState += 1;
            d.write($"Update057Text() Termination, BusThread_RunningState={BusThread_RunningState}");

        }

        public void StopUpdate() //모든걸 멈출때 사용되는 함수
        {

            BusThread_RunningState = 3;

            if (Bus057TermCountThread.Started) Bus057TermCountThread.StopCount();


        }

        private void BusListPanel_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }


}
