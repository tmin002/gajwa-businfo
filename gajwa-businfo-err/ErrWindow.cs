using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace gajwa_businfo_err
{
    public partial class ErrWindow : Form
    {
        public delegate void de();


        //from base_
        public static bool RUNNING_ON_WINPE = Environment.GetEnvironmentVariable("windir").Contains("X:");
        public static string PWD
            // = RUNNING_ON_WINPE ? Environment.GetEnvironmentVariable("systemdrive") + "/gajwa-businfo" : Directory.GetCurrentDirectory();
            = RUNNING_ON_WINPE ? "C:/gajwa-businfo" : Directory.GetCurrentDirectory();

        //from base_ and no longer exists at base_
        public static int ERROR_REBOOT_DELAY = 1000 * 10;

        public static void RebootComputer()
        {
            if (RUNNING_ON_WINPE)
            {
                Process.Start("wpeutil", "reboot");
            }
            else
            {
                Process.Start("shutdown", "-r -t 0");
            }
        }



        public ErrWindow()
        {
            InitializeComponent();
        }


        private void RebootCountDownThread()
        {
            var rebootsecs = (int)(ERROR_REBOOT_DELAY / 1000) - 1; //-1 because of line 42

            Thread.Sleep(1000); //wait untill InitializeComponent(); gets done

            while (rebootsecs > 0)
            {
                this.Invoke(new de(() => AutoRebootLabel.Text = $"Automatic reboot in {rebootsecs.ToString()} seconds.\n"
                + "Input an random signal from the text input device to terminate the reboot procedure \nand terminate session."));
                rebootsecs -= 1;
                Thread.Sleep(1000);
            }

            RebootComputer();
        }

        private void ErrWindow_Load(object sender, EventArgs e)
        {

            string[] args = Environment.GetCommandLineArgs();
            string rawtext = "";
            bool reboot = true; //wip feature. todo: add reboot option

            foreach (string i in args) rawtext += i + " ";
            rawtext.Replace("\\n", "\n");

            File.WriteAllText(PWD + $"/gajwa-businfo-crashlog-{DateTime.Now.ToString()}", rawtext);

            ErrText.Text = rawtext;

            if (reboot)
            {
                AutoRebootLabel.Visible = true;
                Thread t = new Thread(RebootCountDownThread);
                t.Start();
            }


            this.ShowDialog();
        }
    }
}
