using HowLeaky_SimulationEngine.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Outputs.Summaries
{
    public class HowLeakyOutputSummary_Custom
    {
        public string RequiredTimeSeriesIdsCsv{get;set;}        

        public virtual void ConnectToSimulation(HowLeakyEngine sim)
        {

        }
    }
}
