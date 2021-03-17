using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HowLeaky_SimulationEngine.Outputs
{
    public enum OutputVectorType
    {
        None,
        Crop,
        Pesticide,
        SoilLayer
    }
    public class HowLeakyOutputDefinition
    {
        public HowLeakyOutputDefinition()
        {

        }
        public HowLeakyOutputDefinition(PropertyInfo prop, string module, Dictionary<string, Definitions.OutputAttributes> remapdict, string prefix, int? index)
        {
            VectorIndex=index;
            Actions=new List<Action<HowLeakyOutputTimeseriesActive,int>>();
            CodeName=prop.Name;
            Name=!String.IsNullOrEmpty(prefix)?$"{prop.Name}|{prefix}":prop.Name;
            if(remapdict!=null)
            {
                if(remapdict.ContainsKey(prop.Name))
                {
                    var attrs=remapdict[prop.Name];
                    DisplayName=attrs.DisplayName;
                    ColorValue=attrs.ColorValue;
                    CanAccumulate=attrs.CanAccumulate;
                }
                else
                {
                    DisplayName=prop.Name;
                    ColorValue="#000000";
                    CanAccumulate=true;
                }
            }
          
            Module=module;
        }


        

        public int? VectorIndex{get;set;}
        public List<Action<HowLeakyOutputTimeseriesActive,int>> Actions { get; set; }
        public string Module{get;set;}
        public string CodeName{get;set;}
        public string Name{get;set;}
        public string DisplayName{get;set;}
        public string Units{get;set;}
        public string ColorValue{get;set;}
        public bool UserDefinedActive{get;set;}// Means that user as asked to be able to save this output to an outptus file OR display it in the interface
        public bool SummaryDefinedActive{get;set;}//Means that there is an monthly or annual summary analysis that needs to use this time-series

        public bool IsActive(){
            return UserDefinedActive||SummaryDefinedActive;
        }
        public bool CanAccumulate{get;set;}
        public OutputVectorType VectorType{get;set;}
       

        internal string GetMethodName()
        {

            return $"Update{Module}_{CodeName}";
        }
       
    }
}
