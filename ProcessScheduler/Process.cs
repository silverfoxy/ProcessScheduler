using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessScheduler 
{
    class Process : IComparable<Process>
    {
        int _Pid;
        TimeSpan _ArrivalTime;
        TimeSpan _ServiceTime;
        TimeSpan _StartTime;
        TimeSpan _EndTime;
        TimeSpan _SpentTime;
        TimeSpan _TurnaroundTime;
        TimeSpan _WaitingTime;
        double _NormalWaiting;
        double _NormalTurnaround;
        double _Priority;
        bool _Started;

        public int Pid 
        {
            get { return _Pid; }
        }

        public TimeSpan ArrivalTime
        {
            get { return _ArrivalTime; }
        }

        public int CompareTo(Process other)
        {
            if (this.Priority > other.Priority) return -1;
            else if (this.Priority < other.Priority) return 1;
            else return 0;
        }

        public TimeSpan ServiceTime
        {
            get { return _ServiceTime; }
        }

        public TimeSpan StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }

        public TimeSpan EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }

        public TimeSpan SpentTime
        {
            get { return _SpentTime; }
            set { _SpentTime = value; }
        }

        public TimeSpan TurnaroundTime
        {
            get { return _TurnaroundTime; }
            set { _TurnaroundTime = value; }
        }
        public TimeSpan WaitingTime
        {
            get { return _WaitingTime; }
            set { _WaitingTime = value; }
        }

        public double NormalWaiting
        {
            get { return _NormalWaiting; }
            set { _NormalWaiting = value; }
        }
        public double NormalTurnaround
        {
            get { return _NormalTurnaround; }
            set { _NormalTurnaround  = value; }
        }
        public double Priority
        {
            get { return _Priority; }
            set { _Priority = value; }
        }

        public bool Started
        {
            get { return _Started; }
            set { _Started = value;}
        }

        /// <summary>
        /// Initiates a Process object
        /// </summary>
        public Process(int Pid, TimeSpan ArrivalTime, TimeSpan ServiceTime, int Priority)
        {
            _Pid = Pid;
            _ArrivalTime = ArrivalTime;
            _ServiceTime = ServiceTime;
            _Priority = Priority;
            _SpentTime = TimeSpan.FromSeconds(0);
            _WaitingTime = TimeSpan.FromSeconds(0);
            _TurnaroundTime = TimeSpan.FromSeconds(0);
            _NormalTurnaround = 0;
            _NormalWaiting = 0;
            _Started = false;
        }

        public void CalculateWaitingAndTurnaroundTimeAndNormalTurnaroundTimeAndNormalWaitingTime(){
            TurnaroundTime = EndTime - ArrivalTime;
            WaitingTime = TurnaroundTime - ServiceTime;
            
            double waitT = ((double) WaitingTime.Milliseconds/  ServiceTime.Milliseconds);
            NormalWaiting = waitT;
            
            double turnaroundT = ((double)TurnaroundTime.Ticks / ServiceTime.Ticks);
            NormalTurnaround= turnaroundT;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns process info in a single line string</returns>
        public string Info()
        {
            return string.Format("pid: {0} arrival time: {1} service time: {2} priority: {3}", Pid, ArrivalTime, ServiceTime, Priority);
        }
        public string CompleteInfo()
        {
            return string.Format("*************  process {0}  ************* \narrival time: {1}\nservice time: {2} \nstart time : {3} \nend time: {4} \nturnAround time: {5} \nwaiting time: {6}\nnormal WaitingTime : {7} \nnormal TurnaroundTime: {8} ", Pid, ArrivalTime, ServiceTime,StartTime,EndTime,TurnaroundTime,WaitingTime,NormalWaiting,NormalTurnaround);
           
        }
    }
}
