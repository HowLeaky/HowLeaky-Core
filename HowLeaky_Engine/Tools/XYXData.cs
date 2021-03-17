using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Tools
{
    public class XYXData
    {
        public XYXData(List<string> values)
        {
            DateInt=int.Parse(values[0]);
            if(values.Count>=2)
            {
                Value1=double.Parse(values[1]);
            }
            if(values.Count>=3)
            {
                Value2=double.Parse(values[2]);
            }
            if(values.Count>=4)
            {
                Value3=double.Parse(values[3]);
            }
        }

        public double Value1{get;set;}
        public double Value2{get;set;}
        public double Value3{get;set;}
        public int DateInt{get;set;}
    }
}
