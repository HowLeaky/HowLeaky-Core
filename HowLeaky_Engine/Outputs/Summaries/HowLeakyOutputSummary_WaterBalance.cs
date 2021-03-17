using HowLeaky_SimulationEngine.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HowLeaky_SimulationEngine.Outputs.maries
{
    public class HowLeakyOutputSummary_WaterBalance
    {
        public HowLeakyOutputSummary_WaterBalance()
        {
            Rainfall=new List<double>(new double[12]);
            Irrigation=new List<double>(new double[12]);
            Runoff=new List<double>(new double[12]);
            Potevap=new List<double>(new double[12]);
            SoilEvaporation=new List<double>(new double[12]);
            Transpiration=new List<double>(new double[12]);
            Evapotranspiration=new List<double>(new double[12]);
            Overflow=new List<double>(new double[12]);
            Drainage=new List<double>(new double[12]);
            LateralFlow=new List<double>(new double[12]);
            SoilErosion=new List<double>(new double[12]);
            Counts=new List<int>(new int[12]);
        }
        public List<double> Rainfall { get; set; }
        public List<double> Irrigation { get; set; }
        public List<double> Runoff { get; set; }
        public List<double> Potevap { get; set; }
        public List<double> SoilEvaporation { get; set; }
        public List<double> Transpiration { get; set; }
        public List<double> Evapotranspiration { get; set; }
        public List<double> Overflow { get; set; }
        public List<double> Drainage { get; set; }
        public List<double> LateralFlow { get; set; }
        public List<double> SoilErosion { get; set; }
        public List<int> Counts{get;set;}

       

        public void Update(HowLeakyEngine Sim)
        {
            try
            {
                var month=Sim.TodaysDate.Month-1;
                Rainfall[month]+= Sim.ClimateModule.Rain;
                Irrigation[month]+= Sim.SoilModule.Irrigation;
                Runoff[month]+= Sim.SoilModule.Runoff;
                Potevap[month]+= Sim.SoilModule.PotSoilEvap;
                SoilEvaporation[month]+= Sim.SoilModule.SoilEvap;
                Transpiration[month]+= Sim.GetTotalTranspiration();
                Evapotranspiration[month]+= (Sim.SoilModule.SoilEvap + Sim.GetTotalTranspiration());
                Overflow[month]+= Sim.SoilModule.Overflow;
                Drainage[month]+= Sim.SoilModule.DeepDrainage;
                LateralFlow[month]+= Sim.SoilModule.LateralFlow;
                SoilErosion[month]+= Sim.SoilModule.HillSlopeErosion;
                Counts[month]+=1;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public double GetTotalRainfall()
        {
            return Rainfall.Sum();
        }
        public double GetTotalIrrigation()
        {
            return Irrigation.Sum();
        }
        public double GetTotalRunoff()
        {
            return Runoff.Sum();
        }
        public double GetTotalPotevap()
        {
            return Potevap.Sum();
        }
        public double GetTotalSoilEvaporation()
        {
            return SoilEvaporation.Sum();
        }
        public double GetTotalTranspiration()
        {
            return Transpiration.Sum();
        }
        public double GetTotalOverflow()
        {
            return Overflow.Sum();
        }
        
        public double GetTotalDrainage()
        {
            return Drainage.Sum();
        }
        public double GetTotalLateralFlow()
        {
            return LateralFlow.Sum();
        }
        public double GetTotalSoilErosion()
        {
            return SoilErosion.Sum();
        }

        public List<double> GetMonthlyAvgRainfall()
        {
            return Rainfall.Select(x=>x/(((double)Counts[Rainfall.IndexOf(x)])/365.25*12.0)).ToList();
        }
        public List<double> GetMonthlyAvgIrrigation()
        {
            return Irrigation.Select(x=>x/(((double)Counts[Irrigation.IndexOf(x)])/365.25*12.0)).ToList();
        }
        public List<double> GetMonthlyAvgRunoff()
        {
            return Runoff.Select(x=>x/(((double)Counts[Runoff.IndexOf(x)])/365.25*12.0)).ToList();
        }
        public List<double> GetMonthlyAvgPotevap()
        {
            return Potevap.Select(x=>x/(((double)Counts[Potevap.IndexOf(x)])/365.25*12.0)).ToList();
        }
        public List<double> GetMonthlyAvgSoilEvaporation()
        {
            return SoilEvaporation.Select(x=>x/(((double)Counts[SoilEvaporation.IndexOf(x)])/365.25*12.0)).ToList();
        }
        public List<double> GetMonthlyAvgTranspiration()
        {
            return Transpiration.Select(x=>x/(((double)Counts[Transpiration.IndexOf(x)])/365.25*12.0)).ToList();
        }

         public List<double> GetMonthlyAvgEvapotranspiration()
        {
            return Evapotranspiration.Select(x=>x/(((double)Counts[Evapotranspiration.IndexOf(x)])/365.25*12.0)).ToList();
        }
        public List<double> GetMonthlyAvgOverflow()
        {
            return Overflow.Select(x=>x/(((double)Counts[Overflow.IndexOf(x)])/365.25*12.0)).ToList();
        }
        
        public List<double> GetMonthlyAvgDrainage()
        {
            return Drainage.Select(x=>x/(((double)Counts[Drainage.IndexOf(x)])/365.25*12.0)).ToList();
        }
        public List<double> GetMonthlyAvgLateralFlow()
        {
            return LateralFlow.Select(x=>x/(((double)Counts[LateralFlow.IndexOf(x)])/365.25*12.0)).ToList();
        }
        public List<double> GetMonthlyAvgSoilErosion()
        {
            return SoilErosion.Select(x=>x/(((double)Counts[SoilErosion.IndexOf(x)])/365.25*12.0)).ToList();
        }



        public double GetAnnualAvgRainfall()
        {
            var yrs=((float)Counts.Sum()/365.25);
            return Rainfall.Sum()/yrs;
        }
        public double GetAnnualAvgIrrigation()
        {
            return Irrigation.Sum()/((float)Counts.Sum()/365.25);
        }
        public double GetAnnualAvgRunoff()
        {
            return Runoff.Sum()/((float)Counts.Sum()/365.25);
        }
        public double GetAnnualAvgPotevap()
        {
            return Potevap.Sum()/((float)Counts.Sum()/365.25);
        }
        public double GetAnnualAvgSoilEvaporation()
        {
            return SoilEvaporation.Sum()/((float)Counts.Sum()/365.25);
        }
        public double GetAnnualAvgTranspiration()
        {
            return Transpiration.Sum()/((float)Counts.Sum()/365.25);
        }

         public double GetAnnualAvgEvapotranspiration()
        {
            return Evapotranspiration.Sum()/((float)Counts.Sum()/365.25);
        }
        public double GetAnnualAvgOverflow()
        {
            return Overflow.Sum()/((float)Counts.Sum()/365.25);
        }
        
        public double GetAnnualAvgDrainage()
        {
            return Drainage.Sum()/((float)Counts.Sum()/365.25);
        }
        public double GetAnnualAvgLateralFlow()
        {
            return LateralFlow.Sum()/((float)Counts.Sum()/365.25);
        }
        public double GetAnnualAvgSoilErosion()
        {
            return SoilErosion.Sum()/((float)Counts.Sum()/365.25);
        }





        //public double GetMonthlyAvgMaxY()
        //{
        //    var list=new List<double>();
        //    list.Add(GetMonthlyAvgRainfall().Max());
        //    list.Add(GetMonthlyAvgIrrigation().Max());
        //    list.Add(GetMonthlyAvgRunoff().Max());
        //    list.Add(GetMonthlyAvgPotevap().Max());
        //    list.Add(GetMonthlyAvgSoilEvaporation().Max());
        //    list.Add(GetMonthlyAvgTranspiration().Max());
        //    list.Add(GetMonthlyAvgEvapotranspiration().Max());
        //    list.Add(GetMonthlyAvgOverflow().Max());
        //    list.Add(GetMonthlyAvgDrainage().Max());
        //    list.Add(GetMonthlyAvgLateralFlow().Max()); 
        //    var value= (double)RoundOff(list.Max());
        //    return value;
        //}

        //public  int RoundOff (double value)
        //{
        //    return (int) ((Math.Round(value+0.5) / 10.0 ) * 10.0);
        //}
    }
}
