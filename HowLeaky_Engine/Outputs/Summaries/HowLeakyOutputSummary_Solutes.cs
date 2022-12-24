using HowLeaky_SimulationEngine.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HowLeaky_Engine.Outputs.Summaries
{
    public class HowLeakyOutputSummary_Solutes
    {
        public HowLeakyOutputSummary_Solutes()
        {
            SoluteLeach_kg_per_ha = new List<double>(new double[12]);
            SoluteLeach_mg_per_L = new List<double>(new double[12]);
            Drainage_mm = new List<double>(new double[12]);            
            Counts = new List<int>(new int[12]);
        }
        public List<double> SoluteLeach_kg_per_ha { get; set; }
        public List<double> SoluteLeach_mg_per_L { get; set; }
        public List<double> Drainage_mm { get; set; }
        
        public List<int> Counts { get; set; }
        public void Update(HowLeakyEngine Sim)
        {
            try
            {
                var month = Sim.TodaysDate.Month - 1;
                SoluteLeach_kg_per_ha[month] += Sim.SolutesModule.solute_leaching_load_kg_per_ha;
                SoluteLeach_mg_per_L[month] += Sim.SolutesModule.solute_leaching_conc_mg_per_L;
                Drainage_mm[month] += Sim.SoilModule.DeepDrainage;

                Counts[month] += 1;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public double GetTotalSoluteLeach_kg_per_ha()
        {
            return SoluteLeach_kg_per_ha.Sum();
        }
        public double GetTotalLeaching_mm()
        {
            return Drainage_mm.Sum();
        }

        public List<double> GetMonthlyAvgSoluteLeach_kg_per_ha()
        {
            return SoluteLeach_kg_per_ha.Select(x => x / (((double)Counts[SoluteLeach_kg_per_ha.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }
        public List<double> GetMonthlyAvgSoluteLeach_mg_per_L()
        {
            return SoluteLeach_mg_per_L.Select(x => x / (((double)Counts[SoluteLeach_mg_per_L.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }

        public List<double> GetMonthlyAvgDrainage_mm()
        {
            return Drainage_mm.Select(x => x / (((double)Counts[Drainage_mm.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }


        public double GetAnnualAvgSoluteLeach_kg_per_ha()
        {
            var yrs = ((float)Counts.Sum() / 365.25);
            return SoluteLeach_kg_per_ha.Sum() / yrs;
        }

        public double GetAnnualDrainage_mm()
        {
            var yrs = ((float)Counts.Sum() / 365.25);
            return Drainage_mm.Sum() / yrs;
        }
        public double GetAnnualAvgSoluteLeach_mg_per_L()
        {
            return SoluteLeach_mg_per_L.Sum() / ((float)Counts.Sum() / 365.25);
        }
        
    }
}
