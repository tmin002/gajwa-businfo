using System;
using System.Threading;
using System.IO;

namespace gajwa_businfo
{
    public static class Bus057TermCountThread
    {

        public static Single Count = 0;
        public static bool Started = false;
        private static bool escape = false;
        private static Thread cntthread;

        private static void cnter()
        {
            while (!escape)
            {
                Count += 1;
                Thread.Sleep(1000);
                //d.write(Count);
            }
            escape = false;
            Started = false;

        }

        public static void StartCount()
        {
            Count = 0;
            cntthread = new Thread(cnter);
            cntthread.Start();
            Started = true;
        }

        public static void StopCount() => escape = true;
    }
}