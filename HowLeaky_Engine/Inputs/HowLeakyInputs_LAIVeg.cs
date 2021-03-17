using HowLeaky_SimulationEngine.Engine;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Inputs
{
    public class HowLeakyInputs_LAIVeg : _CustomCropInputsModel
    {
        public HowLeakyInputs_LAIVeg(string name):base(null,name)
        {

        }
        public HowLeakyInputs_LAIVeg(Guid? id, string name):base(id,name)
        {
          
        }
        

        public double PotMaxLAI { get; set; }                   // The upper limit of the leaf area index (LAI) - development curve.
        public double PropGrowSeaForMaxLai { get; set; }        // The development stage for potential maximum LAI.
        public double BiomassAtFullCover { get; set; }          // The amount of dry plant residues (ie stubble, pasture litter etc) that results in complete coverage of the ground.  This parameter controls the relationship between the amount of crop residue and cover, which is used in calculating runoff and erosion.
        public double DailyRootGrowth { get; set; }             // The daily increment in root depth.
        public double PropGDDEnd { get; set; } = 80;            // Set the proportion of the growth cycle for which irrigation is possible.
        public double DaysOfStressToDeath { get; set; }         // The number of consecutive days that water supply is less than threshold before the crop is killed.
        public double PercentOfMaxLai1 { get; set; }            // Percent of Maximum LAI for the 1st development stage.
        public double PercentOfGrowSeason1 { get; set; }        // The development stage for the 1st LAI "point".
        public double PercentOfMaxLai2 { get; set; }            // Percent of Maximum LAI for the 2nd development stage.
        public double PercentOfGrowSeason2 { get; set; }        // The development stage for the 2nd LAI "point".
        public double DegreeDaysPlantToHarvest { get; set; }    // The sum of degree-days (temperature less the base temperature) between planting and harvest.  Controls the rate of crop development and the potential duration of the crop. Some plants develop to maturity and harvest more slowly than others - these accumulate more degree-days between plant and harvest.
        public double SenesenceCoef { get; set; }               // Rate of LAI decline after max LAI.
        public double RadUseEffic { get; set; }                 // Biomass production per unit of radiation.
        public double HarvestIndex { get; set; }                // The grain biomass (kg/ha) divided by the above-ground biomass at flowering (kg/ha)
        public double BaseTemp { get; set; }                    // The lower limit of plant development and growth, with respect to temperature (the average day temperature, degrees Celsius). The base temperature of vegetation is dependent on the type of environment in which the plant has evolved, and any breeding for hot or cold conditions.
        public double OptTemp { get; set; }                     // The temperature for maximum biomass production.  Biomass production is a linear function of temperature between the Base temperature and the Optimum temperature.
        public double MaxRootDepth { get; set; }                // located in CustomVegObject - >The maximum depth of the roots from the soil surface.  For the LAI model, the model calculates daily root growth from the root depth increase parameter
        public double SWPropForNoStress { get; set; } = 0.3;    // Ratio of water supply to potential water supply that indicates a stress day
        public double MaxResidueLoss { get; set; }              //Decomposition Rate
        public DayMonthData PlantDate { get; set; }
        public int PlantingRulesOptions { get; set; }
        public DayMonthData PlantingWindowStartDate { get; set; }
        public DayMonthData PlantingWindowEndDate{ get; set; }
        public bool ForcePlantingAtEndOfWindow{ get; set; }
        public bool MultiPlantInWindow { get; set; }
        public int RotationFormat { get; set; }
        public int MinRotationCount { get; set; }
        public int MaxRotationCount{ get; set; }
        public int RestPeriodAfterChangingCrops { get; set; }
        public bool FallowSwitch { get; set; }
        public int MinimumFallowPeriod { get; set; }
        public bool PlantingRainSwitch { get; set; }
        public double RainfallPlantingThreshold { get; set; }
        public int RainfallSummationDays { get; set; }
        public bool SoilWaterSwitch { get; set; }
        public double MinSoilWaterTopLayer { get; set; }
        public double MaxSoilWaterTopLayer { get; set; }
        public double SoilWaterReqToPlant{ get; set; }
        
        public double DepthToSumPlantingWater { get; set; }
        public int SowingDelay{ get; set; }
        public Sequence PlantingSequence{ get; set; }           
        public bool WaterLoggingSwitch { get; set; }
        public double WaterLoggingFactor1 { get; set; }
        public double WaterLoggingFactor2 { get; set; }
        public bool RatoonSwitch{ get; set; }
        public int NumberOfRatoons { get; set; }
        public double ScalingFactorForRatoons { get; set; }

        //TODO: unmatched
        //MaxResidueLoss, WatStressForDeath
        public double MaximumResidueCover { get; set; }
   
    }
}
