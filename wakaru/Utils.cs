using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wakaru.Online;

namespace wakaru
{
    static class Utils
    {
        private static DateTime CreateDatetime(int hour, int minute)
        {
            var current = DateTime.Now;
            var datetime = new DateTime(current.Year, current.Month, current.Day, hour, minute, 0);
            return datetime;
        }
        public static DateTime DelayDatetime(int hour, int minute, int delay)
        {
            var datetime = CreateDatetime(hour, minute);
            return datetime.AddMinutes(delay);
        }

        public static int IndexClass(IList<WakattaClass> classes)
        {
            var hour = DateTime.Now.Hour;
            var minute = DateTime.Now.Minute;
            var totalMinutes = hour * 60 + minute;
            for (var i = 0; i < classes.Count; i++)
            {
                var clazz = classes[i];
                var totalBeginMinutes = clazz.TimeHour * 60 + clazz.TimeMinute;
                var totalOverMinutes = totalBeginMinutes + clazz.TimeDuration;
                if (totalMinutes >= totalBeginMinutes &&  totalMinutes < totalOverMinutes) { return i; }
            }
            return -1;
        }

        public static string GetTimeString(int hour, int minute, int period)
        {
            var current = CreateDatetime(hour, minute);
            var delayed = DelayDatetime(hour, minute, period);
            return string.Format("{0:HH:mm}", current) + " - " + string.Format("{0:HH:mm}", delayed);
        }

        public static string CreateHardwareID()
        {
            var hardwareID = Guid.NewGuid().ToString();
            Configuration.Instance.HardwareID = hardwareID;
            Configuration.Save();
            return hardwareID;
        }
    }
}
