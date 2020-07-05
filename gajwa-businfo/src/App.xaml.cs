using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Diagnostics;
using System.Windows;
using System.Threading;
using System.IO;

namespace gajwa_businfo
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {

        public App()
        {

            Console.WriteLine("BUSINFO-GAJWA v1.0\nbased on selenium.webdriver");
            Console.WriteLine(
                "IMPORTANT: If the developer had already been graduated from gajwa highschool, \n" +
                "or if you're trying to maintain the program without the developer, \n" +
                "visit https://github.com/ssh9930/gajwa-businfo for more information. ");



            /////
            /////
            /////
            d.write("gajwa-businfo launch");

            base_.LoadSchedules();
            base_.Update057Average(0, true);
            base_.UpdateBusShowList();
            ConfigLoader.SaveConfigToFile(base_.SCHEDULES_CONFIGCONTANER, base_.SCHEDULES_FILE_LOCATION);
            
            

        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {

            MessageBox.Show(e.Exception.ToString());

            //a.Start();
            
            //Form1 a = new Form1();
            //a.ShowDialog();
        }


    }
}
