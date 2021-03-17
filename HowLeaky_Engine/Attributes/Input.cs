using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowLeaky_SimulationEngine.Attributes
{
    public class Input : Attribute
    {
        public string Display { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        //This is just a tag at the moment
        public Input(string Display, string Unit = null, string Description = null)
        {
            this.Display = Display;
            this.Unit = Unit;
            this.Description = Description;
        }
    }
}
