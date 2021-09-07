using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Inputs
{
    public class HowLeakyInputs_Solute:_CustomHowLeakyInputsModel
    {
        public HowLeakyInputs_Solute():base(null,null)
        {

        }

        public HowLeakyInputs_Solute(Guid? id, string name):base(id,name)
        {
           
        }
      
        public int StartConcOption { get; set; }
        public double Layer1InitialConc { get; set; }
        public double Layer2InitialConc { get; set; }
        public double Layer3InitialConc { get; set; }
        public double Layer4InitialConc { get; set; }
        public double Layer5InitialConc { get; set; }
        public double Layer6InitialConc { get; set; }
        public double Layer7InitialConc { get; set; }
        public double Layer8InitialConc { get; set; }
        public double Layer9InitialConc { get; set; }
        public double Layer10InitialConc { get; set; }
        public double DefaultInitialConc { get; set; }
        public double RainfallConcentration { get; set; }
        public double IrrigationConcentration { get; set; }
        public double MixingCoefficient { get; set; }  
    }
}
