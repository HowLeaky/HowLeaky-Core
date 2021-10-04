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
            PestRunoffWater = new List<double>(new double[12]);
            PestRunoffSediment = new List<double>(new double[12]);
            PestRunoffTotal = new List<double>(new double[12]);
            Counts = new List<int>(new int[12]);
        }
        public List<double> PestRunoffWater { get; set; }
        public List<double> PestRunoffSediment { get; set; }
        public List<double> PestRunoffTotal { get; set; }
        public List<int> Counts { get; set; }
        public void Update(HowLeakyEngine Sim, HowLeakyEngineModule_Pesticide module)
        {
            try
            {
                var month = Sim.TodaysDate.Month - 1;
                PestRunoffWater[month] += module.PestLostInRunoffWater;
                PestRunoffSediment[month] += module.PestLostInRunoffSediment;
                PestRunoffTotal[month] += module.TotalPestLostInRunoff;

                Counts[month] += 1;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
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
    }
}
