using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ProcessScheduler
{
    /// <summary>
    /// Highest Response Ratio Next (preemptive)
    /// </summary>
    class HRR
    {
        List<Process> pList;
        Logger log;

        /// <summary>
        /// initiates the object and runs the scheduler on given processes.
        /// </summary>
        /// <param name="pList">List of processes</Process></param>

        public HRR(List<Process> pList)
        {
            this.pList = pList.OrderBy(x => x.ArrivalTime).ToList();

            TimeSpan currentTime = pList[0].ArrivalTime;

            log = new Logger();

            while (this.pList.Count > 0)
            {
                if (this.pList[0].ArrivalTime > currentTime)
                {
                    for (int i = 1; i < this.pList.Count; i++)
                    {
                        if (this.pList[i].ArrivalTime <= currentTime)
                        {
                            Process t = this.pList[0];
                            this.pList[0] = this.pList[i];
                            this.pList[i] = t;
                            break;
                        }
                    }
                    currentTime = this.pList[0].ArrivalTime;
                }
                Process currentProcess = this.pList[0];
                this.pList.RemoveAt(0);
                currentProcess.Started = true;
                currentProcess.StartTime = currentTime;
                currentTime += currentProcess.ServiceTime;
                currentProcess.SpentTime = currentProcess.ServiceTime;
                currentProcess.EndTime = currentTime;
                currentProcess.CalculateWaitingAndTurnaroundTimeAndNormalTurnaroundTimeAndNormalWaitingTime();
                log.Log(currentProcess.StartTime, currentProcess.Pid.ToString(), currentProcess.SpentTime, currentProcess.ServiceTime - currentProcess.SpentTime, currentProcess.Priority);
                Console.WriteLine(currentProcess.CompleteInfo() + "\n");
                foreach (Process p in this.pList)
                {
                    if (p.ArrivalTime < currentTime)
                        p.Priority = 1.0 + ((double)(currentTime - p.ArrivalTime).Ticks / (double)p.ServiceTime.Ticks);
                }
                this.pList = this.pList.OrderByDescending(y => y.Priority).ToList();
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
        public void printList(List<Process> list)
        {
            foreach (Process p in list)
            {
                Console.WriteLine("\n**********" + p.Pid);
            }
        }

        
        
    }
}
