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
            //todo: 스케줄에 따라 자동으로 페이지 보여지게 만들기

           // BusInfoPage a = new BusInfoPage();
            //this.Content = a;
            //ClockPage b = new ClockPage();
            FoodInfoPage a = new FoodInfoPage();
            this.Content = a;
            

            


        }
        private void BusListPanel_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
