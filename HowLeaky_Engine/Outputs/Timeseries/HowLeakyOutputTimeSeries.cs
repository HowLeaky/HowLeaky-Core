using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace HowLeaky_SimulationEngine.Outputs
{
    public class HowLeakyOutputTimeSeries
    {


        public HowLeakyOutputTimeSeries()
        {
            CanAccumulate=true;
        }

        public HowLeakyOutputTimeSeries(string name)
        {
            SimulationIndex = null;
            Name = name;
            StartDate = null;
            EndDate = null;
            Index = null;
            DailyValues = new List<double?>();
            CanAccumulate=true;
        }

        public HowLeakyOutputTimeSeries(int? simindex, string name,string color, float width, BrowserDate start, BrowserDate end, int count, bool canAccumulate)
        {
            SimulationIndex = simindex;
            Name = name;
            StartDate = new BrowserDate(start);
            EndDate = new BrowserDate(end);
            Index = null;
            Initialise(count);
            CanAccumulate = canAccumulate;
            ColorValue = color;
            Width = width;
        }

        public HowLeakyOutputTimeSeries(string name, string color, float width,int? index, BrowserDate start, BrowserDate end, List<double?> values, bool canAccumulate)
        {
            Name = name;
            StartDate = new BrowserDate(start);
            EndDate = new BrowserDate(end);
            Index = index;
            ColorValue = color;
            DailyValues = values;
            CanAccumulate = canAccumulate;
            Width=width;
        }
        public HowLeakyOutputTimeSeries(string name, string color, float width, int? index, BrowserDate start, BrowserDate end, List<double> values, bool canAccomulate)
        {
            Name = name;
            StartDate = new BrowserDate(start);
            EndDate = new BrowserDate(end);
            Index = index;
            ColorValue = color;
            DailyValues = values.Select(x => (double?)x).ToList();
            CanAccumulate = canAccomulate;
            Width = width;
        }

        public string DisplayName { get { return $"{SimulationIndex}. {Name}"; } }
        public string Name { get; set; }
        public bool CanAccumulate { get; set; }

        public int? SimulationIndex { get; set; }
        public string ColorValue { get; set; }
        public float Width { get;set;}
        public BrowserDate StartDate { get; set; }
        public BrowserDate EndDate { get; set; }
        public List<double?> DailyValues { get; set; }

        public List<BrowserDate> DateValues { get; set; }

        public int? Index { get; set; }

        public void Initialise(int count)
        {
            DailyValues = new List<double?>(new double?[count]);
        }

        internal void Update(int index, double value)
        {
            //not going to check ranges in here... looking for speed.
            DailyValues[index] = value;
        }


        public List<double?> MonthlyValues()
        {
            var monthlyValues = new List<double?>();
            var lastDate = StartDate;
            double? sum = null;
            int count = 0;
            for (var i = 0; i < DailyValues.Count; ++i)
            {
                var date = StartDate.AddDays(i);
                if (date.Month == lastDate.Month )
                {
                    ++count;
                    if (DailyValues[i] != null)
                    {
                        if (sum != null)
                        {
                            sum += (double)DailyValues[i];
                        }
                        else
                        {
                            sum = (double)DailyValues[i];
                        }
                    }
                }
                else
                {
                    UpdateValues(monthlyValues,sum,count);
                    count=1;
                    sum = DailyValues[i];
                    lastDate = new BrowserDate(date);
                }
            }
            if(sum!=null)
            {
                UpdateValues(monthlyValues, sum, count);
            }
            
            return monthlyValues;
        }

       

        public List<double?> YearlyValues()
        {
            var yearlyValues = new List<double?>();
            var lastDate = new BrowserDate(StartDate);
            double? sum = null;
            int count=0;
            for (var i = 0; i < DailyValues.Count; ++i)
            {
                var date = StartDate.AddDays(i);
                if (date.Year == lastDate.Year )
                {
                    ++count;
                    if (DailyValues[i] != null)
                    {
                        if (sum != null)
                        {
                            sum += (double)DailyValues[i];
                        }
                        else
                        {
                            sum = (double)DailyValues[i];
                        }
                    }
                }
                else
                {
                    UpdateValues(yearlyValues, sum, count);
                    count =1;
                    sum = DailyValues[i];
                    lastDate = new BrowserDate(date);
                }
            }
            if (sum != null)
            {
                UpdateValues(yearlyValues, sum, count);
            }
            return yearlyValues;
        }


        public void UpdateValues(List<double?> values, double? sum, int days)
        {
            if (CanAccumulate)
            {
                values.Add(sum);
            }
            else
            {

                if (sum != null)
                {
                    values.Add(sum / (float)days);
                }
                else
                {
                    values.Add(null);
                }
            }
        }

        public void InfillMissingValues()
        {
            StartDate = DateValues.FirstOrDefault();
            EndDate = DateValues.LastOrDefault();
            var count = EndDate.DateInt - StartDate.DateInt + 1;
            var list = new List<double?>(new double?[count]);

            int last=-1;
            foreach(var date in DateValues)
            {
                if(date.DateInt<last)
                {
                    Debug.WriteLine(":");
                }
                last=date.DateInt;
            }
            BrowserDate lastDate=null;
            if (DateValues != null)
            {
                var currentIndex = 0;
                for (var i = 0; i < count; ++i)
                {

                    var dateInt = StartDate.DateInt + i;
                    if (dateInt == DateValues[currentIndex].DateInt)
                    {
                        list[i] = DailyValues[currentIndex];
                        if(lastDate!=null&&DateValues[currentIndex].DateInt<=lastDate.DateInt)
                        {
                            throw new Exception($"Dates in data file are not sequential starting {lastDate.ToString("dd-MM-yyyy")}. ");
                        }
                        lastDate=DateValues[currentIndex];
                        currentIndex++;
                    }
                    else
                    {
                        list[i] = null;
                    }
                }
            }
            DateValues = null;
            DailyValues = list;
        }
    }
}
