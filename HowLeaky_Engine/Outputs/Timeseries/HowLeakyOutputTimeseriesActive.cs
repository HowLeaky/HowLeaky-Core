using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HowLeaky_SimulationEngine.Outputs
{
    public class HowLeakyOutputTimeseriesActive:HowLeakyOutputTimeSeries
    {
        //public HowLeakyOutputTimeseries(HowLeakyOutputDefinition outputtype, int? index, Action<HowLeakyOutputTimeseries> action)
        //{
           
        //  //  Action=action;
        //    Index=index;
        //}

        public HowLeakyOutputTimeseriesActive(HowLeakyOutputDefinition outputtype,  BrowserDate start, BrowserDate end):base()
        {
            Name=outputtype.DisplayName;//$"{outputtype.DisplayName}";
            StartDate=new BrowserDate(start);
            EndDate=new BrowserDate(end);
            OutputDefn=outputtype;
            Index=outputtype.VectorIndex;
            OrderIndex=outputtype.OrderIndex;
            ColorValue=outputtype.ColorValue;
            Width= outputtype.Width;
            DailyValues=new List<double?>(new double?[EndDate.DateInt-StartDate.DateInt+1]);
            CanAccumulate=outputtype.CanAccumulate;
        }

        

       
        
        public HowLeakyOutputDefinition OutputDefn {get;set;}
      //  public Action<HowLeakyOutputTimeseries> Action { get; set; }

        internal void UpdateTimeseries(int index)
        {
            
            foreach(var action in OutputDefn.Actions)
            {
                action(this,index);
            }
        }

        public double GetAverageAnnualValue()
        {
            if(DailyValues!=null)
            {
                var sum = DailyValues.Sum(x=>x??0);
                var length=EndDate.DateInt-StartDate.DateInt+1;
                if(length>0)
                {
                    if(OutputDefn.CanAccumulate)
                    { 
                        return sum/(length/365.0);
                    }
                    else
                    {
                        return sum/length;
                    }
                }
            }
            return 0;
        }
    }
}
