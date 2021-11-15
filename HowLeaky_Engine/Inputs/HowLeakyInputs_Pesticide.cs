using HowLeaky_SimulationEngine.Enums;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace HowLeaky_SimulationEngine.Inputs
{
    public class HowLeakyInputs_Pesticide:_CustomHowLeakyInputsModel
    {
        public HowLeakyInputs_Pesticide()
        {
        }

        public HowLeakyInputs_Pesticide(string name):base(null,name)
        {

        }
        public HowLeakyInputs_Pesticide(Guid? id, string name):base(id,name)
        {
          
        }

        //public EPestApplicationPosition PestApplicationPosition { get; set; }          // Describes where the pesticide is applied relative to the crop. It is used to determine the fraction of the applied pesticide that is assumed to have been intercepted by the vegetation and/or stubble rather than entering the soil.        
        public double HalfLifeVeg { get; set; }                         // The time required (days) for a pesticide to under go dissipation or degradation to half of the initial concentration on the vegetation.       

       

        public double RefTempHalfLifeVeg { get; set; }                  // The mean air temperature at which the Half-life (Veg) was determined (oC).      
        public double HalfLifeStubble { get; set; }                     // The time required (days) for a pesticide to under go dissipation or degradation to half of the initial concentration in the stubble.    
        public double RefTempHalfLifeStubble { get; set; }              // The mean air temperature at which the Half-life (Stubble) was determined (oC).      
        public double HalfLifeSoil { get; set; }                            // The time required (days) for a pesticide to under go dissipation or degradation to half of the initial concentration in the soil.       
        public double RefTempHalfLifeSoil { get; set; }                 // The mean air temperature at which the Half-life (Soil) was determined (oC).      
        public double CritPestConc { get; set; }                        // Concentration of a pesticide that should not be exceeded in runoff (ug/L).      
        public double ConcActiveIngred { get; set; }                    // The concentration of the pesticide active ingredient (e.g. glyphosate) in the applied product (e.g. Roundup) (g/L). This value is multiplied by the application rate (L/ha) to calculate the amount of active ingredient applied (kg/ha).     
        public double PestEfficiency { get; set; }                      // The percent of total applied pesticide (concentration of active ingredient x application rate) that is retained in the paddock (on the vegetation, stubble or soil) immediately following application. This percent may be less than 100 if there is significant spray drift or other losses between the point of application and the vegetation, stubble and soil.      
        public double DegradationActivationEnergy { get; set; }         // The energetic threshold for thermal decomposition reactions (J/mol).   
        public int MixLayerThickness { get; set; }                      // Depth of the surface soil layer into which an applied pesticide is mixed (mm). This depth is used to calculate a pesticide concentration in the soil following application.        
        public double SorptionCoefficient { get; set; }                 // The sorption coefficient is the ratio of the amount of pesticide bound to soil/sediment versus the amount in the water phase (Kd). Kd values can be derived empirically or estimated from published organic carbon sorption coefficients (Koc) where Kd=Koc x fraction of organic carbon.      
        public double ExtractCoefficient { get; set; }                  // The fraction of pesticide present in soil that will be extracted into runoff. This includes pesticides present in runoff in both the sorbed and dissolved phase. The value of 0.02 has been derived empirically (Silburn, DM, 2003. Characterising pesticide runoff from soil on cotton farms using a rainfall simulator. PhD Thesis, University of Sydney.) and was found to be relevant to data from a range of published studies.        
        public double CoverWashoffFraction { get; set; }                // The fraction of a pesticide that will move off the surface of the vegetation or stubble and into the soil following a rainfall event of greater than 5mm.        
        public double BandSpraying { get; set; }                        // The percent area of a paddock to which a pesticide is applied.        
        public Sequence PestApplicationDateList { get; set; }
        //public List<double> PestApplicationValueList { get; set; } = new List<double>();
        public int ApplicationTiming{ get; set; }
        public DayMonthData ApplicationDate { get; set; }
        public double ProductRate { get; set; }
        public double SubsequentProductRate { get; set; }
        public int TriggerGGDFirst { get; set; }
        public int TriggerGGDSubsequent { get; set; }
        public int TriggerDaysFirst { get; set; }
        public int TriggerDaysSubsequent{ get; set; }
        public int ApplicationPosition{ get; set; }
        public int WashoffMethod { get;  set; }
        public double RainWashoffCoefficient { get; set; }
        public double CoverWashoffFraction2 { get;  set; }


        //public bool tbPestVegIndex1 { get; set; }

        //public bool tbPestVegIndex2 { get; set; }

        //public bool tbPestVegIndex3 { get; set; }

        //public bool tbPestVegIndex4 { get; set; }

        //public bool tbPestVegIndex5 { get; set; }

        //public bool tbPestVegIndex6 { get; set; }

        //public bool tbPestVegIndex7 { get; set; }

        //public bool tbPestVegIndex8 { get; set; }

        //public bool tbPestVegIndex9 { get; set; }

        //public bool tbPestVegIndex10 { get; set; }




        internal string GetName()
        {
            var name=Name.ToLower().Replace("-","_");
            if(name.Contains("24_d"))return "24_d";
            if(name.Contains("atrazine"))return "atrazine";
            if(name.Contains("fluroxypyr"))return "fluroxypyr";
            if(name.Contains("glyphosate"))return "glyphosate";
            if(name.Contains("isoxaflutole"))return "isoxaflutole";
            if(name.Contains("mcpa"))return "mcpa";
            if(name.Contains("metsulfuron_methyl"))return "metsulfuron_methyl";
            if(name.Contains("pendimethalin"))return "pendimethalin";
            if(name.Contains("s_metolachlor"))return "s_metolachlor";
            if(name.Contains("simazine"))return "simazine";
            if(name.Contains("terbuthylazine"))return "terbuthylazine";
            return Name;

        }


        // public Sequence PesticideDatesAndRates { get; set; }

        


    }
}
