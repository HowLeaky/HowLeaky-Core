using HowLeaky_SimulationEngine.Attributes;
using System;
using System.Linq;
using System.Xml.Serialization;

namespace HowLeaky_SimulationEngine.Tools
{
    public class DayMonthData
    {


        public int Day { get; set; }
        public int Month { get; set; }
        public bool Enabled { get; set; } = true;

        public DayMonthData()
        {
            Day = MathTools.MISSING_DATA_VALUE;
            Month = MathTools.MISSING_DATA_VALUE;
        }

        public DayMonthData(int day, int month)
        {
            Day = day;
            Month = month;
        }

        public DayMonthData(string stringvalue)            
        {
            var items=stringvalue.Split(',').ToList();
            if(items.Count==2)
            {
                Day=int.Parse(items[0]);
                Month=int.Parse(items[1]);
            }
        }

        public bool MatchesDate(BrowserDate date)
        {
            if(Day == MathTools.MISSING_DATA_VALUE || Month==MathTools.MISSING_DATA_VALUE || Enabled == false)
            {
                return false;
            }
            return date.Day==Day &&date.Month==Month;
        }

        internal bool IsValid()
        {
            return (Month>=1&&Month<=12&&Day>=1&&Day<=31);
            
        }
    }
}
