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
using System.IO;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace errshow
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

            string[] raw = Environment.GetCommandLineArgs();

            try
            {

                switch (raw[1])
                {

                    case "##@#gajwa##$#--NETWORK_CONNECTION_FAILURE":

                        lab.Content = "네트워크 장애";
                        break;


                    case "##@#gajwa##$#--NOT-SERVICE-TIME":

                        lab.Content = "서비스 시간대가 아닙니다.";
                        break;

                    default:

                        break;


                }
            }
            catch
            {

            }



            foreach (string i in raw)
            {
                if (!i.Contains("##@#gajwa##$#--") && i != Environment.GetCommandLineArgs()[0])
                {
                    MessageBox.Show(i);
                    tt.Content += i+" ";
                }
            }
        }

        private void t_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
