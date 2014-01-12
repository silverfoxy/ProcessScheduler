using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ProcessScheduler
{
    /// <summary>
    /// Lottery (non-preemptive)
    /// </summary>
    class Lottery
    {
        List<Process> pList;
        Logger log;

        /// <summary>
        /// initiates the object and runs the scheduler on given processes.
        /// </summary>
        /// <param name="pList">List of processes</Process></param>

        public Lottery(List<Process> pList, double quantum)
        {
            this.pList = pList.OrderBy(x => x.ArrivalTime).ToList();
            List<Process> availList = new List<Process>();
            availList.Add(this.pList[0]);
            this.pList.RemoveAt(0);
            TimeSpan currentTime = availList[0].ArrivalTime;

            log = new Logger();

            while (this.pList.Count > 0 || availList.Count > 0)
            {
                if (availList.Count == 0)
                {
                    currentTime = this.pList[0].ArrivalTime;
                }
                List<int> toBeRemoved = new List<int>();
                for (int i = 0; i < this.pList.Count; i++)
                {
                    if (this.pList[i].ArrivalTime <= currentTime)
	                {
                        availList.Add(this.pList[i]);
                        toBeRemoved.Add(i);
                    }
                }
                foreach (int item in toBeRemoved)
                {
                    try
                    {
                        this.pList.RemoveAt(item);
                    }
                    catch (Exception)
                    {
                        
                    }
                }
                foreach (Process p in availList)
                {
                    Random r = new Random();
                    p.Priority = r.Next(1, availList.Count);
                }
                availList = availList.OrderByDescending(x => x.Priority).ToList();
                Process currentProcess = availList[0];
                availList.RemoveAt(0);
                if (!currentProcess.Started)
                {
                    currentProcess.Started = true;
                    currentProcess.StartTime = currentTime;
                }
                if (currentProcess.ServiceTime - currentProcess.SpentTime > TimeSpan.FromSeconds(quantum))
                {
                    currentTime += TimeSpan.FromSeconds(quantum);
                    currentProcess.SpentTime += TimeSpan.FromSeconds(quantum);
                    availList.Add(currentProcess);
                }
                else
                {
                    TimeSpan remainingTime = currentProcess.ServiceTime - currentProcess.SpentTime;
                    currentTime += remainingTime;
                    currentProcess.SpentTime += remainingTime;
                    currentProcess.EndTime = currentTime;
                    currentProcess.CalculateWaitingAndTurnaroundTimeAndNormalTurnaroundTimeAndNormalWaitingTime();
                }
                log.Log(currentTime, currentProcess.Pid.ToString(), currentProcess.SpentTime, currentProcess.ServiceTime - currentProcess.SpentTime, currentProcess.Priority);
                //Console.WriteLine(currentProcess.CompleteInfo() + "\n");
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
