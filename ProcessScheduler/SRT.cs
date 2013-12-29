using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessScheduler
{
    class SRT
    {
        List<Process> ProcessByPriority;//priority then arrival
        List<Process> SortedAPList;//arrival then priority
        Logger log;
        List<Process> pList;

        /// <summary>
        /// initiates the object and runs the scheduler on given processes.
        /// </summary>
        /// <param name="pList">List of processes</Process></param>

        public SRT(List<Process> ppList)
        {
            pList = ppList;
            SortedAPList = new List<Process>();
            ProcessByPriority = new List<Process>();
            
            SortedAPList = pList.OrderBy(p => p.ArrivalTime).ThenByDescending(p=> p.Priority).ToList();

            TimeSpan currentTime = SortedAPList[0].ArrivalTime;
            TimeSpan nextTime = SortedAPList[0].ArrivalTime;
            log = new Logger();
            //Process currentProcess;
            
            while (SortedAPList.Count > 0  )
            {
                //get all processes with time = cur time and insert in list Sorted P
                while( SortedAPList.Count>0 && SortedAPList[0].ArrivalTime == currentTime)
                {
                    ProcessByPriority.Add(SortedAPList[0]);
          
                    SortedAPList.Remove(SortedAPList[0]);
                }
                //set next time 
                if (SortedAPList.Count > 0)
                {
                    nextTime = SortedAPList[0].ArrivalTime;
                }
                else
                {
                    nextTime =TimeSpan.FromMilliseconds( 1000000);
                }
                //set a remaining time and  sort by remaining time minimum remingin time= first
                foreach (Process p in ProcessByPriority)
                {
                   p.Priority =  (p.ServiceTime.Milliseconds - p.SpentTime.Milliseconds);

                }
                ProcessByPriority=ProcessByPriority.OrderBy(p =>p.Priority).ThenBy(p => p.ArrivalTime).ToList();
                //do tasks with most priority in time interval of next-current time;

                TimeSpan diffTime = nextTime - currentTime;
                while (ProcessByPriority.Count > 0 && diffTime.TotalMilliseconds > 0)
                {
                    if (ProcessByPriority[0].ServiceTime - ProcessByPriority[0].SpentTime > diffTime)
                    {
                        if (ProcessByPriority[0].SpentTime.TotalMilliseconds == 0)
                        {
                            ProcessByPriority[0].StartTime = currentTime;
                        }
                        currentTime += diffTime;
                        
                        ProcessByPriority[0].SpentTime += diffTime;
                        diffTime =TimeSpan.FromMilliseconds(0);
                        log.Log(currentTime, ProcessByPriority[0].Pid.ToString(), ProcessByPriority[0].SpentTime, ProcessByPriority[0].ServiceTime - ProcessByPriority[0].SpentTime);
                    }
                    else
                    {

                        if (ProcessByPriority[0].SpentTime.TotalMilliseconds == 0)
                        {
                            ProcessByPriority[0].StartTime = currentTime;
                                
                        }
                        
                        currentTime += ProcessByPriority[0].ServiceTime - ProcessByPriority[0].SpentTime;
                        ProcessByPriority[0].EndTime = currentTime;
                        diffTime -= ProcessByPriority[0].ServiceTime - ProcessByPriority[0].SpentTime;
                        log.Log(currentTime, ProcessByPriority[0].Pid.ToString(), ProcessByPriority[0].SpentTime, TimeSpan.FromSeconds(0));
                        ProcessByPriority[0].CalculateWaitingAndTurnaroundTimeAndNormalTurnaroundTimeAndNormalWaitingTime();
                       // Console.WriteLine(ProcessByPriority[0].CompleteInfo() + "\n");
                        ProcessByPriority.Remove(ProcessByPriority[0]);
                    }
                    
                }

                //current time = next time;
                
                currentTime = nextTime;

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
