using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Outputs.Definitions
{
    public class OutputAttributes
    {
        public OutputAttributes(string codeName, string displayedName, string color, bool canAccumulate)
        {
            CodeName = codeName;
            DisplayName=displayedName;
            ColorValue=color;
            CanAccumulate=canAccumulate;
        }

        public string DisplayName{get;set;}
        public string CodeName{get;set;}
        public string ColorValue{get;set;}
        public bool CanAccumulate{get;set;}
    }
}
