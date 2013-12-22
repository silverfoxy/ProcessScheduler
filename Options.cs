using System;
using CommandLine;
using CommandLine.Text;

namespace ProcessScheduler
{
    public class Options
    {
        [Option("i", "input", Required = true, HelpText = "Input file to read.")]
        public string InputFile { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var usage = new StringBuilder();
            usage.AppendLine("Process Scheduler");
            usage.AppendLine("-i, --input [filename]");
            return usage.ToString();
        }
    }
}