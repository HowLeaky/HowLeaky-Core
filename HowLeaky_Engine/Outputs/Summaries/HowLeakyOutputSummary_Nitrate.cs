using HowLeaky_SimulationEngine.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HowLeaky_Engine.Outputs.Summaries
{
    public class HowLeakyOutputSummary_Nitrate
    {
        public HowLeakyOutputSummary_Nitrate()
        {
            NO3NRunoff = new List<double>(new double[12]);
            ParticulateNRunoff = new List<double>(new double[12]);
            NO3NLeachate = new List<double>(new double[12]);
            Counts=new List<int>(new int[12]);
        }


        public List<double> NO3NRunoff { get; set; }
        public List<double> ParticulateNRunoff { get; set; }
        public List<double> NO3NLeachate { get; set; }
        public List<int> Counts { get; set; }
        public void Update(HowLeakyEngine Sim)
        {
            try
            {
                var month = Sim.TodaysDate.Month - 1;
                NO3NRunoff[month] += Sim.NitrateModule.NO3NRunoffLoad;
                ParticulateNRunoff[month] += Sim.NitrateModule.ParticNInRunoff;
                NO3NLeachate[month] += Sim.NitrateModule.NO3NLeachingLoad;

                Counts[month] += 1;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public double GetTotalNO3NRunoff()
        {
            return NO3NRunoff.Sum();
        }
        public double GetTotalParticulateNRunoff()
        {
            return ParticulateNRunoff.Sum();
        }
        public double GetTotalNO3NLeachate()
        {
            return NO3NLeachate.Sum();
        }

        public List<double> GetMonthlyAvgNO3NRunoff()
        {
            return NO3NRunoff.Select(x => x / (((double)Counts[NO3NRunoff.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }
        public List<double> GetMonthlyAvgParticulateNRunoff()
        {
            return ParticulateNRunoff.Select(x => x / (((double)Counts[ParticulateNRunoff.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }
        public List<double> GetMonthlyAvgNO3NLeachate()
        {
            return NO3NLeachate.Select(x => x / (((double)Counts[NO3NLeachate.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }

        public double GetAnnualAvgNO3NRunoff()
        {
            var yrs = ((float)Counts.Sum() / 365.25);
            return NO3NRunoff.Sum() / yrs;
        }
        public double GetAnnualAvgParticulateNRunoff()
        {
            var yrs = ((float)Counts.Sum() / 365.25);
            return ParticulateNRunoff.Sum() / yrs;
        }
        public double GetAnnualAvgNO3NLeachate()
        {
            var yrs = ((float)Counts.Sum() / 365.25);
            var sum= NO3NLeachate.Sum();
            return  sum/ yrs;
        }
    }
}
