using HowLeaky_SimulationEngine.Engine;
using HowLeaky_SimulationEngine.Inputs;
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

            Application = new List<double>(new double[12]);
            Mineralisation = new List<double>(new double[12]);
            CropUse = new List<double>(new double[12]);
            Denitrification = new List<double>(new double[12]);
            Pool = new List<double>(new double[12]);


            Counts = new List<int>(new int[12]);
        }


        public List<double> NO3NRunoff { get; set; }
        public List<double> ParticulateNRunoff { get; set; }
        public List<double> NO3NLeachate { get; set; }

        public List<double> Application { get; set; }
        public List<double> Mineralisation { get; set; }
        public List<double> CropUse { get; set; }
        public List<double> Denitrification { get; set; }
        public List<double> Pool { get; set; }
        public bool Extended { get;set;}

        public List<int> Counts { get; set; }
        public void Update(HowLeakyEngine Sim)
        {
            try
            {
                var month = Sim.TodaysDate.Month - 1;
                NO3NRunoff[month] += Sim.NitrateModule.NO3NRunoffLoad;
                ParticulateNRunoff[month] += Sim.NitrateModule.ParticNInRunoff;
                NO3NLeachate[month] += Sim.NitrateModule.NO3NLeachingLoad;


                Application[month] += Sim.NitrateModule.NitrogenApplication;
                Mineralisation[month] += Sim.NitrateModule.Mineralisation;
                CropUse[month] += Sim.NitrateModule.CropUsePlant;
                Denitrification[month] += Sim.NitrateModule.Denitrification;
                Pool[month] += Sim.NitrateModule.ExcessN;

                Extended = Sim.NitrateModule.InputModel.DissolvedNinLeachingOptions== DissolvedNinLeachingType.ModifiedSafegaugeModel;

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
        public double GetTotalApplication()
        {
            return Application.Sum();
        }
        public double GetTotalMineralisation()
        {
            return Mineralisation.Sum();
        }
        public double GetTotalCropUse()
        {
            return CropUse.Sum();
        }
        public double GetTotalDenitrification()
        {
            return Denitrification.Sum();
        }
        public double GetTotalPool()
        {
            return Pool.Sum();
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


        public List<double> GetMonthlyAvgApplication()
        {
            return Application.Select(x => x / (((double)Counts[Application.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }
        public List<double> GetMonthlyAvgMineralisation()
        {
            return Mineralisation.Select(x => x / (((double)Counts[Mineralisation.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }
        public List<double> GetMonthlyAvgCropUse()
        {
            return CropUse.Select(x => x / (((double)Counts[CropUse.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }
        public List<double> GetMonthlyAvgDenitrification()
        {
            return Denitrification.Select(x => x / (((double)Counts[Denitrification.IndexOf(x)]) / 365.25 * 12.0)).ToList();
        }
        public List<double> GetMonthlyAvgPool()
        {
            return Pool.Select(x => x / (((double)Counts[Pool.IndexOf(x)]) / 365.25 * 12.0)).ToList();
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
            var sum = NO3NLeachate.Sum();
            return sum / yrs;
        }


        public double GetAnnualAvgApplication()
        {
            var yrs = ((float)Counts.Sum() / 365.25);
            var sum = Application.Sum();
            return sum / yrs;
        }
        public double GetAnnualAvgMineralisation()
        {
            var yrs = ((float)Counts.Sum() / 365.25);
            var sum = Mineralisation.Sum();
            return sum / yrs;
        }
        public double GetAnnualAvgCropUse()
        {
            var yrs = ((float)Counts.Sum() / 365.25);
            var sum = CropUse.Sum();
            return sum / yrs;
        }
        public double GetAnnualAvgDenitrification()
        {
            var yrs = ((float)Counts.Sum() / 365.25);
            var sum = Denitrification.Sum();
            return sum / yrs;
        }
        public double GetAnnualAvgPool()
        {
            var yrs = ((float)Counts.Sum() / 365.25);
            var sum = Pool.Sum();
            return sum / yrs;
        }



    }
}
