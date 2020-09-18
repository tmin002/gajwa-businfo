using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;

namespace gajwa_businfo
{
    public static class ScheduleManager //경고: 스케쥴간 시간간격을 최소 1분으로 할것. 간격이 없으면 OffTime 이벤트가 나중에 갈 경우 그냥 BlankScreen 된다.
    {

        public enum ScheduleChangedEvents
        {
            ScreenOn,
            ScreenOff,
            ClockScreenOn,
            ClockScreenOff,
            FoodScreenOn,
            FoodScreenOff,
            BusScreenOn,
            BusScreenOff,
            Reboot
        }

        private static bool StopWatching = false;
        private static int nmin_last = 0;
        private static int nmin
        {
            get { return DateTime.Now.Hour * 60 + DateTime.Now.Minute; }
        }

        private static int LastScheduleLoadDate = -1; //처음엔 무조건 값을 다르게 해서 설정 초기화 유도
        private static Dictionary<ScheduleManager.ScheduleChangedEvents, int> LastScheduleKeys;
        public static void ManuallyUpdateScheduleKeys(Schedule s)
        {
            LastScheduleKeys = new Dictionary<ScheduleChangedEvents, int>() //todo: 효율성 개선 필요.
                    {
                        {ScheduleManager.ScheduleChangedEvents.ScreenOn,  base_.ConvertArrayToTime(s.ScreenOnTime) },
                        {ScheduleManager.ScheduleChangedEvents.ScreenOff, base_.ConvertArrayToTime(s.ScreenOffTime) },
                        {ScheduleManager.ScheduleChangedEvents.ClockScreenOn, base_.ConvertArrayToTime(s.ClockScreenOnTime) },
                        {ScheduleManager.ScheduleChangedEvents.ClockScreenOff, base_.ConvertArrayToTime(s.ClockScreenOffTime) },
                        {ScheduleManager.ScheduleChangedEvents.FoodScreenOn, base_.ConvertArrayToTime(s.FoodScreenOnTime) },
                        {ScheduleManager.ScheduleChangedEvents.FoodScreenOff, base_.ConvertArrayToTime(s.FoodScreenOffTime) },
                        {ScheduleManager.ScheduleChangedEvents.BusScreenOn, base_.ConvertArrayToTime(s.BusScreenOnTime) },
                        {ScheduleManager.ScheduleChangedEvents.BusScreenOff, base_.ConvertArrayToTime(s.BusScreenOffTime) },
                        {ScheduleManager.ScheduleChangedEvents.Reboot, base_.ConvertArrayToTime(s.RebootTime) }
                    };
        }
        private static Dictionary<ScheduleManager.ScheduleChangedEvents, int> ScheduleKeys
        {
            get {
                if (DateTime.Now.DayOfYear != LastScheduleLoadDate)
                {

                    LastScheduleLoadDate = DateTime.Now.DayOfYear;
                    ManuallyUpdateScheduleKeys(base_.TODAY_SCHEDULE);
                }

                return LastScheduleKeys;  
            }

            set { LastScheduleKeys = value; }
        }


        public static event ScheduleChangedEventHandler ScheduleChanged;
        public delegate void ScheduleChangedEventHandler(ScheduleChangedEventArgs e);
        private static Thread ScheduleWatcher = new Thread(() =>
        {
            //처음에만 하는 것. 늦게 켜졌을때도 화면 바뀔수 있도록.

            var ring_first = false;

            if (ScheduleKeys[ScheduleManager.ScheduleChangedEvents.ScreenOn] <= nmin && nmin < ScheduleKeys[ScheduleManager.ScheduleChangedEvents.ScreenOff])
            { 
                RaiseEvent(ScheduleManager.ScheduleChangedEvents.ScreenOn);
                ring_first = true;
            }

            if (ScheduleKeys[ScheduleManager.ScheduleChangedEvents.ClockScreenOn] <= nmin && nmin < ScheduleKeys[ScheduleManager.ScheduleChangedEvents.ClockScreenOff])
            {
                RaiseEvent(ScheduleManager.ScheduleChangedEvents.ClockScreenOn);
                ring_first = true;

            }

            if (ScheduleKeys[ScheduleManager.ScheduleChangedEvents.FoodScreenOn] <= nmin && nmin < ScheduleKeys[ScheduleManager.ScheduleChangedEvents.FoodScreenOff])
            {
                RaiseEvent(ScheduleManager.ScheduleChangedEvents.FoodScreenOn);
                ring_first = true;

            }

            if (ScheduleKeys[ScheduleManager.ScheduleChangedEvents.BusScreenOn] <= nmin && nmin < ScheduleKeys[ScheduleManager.ScheduleChangedEvents.BusScreenOff])
            {
                RaiseEvent(ScheduleManager.ScheduleChangedEvents.BusScreenOn);
                ring_first = true;

            }

            if (ring_first) nmin_last = nmin;


            while (!StopWatching) //계속 루프 돌기
            {
                if (ScheduleKeys[ScheduleChangedEvents.Reboot] == nmin) base_.RebootComputer();



                foreach (ScheduleManager.ScheduleChangedEvents i in ScheduleKeys.Keys)
                {
                    if (ScheduleKeys[i] == nmin && nmin_last != nmin) RaiseEvent(i);
                }

                nmin_last = nmin;
                Thread.Sleep(base_.SCHEDULE_WATCHER_DELAY);

            }
            StopWatching = true;
        });


        ////
        ////
        ////

        private static void RaiseEvent(ScheduleManager.ScheduleChangedEvents type)
        {

            if (base_.TODAY_SCHEDULE.Enable)
            {
                d.write($"[ScheduleWatcher] {type.ToString()} raising");

                if (ScheduleChanged == null) Thread.Sleep(base_.SCHEDULE_WATCHER_EVENT_DECLARE_DELAY);
                ScheduleChanged.Invoke(new ScheduleChangedEventArgs(type));
            }
            else
            {
                d.write($"[ScheduleWatcher] was to raise {type.ToString()}, but today's not display day. ignoring request.");
            }

        }

        public static void StartWatcher() => ScheduleWatcher.Start();
        public static void StopWatcher() => StopWatching = true;

   
       
    }

    public class ScheduleChangedEventArgs
    {

        public ScheduleManager.ScheduleChangedEvents EventType;
        public ScheduleChangedEventArgs(ScheduleManager.ScheduleChangedEvents eventtype) { EventType = eventtype; }

    }


}
