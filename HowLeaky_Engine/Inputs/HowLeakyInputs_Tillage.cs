using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Inputs
{
    public class HowLeakyInputs_Tillage:_CustomHowLeakyInputsModel
    {
        public HowLeakyInputs_Tillage(string name):base(null,name)
        {

        }
         public HowLeakyInputs_Tillage(Guid? id, string name):base(id,name)
        {
         
        }
        public int PrimaryTillType { get;set;}//Redundant parameter.... just here to keep legacy happy.

        public DayMonthData StartTillWindow { get; set; }

        public DayMonthData EndTillWindow { get; set; }

        public DayMonthData PrimaryTillDate { get; set; }

        public DayMonthData SecondaryTillDate1 { get; set; }

        public DayMonthData SecondaryTillDate2 { get; set; }

        public DayMonthData SecondaryTillDate3 { get; set; }

        public Sequence PrimaryTillageDates { get; set; }

        public double PrimaryCropResMultiplier { get; set; }

        public double PrimaryRoughnessRatio { get; set; }

        //public double SecondaryCropResMultiplier { get; set; }

        //public double SecondaryRoughnessRatio { get; set; }

        public double RainForPrimaryTill { get; set; }

        public int NoDaysToTotalRain { get; set; }

        public int MinDaysBetweenTills { get; set; }
      
        public int Format { get; set; }
        public int Type { get; set; }
   
    }
}
