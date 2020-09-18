using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Diagnostics;
using System.Windows;
using System.Threading;
using System.Windows.Threading;
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

            Console.WriteLine("BUSINFO-GAJWA v2.0\nbased on selenium.webdriver");
            Console.WriteLine(
                "IMPORTANT: If the developer had already been graduated from gajwa highschool, \n" +
                "or if you're trying to maintain the program without the developer, \n" +
                "visit https://github.com/ssh9930/gajwa-businfo for more information. ");



            /////
            /////
            /////
            ///

            d.write("gajwa-businfo launch");
            d.write("checking vital files");

            string[] vitalFiles = new string[] {"057-average.txt", "buslist.txt", "schedule.txt" };
            string vitalFileChkFailMsg = "Vital file missing: ";
            bool vitalFileChk = true;

            foreach (string i in vitalFiles)
            {
                if (!File.Exists(base_.PWD + "/" + i))
                {
                    vitalFileChkFailMsg += i + ", ";
                    vitalFileChk = false;
                }
            }

            if (!vitalFileChk)
            {
                Process.Start(base_.ERR_PROGRAM_LOCATION, vitalFileChkFailMsg.Replace("\n", "\\n"));
            }

            base_.LoadSchedules();
            base_.Update057Average(0, true);
            base_.UpdateBusShowList();


            /////
            /////
            /////
            ///

            int cntdown = base_.NETWORK_CHECK_TIMEOUT_SEC;
            while (!base_.IsNetworkAvailable())
            {

                if (cntdown <= 0)
                {
                    d.write("Network check timeout, terminating.");
                    Environment.Exit(1);
                }

                cntdown -= 1;
                Thread.Sleep(1000);
            }

            ////
            ////
            ///

            ArduinoSerialControl.OpenConnection(base_.ARDUINO_SERIAL_COMPORT);


        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {

            //될지 모르겠는 기능임.
            //d.write(e.Exception.ToString());

            //Process.Start("C:/Users/team9/Desktop/project/gajwa-businfo/gajwa-businfo/executables/gajwa-businfo-err.exe", e.Exception.ToString().Replace("\n", "\\n"));
             args.Handled = true;
             MessageBox.Show(args.Exception.ToString());

            //a.Start();

            //Form1 a = new Form1();
            //a.ShowDialog();
        }


    }
}
