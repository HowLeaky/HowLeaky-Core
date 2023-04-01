using HowLeaky_Engine.Outputs;
using HowLeaky_SimulationEngine.Engine;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HowLeaky_SimulationEngine.Outputs
{
    public class HowLeakyOutputs
    {

        public HowLeakyOutputs(BrowserDate startDate,BrowserDate endDate)
        {
            StartDate=new BrowserDate(startDate);
            EndDate=new BrowserDate(endDate);
            TimeSeries=new List<HowLeakyOutputTimeseriesActive>();
         //   Definitions=new List<HowLeakyOutputDefinition>();
        }

        public HowLeakyOutputs()
        {
        }

        public BrowserDate StartDate{get;set;}
        public BrowserDate EndDate{get;set;}

        public SimulationSummaryResults Summaries { get; set; }

       //public List<int>DateInts{get;set;}
       //  public List<HowLeakyOutputDefinition>Definitions{get;set;}

       //public void LoadDefaultDefinitions(List<System.Reflection.PropertyInfo> props, string module)
       //{
       //    foreach(var prop in props)
       //    {
       //        Definitions.Add(new HowLeakyOutputDefinition(prop, module));
       //    }
       //}

       public string GetNamesCSV()
        {
            return String.Join(",",TimeSeries.Select(x=>x.OutputDefn.CodeName).ToList());
        }


        public List<HowLeakyOutputTimeseriesActive>TimeSeries{get;set;}
        
        public void LoadSummaries(HowLeakyEngine engine)
        {
            Summaries = new SimulationSummaryResults(engine);
        }
      
        public string GetValueString(int tsindex, int dayindex)
        {
            return $"{TimeSeries[tsindex].DailyValues[dayindex]:F4}";
        }

        //public void SelectTimeSeries(string csvlist)
        //{
        //    var items=csvlist.Split(',').ToList();
        //    foreach(var item in items)
        //    {
        //        ActivateOutput(item.Trim());               
        //    }
        //}

        //public bool ActivateOutput(string codename)
        //{
        //    var outputtype=Definitions.FirstOrDefault(x=>x.CodeName==codename);
        //    if (outputtype != null)
        //    { 
        //        outputtype.UserDefinedActive=true;
        //        return true;
        //    }
        //    return false;
        //}

        // public void AssignDelegates(HowLeakyEngine engine)
        //{
        //    try
        //    {            
        //        var list=new List<HowLeakyOutputTimeseries>();
        //        foreach(var outputtype in Definitions.Where(x=>x.IsActive()).ToList())
        //        {
        //            var engineType = engine.GetType();
        //            var methodInfo = engineType.GetMethod(outputtype.GetMethodName()); 
        //            if(methodInfo!=null)
        //            {
        //                //var action=DelegateBuilder.BuildDelegate<Action<HowLeakyOutputTimeseries>>(methodInfo);                                            

        //                var action = (Action<HowLeakyOutputTimeseries>)Delegate.CreateDelegate(typeof(Action<HowLeakyOutputTimeseries>), engine,methodInfo);


        //                if(outputtype.VectorType==OutputVectorType.None)
        //                {                   
        //                    list.Add(new HowLeakyOutputTimeseries(outputtype,null,action));
        //                }
        //                else if(outputtype.VectorType==OutputVectorType.Crop)
        //                {
        //                    var index=0;
        //                    foreach(var crop in engine.VegetationModules)
        //                    {
        //                        list.Add(new HowLeakyOutputTimeseries(outputtype,index,action));
        //                        ++index;
        //                    }
        //                }
        //                else if(outputtype.VectorType==OutputVectorType.Pesticide)
        //                {
        //                     var index=0;
        //                    foreach(var pest in engine.PesticideModules)
        //                    {
        //                        list.Add(new HowLeakyOutputTimeseries(outputtype,index,action));
        //                        ++index;
        //                    }
        //                }
        //                else if(outputtype.VectorType==OutputVectorType.SoilLayer)
        //                {
        //                    var layercount=engine.SoilModule.LayerCount;
        //                    for(var index=0;index<layercount;++index)
        //                    {
        //                        list.Add(new HowLeakyOutputTimeseries(outputtype,index,action));
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                var name=outputtype.GetMethodName();
        //                throw new Exception($"Couldn't find {name} in HowLeakyEngine:AssignDelegates");

        //            }
        //        }
        //        TimeSeries=list;
        //    }
        //    catch (Exception ex)
        //    { 
        //        throw new Exception("Error Assigning delegates in HowLeakyOutputs");
        //    }
        //}

        public void UpdateDailyTimeseries(int index)
        {
            if (TimeSeries != null)
            {
                foreach (var output in TimeSeries)
                {

                    output.UpdateTimeseries(index);
                }
            }
           
        }

        
    }

    
}
