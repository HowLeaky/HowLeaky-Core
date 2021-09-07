using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;

namespace HowLeaky_SimulationEngine.Inputs
{

    public enum DissolvedNinLeachingType
    {
        None=0,
        HowLeaky2012=1,        
        ModifiedSafegaugeModel=2

    }
     public enum DissolvedNinRunoffType
    {
       None=0,
       HowLeaky2012=1,
       BananaEmpiricalModel=2,

        
    }
     public enum ParticulateNinRunoffType
    {
        None=0,
        HowLeaky2012=1

        
    }


    public class HowLeakyInputs_Nitrate:_CustomHowLeakyInputsModel
    {
        public HowLeakyInputs_Nitrate():base(null,null)
        {

        }
        public HowLeakyInputs_Nitrate(Guid? id, string name):base(id,name)
        {
          
        }

        public DissolvedNinLeachingType DissolvedNinLeachingOptions { get; set; }
        public DissolvedNinRunoffType DissolvedNinRunoffOptions { get; set; }
        public ParticulateNinRunoffType ParticulateNinRunoffOptions { get; set; }

        public Sequence FertilizerInputDateSequences { get; set; } = null;     
        public Sequence SoilNLoadData1 { get; set; } = null;    
        public Sequence SoilNLoadData2 { get; set; } = null;
        public Sequence SoilNLoadData3 { get; set; } = null;
        // public TimeSeriesData NLoadInSurfaceLayerTimeSeries { get; set; } = null;   
        // public TimeSeriesData NLoadInLowerLayersTimeSeries { get; set; } = null;

       
        // public TimeSeriesData InorganicNitrateNTimeseries { get; set; }    
        // public TimeSeriesData InorganicAmmoniumNTimeseries { get; set; }
        // public TimeSeriesData OrganicNTimeseries { get; set; }      
       

        public double NDepthTopLayer1 { get; set; }      
        public double Nk { get; set; }      
        public double Ncv { get; set; }      
        public double NAlpha_Disolved { get; set; }
        public double NBeta_Disolved { get; set; }
        public double NBeta_Particulate { get; set; }
        public double NAlpha_Particulate { get; set; }
        public double SoilNitrateLoadWeighting1 { get; set; }      
         public double SoilNitrateLoadWeighting2 { get; set; }
          public double SoilNitrateLoadWeighting3 { get; set; }

        
        
        
        // public double N_DanRat_Alpha { get; set; }      
        // public double N_DanRat_Beta { get; set; }      
        public double N_DanRat_MaxRunOffConc { get; set; }      
        public double N_DanRat_MinRunOffConc { get; set; }
      
       
        public double DepthBottomLayer { get; set; }
        public double NitrateLeachingEfficiency { get; set; }
       
        
        public double NDepthTopLayer2 { get; set; }
        public double NEnrichmentRatio { get; set; }
       
     
        
        
        public double Denitrification { get; set; }
        public double NitrateDrainageRetention { get; set; } 
                 
         public double InitialExcessN { get; set; }    
        public double CNSlope { get; set; } 
        public double CNMax { get; set; } 
        public int MainStemDuration { get; set; } 
        public double RatoonA { get; set; }
        public double PlantA { get; set; }
        public double RatoonB { get; set; }
        public double PlantB { get; set; }
         public double RatoonAnnual { get; set; } // This is something that Al did. Is listed as Annual in file
        public double PlantAnnual { get; set; } // This is something that Al did.  Is listed as Annual in file
        public double RatoonDaily { get{return RatoonAnnual/365.0; } } // This is something that Al did. Is listed as Annual in file
        public double PlantDaily { get{return PlantAnnual/365.0; } } // This is something that Al did.  Is listed as Annual in file

         //public TimeSeriesData SoilNitrateTimeseries { get; set; }        
          //public List<XYXData> SoilNitrateLevels { get; set; } = null;
         //public double Mineralisation { get; set; } 
        //public double VolSat { get; set; } 
        //public double NitrogenApplication { get; set; }
        //public double NitrogenFrequency { get; set; } 
        //public double SoilCarbon { get; set; } 


    }
}
