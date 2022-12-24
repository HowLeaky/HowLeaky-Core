using HowLeaky_SimulationEngine.Engine;
using HowLeaky_SimulationEngine.Outputs.Summaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HowLeaky_SimulationEngine.Outputs
{
    public class HowLeakyOutputSummary_Pesticide
    {
        public HowLeakyOutputSummary_Pesticide()
        {

        }
        public HowLeakyOutputSummary_Pesticide(double conc)
        {
            PestApplied = new List<double>(new double[12]);
            PestRunoffWater = new List<double>(new double[12]);
            PestRunoffSediment = new List<double>(new double[12]);
            PestRunoffTotal = new List<double>(new double[12]);
            //DaysGreaterCrit1=new List<double>(new double[12]);
            //DaysGreaterCrit2 = new List<double>(new double[12]);
            //DaysGreaterCrit3 = new List<double>(new double[12]);
            //DaysGreaterCrit4 = new List<double>(new double[12]);
            Counts = new List<int>(new int[12]);
            CriticalPestConc1 = conc * 1;
            CriticalPestConc2 = conc * 0.5;
            CriticalPestConc3 = conc * 2;
            CriticalPestConc4 = conc * 10;
            RunoffTotal=0;
        }
        public List<double> PestApplied { get; set; }
        public List<double> PestRunoffWater { get; set; }
        public List<double> PestRunoffSediment { get; set; }
        public List<double> PestRunoffTotal { get; set; }

        public double DaysGreaterCrit1 { get;set;}
        public double DaysGreaterCrit2 { get; set; }
        public double DaysGreaterCrit3 { get; set; }
        public double DaysGreaterCrit4 { get; set; }

        public List<int> Counts { get; set; }
        public double CriticalPestConc1 { get;set;}
        public double CriticalPestConc2 { get; set; }
        public double CriticalPestConc3 { get; set; }
        public double CriticalPestConc4 { get; set; }
        public double RunoffTotal { get;set;}
        public void Update(HowLeakyEngine Sim, HowLeakyEngineModule_Pesticide module)
        {
            try
            {
                var month = Sim.TodaysDate.Month - 1;
                PestApplied[month]+=module.pest_application;
                PestRunoffWater[month] += module.PestLostInRunoffWater;
                PestRunoffSediment[month] += module.PestLostInRunoffSediment;
                PestRunoffTotal[month] += module.TotalPestLostInRunoff;
                DaysGreaterCrit1= module.DaysGreaterCrit1;
                DaysGreaterCrit2= module.DaysGreaterCrit2;
                DaysGreaterCrit3= module.DaysGreaterCrit3;
                DaysGreaterCrit4= module.DaysGreaterCrit4;
                
                Counts[month] += 1;
                RunoffTotal+=Sim.SoilModule.Runoff;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public double GetTotalPestApplied()
        {
            return PestApplied.Sum();
        }
        public double GetTotalPestRunoffWater()
        {
            return PestRunoffWater.Sum();
        }
        public double GetTotalPestRunoffSediment()
        {
            return PestRunoffSediment.Sum();
        }
        public double GetTotalPestRunoffTotal()
        {
            return PestRunoffTotal.Sum();
        }

        //public double GetTotalDaysGreaterCrit1()
        //{
        //    return DaysGreaterCrit1.Sum();
        //}
        //public double GetTotalDaysGreaterCrit2()
        //{
        //    return DaysGreaterCrit2.Sum();
        //}
        //public double GetTotalDaysGreaterCrit3()
        //{
        //    return DaysGreaterCrit3.Sum();
        //}
        //public double GetTotalDaysGreaterCrit4()
        //{
        //    return DaysGreaterCrit4.Sum();
        //}
        public List<double> GetMonthlyAvgPestApplied()
        {
            return PestApplied.Select(x => x / (((double)Counts[PestApplied.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }
        public List<double> GetMonthlyAvgPestRunoffWater()
        {
            return PestRunoffWater.Select(x => x / (((double)Counts[PestRunoffWater.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }
        public List<double> GetMonthlyAvgPestRunoffSediment()
        {
            return PestRunoffSediment.Select(x => x / (((double)Counts[PestRunoffSediment.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }
        public List<double> GetMonthlyAvgPestRunoffTotal()
        {
            return PestRunoffTotal.Select(x => x / (((double)Counts[PestRunoffTotal.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }

        //public List<double> GetMonthlyAvgDaysGreaterCrit1()
        //{
        //    return DaysGreaterCrit1.Select(x => x / (((double)Counts[DaysGreaterCrit1.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        //}
        //public List<double> GetMonthlyAvgDaysGreaterCrit2()
        //{
        //    return DaysGreaterCrit2.Select(x => x / (((double)Counts[DaysGreaterCrit2.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        //}
        //public List<double> GetMonthlyAvgDaysGreaterCrit3()
        //{
        //    return DaysGreaterCrit3.Select(x => x / (((double)Counts[DaysGreaterCrit3.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        //}
        //public List<double> GetMonthlyAvgDaysGreaterCrit4()
        //{
        //    return DaysGreaterCrit4.Select(x => x / (((double)Counts[DaysGreaterCrit4.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        //}

        public double GetAnnualAvgPestApplied()
        {
            var yrs = ((float)Counts.Sum() / 365.25);
            return PestApplied.Sum() / yrs;
        }

        public double GetAnnualAvgPestRunoffWater()
        {
            var yrs = ((float)Counts.Sum() / 365.25);
            return PestRunoffWater.Sum() / yrs;
        }
        public double GetAnnualAvgPestRunoffSediment()
        {
            return PestRunoffSediment.Sum() / ((float)Counts.Sum() / 365.25);
        }
        public double GetAnnualAvgPestRunoffTotal()
        {
            return PestRunoffTotal.Sum() / ((float)Counts.Sum() / 365.25);
        }

        public double GetAnnualAvgDaysGreaterCrit1()
        {
            return DaysGreaterCrit1 / ((float)Counts.Sum() / 365.25);
        }

        public double GetAnnualAvgDaysGreaterCrit2()
        {
            return DaysGreaterCrit2 / ((float)Counts.Sum() / 365.25);
        }

        public double GetAnnualAvgDaysGreaterCrit3()
        {
            return DaysGreaterCrit3 / ((float)Counts.Sum() / 365.25);
        }

        public double GetAnnualAvgDaysGreaterCrit4()
        {
            return DaysGreaterCrit4 / ((float)Counts.Sum() / 365.25);
        }

        public double GetEMC()
        {
            if(RunoffTotal>0)
            {
                return 100.0*PestRunoffTotal.Sum()/RunoffTotal;
            }
            return 0;
        }
    }
}
