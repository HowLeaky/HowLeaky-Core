using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Outputs.Definitions
{
    public class OutputAttributes
    {
        public OutputAttributes(string codeName, int? dataIndex, string displayedName, string color, float width,int orderindex, bool canAccumulate)
        {
            OrderIndex= orderindex;
            CodeName = codeName;
            DisplayName=displayedName;
            ColorValue=color;
            CanAccumulate=canAccumulate;
            Width=width;
            DataIndex=dataIndex;
        }
        public int OrderIndex { get;set;}
        public string DisplayName{get;set;}
        public string CodeName{get;set;}
        public string ColorValue{get;set;}
        public float Width { get;set;}
        public bool CanAccumulate{get;set;}

        public int? DataIndex { get;set;}
    }
}
