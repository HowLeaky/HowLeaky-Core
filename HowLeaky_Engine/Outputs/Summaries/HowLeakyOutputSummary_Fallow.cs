using HowLeaky_SimulationEngine.Engine;
using HowLeaky_SimulationEngine.Outputs.Summaries;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Outputs
{
    public class HowLeakyOutputSummary_Fallow: HowLeakyOutputSummary_Custom
    {
        public override void ConnectToSimulation(HowLeakyEngine sim)
        {
        }
        
        public double SumFallowRainfall { get; set; }
        public double SumFallowIrrigation { get; set; }
        public double SumFallowOverflow { get; set; }
        public double SumFallowLateralFlow { get; set; }
        public double SumFallowRunoff { get; set; }
        public double SumFallowSoilevaporation { get; set; }
        public double SumFallowDrainage { get; set; }
        public double SumFallowSoilerosion { get; set; }
        public double FallowEfficiency { get; set; }
        public double SumFallowSoilwater { get; set; }
        public double FallowDaysWithMore50pcCov { get; set; }
        public double AccumulatedFallowSedDeliv { get; set; }


         public void UpdateFallowWaterBalance(HowLeakyEngine Sim)
        {
            try
            {
                if (Sim.InFallow())
                {
                    SumFallowRainfall += Sim.ClimateModule.Rain;
                    SumFallowRunoff += Sim.SoilModule.Runoff;
                    SumFallowSoilevaporation += Sim.SoilModule.SoilEvap;
                    SumFallowDrainage += Sim.SoilModule.DeepDrainage;
                    SumFallowSoilerosion += Sim.SoilModule.HillSlopeErosion;

                    if (Sim.SoilModule.TotalCoverAllCrops > 0.5)
                    {
                        ++FallowDaysWithMore50pcCov;
                    }
                }
                else if (Sim.IsPlanting())
                {
                    SumFallowSoilwater += Sim.CalcFallowSoilWater();
                }
            }
            catch (Exception e)
            {                
                throw new Exception(e.Message);
            }
        }
        
    }
}
