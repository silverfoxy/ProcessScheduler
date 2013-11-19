using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessScheduler
{
    class ShortestProcessNext
    {
        List<Process> pList;
        Logger log;

        public ShortestProcessNext(List<Process> pList)
        {
            this.pList = pList.OrderBy(o => o.ArrivalTime).ToList();
            TimeSpan currentTime = this.pList[0].ArrivalTime;
            log = new Logger();
            foreach (Process p in this.pList)
            {
                if (currentTime < p.ArrivalTime)
                {
                    currentTime = p.ArrivalTime;
                }
                p.Started = true;
                p.StartTime = currentTime;                
                currentTime += p.ServiceTime;
                p.SpentTime = currentTime;
                p.EndTime = currentTime;
                log.Log(currentTime, p.Pid.ToString(), p.SpentTime, p.ServiceTime - p.SpentTime);
            }
        }

        /*public void Add(Process p)
        {
            pList.Add(p);
        }*/

        public string ViewLog()
        {
            return log.GetLog();
        }
    }
}
