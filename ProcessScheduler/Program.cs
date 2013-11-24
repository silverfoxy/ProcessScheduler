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
        static double DEBUG_QTIME = 0.8;
        static bool DEBUG_SHOW_PROCESS_LIST = true;
        static Algorithm DEBUG_ALGORITHM = Algorithm.FIFO;

        enum Algorithm
        { 
            SPN,
            FIFO,
            RR
        };

        static void Main(string[] args)
        {
            if (CLEAR_CONSOLE)
                Console.Clear();

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
            if (method.ToLower() == "fifo" || DEBUG_ALGORITHM == Algorithm.FIFO)
            {
                FIFO f = new FIFO(pList);
                Console.WriteLine(string.Format("{0," + Console.WindowWidth / 2 + "}\r\n", "FIFO"));
                Console.Write(f.ViewLog());
            }
            else if (method.ToLower() == "rr" || method.ToLower() == "roundrobin" || DEBUG_ALGORITHM == Algorithm.RR)
            {
                double quantum = 1;
                try
                {
                    if (CommandLine["q"] != null)
                        quantum = double.Parse(CommandLine["q"]);
                    else if (CommandLine["quantum"] != null)
                        quantum = double.Parse(CommandLine["quantum"]);
                    else if (DEBUG_MODE)
                        quantum = DEBUG_QTIME;
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
                Console.WriteLine(string.Format("{0," + Console.WindowWidth / 2 + "}\r\nQuantum Time: {1} Second(s)\r\n", "RoundRobin", quantum.ToString()));

                Console.Write(rr.ViewLog());
            }
            else if (method.ToLower() == "spn" || DEBUG_ALGORITHM == Algorithm.SPN)
            {
                double quantum = 1;
                try
                {
                    if (CommandLine["q"] != null)
                        quantum = double.Parse(CommandLine["q"]);
                    else if (CommandLine["quantum"] != null)
                        quantum = double.Parse(CommandLine["quantum"]);
                    else if (DEBUG_MODE)
                        quantum = DEBUG_QTIME;
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
                ShortestProcessNext spn = new ShortestProcessNext(pList, quantum);
                Console.WriteLine(string.Format("{0," + Console.WindowWidth / 2 + "}\r\nQuantum Time: {1} Second(s)\r\n", "ShortestProcessNext", quantum.ToString()));

                Console.Write(spn.ViewLog());
            }
            else
            {
                Console.WriteLine("Unknown method");
                ShowAlgorithms();
                return;
            }
            #endregion
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
            Console.WriteLine("\n\r\t/a, -a, --algorithm [fifo|rr/roundrobin|spn]");
            Console.WriteLine("\n\r\tRoundRobin/ShortestProcessNext Options:");
            Console.WriteLine("\n\r\t\t/q -q --quantum (in seconds)");
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
    }
}
