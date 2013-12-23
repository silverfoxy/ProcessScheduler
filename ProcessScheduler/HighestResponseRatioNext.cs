using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessScheduler
{
    class HighestResponseRatioNext
    {
        List<Process> pList;
        Logger log;

        /// <summary>
        /// initiates the object and runs the scheduler on given processes.
        /// </summary>
        /// <param name="pList">List of processes</Process></param>
        /// <param name="refreshTime">represented in seconds</param>
        public HighestResponseRatioNext(List<Process> pList, double refreshTime)
        {
            log = new Logger();
            
            this.pList = pList.OrderBy(o => o.ArrivalTime).ToList();
            TimeSpan currentTime = this.pList[0].ArrivalTime;
            while (pList.Count > 0)
            {
                Process currentProcess = this.pList[0];

                for (int i = 0; i < this.pList.Count; i++)
                {
                    if (this.pList[i].ArrivalTime < currentTime)
                        this.pList[i].Priority = 1 + (currentTime - this.pList[i].ArrivalTime).TotalMilliseconds;
                }

                TimeSpan spenttime = TimeSpan.FromSeconds(refreshTime);
                currentProcess.SpentTime += spenttime;
                log.Log(currentTime, currentProcess.Pid.ToString(), currentProcess.SpentTime, (currentProcess.ServiceTime >= currentProcess.SpentTime) ? (currentProcess.ServiceTime - currentProcess.SpentTime) : TimeSpan.FromSeconds(0));
                currentTime += spenttime;
                if (currentProcess.SpentTime >= currentProcess.ServiceTime)
                    currentProcess.EndTime = currentProcess.StartTime + currentProcess.SpentTime;
                else
                    pQueue.Enqueue(currentProcess);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns the result of running this algorithm.</returns>
        public string ViewLog()
        {
            return log.GetLog();
        }
    }
}
