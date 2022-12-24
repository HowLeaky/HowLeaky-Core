using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Inputs
{
    public class HowLeakyInputs_CoverVeg:_CustomCropInputsModel
    {
        public HowLeakyInputs_CoverVeg():base()
        {
        }

        public HowLeakyInputs_CoverVeg(string name):base(null,name)
        {

        }
        public HowLeakyInputs_CoverVeg(Guid? id, string name):base(id,name)
        {
            
        }

        
        
        public int PlantDay { get; set; } = 1;
        public int CoverDataType { get; set; } = 0;             //for no time series
        public ProfileData CoverProfile { get; set; }
        public TimeSeriesData GreenCoverTimeSeries { get; set; }
        public TimeSeriesData ResidueCoverTimeSeries { get; set; }
        public TimeSeriesData RootDepthTimeSeries { get; set; }
        public double TranspirationEfficiency { get; set; }     // The ratio of grain production (kg/ha) to water supply (mm).
        public double HarvestIndex { get; set; }                // The grain biomass (kg/ha) divided by the above-ground biomass at flowering (kg/ha).
        public int DaysPlantingToHarvest { get; set; }          // The number of days between planting and harvest.
        public double GreenCoverMultiplier { get; set; } = 1;   // Scaling factor for green cover
        public double ResidueCoverMultiplier { get; set; } = 1; // Scaling factor for residue cover
        public double RootDepthMultiplier { get; set; } = 1;    // Scaling factor for root depth
        
        public double MaxRootDepth { get; set; }                // located in CustomVegObject - >The maximum depth of the roots from the soil surface.  For the LAI model, the model calculates daily root growth from the root depth increase parameter
        public double SWPropForNoStress { get; set; } = 0.3;

    }
}
