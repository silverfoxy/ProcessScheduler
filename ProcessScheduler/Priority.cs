using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessScheduler
{
    class Priority
    {
        List<Process> SortedPAList;//priority then arrival
        List<Process> SortedAPList;//arrival then priority
        Logger log;

        /// <summary>
        /// initiates the object and runs the scheduler on given processes.
        /// </summary>
        /// <param name="pList">List of processes</Process></param>

        public Priority(List<Process> pList)
        {
            SortedPAList = new List<Process>();
            SortedPAList= pList.OrderByDescending(p =>p.Priority).ThenBy(p => p.ArrivalTime).ToList();

            SortedAPList = new List<Process>();
            SortedAPList = pList.OrderBy(p => p.ArrivalTime).ThenByDescending(p=> p.Priority).ToList();
           
            TimeSpan currentTime =SortedAPList[0].ArrivalTime;;

            log = new Logger();
            Process pp;

             while (SortedPAList.Count > 0)
             {
                 if (SortedPAList[0].ArrivalTime <= currentTime)
                     pp = SortedPAList[0];
                 else
                 {
                     pp = SortedAPList[0];
                     if (pp.ArrivalTime > currentTime)
                     {
                         currentTime = pp.ArrivalTime;
                     }
                 }
                 SortedPAList.Remove(pp);
                 SortedAPList.Remove(pp);
                 pp.Started = true;
                 pp.StartTime = currentTime;
                 currentTime += pp.ServiceTime;
                 pp.SpentTime = pp.ServiceTime;
                 pp.EndTime = currentTime;
                 log.Log(currentTime, pp.Pid.ToString(), pp.SpentTime, pp.ServiceTime - pp.SpentTime);
                 pp.CalculateWaitingAndTurnaroundTimeAndNormalTurnaroundTimeAndNormalWaitingTime();
                 //Console.WriteLine(pp.CompleteInfo() + "\n");
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
