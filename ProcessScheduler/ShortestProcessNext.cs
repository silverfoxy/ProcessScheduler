using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessScheduler
{
    class ShortestProcessNext
    {
        List<Process> pList;
        SortedDictionary<TimeSpan, Process> arrivedPList;
        Logger log;

        public ShortestProcessNext(List<Process> pList, double quantumTime)
        {
            this.pList = pList.OrderBy(o => o.ArrivalTime).ToList();
            TimeSpan currentTime = this.pList[0].ArrivalTime;
            List<Process> tmplist = new List<Process>(this.pList);
            arrivedPList = new SortedDictionary<TimeSpan, Process>();
            log = new Logger();
            while (tmplist.Count > 0 && tmplist[0].ArrivalTime <= currentTime)
            {
                arrivedPList.Add(tmplist[0].ServiceTime - tmplist[0].SpentTime, tmplist[0]);
                tmplist.RemoveAt(0);
            }
            while (tmplist.Count > 0 || arrivedPList.Count > 0)
            {
                if (arrivedPList.Count > 0)
                {
                    Process p = arrivedPList[arrivedPList.Keys.Min()];
                    arrivedPList.Remove(arrivedPList.Keys.Min());
                    if (!p.Started)
                    {
                        p.StartTime = currentTime;
                        p.Started = true;
                    }
                    TimeSpan spenttime = TimeSpan.FromSeconds(quantumTime);
                    p.SpentTime += spenttime;
                    log.Log(currentTime, p.Pid.ToString(), p.SpentTime, (p.ServiceTime >= p.SpentTime) ? (p.ServiceTime - p.SpentTime) : TimeSpan.FromSeconds(0));
                    currentTime += spenttime;
                    if (p.SpentTime >= p.ServiceTime)
                        p.EndTime = p.StartTime + p.SpentTime;
                    else
                        arrivedPList.Add(p.ServiceTime - p.SpentTime, p);
                }
                else
                    currentTime = tmplist[0].ArrivalTime;
                while (tmplist.Count > 0 && tmplist[0].ArrivalTime <= currentTime)
                {
                    arrivedPList.Add(tmplist[0].ServiceTime - tmplist[0].SpentTime, tmplist[0]);
                    tmplist.RemoveAt(0);
                }
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
