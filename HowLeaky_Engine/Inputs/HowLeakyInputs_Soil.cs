using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Inputs
{
    public class HowLeakyInputs_Soil:_CustomHowLeakyInputsModel
    {

        public HowLeakyInputs_Soil():base(null,null)
        {
            Depths=new List<double>();
            AirDryLimit=new List<double>();
            WiltingPoint=new List<double>();
            FieldCapacity=new List<double>();
            Saturation=new List<double>();
            MaxDailyDrainRate=new List<double>();
            BulkDensity=new List<double>();
        }

        public HowLeakyInputs_Soil(Guid? id, string name):base(id,name)
        {
             Depths=new List<double>();
            AirDryLimit=new List<double>();
            WiltingPoint=new List<double>();
            FieldCapacity=new List<double>();
            Saturation=new List<double>();
            MaxDailyDrainRate=new List<double>();
            BulkDensity=new List<double>();
        }
        
        public int LayerCount { get; set; }
        public List<double> Depths { get; set; }
        public List<double> AirDryLimit { get; set; }
        public List<double> WiltingPoint { get; set; }
        public List<double> FieldCapacity { get; set; }
        public List<double> Saturation { get; set; }
        public List<double> MaxDailyDrainRate { get; set; }
        public List<double> BulkDensity { get; set; }
       // public bool SoilCrack { get; set; } Remove Mar 2022
        public double Stage2SoilEvapCona { get; set; }
        public double Stage1SoilEvapU { get; set; }
        public double RunoffCurveNumber { get; set; }
        public double RedInCNAtFullCover { get; set; }
        public double MaxRedInCNDueToTill { get; set; }
        public double RainToRemoveRough { get; set; }
        public double USLEK { get; set; }
        public double USLEP { get; set; }
        public double FieldSlope { get; set; }
        public double SlopeLength { get; set; }
        public double RillRatio { get; set; }
        //public double MaxInfiltIntoCracks { get; set; }//Remove mar 2022
        public double SedDelivRatio { get; set; }


        public double OrganicCarbon { get; set; } = 1;
        public double CarbonNitrogenRatio { get;set;}
      

        public double PAWC{get;set;}

    }
}
