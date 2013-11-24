using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessScheduler
{
    class RoundRobin
    {
        List<Process> pList;
        Queue<Process> pQueue;
        Logger log;

        /// <summary>
        /// initiates the object and runs the scheduler on given processes.
        /// </summary>
        /// <param name="pList">List of processes</Process></param>
        /// <param name="quantumTime">represented in seconds</param>
        public RoundRobin(List<Process> pList, double quantumTime)
        {
            log = new Logger();
            pQueue = new Queue<Process>();
            this.pList = pList.OrderBy(o => o.ArrivalTime).ToList();
            foreach (Process p in this.pList)
            {
                pQueue.Enqueue(p);
            }
            TimeSpan currentTime = this.pList[0].ArrivalTime;
            while (pQueue.Count > 0)
            {
                Process currentProcess = pQueue.Dequeue();
                if (!currentProcess.Started)
                {
                    currentProcess.StartTime = currentTime;
                    currentProcess.Started = true;
                }
                TimeSpan spenttime = TimeSpan.FromSeconds(quantumTime);
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
