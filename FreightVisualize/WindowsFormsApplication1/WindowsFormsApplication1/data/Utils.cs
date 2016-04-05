using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreightThreading.data
{
    public static class Utils
    {
        public static DateTime StringToDateTime(string timeOfTheDay)
        {
            DateTime date = new DateTime(2016, 1, 1);
            string[] array = timeOfTheDay.Split(':');
            int hour = Convert.ToInt32(array[0]);
            int minute = Convert.ToInt32(array[1]);
            int second = Convert.ToInt32(array[2]);
            return date + new TimeSpan(hour, minute, second);
        }

        public static int TimeToMinutes(DateTime time)
        {
            return time.Hour * 60 + time.Minute * 60;
        }

        public static DateTime MinutesToTime(int minutes)
        {
            DateTime date = new DateTime(2016, 1, 1);
            return date.AddMinutes(minutes);
        }
    }
}
