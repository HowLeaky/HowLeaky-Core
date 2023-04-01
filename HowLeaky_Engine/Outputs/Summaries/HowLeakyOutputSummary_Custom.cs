using HowLeaky_SimulationEngine.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Outputs
{
    public class HowLeakyOutputSummary_Custom
    {

        public HowLeakyOutputSummary_Custom()
        {

        }
        // public string RequiredTimeSeriesIdsCsv{get;set;}        
        public string Name { get; set; }
        public virtual void ConnectToSimulation(HowLeakyEngine sim)
        {

        }

        protected void CombineAndScaleArray(List<double> data1, List<double> data2, double scale)
        {
            var count = data1.Count;
            for (var i = 0; i < count; i++)
            {
                data1[i] = data1[i]+ data2[i] * scale;
            }
        }

        protected void ScaleArray(List<double> data, double scale)
        {
            var count = data.Count;
            for (var i = 0; i < count; i++)
            {
                data[i] = data[i]* scale;
            }
        }
    }
}
