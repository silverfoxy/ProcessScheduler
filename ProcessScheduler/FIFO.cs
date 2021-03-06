﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessScheduler
{
    class FIFO
    {
        List<Process> pList;
        Logger log;

        /// <summary>
        /// initiates the object and runs the scheduler on given processes.
        /// </summary>
        /// <param name="pList">List of processes</Process></param>
        public FIFO(List<Process> pList)
        {
            this.pList = pList.OrderBy(o => o.ArrivalTime).ToList();
            TimeSpan currentTime = TimeSpan.FromSeconds(0);
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
                p.SpentTime = p.ServiceTime;
                p.EndTime = currentTime;
                log.Log(currentTime, p.Pid.ToString(), p.SpentTime,p.ServiceTime-p.SpentTime);
                p.CalculateWaitingAndTurnaroundTimeAndNormalTurnaroundTimeAndNormalWaitingTime();
                //Console.WriteLine(p.CompleteInfo() + "\n");
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
