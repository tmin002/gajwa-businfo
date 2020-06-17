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

        private List<BusListPanel> BusListPanelList = new List<BusListPanel>();
        private Thread BusListPanelsUpdateThread;
        private bool BusListPanelsUpdateThread_Stop = false;
        private delegate void de();

        public MainWindow()
        {
            InitializeComponent();

            this.Top = 0; this.Left = 0;

            //업데이트
            BusInfo.UpdateBusInfoList();
            base_.Update057Average(0, true);
            base_.UpdateBusShowList();

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
                        TopLine.Y1 + bus_list_panel_add_cnt * b.Height,
                        0,
                        this.Height - (TopLine.Y1 + bus_list_panel_add_cnt * b.Height + b.Height)

                        );



                    BusListPanelList.Add(b);
                    bus_list_panel_add_cnt += 1;

                }
            }

            //BusListPanel 업데이트 쓰레드 설정
            BusListPanelsUpdateThread = new Thread(UpdateBusListPanels);
            BusListPanelsUpdateThread.Start();



        }

        private void UpdateBusListPanels() //외부 쓰레드에서 호출되는 함수
        {

            while (!BusListPanelsUpdateThread_Stop)
            {
                BusInfo.UpdateBusInfoList();

                foreach (BusListPanel b in BusListPanelList)
                {
                    this.Dispatcher.Invoke(new de(() => b.SetPanelBusInfo(BusInfo.FindByBusName(b.BusName.Content.ToString()))));
                }

                Thread.Sleep(base_.BUS_UPDATE_TERM);
            }

            BusListPanelsUpdateThread_Stop = false;

        }

        public void StopUpdateBusListPanelThread() => BusListPanelsUpdateThread_Stop = true;
        private void BusListPanel_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
