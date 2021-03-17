using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_ValidationEngine.Models
{
    class TimeSeries
    {
        public TimeSeries()
        {
            Dates=new List<BrowserDate>();
            Values=new List<double?>();
        }
        public List<BrowserDate> Dates { get;set;}
        public List<double?> Values { get;set;}
    }
}
