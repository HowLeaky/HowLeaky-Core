using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Attributes
{
   

    public class Unit : Attribute
    {
        public string unit { get; set; }

        public Unit(string unit)
        {
            this.unit = unit;
        }
    }
}
