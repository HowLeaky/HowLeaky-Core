using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HowLeaky_ValidationEngine.Models.Validation
{
    public enum ScatterType
    {
        MeasuredPredicted,
        CompareVersions
    }
    public class Validation_ScatterChart
    {
        public Validation_ScatterChart()
        {

        }
        public Guid Id { get;set;}
        public Guid ProjectId { get;set;}
        public string Title { get; set; }
        public int SeriesCount { get;set;}
        public string DataCsv { get; set; }
        public string NamesCsv { get; set; }
        public string ColorsCsv { get;set;}
        public double R2 { get; set; }
        public double RMSE { get; set; }
        public double Slope { get;set;}
        public double Intercept { get;set;}

        public ScatterType Type { get;set;}
    }
}
