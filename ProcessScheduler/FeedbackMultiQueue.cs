using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessScheduler
{
    class FeedbackMultiQueue
    {
        List<Process> pList;
        Logger log;
        Queue<Process>[] queue;
        public FeedbackMultiQueue(List<Process> pList, double quantum)
        {
            queue = new Queue<Process>[4];
            for (int i = 0; i < queue.Length; i++)
            {
                queue[i] = new Queue<Process>();
            }
            this.pList = pList.OrderBy(x => x.ArrivalTime).ToList();
            TimeSpan currentTime = this.pList[0].ArrivalTime;
            log = new Logger();
            Process currentProcess;
            while (this.pList.Count > 0 || queue[0].Count > 0 || queue[1].Count > 0 || queue[2].Count > 0 || queue[3].Count > 0)
            {
                if (this.pList.Count > 0)
                {
                    if (this.pList[0].ArrivalTime <= currentTime)
                    {
                        queue[0].Enqueue(this.pList[0]);
                        this.pList.RemoveAt(0);
                    }
                    else
                    {
                        currentTime = this.pList[0].ArrivalTime;
                        queue[0].Enqueue(this.pList[0]);
                        this.pList.RemoveAt(0);
                    }
                }
                for (int i = 0; i < queue.Length; i++)
                {
                    if (queue[i].Count > 0)
                    {
                        double Qquantum = quantum * (i + 1);
                        currentProcess = queue[i].Dequeue();
                        if (!currentProcess.Started)
                        {
                            currentProcess.Started = true;
                            currentProcess.StartTime = currentTime;
                        }
                        if (currentProcess.ServiceTime - currentProcess.SpentTime > TimeSpan.FromSeconds(Qquantum))
                        {
                            currentTime += TimeSpan.FromSeconds(Qquantum);
                            currentProcess.SpentTime += TimeSpan.FromSeconds(Qquantum);
                            if (i < queue.Length - 1)
                                queue[i + 1].Enqueue(currentProcess);
                            else
                                queue[i].Enqueue(currentProcess);
                            log.Log(currentTime, currentProcess.Pid.ToString(), currentProcess.SpentTime, currentProcess.ServiceTime - currentProcess.SpentTime, currentProcess.Priority);
                            break;
                        }
                        else
                        {
                            TimeSpan remainingTime = currentProcess.ServiceTime - currentProcess.SpentTime;
                            currentTime += remainingTime;
                            currentProcess.SpentTime += remainingTime;
                            currentProcess.EndTime = currentTime;
                            currentProcess.CalculateWaitingAndTurnaroundTimeAndNormalTurnaroundTimeAndNormalWaitingTime();
                            log.Log(currentTime, currentProcess.Pid.ToString(), currentProcess.SpentTime, currentProcess.ServiceTime - currentProcess.SpentTime, currentProcess.Priority);
                            currentProcess.CalculateWaitingAndTurnaroundTimeAndNormalTurnaroundTimeAndNormalWaitingTime();
                            break;
                        }
                    }
                }
            }
        }
        public string ViewLog()
        {
            return log.GetLog();
        }
    }
}
