using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;

namespace gajwa_businfo_manager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Properties.Settings s = new Properties.Settings();
            textBox1.Text = s.monday_str;
            textBox2.Text = s.tuesday_str;
            textBox3.Text = s.wednesday_str;
            textBox4.Text = s.thursday_str;
            textBox5.Text = s.friday_str;
            textBox6.Text = s.saturday_str;
            textBox7.Text = s.sunday_str;

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings s = new Properties.Settings();
            Properties.Settings.Default.monday_str = textBox1.Text;
            Properties.Settings.Default.tuesday_str = textBox2.Text;
            Properties.Settings.Default.wednesday_str = textBox3.Text;
            Properties.Settings.Default.thursday_str = textBox4.Text;
            Properties.Settings.Default.friday_str = textBox5.Text;
            Properties.Settings.Default.saturday_str = textBox6.Text;
            Properties.Settings.Default.sunday_str = textBox7.Text;
            Properties.Settings.Default.Save();

            MessageBox.Show("설정이 저장되었습니다.", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private List<times> timelist = new List<times>(); //!!

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            TextBox[] lst = { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7 };


            foreach (TextBox i in lst)
            {
                if (i.Text.Replace(" ", "").Replace("    ", "").Length == 0)
                {
                    timelist.Add(new times(-1,0,0,0));
                    continue;
                }


                var rawstart = i.Text.Split('-')[0];
                var rawend = i.Text.Split('-')[1];

                var starth = Convert.ToInt32(rawstart.Split(':')[0]);
                var startm = Convert.ToInt32(rawstart.Split(':')[1]);
                var endh = Convert.ToInt32(rawend.Split(':')[0]);
                var endm = Convert.ToInt32(rawend.Split(':')[1]);

                timelist.Add(new times(starth, startm, endh, endm));
            }

            /////////////////////////

            Thread waitt = new Thread(waitthread);
            waitt.Start();


        }

        private void waitthread()
        {
            writeToRtbox("waitthread 시작됨\n");
            writeToRtbox("월요일부터 일요일 순으로 저장된 설정 출력 (-1은 활성화 안하기) :");
            for (int i = 0; i < timelist.Count; i++)
                writeToRtbox($"#{i} day     {timelist[i].StartHour}:{timelist[i].StartMinute}-{timelist[i].EndHour}:{timelist[i].EndMinute}");
            writeToRtbox("\n이제부터 기다립니다..");

            while (true)
            {
                switch (DateTime.Now.DayOfWeek)
                {

                    case DayOfWeek.Monday:
                        if (timelist[0].StartHour != -1 &&
                           timelist[0].StartHour*60 + timelist[0].StartMinute <= DateTime.Now.Hour*60 + DateTime.Now.Minute &&
                           timelist[0].EndHour * 60 + timelist[0].EndMinute > DateTime.Now.Hour * 60 + DateTime.Now.Minute )

                            Start();

                        if (timelist[0].EndHour != -1 &&
                            timelist[0].EndHour == DateTime.Now.Hour &&
                            timelist[0].EndMinute == DateTime.Now.Minute)
                            Stop();

                        Thread.Sleep(1000);
                        break;

                    case DayOfWeek.Tuesday:
                        if (timelist[1].StartHour != -1 &&
                           timelist[1].StartHour * 60 + timelist[1].StartMinute <= DateTime.Now.Hour * 60 + DateTime.Now.Minute &&
                           timelist[1].EndHour * 60 + timelist[1].EndMinute > DateTime.Now.Hour * 60 + DateTime.Now.Minute)

                            Start();

                        if (timelist[1].EndHour != -1 &&
                            timelist[1].EndHour == DateTime.Now.Hour &&
                            timelist[1].EndMinute == DateTime.Now.Minute)
                            Stop();

                        Thread.Sleep(1000);
                        break;

                    case DayOfWeek.Wednesday:
                        if (timelist[2].StartHour != -1 &&
                           timelist[2].StartHour * 60 + timelist[2].StartMinute <= DateTime.Now.Hour * 60 + DateTime.Now.Minute &&
                           timelist[2].EndHour * 60 + timelist[2].EndMinute > DateTime.Now.Hour * 60 + DateTime.Now.Minute)

                            Start();

                        if (timelist[2].EndHour != -1 &&
                            timelist[2].EndHour == DateTime.Now.Hour &&
                            timelist[2].EndMinute == DateTime.Now.Minute)
                            Stop();

                        Thread.Sleep(1000);
                        break;

                    case DayOfWeek.Thursday:
                        if (timelist[3].StartHour != -1 &&
                           timelist[3].StartHour * 60 + timelist[3].StartMinute <= DateTime.Now.Hour * 60 + DateTime.Now.Minute &&
                           timelist[3].EndHour * 60 + timelist[3].EndMinute > DateTime.Now.Hour * 60 + DateTime.Now.Minute)

                            Start();

                        if (timelist[3].EndHour != -1 &&
                            timelist[3].EndHour == DateTime.Now.Hour &&
                            timelist[3].EndMinute == DateTime.Now.Minute)
                            Stop();

                        Thread.Sleep(1000);
                        break;

                    case DayOfWeek.Friday:
                        if (timelist[4].StartHour != -1 &&
                           timelist[4].StartHour * 60 + timelist[4].StartMinute <= DateTime.Now.Hour * 60 + DateTime.Now.Minute &&
                           timelist[4].EndHour * 60 + timelist[4].EndMinute > DateTime.Now.Hour * 60 + DateTime.Now.Minute)

                            Start();

                        if (timelist[4].EndHour != -1 &&
                            timelist[4].EndHour == DateTime.Now.Hour &&
                            timelist[4].EndMinute == DateTime.Now.Minute)
                            Stop();

                        Thread.Sleep(1000);
                        break;

                    case DayOfWeek.Saturday:
                        if (timelist[5].StartHour != -1 &&
                           timelist[5].StartHour * 60 + timelist[5].StartMinute <= DateTime.Now.Hour * 60 + DateTime.Now.Minute &&
                           timelist[5].EndHour * 60 + timelist[5].EndMinute > DateTime.Now.Hour * 60 + DateTime.Now.Minute)

                            Start();

                        if (timelist[5].EndHour != -1 &&
                            timelist[5].EndHour == DateTime.Now.Hour &&
                            timelist[5].EndMinute == DateTime.Now.Minute)
                            Stop();

                        Thread.Sleep(1000);
                        break;

                    case DayOfWeek.Sunday:
                        if (timelist[6].StartHour != -1 &&
                           timelist[6].StartHour * 60 + timelist[6].StartMinute <= DateTime.Now.Hour * 60 + DateTime.Now.Minute &&
                           timelist[6].EndHour * 60 + timelist[6].EndMinute > DateTime.Now.Hour * 60 + DateTime.Now.Minute)

                            Start();

                        if (timelist[6].EndHour != -1 &&
                            timelist[6].EndHour == DateTime.Now.Hour &&
                            timelist[6].EndMinute == DateTime.Now.Minute)
                            Stop();

                        Thread.Sleep(1000);
                        break;




                }
            }
        }

        private delegate void de();

        private void Start()
        {
            writeToRtbox("활성화 시간 됨, 프로그램 실행");
            Process.Start("gajwa-businfo.exe");
            Thread.Sleep(10 * 6000);
        }

        private void Stop()
        {
            writeToRtbox("비활성화 시간 됨, 프로그램 종료");

            Process[] gs = Process.GetProcessesByName("gajwa-businfo");
            Process[] hs = Process.GetProcessesByName("geckodriver");
            Process[] fs = Process.GetProcessesByName("firefox");

            foreach (Process i in gs) i.Kill();
            foreach (Process i in hs) i.Kill();
            foreach (Process i in fs) i.Kill();

            Thread.Sleep(10 * 6000);
        }

        private void writeToRtbox(string text)
        {
            this.Invoke(new de(() => richTextBox1.Text += $"[{DateTime.Now.ToString()}] {text}\n"));
        }
    }
}
