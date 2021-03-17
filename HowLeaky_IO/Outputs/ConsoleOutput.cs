using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowLeaky_IO.Outputs
{
    public class ConsoleOutputLogger : INotifyPropertyChanged
    {
        public ConsoleOutputLogger(bool logtoconsole = false)
        {
            LogToConsole = logtoconsole;
            ConsoleOutputList = new List<string>();
            ErrorOutputList = new HashSet<string>();
        }
        public bool LogToConsole { get; set; }
        public string ConsoleOutput { get; set; }
        public List<string> ConsoleOutputList { get; set; }
        public HashSet<string> ErrorOutputList { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddConsoleOutput(string text, bool logtime = true)
        {
            if (LogToConsole)
            {
                Console.WriteLine(text);
            }
            else
            {
                ConsoleOutputList.Add(text);
                ConsoleOutput = string.Join("\n", ConsoleOutputList);
            }
        }


        static readonly object _lockObject = new object();
        public void AddErrorOutput(string text)
        {
            lock (_lockObject)
            {
                if (LogToConsole)
                {
                    Console.WriteLine($"ERROR :{text}");
                    ErrorOutputList.Add(text);
                }
                else
                {
                    var initialcount = ErrorOutputList.Count;
                    ErrorOutputList.Add(text);
                    if (ErrorOutputList.Count > initialcount)
                    {
                        var errortext = $"ERROR {initialcount + 1}: {text}";
                        ConsoleOutputList.Add(errortext);
                        ConsoleOutput = string.Join("\n", ConsoleOutputList);
                    }
                }
            }
        }

        public void ClearConsoleOutput()
        {
            ConsoleOutputList = new List<string>();
            ErrorOutputList = new HashSet<string>();
            ConsoleOutput = "";
        }


    }
}
