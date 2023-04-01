using HowLeaky_SimulationEngine.Engine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HowLeaky_SimulationEngine.Outputs
{
    public class HowLeakyOutputSummary_RingTank : HowLeakyOutputSummary_Custom
    {
        public HowLeakyOutputSummary_RingTank()
        {
        }
        public HowLeakyOutputSummary_RingTank(HowLeakyEngine Sim)
        {
            RainfallInflow = new List<double>(new double[12]);
            RunoffCaptureInflow = new List<double>(new double[12]);
            AdditionalInflow = new List<double>(new double[12]);
            EvaporationLosses = new List<double>(new double[12]);
            SeepageLosses = new List<double>(new double[12]);
            OvertoppingLosses = new List<double>(new double[12]);
            IrrigationLosses = new List<double>(new double[12]);
            Counts = new List<int>(new int[12]);
        }
        public List<double> RainfallInflow { get; set; }
        public List<double> RunoffCaptureInflow { get; set; }
        public List<double> AdditionalInflow { get; set; }

        public List<double> EvaporationLosses { get; set; }
        public List<double> SeepageLosses { get; set; }
        public List<double> OvertoppingLosses { get; set; }

        public List<double> IrrigationLosses { get; set; }


        public List<int> Counts { get; set; }
        public void Update(HowLeakyEngine Sim)
        {
            try
            {
                Name = Sim.IrrigationModule.Name;
                var month = Sim.TodaysDate.Month - 1;
                RainfallInflow[month] += Sim.IrrigationModule.RingTankRainfalInflow;
                RunoffCaptureInflow[month] += Sim.IrrigationModule.RingTankRunoffCaptureInflow;
                AdditionalInflow[month] += Sim.IrrigationModule.RingTankTotalAdditionalInflow;

                EvaporationLosses[month] += Sim.IrrigationModule.RingTankEvaporationLosses;
                SeepageLosses[month] += Sim.IrrigationModule.RingTankSeepageLosses;
                OvertoppingLosses[month] += Sim.IrrigationModule.RingTankOvertoppingLosses;
                IrrigationLosses[month] += Sim.IrrigationModule.RingTankIrrigationLosses;

                Counts[month] += 1;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public double GetTotalRainfalInflow()
        {
            return RainfallInflow.Sum();
        }
        public double GetTotalRunoffCaptureInflow()
        {
            return RunoffCaptureInflow.Sum();
        }

        public void ScaleValues(double scale)
        {
            throw new NotImplementedException();
        }

        public double GetTotalAdditionalInflow()
        {
            return AdditionalInflow.Sum();
        }

        public double GetTotalEvaporationLosses()
        {
            return EvaporationLosses.Sum();
        }
        public double GetTotalSeepageLosses()
        {
            return SeepageLosses.Sum();
        }
        public double GetTotalOvertoppingLosses()
        {
            return OvertoppingLosses.Sum();
        }
        public double GetTotalIrrigationLosses()
        {
            return IrrigationLosses.Sum();
        }




        public List<double> GetMonthlyAvgRainfallInflow()
        {
            return RainfallInflow.Select(x => x / (((double)Counts[RainfallInflow.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }
        public List<double> GetMonthlyAvgRunoffCaptureInflow()
        {
            return RunoffCaptureInflow.Select(x => x / (((double)Counts[RunoffCaptureInflow.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }

        public void CombineScaledValues(HowLeakyOutputSummary_RingTank ringTank, double scale)
        {
            throw new NotImplementedException();
        }

        public List<double> GetMonthlyAvgAdditionalInflow()
        {
            return AdditionalInflow.Select(x => x / (((double)Counts[AdditionalInflow.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }

        public List<double> GetMonthlyAvgEvaporationLosses()
        {
            return EvaporationLosses.Select(x => x / (((double)Counts[EvaporationLosses.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }
        public List<double> GetMonthlyAvgSeepageLosses()
        {
            return SeepageLosses.Select(x => x / (((double)Counts[SeepageLosses.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }
        public List<double> GetMonthlyAvgOvertoppingLosses()
        {
            return OvertoppingLosses.Select(x => x / (((double)Counts[OvertoppingLosses.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }
        public List<double> GetMonthlyAvgIrrigationLosses()
        {
            return IrrigationLosses.Select(x => x / (((double)Counts[IrrigationLosses.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }




        public double GetAnnualAvgRainfallInflow()
        {
            var yrs = ((float)Counts.Sum() / 365.25);
            return RainfallInflow.Sum() / yrs;
        }
        public double GetAnnualAvgRunoffCaptureInflow()
        {
            return RunoffCaptureInflow.Sum() / ((float)Counts.Sum() / 365.25);
        }
        public double GetAnnualAvgAdditionalInflow()
        {
            return AdditionalInflow.Sum() / ((float)Counts.Sum() / 365.25);
        }

        public double GetAnnualAvgEvaporationLosses()
        {
            return EvaporationLosses.Sum() / ((float)Counts.Sum() / 365.25);
        }
        public double GetAnnualAvgSeepageLosses()
        {
            return SeepageLosses.Sum() / ((float)Counts.Sum() / 365.25);
        }

        public double GetAnnualAvgOvertoppingLosses()
        {
            return OvertoppingLosses.Sum() / ((float)Counts.Sum() / 365.25);
        }

        public double GetAnnualAvgIrrigationLosses()
        {
            return IrrigationLosses.Sum() / ((float)Counts.Sum() / 365.25);
        }
    }
}
