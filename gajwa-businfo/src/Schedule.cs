using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gajwa_businfo
{
    public class Schedule
    {

        public string DayOfWeek = ""; //null -> special, thursday
        public string Date = ""; //null -> default, 0000-00-00

        public int[] ScreenOnTime = { 8, 0 }; //default 값
        public int[] ScreenOffTime = { 18,30 };

        public int[] ClockScreenOnTime = { 8, 50 };
        public int[] ClockScreenOffTime = { 9, 0 };

        public int[] FoodScreenOnTime = { 13, 0 };
        public int[] FoodScreenOffTime = { 13, 45 };

        public int[] BusScreenOnTime = { 15, 0 };
        public int[] BusScreenOffTime = { 18, 0 };

        public int[] BellRingTime = { 9, 0 };

        public int[] RebootTime = { 0, 0 };

        public bool Enable = true;

        public static int[] ParseStringToTime(string str) //00:00
        {
            string h = str.Replace(" ", "").Replace("    ", "").Replace("\n", "").Split(':')[0];
            string m = str.Replace(" ", "").Replace("    ", "").Replace("\n", "").Split(':')[1];
            return new int[] { Convert.ToInt32(h), Convert.ToInt32(m) };
        }


    }
}
