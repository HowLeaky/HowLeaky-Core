using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Enums
{
    public enum ManagementEvent 
    { 
        Planting, 
        Harvest, 
        Tillage, 
        Pesticide, 
        Irrigation, 
        CropGrowing, 
        InPlantingWindow, 
        MeetsSoilWaterPlantCritera, 
        MeetsDaysSinceHarvestPlantCritera, 
        MeetsRainfallPlantCritera, 
        None 
    }
    
}
