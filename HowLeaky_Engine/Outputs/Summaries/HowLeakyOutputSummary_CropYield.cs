using HowLeaky_SimulationEngine.Attributes;
using HowLeaky_SimulationEngine.Engine;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Outputs
{
    public class HowLeakyOutputSummary_CropYield : HowLeakyOutputSummary_Custom
    {
        public HowLeakyOutputSummary_CropYield()
        {

        }

        public HowLeakyOutputSummary_CropYield(HowLeakyEngine Sim)
        {

        }


        //Summary variables
        [Output] public double YieldPerHarvest { get; set; }
        [Output] public double YieldPerPlant { get; set; }
        [Output] public double YieldPerYear { get; set; }
        [Output] public double YieldDivTranspir { get; set; }
        [Output] public double ResidueCovDivTranspir { get; set; }
        [Output] public double CropsHarvestedDivCropsPlanted { get; set; }

         public void InitialiseCropSummaryParameters()
        {
            YieldPerHarvest = 0;
            YieldPerPlant = 0;
            YieldPerYear = 0;
            CropsHarvestedDivCropsPlanted = 0;
            YieldDivTranspir = 0;
            ResidueCovDivTranspir = 0;
        }

        public void UpdateCropSummary(HowLeakyEngine Sim, _CustomHowLeakyEngine_VegModule crop)
        {
            double numyears = (double)(Sim.NumberOfDaysInSimulation / 365.0);

            //TODO: Should this be a double
            if (!MathTools.DoublesAreEqual(crop.HarvestCount, 0))
            {
                YieldPerHarvest = crop.CumulativeYield / (double)crop.HarvestCount;
            }
            else
            {
                YieldPerHarvest = 0;
            }
            YieldPerYear = crop.CumulativeYield / numyears;

            if (!MathTools.DoublesAreEqual(crop.PlantingCount, 0))
            {
                CropsHarvestedDivCropsPlanted = crop.HarvestCount / (double)crop.PlantingCount * 100.0;
                YieldPerPlant = crop.CumulativeYield / (double)crop.PlantingCount;
            }
            else
            {
                CropsHarvestedDivCropsPlanted = 0;
                YieldPerPlant = 0;
            }
            if (!MathTools.DoublesAreEqual(crop.AccumulatedTranspiration, 0))
            {
                YieldDivTranspir = crop.CumulativeYield / crop.AccumulatedTranspiration;
                ResidueCovDivTranspir = crop.AccumulatedResidue / crop.AccumulatedTranspiration;
            }
            else
            {
                YieldDivTranspir = 0;
                ResidueCovDivTranspir = 0;
            }

        }
    }
}
