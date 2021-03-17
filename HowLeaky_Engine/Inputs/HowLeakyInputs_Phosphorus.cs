using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Inputs
{
    public class HowLeakyInputs_Phosphorus:_CustomHowLeakyInputsModel
    {
        public HowLeakyInputs_Phosphorus():base(null,null)
        {

        }
        public HowLeakyInputs_Phosphorus(Guid? id, string name):base(id,name)
        {
        }

        public double TotalPConc { get; set; }                                                  // The total P content of the soil (extracted with hot acid)
        public double ColwellP { get; set; }                                                    // The amount of easily-extracted P in the topsoil (0-10 cm, extracted with bicarbonate).
        public double PBI { get; set; }                                                         // The degree to which soils bind P (related to the %clay, clay weathering and Fe content)
                                                                   
        public double EnrichmentRatio{ get; set; }   
        public double ClayPercentage { get; set; }  
      
        public int PEnrichmentOpt  { get; set; }  
        public int DissolvedPOpt  { get; set; }                      // Two options.  VIC DPI Method: p_max_sorption = 1447 * (1-exp(-0.001 * PBI)), QLD REEF Method: p_max_sorption=5.84*PBI-0.0096*PBI^2  (min of 50). Phos_Conc_Dissolve_mg_per_L is also calculated slightly differently.
   
    }
}
