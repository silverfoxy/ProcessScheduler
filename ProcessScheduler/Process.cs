using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessScheduler
{
    class Process
    {
        int _Pid;
        TimeSpan _ArrivalTime;
        TimeSpan _ServiceTime;
        TimeSpan _StartTime;
        TimeSpan _EndTime;
        TimeSpan _SpentTime;
        int _Priority;
        bool _Started;

        public int Pid 
        {
            get { return _Pid; }
        }

        public TimeSpan ArrivalTime
        {
            get { return _ArrivalTime; }
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

        public int Priority
        {
            get { return _Priority; }
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
            _Started = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns process info in a single line string</returns>
        public string Info()
        {
            return string.Format("pid: {0} arrival time: {1} service time: {2} priority: {3}", Pid, ArrivalTime, ServiceTime, Priority);
        }
    }
}
