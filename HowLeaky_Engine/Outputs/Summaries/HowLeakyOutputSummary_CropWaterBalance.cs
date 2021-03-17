using HowLeaky_SimulationEngine.Engine;
using HowLeaky_SimulationEngine.Outputs.Summaries;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Outputs
{
    public class HowLeakyOutputSummary_CropWaterBalance:HowLeakyOutputSummary_Custom
    {
        public override void ConnectToSimulation(HowLeakyEngine sim)
        {
        }
          
        public double SumCropRainfall { get; set; }
        public double SumCropIrrigation { get; set; }
        public double SumCropRunoff { get; set; }
        public double SumCropSoilEvaporation { get; set; }
        public double SumCropTranspiration { get; set; }
        public double SumCropEvapotranspiration { get; set; }
        public double SumCropOverflow { get; set; }
        public double SumCropDrainage { get; set; }
        public double SumCropLateralFlow { get; set; }
        public double SumCropSoilErosion { get; set; }       
        public double AccumulateCovDayBeforePlanting { get; set; }       
        public double TotalNumberPlantings { get; set; }
        public double AccumulatedCropSedDeliv { get; set; }



        public double AvgCropRainfall  { get; set; }
        public double AvgCropIrrigation  { get; set; }
        public double AvgCropRunoff  { get; set; }
        public double CropSoilEvaporation  { get; set; }
        public double CropTranspiration  { get; set; }
        public double AvgCropEvapoTranspiration  { get; set; }
        public double AvgCropOverflow  { get; set; }
        public double AvgCropDrainage  { get; set; }
        public double AvgCropLateralFlow  { get; set; }
        public double AvgCropSoilErrosion  { get; set; }
        public double AnnualCropSedimentDelivery  { get; set; }


       
        public void UpdateCropWaterBalance(HowLeakyEngine Sim, _CustomHowLeakyEngine_VegModule crop)
        {
            try
            {
                if (crop.ExistsInTheGround)
                {
                    SumCropRainfall += Sim.ClimateModule.Rain;
                    SumCropIrrigation += Sim.SoilModule.Irrigation;
                    SumCropRunoff += Sim.SoilModule.Runoff;
                    SumCropSoilEvaporation += Sim.SoilModule.SoilEvap;
                    SumCropTranspiration += crop.TotalTranspiration;
                    SumCropEvapotranspiration = SumCropSoilEvaporation + SumCropTranspiration;
                    SumCropOverflow += Sim.SoilModule.Overflow;
                    SumCropDrainage += Sim.SoilModule.DeepDrainage;
                    SumCropLateralFlow += Sim.SoilModule.LateralFlow;
                    SumCropSoilErosion += Sim.SoilModule.HillSlopeErosion;
                }

            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
        }

         public void CalculateSummaryOutputs(HowLeakyEngine Sim, _CustomHowLeakyEngine_VegModule crop)
        {
            double denom = crop.PlantingCount;
            AvgCropRainfall = MathTools.Divide(SumCropRainfall, denom);
            AvgCropIrrigation = MathTools.Divide(SumCropIrrigation, denom);
            AvgCropRunoff = MathTools.Divide(SumCropRunoff, denom);
            CropSoilEvaporation = MathTools.Divide(SumCropSoilEvaporation, denom);
            CropTranspiration = MathTools.Divide(SumCropTranspiration, denom);
            AvgCropEvapoTranspiration = MathTools.Divide(SumCropEvapotranspiration, denom);
            AvgCropOverflow = MathTools.Divide(SumCropOverflow, denom);
            AvgCropDrainage = MathTools.Divide(SumCropDrainage, denom);
            AvgCropLateralFlow = MathTools.Divide(SumCropLateralFlow, denom);
            AvgCropSoilErrosion = MathTools.Divide(SumCropSoilErosion, denom);
            AnnualCropSedimentDelivery = MathTools.Divide(SumCropSoilErosion, denom) * Sim.SoilModule.InputModel.SedDelivRatio;
        }
        
    }
}
