using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HowLeaky_ValidationEngine.Models.Validation
{
    public class Validation_CumulativeChart
    {
        public Validation_CumulativeChart()
        {

        }
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Title { get;set;}
        public string Data { get;set;}
        public string Names { get;set;}
        public string Colors { get;set;}
        public double Difference { get;set;}

        public int StartDateInt { get;set;}
        public int EndDateInt { get;set;}
    }
}
