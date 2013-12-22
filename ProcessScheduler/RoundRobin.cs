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
            TimeSpan currentTime = TimeSpan.FromSeconds(0) ;
            while (pQueue.Count > 0)
            {
                Process currentProcess = pQueue.Dequeue();
                if (!currentProcess.Started)
                {
                    if (currentTime < currentProcess.ArrivalTime)
                    {
                        currentTime = currentProcess.ArrivalTime;
                    }
                    currentProcess.StartTime = currentTime;
                    currentProcess.Started = true;
                }
                ///<summary> 
                /// we assume two states if remaining time > quantum time or else
                /// <summary>
                if (currentProcess.ServiceTime - currentProcess.SpentTime > TimeSpan.FromSeconds(quantumTime))
                {
                    TimeSpan spenttime = TimeSpan.FromSeconds(quantumTime);
                    currentProcess.SpentTime += spenttime;
                    log.Log(currentTime, currentProcess.Pid.ToString(), currentProcess.SpentTime,currentProcess.ServiceTime - currentProcess.SpentTime);
                    currentTime += spenttime;
                    pQueue.Enqueue(currentProcess);
                }

                else if (currentProcess.ServiceTime - currentProcess.SpentTime >= TimeSpan.FromSeconds(0.0))
                {
                    TimeSpan spenttime = currentProcess.ServiceTime - currentProcess.SpentTime;
                    currentProcess.SpentTime += spenttime;
                    log.Log(currentTime, currentProcess.Pid.ToString(), currentProcess.SpentTime, TimeSpan.FromSeconds(0));
                    currentTime += spenttime;
                    currentProcess.EndTime = currentTime;
                    currentProcess.CalculateWaitingAndTurnaroundTimeAndNormalTurnaroundTimeAndNormalWaitingTime();
                    Console.WriteLine(currentProcess.CompleteInfo()+"\n");
                    
                }
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
