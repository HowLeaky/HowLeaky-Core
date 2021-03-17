using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HowLeaky_SimulationEngine.Tools
{
    public class TimeSeriesData
    {
        List<BrowserDate> Dates { get; set; } = new List<BrowserDate>();
        List<double> Values { get; set; } = new List<double>();


        public TimeSeriesData()
        {
            Dates = new List<BrowserDate>();
            Values = new List<double>();
        }

        public TimeSeriesData(List<BrowserDate> dates, List<double> values)
        {
            dates.CopyTo(Dates.ToArray());
            values.CopyTo(Values.ToArray());
        }

        public TimeSeriesData(string stringvalue)
        {
        }

        public int GetCount()
        {
            return Dates.Count;

        }

       public double GetValueAtDate(BrowserDate date)
        {
            var dateitem=Dates.FirstOrDefault(x=>x.DateInt==date.DateInt);
            if(dateitem!=null)
            {
                int index = Dates.IndexOf(dateitem);

                if (index >= 0)
                {
                    return Values[index];
                }
            }
            return MathTools.MISSING_DATA_VALUE;
        }
    }
}
