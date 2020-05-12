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
using System.IO;
using System.Threading;
namespace gajwa_businfo
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Top = 0; this.Left = 0;

            base_.updateThread = new Thread(UpdateContent);
            base_.clockThread = new Thread(UpdateClock);
            base_.updateThread.Start();
            base_.clockThread.Start();

            Bus057AverageUpdater(0, true);
  
        }

        private delegate void de();



        private void UpdateClock()
        {
            while (true)
            {
                if (base_.STOP_EVERYTHING)
                {
                    Thread.Sleep(2000);
                    continue;
                }


                this.Dispatcher.Invoke(new de(() =>
               {

                  string min = Math.Truncate(Bus057TermCountThread.Count / 60).ToString();
                  string sec = (Bus057TermCountThread.Count % 60).ToString();

                   if (Bus057TermCountThread.Count != 0)
                   {

                       timer057.Visibility = Visibility.Visible;
                       timercap2.Visibility = Visibility.Visible; 

                       if (min.Length == 1) min = "0" + min;
                       if (sec.Length == 1) sec = "0" + sec;

                       timer057.Content = min + ":" + sec;

                       timercap1.Content = "마지막 출발로부터";
                   }
                   else
                   {
                       timer057.Visibility = Visibility.Collapsed;
                       timercap2.Visibility = Visibility.Collapsed;

                       timercap1.Content = "출발정보 축적중입니다.";

                   }

               }));
                Thread.Sleep(500);
            }
        }


        private void Bus057AverageUpdater(Single elapsed_secs, bool updateOnly = false)
        {
            //!!!!주의: 초기화시 "0,0"이라고 써진 057-average.txt를 실행파일 옆에 반드시 저장할 것!!!!
            //(시행 횟수),(마지막으로 구한 평균)

            string raw = File.ReadAllText(base_.BUS057_AVERAGE_FILE_NAME);
            int cnt = Convert.ToInt32(raw.Split(',')[0]) + 1;
            int lastavg = Convert.ToInt32(raw.Split(',')[1]);
            int avg = (int)(((cnt - 1) * lastavg + elapsed_secs) / cnt);

            if (updateOnly)
            {
                this.Dispatcher.Invoke(new de(() => {
                    avg057.Content = "약 " + Math.Round((double)lastavg / 60, 2).ToString() + "분";

                }));

            }
            else
            {
                this.Dispatcher.Invoke(new de(() => {
                    avg057.Content = "약 " + Math.Round((double)avg / 60, 2).ToString() + "분";

                }));

                File.WriteAllText(base_.BUS057_AVERAGE_FILE_NAME, cnt.ToString() + "," + avg.ToString());
            }

        }


        private static string LastBus057time = "";
        private void UpdateContent()
        {
             while (true)
            {
                    if (base_.STOP_EVERYTHING)
                    {
                        Thread.Sleep(2000);
                        continue;
                    }

                    BusInfo.UpdateBusInfoList();

                    base_.BUSINFO_UPDATE_CNT += 1;
                    d.write($"businfoupdate cnt={base_.BUSINFO_UPDATE_CNT.ToString()}");
                    BusInfo.UpdateBusInfoList();
                   // BusInfo.PrintBusInfoList();

                    /////////////////
                    ///여기서부터는 057 평균 배차간격 계산 부분
                    /////////////////
                    var bsinfo = BusInfo.FindByBusName("057");

                    //d.write($"last={LastBus057time}, this={bsinfo.bustime}");

                    if (bsinfo.bustime.Replace(" ", "").Replace(".", "").Replace("    ", "").Length == 0) //가끔 빈 값이 올때 발생하는 오류를 사전방지
                    {
                        d.write("[057watcher] null string received, trying again after 1000ms.");
                        Thread.Sleep(5000); //5초 쉬었다 재시도
                                            //Bus057TermCountThread.StopCount(); //측정값 정확하지 않을수 있으므로 리셋
                        continue;
                    }

                    if (!bsinfo.bustime.Contains("잠시 후") && LastBus057time.Contains("잠시 후")) //버스 도착 순간
                    {
                        d.write("[057watcher] 057 arrived");
                        d.write("[057watcher] Starting new term counter");
                        Bus057TermCountThread.StartCount();
                    }

                    if (bsinfo.bustime.Contains("잠시 후") && !LastBus057time.Contains("잠시 후") && Bus057TermCountThread.Started)
                    {
                        Bus057TermCountThread.StopCount();
                        Bus057AverageUpdater(Bus057TermCountThread.Count);
                        d.write($"[057watcher] term counter stopped, term={Bus057TermCountThread.Count + 60*2}sec, saving result.");
                    }



                    LastBus057time = bsinfo.bustime;
                    Thread.Sleep(1000);

                    ///////////////
                    ///////////////
                    List<Canvas> canvaslist = new List<Canvas>() { bus1, bus2, bus3, bus4, bus5, bus6 };

                    VisibilityToFalse(new List<Canvas>() { bus1, bus2, bus3, bus4, bus5, bus6 });
                    this.Dispatcher.Invoke(new de(() =>
                    {
                        foreach (Canvas i in canvaslist) foreach (UIElement j in i.Children)
                            {
                                j.Visibility = Visibility.Visible;
                            }
                    }));


                    List<BusInfo> displaylist = new List<BusInfo>();
                    foreach (string i in base_.BUS_SHOW_LIST) foreach (BusInfo j in BusInfo.BusInfoList)
                        {
                            //d.write(i + "," + j.busname + "," + (j.busname == i).ToString());
                            if (j.busname == i) displaylist.Add(j);
                        }


                    /////////////////
                    /////////////////
                    if (displaylist.Count >= 1)
                    {
                        VisibilityToTrue(new List<Canvas>() { bus1 });
                        this.Dispatcher.Invoke(new de(() =>
                                 {
                                     busname1.Content = displaylist[0].busname;
                                     status1.Content = displaylist[0].busstatus;
                                     time1.Content = displaylist[0].bustime;
                                     text1.Visibility = Visibility.Hidden;

                                 }));

                        string text_str = "";

                        if (displaylist[0].bustime.Contains("차고")) text_str = "차고지 대기중";
                        if (displaylist[0].bustime.Contains("회차")) text_str = "회차지 대기중";
                        if (displaylist[0].bustime.Contains("운행정보")) text_str = "운행정보없음";

                        if (text_str.Length != 0) this.Dispatcher.Invoke(new de(() =>
                        {
                            status1.Visibility = Visibility.Hidden;
                            time1.Visibility = Visibility.Hidden;
                            text1.Visibility = Visibility.Visible;
                            text1.Content = text_str;

                        }));

                        if (displaylist[0].bustime.Contains("잠시 후"))
                        {
                            this.Dispatcher.Invoke(new de(() =>
                              {
                                  time1.Content = "잠시후";
                                  time1.Foreground = new SolidColorBrush(Color.FromRgb(210, 117, 117));

                              }));
                        }
                        else
                        {
                            this.Dispatcher.Invoke(new de(() =>
                            {
                                time1.Foreground = new SolidColorBrush(Color.FromRgb(114, 157, 210));

                            }));
                        }


                    }
                    if (displaylist.Count >= 2)
                    {
                        VisibilityToTrue(new List<Canvas>() { bus2 });
                        this.Dispatcher.Invoke(new de(() =>
                        {
                            busname2.Content = displaylist[1].busname;
                            status2.Content = displaylist[1].busstatus;
                            time2.Content = displaylist[1].bustime;
                            text2.Visibility = Visibility.Hidden;
                        }));

                        string text_str = "";

                        if (displaylist[1].bustime.Contains("차고")) text_str = "차고지 대기중";
                        if (displaylist[1].bustime.Contains("회차")) text_str = "회차지 대기중";
                        if (displaylist[1].bustime.Contains("운행정보")) text_str = "운행정보없음";

                        if (text_str.Length != 0) this.Dispatcher.Invoke(new de(() =>
                        {
                            status2.Visibility = Visibility.Hidden;
                            time2.Visibility = Visibility.Hidden;
                            text2.Visibility = Visibility.Visible;
                            text2.Content = text_str;

                        }));

                        if (displaylist[1].bustime.Contains("잠시 후"))
                        {
                            this.Dispatcher.Invoke(new de(() =>
                            {
                                time2.Content = "잠시후";
                                time2.Foreground = new SolidColorBrush(Color.FromRgb(210, 117, 117));

                            }));
                        }
                        else
                        {
                            this.Dispatcher.Invoke(new de(() =>
                            {
                                time2.Foreground = new SolidColorBrush(Color.FromRgb(114, 157, 210));

                            }));
                        }


                    }
                    if (displaylist.Count >= 3)
                    {
                        VisibilityToTrue(new List<Canvas>() { bus3 });
                        this.Dispatcher.Invoke(new de(() =>
                        {
                            busname3.Content = displaylist[2].busname;
                            status3.Content = displaylist[2].busstatus;
                            time3.Content = displaylist[2].bustime;
                            text3.Visibility = Visibility.Hidden;
                        }));

                        string text_str = "";

                        if (displaylist[2].bustime.Contains("차고")) text_str = "차고지 대기중";
                        if (displaylist[2].bustime.Contains("회차")) text_str = "회차지 대기중";
                        if (displaylist[2].bustime.Contains("운행정보")) text_str = "운행정보없음";

                        if (text_str.Length != 0) this.Dispatcher.Invoke(new de(() =>
                        {
                            status3.Visibility = Visibility.Hidden;
                            time3.Visibility = Visibility.Hidden;
                            text3.Visibility = Visibility.Visible;
                            text3.Content = text_str;

                        }));

                        if (displaylist[2].bustime.Contains("잠시 후"))
                        {
                            this.Dispatcher.Invoke(new de(() =>
                            {
                                time3.Content = "잠시후";
                                time3.Foreground = new SolidColorBrush(Color.FromRgb(210, 117, 117));

                            }));
                        }
                        else
                        {
                            this.Dispatcher.Invoke(new de(() =>
                            {
                                time3.Foreground = new SolidColorBrush(Color.FromRgb(114, 157, 210));

                            }));
                        }


                    }
                    if (displaylist.Count >= 4)
                    {
                        VisibilityToTrue(new List<Canvas>() { bus4 });
                        this.Dispatcher.Invoke(new de(() =>
                        {
                            busname4.Content = displaylist[3].busname;
                            status4.Content = displaylist[3].busstatus;
                            time4.Content = displaylist[3].bustime;
                            text4.Visibility = Visibility.Hidden;
                        }));

                        string text_str = "";

                        if (displaylist[3].bustime.Contains("차고")) text_str = "차고지 대기중";
                        if (displaylist[3].bustime.Contains("회차")) text_str = "회차지 대기중";
                        if (displaylist[3].bustime.Contains("운행정보")) text_str = "운행정보없음";

                        if (text_str.Length != 0) this.Dispatcher.Invoke(new de(() =>
                        {
                            status4.Visibility = Visibility.Hidden;
                            time4.Visibility = Visibility.Hidden;
                            text4.Visibility = Visibility.Visible;
                            text4.Content = text_str;

                        }));

                        if (displaylist[3].bustime.Contains("잠시 후"))
                        {
                            this.Dispatcher.Invoke(new de(() =>
                            {
                                time4.Content = "잠시후";
                                time4.Foreground = new SolidColorBrush(Color.FromRgb(210, 117, 117));

                            }));
                        }
                        else
                        {
                            this.Dispatcher.Invoke(new de(() =>
                            {
                                time4.Foreground = new SolidColorBrush(Color.FromRgb(114, 157, 210));

                            }));
                        }


                    }
                    if (displaylist.Count >= 5)
                    {
                        VisibilityToTrue(new List<Canvas>() { bus5 });
                        this.Dispatcher.Invoke(new de(() =>
                        {
                            busname5.Content = displaylist[4].busname;
                            status5.Content = displaylist[4].busstatus;
                            time5.Content = displaylist[4].bustime;
                            text5.Visibility = Visibility.Hidden;
                        }));

                        string text_str = "";

                        if (displaylist[4].bustime.Contains("차고")) text_str = "차고지 대기중";
                        if (displaylist[4].bustime.Contains("회차")) text_str = "회차지 대기중";
                        if (displaylist[4].bustime.Contains("운행정보")) text_str = "운행정보없음";

                        if (text_str.Length != 0) this.Dispatcher.Invoke(new de(() =>
                        {
                            status5.Visibility = Visibility.Hidden;
                            time5.Visibility = Visibility.Hidden;
                            text5.Visibility = Visibility.Visible;
                            text5.Content = text_str;

                        }));

                        if (displaylist[4].bustime.Contains("잠시 후"))
                        {
                            this.Dispatcher.Invoke(new de(() =>
                            {
                                time5.Content = "잠시후";
                                time5.Foreground = new SolidColorBrush(Color.FromRgb(210, 117, 117));

                            }));
                        }
                        else
                        {
                            this.Dispatcher.Invoke(new de(() =>
                            {
                                time5.Foreground = new SolidColorBrush(Color.FromRgb(114, 157, 210));

                            }));
                        }


                    }
                    if (displaylist.Count >= 6)
                    {
                        VisibilityToTrue(new List<Canvas>() { bus6 });
                        this.Dispatcher.Invoke(new de(() =>
                        {
                            busname6.Content = displaylist[5].busname;
                            status6.Content = displaylist[5].busstatus;
                            time6.Content = displaylist[5].bustime;
                            text6.Visibility = Visibility.Hidden;
                        }));

                        string text_str = "";

                        if (displaylist[5].bustime.Contains("차고")) text_str = "차고지 대기중";
                        if (displaylist[5].bustime.Contains("회차")) text_str = "회차지 대기중";
                        if (displaylist[5].bustime.Contains("운행정보")) text_str = "운행정보없음";

                        if (text_str.Length != 0) this.Dispatcher.Invoke(new de(() =>
                        {
                            status6.Visibility = Visibility.Hidden;
                            time6.Visibility = Visibility.Hidden;
                            text6.Visibility = Visibility.Visible;
                            text6.Content = text_str;

                        }));

                        if (displaylist[5].bustime.Contains("잠시 후"))
                        {
                            this.Dispatcher.Invoke(new de(() =>
                            {
                                time6.Content = "잠시후";
                                time6.Foreground = new SolidColorBrush(Color.FromRgb(210, 117, 117));

                            }));
                        }
                        else
                        {
                            this.Dispatcher.Invoke(new de(() =>
                            {
                                time6.Foreground = new SolidColorBrush(Color.FromRgb(114, 157, 210));

                            }));
                        }


                    }




                    BusInfo bus057 = new BusInfo("057-search-string", "", "");
                    foreach (BusInfo i in BusInfo.BusInfoList)
                    {
                        if (i.busname == "057") bus057 = i; //057 businfo 리스트에서 뽑음
                    }

                    if (bus057.bustime.Contains("잠시 후")) //057 잠시후냐 아니냐에 따라 보여주는 패널 달라짐
                    {
                        VisibilityToTrue(new List<Canvas> { bus057_active_state_panel });
                        VisibilityToFalse(new List<Canvas> { bus057_normal_state_panel });
                    }
                    else
                    {
                        VisibilityToFalse(new List<Canvas> { bus057_active_state_panel });
                        VisibilityToTrue(new List<Canvas> { bus057_normal_state_panel });
                    }


                Thread.Sleep(5000);
                
            }

        }

        private void VisibilityToFalse(List<Canvas> ControlList) {  //ui쓰레드 아닌 곳에서 사용
            this.Dispatcher.Invoke(new de( () => { 
                foreach (Canvas c in ControlList) c.Visibility = Visibility.Hidden;
            }));
        }
        private void VisibilityToTrue(List<Canvas> ControlList) { //ui쓰레드 아닌 곳에서 사용
            this.Dispatcher.Invoke(new de(() => {
                foreach (Canvas c in ControlList) c.Visibility = Visibility.Visible;
            }));
        }



    }
}
