using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wakaru.Online
{
    static class OnlineUtils
    {
        public static string CreateHardwareID()
        {
            var hardwareID = Guid.NewGuid().ToString();
            Configuration.Instance.HardwareID = hardwareID;
            Configuration.Save();
            return hardwareID;
        }

        public static DateTime DelayDatetime(int hour, int minute, int delay)
        {
            var current = DateTime.Now;
            var datetime = new DateTime(current.Year, current.Month, current.Day, hour, minute, 0);
            return datetime.AddMinutes(delay);
        }
    }
}
