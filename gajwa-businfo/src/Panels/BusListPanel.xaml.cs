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

namespace gajwa_businfo
{
    /// <summary>
    /// BusListPanel.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BusListPanel : UserControl
    {

        public string PanelBusTime
        {
            set //value에는 그냥 스크래핑으로 나온거 다 집어넣어
            {


                var v = true;

                if (value.Contains("차고"))
                {
                    v = false;
                    BusStation.Content = "차고지 대기중";
                }

                if (value.Contains("회차"))
                {
                    v = false;
                    BusStation.Content = "회차점 대기중";
                }

                if (value.Contains("운행"))
                {
                    v = false;
                    BusStation.Content = "운행시간 아님";
                }



                if (v)
                {
                    BusTime.Visibility = Visibility.Visible;
                    BusStation.Foreground = base_.HexColorToSolidBrush(base_.DESIGNER_BLUE);
                }
                else
                {
                    BusTime.Visibility = Visibility.Hidden;
                    BusStation.Foreground = base_.HexColorToSolidBrush(base_.DESIGNER_RED);
                }



                //여기까지 차고지 회자점 운행안함 표시 기능

                if (value.Contains("분"))
                {
                    BusTime.Content = value;
                }

                if (value.Contains("잠시 후"))
                {
                    BusTime.Foreground = base_.HexColorToSolidBrush(base_.DESIGNER_RED);
                    BusTime.Content = "잠시후";

                }
                else
                {
                    BusTime.Foreground = base_.HexColorToSolidBrush(base_.DESIGNER_BLUE);
                }

            }
        }

        public string PanelBusStation
        {
            set { if (value.Replace(" ","").Replace("  ","").Replace("\n","").Length != 0) BusStation.Content = value; }
        }

        public string PanelBusName
        { 
            set { BusName.Content = value; }
        }


        public void SetPanelBusInfo(BusInfo b)
        {
            if (b != null)
            {
                //d.write(b.busname);
                PanelBusName = b.busname;
                PanelBusTime = b.bustime;
                PanelBusStation = b.busstatus;
            }
            else
            {
                PanelBusName = "";
                PanelBusTime = "";
                PanelBusStation = "";
            }

        }


        public BusListPanel()
        {
            InitializeComponent();

        }



    }
}
