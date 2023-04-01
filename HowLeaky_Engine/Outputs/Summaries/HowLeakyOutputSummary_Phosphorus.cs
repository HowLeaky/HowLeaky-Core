using HowLeaky_SimulationEngine.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HowLeaky_SimulationEngine.Outputs
{
    public class HowLeakyOutputSummary_Phosphorus : HowLeakyOutputSummary_Custom
    {
        public HowLeakyOutputSummary_Phosphorus()
        {
        }
        public HowLeakyOutputSummary_Phosphorus(HowLeakyEngine Sim)
        {
            PDissolved = new List<double>(new double[12]);
            PParticulate = new List<double>(new double[12]);
            PTotal = new List<double>(new double[12]);
            Counts = new List<int>(new int[12]);
        }
        public List<double> PDissolved { get; set; }
        public List<double> PParticulate { get; set; }
        public List<double> PTotal { get; set; }




        public List<int> Counts { get; set; }
        public void Update(HowLeakyEngine Sim)
        {
            try
            {
                Name = Sim.PhosphorusModule.Name;
                var month = Sim.TodaysDate.Month - 1;
                PDissolved[month] += Sim.PhosphorusModule.PhosExportDissolve;
                PParticulate[month] += Sim.PhosphorusModule.ParticPExport;
                PTotal[month] += Sim.PhosphorusModule.TotalP;

                Counts[month] += 1;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public double GetTotalPDissolved()
        {
            return PDissolved.Sum();
        }
        public double GetTotalPParticulate()
        {
            return PParticulate.Sum();
        }
        public double GetTotalPTotal()
        {
            return PTotal.Sum();
        }

        public List<double> GetMonthlyAvgPDissolved()
        {
            return PDissolved.Select(x => x / (((double)Counts[PDissolved.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }
        public List<double> GetMonthlyAvgPParticulate()
        {
            return PParticulate.Select(x => x / (((double)Counts[PParticulate.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }
        public List<double> GetMonthlyAvgPTotal()
        {
            return PTotal.Select(x => x / (((double)Counts[PTotal.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }

        public double GetAnnualAvgPDissolved()
        {
            var yrs = ((float)Counts.Sum() / 365.25);
            return PDissolved.Sum() / yrs;
        }
        public double GetAnnualAvgPParticulate()
        {
            return PParticulate.Sum() / ((float)Counts.Sum() / 365.25);
        }
        public double GetAnnualAvgPTotal()
        {
            return PTotal.Sum() / ((float)Counts.Sum() / 365.25);
        }

        public void ScaleValues(double scale)
        {
            ScaleArray(PDissolved, scale);
            ScaleArray(PParticulate, scale);
            ScaleArray(PTotal, scale);
        }

        public void CombineScaledValues(HowLeakyOutputSummary_Phosphorus phosphorus, double scale)
        {
            CombineAndScaleArray(PDissolved, phosphorus.PDissolved, scale);
            CombineAndScaleArray(PParticulate, phosphorus.PParticulate, scale);
            CombineAndScaleArray(PTotal, phosphorus.PTotal, scale);
        }
    }
}
