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
        public HowLeakyOutputTimeseriesActive(string name, BrowserDate start, BrowserDate end, List<double> values, bool canAccumulate) : base()
        {
            Name = name;
            StartDate = new BrowserDate(start);
            EndDate = new BrowserDate(end);
            DailyValues = values.Select(x=>(double?)x).ToList();
            CanAccumulate = canAccumulate;
        }

        public string GetName()
        {
            if(OutputDefn!=null)
                return OutputDefn.Name;
            return Name;
        }

        public HowLeakyOutputTimeseriesActive(string simulationname, Guid?simulationId, HowLeakyOutputDefinition outputtype,  BrowserDate start, BrowserDate end):base()
        {
            Id = outputtype.Id;
            SimulationId = simulationId;
            if (outputtype.IncludeSimReference)
            {
                Name = $"{simulationname} {outputtype.DisplayName}";//$"{outputtype.DisplayName}";
            }
            else
            {
                Name = outputtype.Name;
            }
            StartDate=new BrowserDate(start);
            EndDate=new BrowserDate(end);
            OutputDefn=outputtype;
            Index=outputtype.VectorIndex;
            OrderIndex=outputtype.OrderIndex;
            ColorValue=outputtype.ColorValue;
            Width= outputtype.Width;
            DailyValues=new List<double?>(new double?[EndDate.DateInt-StartDate.DateInt+1]);
            CanAccumulate=outputtype.CanAccumulate;
            Displayed = outputtype.Displayed;
            Units=outputtype.Units;
        }

        public HowLeakyOutputTimeseriesActive(int? simindex, string name, BrowserDate start, BrowserDate end, int count, bool canAccumulate):
            base(simindex,name,start,end,count,canAccumulate)
        {
        }

        public bool Displayed { get; set; }
        
        
       
        
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
            if(DailyValues!=null&&DailyValues.Count>0)
            {
                var sum = DailyValues.Sum(x=>x??0);
                var length=EndDate.DateInt-StartDate.DateInt+1;
                if(length>0)
                {
                    if(OutputDefn!=null?OutputDefn.CanAccumulate:true)
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
