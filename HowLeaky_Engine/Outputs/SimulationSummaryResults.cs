using HowLeaky_SimulationEngine.Engine;
using HowLeaky_SimulationEngine.Outputs;
using System;
using System.Collections.Generic;

namespace HowLeaky_Engine.Outputs
{
    public class SimulationSummaryResults
    { 
        public SimulationSummaryResults()
        {

        }

        public SimulationSummaryResults(HowLeakyEngine engine)
        {
            if (engine.WaterBalanceSummary != null)
            {
                WaterBalance = engine.WaterBalanceSummary;

            }
            if (engine.PesticideModules != null && engine.PesticideModules.Count > 0)
            {
                Pesticides = new List<HowLeakyOutputSummary_Pesticide>();
                foreach (var module in engine.PesticideModules)
                {
                    Pesticides.Add(module.Summary);
                }
            }
            if (engine.IrrigationModule != null && engine.IrrigationModule.InputModel.UseRingTank)
            {
                RingTank = engine.IrrigationModule.RingTankSummary;
            }
            if (engine.PhosphorusModule != null)
            {
                Phosphorus = engine.PhosphorusModule.Summary;
            }
            if (engine.NitrateModule != null)
            {
                Nitrate = engine.NitrateModule.Summary;
            }
            if (engine.SolutesModule != null)
            {
                Solutes = engine.SolutesModule.Summary;
            }


        }

        //public HowLeakyOutputSummary_CropYield CropYield { get; set; }
        //public HowLeakyOutputSummary_CropWaterBalance CropWaterBalance { get; set; }
        //public HowLeakyOutputSummary_Fallow Fallow { get; set; }
        public List<HowLeakyOutputSummary_Pesticide> Pesticides { get; set; }
        public HowLeakyOutputSummary_RingTank RingTank { get; set; }
        public HowLeakyOutputSummary_Phosphorus Phosphorus { get; set; }
        public HowLeakyOutputSummary_Solutes Solutes { get; set; }
        public HowLeakyOutputSummary_Nitrate Nitrate { get; set; }
        public HowLeakyOutputSummary_WaterBalance WaterBalance { get; set; }

        public void ScaleValues(double scale)
        {
            if (WaterBalance != null)
            {
                WaterBalance.ScaleValues(scale);
            }
            if (Pesticides != null)
            {
                foreach (var summary in Pesticides)
                {
                    summary.ScaleValues(scale);
                }
            }
            if (RingTank != null)
            {
                RingTank.ScaleValues(scale);
            }
            if (Phosphorus != null)
            {
                Phosphorus.ScaleValues(scale);
            }
            if (Solutes != null)
            {
                Solutes.ScaleValues(scale);
            }
            if (Nitrate != null)
            {
                Nitrate.ScaleValues(scale);
            }
        }

        public void CombineScaledValues(SimulationSummaryResults newSummaries, double scale)
        {
            if (WaterBalance != null)
            {
                WaterBalance.CombineScaledValues(newSummaries.WaterBalance, scale);
            }
            if (Pesticides != null)
            {

                foreach (var summary in Pesticides)
                {
                    summary.CombineScaledValues(newSummaries.Pesticides, scale);
                }
            }
            if (RingTank != null)
            {
                RingTank.CombineScaledValues(newSummaries.RingTank, scale);
            }
            if (Phosphorus != null)
            {
                Phosphorus.CombineScaledValues(newSummaries.Phosphorus, scale);
            }
            if (Solutes != null)
            {
                Solutes.CombineScaledValues(newSummaries.Solutes, scale);
            }
            if (Nitrate != null)
            {
                Nitrate.CombineScaledValues(newSummaries.Nitrate, scale);
            }
        }
    }
}
