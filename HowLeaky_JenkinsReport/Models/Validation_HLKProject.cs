using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HowLeaky_ValidationEngine.Models.Validation
{
    public class Validation_HLKProject
    {
        public Validation_HLKProject()
        {

        }
        public Guid Id { get;set;}
        public Guid ReportID { get;set;}

        public string ProjectFileName { get;set;}
        public string SimulationErrors { get; set; }
    }
}
