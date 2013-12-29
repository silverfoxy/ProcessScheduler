using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using CommandLine.Utility;

namespace ProcessScheduler
{
    class Program
    {
        static bool CLEAR_CONSOLE = false;
        static bool DEBUG_MODE = false;
        static double DEBUG_QTIME = 0.5;
        static bool DEBUG_SHOW_PROCESS_LIST = true;
        static Algorithm DEBUG_ALGORITHM = Algorithm.FIFO;

        enum Algorithm
        { 
            SPN,
            FIFO,
            RR,
            Priority,
            SRT,
            HRR,
            Lottery,
            MLFQ,
            CFS
        };

        static void Main(string[] args)
        {
            
            if (CLEAR_CONSOLE)
                Console.Clear();
            Console.WindowWidth = 100;
            #region Parse Commandline Args
            string fileName;
            Arguments CommandLine = new Arguments(args);

            if (DEBUG_MODE)
            {
                fileName = "ProcessList.txt";
            }
            else
            {
                if (CommandLine["i"] != null)
                    fileName = CommandLine["i"];
                else if (CommandLine["input"] != null)
                    fileName = CommandLine["input"];
                else
                {
                    ShowHelp();
                    return;
                }
            }


            List<Process> pList;
            try
            {
                pList = ReadFromFile(fileName);
             
            }
            catch (Exception)
            {
                Console.WriteLine(string.Format("Failed to read from {0}.", fileName));
                return;
            }
            if (CommandLine["s"] != null || CommandLine["showprocesslist"] != null || DEBUG_SHOW_PROCESS_LIST)
            {
                Console.WriteLine(string.Format("{0," + Console.WindowWidth / 2 + "}\r\n", "Text File"));
                foreach (var item in pList)
                {
                    Console.WriteLine(string.Format("PID:{0, -5} Arrival:{1, -17} Service:{2, -17} Prio:{3}", item.Pid, item.ArrivalTime, item.ServiceTime, item.Priority));
                }
                Console.WriteLine();
            }
            string method = string.Empty;
            if (CommandLine["a"] != null)
                method = CommandLine["a"];
            else if (CommandLine["algorithm"] != null)
                method = CommandLine["algorithm"];
            else if (!DEBUG_MODE)
            {
                return;
            }
            if (method.ToLower() == "fifo" || (DEBUG_MODE && DEBUG_ALGORITHM == Algorithm.FIFO))
            {
                FIFO f = new FIFO(pList);
                foreach (Process item in pList)
                {
                    Console.WriteLine(item.CompleteInfo() + "\n");
                }
                Console.WriteLine(string.Format("{0," + Console.WindowWidth / 2 + "}\r\n", "FIFO"));
                Console.Write(f.ViewLog());
                //*****************view AVerage Waiting & TurnAround*********
                Console.WriteLine("\r\nAverage Waiting Time :  " + CalculateAverageWaitingTime(pList) + "\n");
                Console.WriteLine("Average TurnAround Time : " + CalculateAverageTurnaroundTime(pList) + "\n");
                //********************
                
            }
            else if (method.ToLower() == "rr" || method.ToLower() == "roundrobin" || (DEBUG_MODE && DEBUG_ALGORITHM == Algorithm.RR))
            {
                double quantum = 1;
                try
                {
                    if (CommandLine["q"] != null)
                        quantum = double.Parse(CommandLine["q"]);
                    else if (CommandLine["quantum"] != null)
                        quantum = double.Parse(CommandLine["quantum"]);
                    else if (DEBUG_MODE)
                    {
                        quantum = DEBUG_QTIME;

                    }
                    else
                    {
                        ShowHelp_MissingQTime();
                        return;
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("[ERROR] quantum time not in correct format");
                    return;
                }
                RoundRobin rr = new RoundRobin(pList, quantum);
                foreach (Process item in pList)
                {
                    Console.WriteLine(item.CompleteInfo() + "\n");
                }
                Console.WriteLine(string.Format("{0," + Console.WindowWidth / 2 + "}\r\nQuantum Time: {1} Second(s)\r\n", "RoundRobin", quantum.ToString()));
                //**************************viewLog
                Console.Write(rr.ViewLog());
                //*****************view AVerage Waiting & TurnAround*********
                Console.WriteLine("\r\nAverage Waiting Time :  " + CalculateAverageWaitingTime(pList) + "\n");
                Console.WriteLine("Average TurnAround Time : " + CalculateAverageTurnaroundTime(pList) + "\n");
                //********************
            }
            else if (method.ToLower() == "lottery" || (DEBUG_MODE && DEBUG_ALGORITHM == Algorithm.Lottery))
            {
                double quantum = 1;
                try
                {
                    if (CommandLine["q"] != null)
                        quantum = double.Parse(CommandLine["q"]);
                    else if (CommandLine["quantum"] != null)
                        quantum = double.Parse(CommandLine["quantum"]);
                    else if (DEBUG_MODE)
                    {
                        quantum = DEBUG_QTIME;

                    }
                    else
                    {
                        ShowHelp_MissingQTime();
                        return;
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("[ERROR] quantum time not in correct format");
                    return;
                }
                Lottery l = new Lottery(pList, quantum);
                foreach (Process item in pList)
                {
                    Console.WriteLine(item.CompleteInfo() + "\n");
                }
                Console.WriteLine(string.Format("{0," + Console.WindowWidth / 2 + "}\r\nQuantum Time: {1} Second(s)\r\n", "RoundRobin", quantum.ToString()));
                //**************************viewLog
                Console.Write(l.ViewLog());
                //*****************view AVerage Waiting & TurnAround*********
                Console.WriteLine("\r\nAverage Waiting Time :  " + CalculateAverageWaitingTime(pList) + "\n");
                Console.WriteLine("Average TurnAround Time : " + CalculateAverageTurnaroundTime(pList) + "\n");
                //********************
            }
            else if (method.ToLower() == "mlfq" || (DEBUG_MODE && DEBUG_ALGORITHM == Algorithm.MLFQ))
            {
                double quantum = 1;
                try
                {
                    if (CommandLine["q"] != null)
                        quantum = double.Parse(CommandLine["q"]);
                    else if (CommandLine["quantum"] != null)
                        quantum = double.Parse(CommandLine["quantum"]);
                    else if (DEBUG_MODE)
                    {
                        quantum = DEBUG_QTIME;

                    }
                    else
                    {
                        ShowHelp_MissingQTime();
                        return;
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("[ERROR] quantum time not in correct format");
                    return;
                }
                FeedbackMultiQueue mlfq = new FeedbackMultiQueue(pList, quantum);
                foreach (Process item in pList)
                {
                    Console.WriteLine(item.CompleteInfo() + "\n");
                }
                Console.WriteLine(string.Format("{0," + Console.WindowWidth / 2 + "}\r\nQuantum Time: {1} Second(s)\r\n", "Multilevel Feedback Queue", quantum.ToString()));
                //**************************viewLog
                Console.Write(mlfq.ViewLog());
                //*****************view AVerage Waiting & TurnAround*********
                Console.WriteLine("\r\nAverage Waiting Time :  " + CalculateAverageWaitingTime(pList) + "\n");
                Console.WriteLine("Average TurnAround Time : " + CalculateAverageTurnaroundTime(pList) + "\n");
                //********************
            }
            else if (method.ToLower() == "cfs" || (DEBUG_MODE && DEBUG_ALGORITHM == Algorithm.CFS))
            {
                double quantum = 1;
                try
                {
                    if (CommandLine["q"] != null)
                        quantum = double.Parse(CommandLine["q"]);
                    else if (CommandLine["quantum"] != null)
                        quantum = double.Parse(CommandLine["quantum"]);
                    else if (DEBUG_MODE)
                    {
                        quantum = DEBUG_QTIME;

                    }
                    else
                    {
                        ShowHelp_MissingQTime();
                        return;
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("[ERROR] quantum time not in correct format");
                    return;
                }
                CFS cfs = new CFS(pList, quantum);
                foreach (Process item in pList)
                {
                    Console.WriteLine(item.CompleteInfo() + "\n");
                }
                Console.WriteLine(string.Format("{0," + Console.WindowWidth / 2 + "}\r\nQuantum Time: {1} Second(s)\r\n", "Completely Fair Scheduler", quantum.ToString()));
                //**************************viewLog
                Console.Write(cfs.ViewLog());
                //*****************view AVerage Waiting & TurnAround*********
                Console.WriteLine("\r\nAverage Waiting Time :  " + CalculateAverageWaitingTime(pList) + "\n");
                Console.WriteLine("Average TurnAround Time : " + CalculateAverageTurnaroundTime(pList) + "\n");
                //********************
            }
            else if (method.ToLower() == "hrr" || (DEBUG_MODE && DEBUG_ALGORITHM == Algorithm.HRR))
            {
                HRR hrr = new HRR(pList);
                foreach (Process item in pList)
                {
                    Console.WriteLine(item.CompleteInfo() + "\n");
                }
                Console.WriteLine(string.Format("{0," + Console.WindowWidth / 2 + "}\r\n", "Highest Response Ratio"));
                Console.Write(hrr.ViewLog());
                //*****************view AVerage Waiting & TurnAround*********
                Console.WriteLine("\nAverage Waiting Time :  " + CalculateAverageWaitingTime(pList) + "\n");
                Console.WriteLine("Average TurnAround Time : " + CalculateAverageTurnaroundTime(pList) + "\n");
                //*********************

            }
            else if (method.ToLower() == "priority" || (DEBUG_MODE && DEBUG_ALGORITHM == Algorithm.Priority))
            {
                Priority p = new Priority(pList);
                foreach (Process item in pList)
                {
                    Console.WriteLine(item.CompleteInfo() + "\n");
                }
                Console.WriteLine(string.Format("{0," + Console.WindowWidth / 2 + "}\r\n", "Priority Algorithm"));
                Console.Write(p.ViewLog());
                //*****************view AVerage Waiting & TurnAround*********
                Console.WriteLine("\nAverage Waiting Time :  " + CalculateAverageWaitingTime(pList) + "\n");
                Console.WriteLine("Average TurnAround Time : " + CalculateAverageTurnaroundTime(pList) + "\n");
                //*********************
            }
            else if (method.ToLower() == "spn" || (DEBUG_MODE && DEBUG_ALGORITHM == Algorithm.SPN))
            {
                SPN spn = new SPN(pList);
                foreach (Process item in pList)
                {
                    Console.WriteLine(item.CompleteInfo() + "\n");
                }
                Console.WriteLine(string.Format("{0," + Console.WindowWidth / 2 + "}\r\n", "SPN Algorithm"));
                Console.Write(spn.ViewLog());
                //*****************view AVerage Waiting & TurnAround*********
                Console.WriteLine("\nAverage Waiting Time :  " + CalculateAverageWaitingTime(pList) + "\n");
                Console.WriteLine("Average TurnAround Time : " + CalculateAverageTurnaroundTime(pList) + "\n");
                //*********************
            }
            else if (method.ToLower() == "srt" || (DEBUG_MODE && DEBUG_ALGORITHM == Algorithm.SRT))
            {
                SRT srt = new SRT(pList);
                foreach (Process item in pList)
                {
                    Console.WriteLine(item.CompleteInfo() + "\n");
                }
                Console.WriteLine(string.Format("{0," + Console.WindowWidth / 2 + "}\r\n", "SRT Algorithm"));
                Console.Write(srt.ViewLog());
                //*****************view AVerage Waiting & TurnAround*********
                Console.WriteLine("\nAverage Waiting Time :  " + CalculateAverageWaitingTime(pList) + "\n");
                Console.WriteLine("Average TurnAround Time : " + CalculateAverageTurnaroundTime(pList) + "\n");
                //*********************
            }
            else
            {
                Console.WriteLine("Unknown method");
                ShowAlgorithms();
                return;
            }
            
            
            #endregion
            
             
            
            if (DEBUG_MODE)
                Console.ReadLine();
        }

        static void ShowHelp()
        {
            Console.WriteLine(string.Format("Usage: {0} -i [process list filename] [OPTIONS]", System.AppDomain.CurrentDomain.FriendlyName));
            Console.WriteLine("\n\r\t/i, -i, --input [process list filename]");
            Console.WriteLine("\n\r\t/s, -s, --showprocesslist");
            ShowAlgorithms();
        }

        static void ShowAlgorithms()
        {
            Console.WriteLine("\n\r\t/a, -a, --algorithm [fifo|rr/roundrobin|spn|srt|hrr|lottery|mlfq|cfs|priority]");
            Console.WriteLine("\n\r\tOptions:");
            Console.WriteLine("\n\r\t\t/q, -q, --quantum (in seconds)");
        }

        static void ShowHelp_MissingQTime()
        {
            Console.WriteLine("[ERROR] quantum time is not specified (-q)");
        }

        /// <summary>
        /// Initiates processes from specified text file.
        /// File Structure:
        /// [Pid] [Arrival Time Format:(HH:MM:SS.fff)] [Service Time Format:(SS.fff)] [Priority]
        /// ex: 0 0:0:24.100 1.02 1
        /// </summary>
        /// <param name="file">Text file address</param>
        static List<Process> ReadFromFile(string file)
        {
            try
            {
                List<Process> pList;
                using (StreamReader sr = new StreamReader(file))
                {
                    string line;
                    pList = new List<Process>();
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            try
                            {
                                string[] items = line.Split(' ');
                                Process p = new Process(int.Parse(items[0]), TimeSpan.Parse(items[1]), TimeSpan.FromSeconds(double.Parse(items[2])), int.Parse(items[3]));
                                pList.Add(p);
                            }
                            catch (Exception ex)
                            {
                                //Log or skip
                                throw ex;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //failed?
                        throw ex;
                    }
                    finally { sr.Close(); }
                }
                return pList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        static TimeSpan CalculateAverageWaitingTime(List<Process> processlist)
        {
            TimeSpan totalwaitingTime=TimeSpan.FromSeconds(0.0) ;
            foreach (Process p in processlist)
            {
                totalwaitingTime += p.WaitingTime;
            }
            TimeSpan AverageWaitingTime ;
            return  AverageWaitingTime = new TimeSpan(totalwaitingTime.Ticks / processlist.Count);
        }

        static TimeSpan CalculateAverageTurnaroundTime(List<Process> processlist)
        {
            TimeSpan totalTurnaroundTime = TimeSpan.FromSeconds(0.0);
            foreach (Process p in processlist)
            {
                totalTurnaroundTime += p.TurnaroundTime;
            }
            TimeSpan AverageTurnaroundTime;
            return AverageTurnaroundTime = new TimeSpan(totalTurnaroundTime.Ticks / processlist.Count);
        }

    }
}
