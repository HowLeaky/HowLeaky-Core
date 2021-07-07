using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HowLeaky_IO
{
    public class GlobalProgress:INotifyPropertyChanged
    {
        public GlobalProgress()
        {
            
            FileNames =new List<string>();
            Reset();
            Max=100;
            progressReport = new Progress<int>((i) => Percent=100 * i / (Max - 1));
        }

        public GlobalProgress(int limit)
        {
            LogToConsole=true;
            Max =limit;
            progressReport = new Progress<int>((i) => Console.Write($"PROGRESS: {100 * i / (Max - 1)}%"));
        }
        public IProgress<int> progressReport;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool LogToConsole { get;set;}
        public int Max{get;set;}
        public int Value{get;set;}
        public int Percent{get;set;}

        public DateTime Start{get;set;}

        public string Message{get;set;}
        public List<string>FileNames{get;set;}

        public void Increment()
        {
            if(Max>1)
            {
                Value++;
                Percent=100 * Value / (Max - 1);
                Message=$"Simulating {Value}/{Max} ({Percent}%) - {GetTime()}";
            }
            else
            {
                Message=$"FINISHED - {GetTime()}";
            }
            if(LogToConsole)
            {
                Console.WriteLine(Message);
            }
          //  progressReport.Report(Value);
        }

        public string GetTime()
        {
            
            var ts=DateTime.Now-Start;
            return ts.ToString("h'h 'm'm 's's'");
        }

        public void AddCompletionMessage(string msg)
        {
            Message=$"{msg} ({GetTime()})";
            //Value=Max;
        }

        public void Reset()
        {
            Value=0;
            Message="";
        }

        public void PrepareForExecution()
        {
            Message="Ready for Simulation";
            Value=0;
        }

        public void StartExecution()
        {
            Value=0;
            Message="Simulating";
            Start=DateTime.Now;
        }

        public void LogFileName(string filename)
        {
            FileNames.Add(filename);
        }
    }
}
