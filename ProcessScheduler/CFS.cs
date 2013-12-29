using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RedBlackCS;

namespace ProcessScheduler
{
    class CFS
    {
        List<Process> pList;
        Logger log;
        public CFS(List<Process> pList, double quantum)
        {
            RedBlackWrapper rb = new RedBlackWrapper();
            this.pList = pList.OrderBy(x => x.ArrivalTime).ToList();
            TimeSpan currentTime = this.pList[0].ArrivalTime;
            log = new Logger();
            while (this.pList.Count > 0 || !rb.IsEmpty())
            {
                List<int> toBeRemoved = new List<int>();
                for (int i = 0; i < this.pList.Count; i++)
                {
                    if (this.pList[i].ArrivalTime <= currentTime)
	                {
                        rb.Add(this.pList[i].SpentTime, this.pList[i]);
                        toBeRemoved.Add(i);
                    }
                }
                foreach (int item in toBeRemoved)
                {
                    this.pList.RemoveAt(item);
                }
                Process currentProcess = (Process)((List<object>)rb.GetMinValue())[0];
                rb.RemoveMin();
                if (!currentProcess.Started)
                {
                    currentProcess.Started = true;
                    currentProcess.StartTime = currentTime;
                }
                if (currentProcess.ServiceTime - currentProcess.SpentTime > TimeSpan.FromSeconds(quantum))
                {
                    currentTime += TimeSpan.FromSeconds(quantum);
                    currentProcess.SpentTime += TimeSpan.FromSeconds(quantum);
                    rb.Add(currentProcess.SpentTime, currentProcess);
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

        public string ViewLog()
        {
            return log.GetLog();
        }
    }
}
