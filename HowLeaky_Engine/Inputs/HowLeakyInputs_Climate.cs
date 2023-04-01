using HowLeaky_SimulationEngine.Enums;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace HowLeaky_SimulationEngine.Inputs
{
    public class HowLeakyInputs_Climate:_CustomHowLeakyInputsModel
    {
        public HowLeakyInputs_Climate():base(null,null)
        {

        }
        public HowLeakyInputs_Climate(Guid?id, string name):base(id,name)
        {

        }
        public double Latitude{get;set;}
        public BrowserDate EndDate { get; set; } = null;
        public BrowserDate StartDate { get; set; } = null;

        public List<double?> Rain { get; set; }
        public List<double?> MaxT { get; set; }
        public List<double?> MinT { get; set; }
        public List<double?> PanEvap { get; set; }
        public List<double?> Radiation { get; set; }
        public List<double?> VP { get; set; }

        public EEvaporationInputOptions EvaporationInputOptions { get; set; } = EEvaporationInputOptions.Use_EPan;
        
        public double PanEvapMultiplier { get; set; } = 1;
        public double RainfallMultiplier { get; set; } = 1;
    }
}
