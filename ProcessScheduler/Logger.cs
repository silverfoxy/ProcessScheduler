using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessScheduler
{
    class Logger
    {
        StringBuilder activity;

        public Logger()
        {
            activity = new StringBuilder();
        }

        public void Log(TimeSpan timeStamp, string activity, TimeSpan spentTime, TimeSpan remainingTime)
        {
            this.activity.Append(String.Format("{0,-17} pid {1, -5} spent {2, -17} rem {3}\n", timeStamp, activity, spentTime, remainingTime));
        }

        public void Log(TimeSpan timeStamp, string activity, TimeSpan spentTime, TimeSpan remainingTime, double priority)
        {
            this.activity.Append(String.Format("{0,-17} pid {1, -5} spent {2, -17} rem {3} prio {4, -17}\n", timeStamp, activity, spentTime, remainingTime, priority));
        }

        public string GetLog()
        {
            return activity.ToString();
        }
    }
}
