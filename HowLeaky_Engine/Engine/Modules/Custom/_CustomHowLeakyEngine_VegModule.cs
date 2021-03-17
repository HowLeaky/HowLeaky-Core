using HowLeaky_SimulationEngine.Attributes;
using HowLeaky_SimulationEngine.Enums;
using HowLeaky_SimulationEngine.Tools;
using System;

namespace HowLeaky_SimulationEngine.Engine
{
    public class _CustomHowLeakyEngine_VegModule:_CustomHowLeakyEngineModule
    {

      
        public _CustomHowLeakyEngine_VegModule(HowLeakyEngine sim) : base(sim)
        {
        }

        public bool IsLAI()
        {
            return this is HowLeakyEngineModule_LAIVeg;
        }

        public virtual string GetName()
        {
            return "Unknown";
        }
         public bool TodayIsHarvestDay { get; set; }
        public bool PredefinedResidue { get; set; }
        public CropStatus CropStatus { get; set; }
        public BrowserDate LastSowingDate { get; set; } 
        public BrowserDate LastHarvestDate { get; set; } 
        public BrowserDate FirstRotationDate { get; set; } 
        //public int DaysSincePlanting { get; set; }
        public int RotationCount { get; set; }
        public int MissedRotationCount { get; set; }
        //public int PlantingCount { get; set; }
        //public int HarvestCount { get; set; }
        //public int CropDeaths { get; set; }
        //public int NumberOfFallows { get; set; }
        public int KillDays { get; set; }
        public double MaximumRootDepth { get; set; }
        public double TotalTranspiration { get; set; }

        //These should be reported
        public double CropStage { get; set; }
        public double CropCover { get; set; }
        public double CropCoverPercent { get; set; }
        public double CropResidue { get; set; }
        //public double Runoff { get; set; }
        //public double Drainage { get; set; }
        public double TotalEvapotranspiration { get; set; }
        public double SoilWaterAtPlanting { get; set; }
        public double SoilWaterAtHarvest { get; set; }
        public double CumulativeYield { get; set; }
        public double AccumulatedCover { get; set; }
        public double AccumulatedResidue { get; set; }
        public double AccumulatedTranspiration { get; set; }

        [Internal] public double HeatUnits { get; set; }
        [Internal] public double HeatUnitIndex { get; set; }

        //Reportable Outputs
        [Output]        public int DaysSincePlanting { get; set; }
        [Output]        public double LAI { get; set; }
        [Output]        public double GreenCover { get; set; }
        [Output]        public double ResidueCover { get; set; }
        [Output]        public double TotalCover { get; set; }
        [Output]        public double ResidueAmount { get; set; }
        [Output]        public double DryMatter { get; set; }
        [Output]        public double RootDepth { get; set; }
        [Output]        public double Yield { get; set; }
        [Output]        public double PotTranspiration { get; set; }
        [Output]        public double GrowthRegulator { get; set; }
        [Output]        public double WaterStressIndex { get; set; }
        [Output]        public double TempStressIndex { get; set; }
        [Output]        public double CropRainfall { get; set; }
        [Output]        public double CropIrrigation { get; set; }
        [Output]        public double CropRunoff { get; set; }
        [Output]        public double SoilEvaporation { get; set; }
        [Output]        public double CropTranspiration { get; set; }
        [Output]        public double CropEvapoTranspiration { get; set; } //Not currently set

        

        [Output]        public double CropDrainage { get; set; }
        [Output]        public double CropLateralFlow { get; set; }
        [Output]        public double CropOverflow { get; set; }
        [Output]        public double CropSoilErosion { get; set; }
        [Output]        public double CropSedimentDelivery { get; set; }
        [Output]        public double PlantingCount { get; set; }
        [Output]        public double HarvestCount { get; set; }
        [Output]        public double CropDeathCount { get; set; }
        [Output]        public double FallowCount { get; set; }


        //public virtual void Simulate(){}

       

        public bool TodayIsPlantDay { get;set;}
        

        public int CropIndex
        {
            get
            {
                return Engine.GetCropIndex(this);
            }
        }

 
        public bool GetExistsInTheGround()
        {
            return (CropStatus == CropStatus.Planting || CropStatus == CropStatus.Growing);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ExistsInTheGround
        {
            get
            {
                return GetExistsInTheGround();
            }
        }

        public override void ResetAnyParametersIfRequired()
        {
            ResetCropParametersAfterHarvest();
            TodayIsHarvestDay=false;
            TodayIsPlantDay=false;
        }

        //Virtual methods

        public virtual void ResetCropParametersAfterHarvest() { }
        public virtual bool IsCropUnderMaxContinuousRotations() { return true; }
        public virtual bool SatisifiesMultiPlantInWindow() { return true; }
        public virtual bool HasCropHadSufficientContinuousRotations() { return true; }
        public virtual bool HasCropBeenAbsentForSufficientYears(BrowserDate today) { return true; }
        public virtual bool IsReadyToPlant() { return true; }
        public virtual bool StillRequiresIrrigating() { return false; }
        public virtual bool IsSequenceCorrect() { return true; }
        public virtual bool IsGrowing() { return (CropStatus == CropStatus.Growing); }
        public virtual bool InitialisedMeasuredInputs() { return false; }
        public virtual bool DoesCropMeetSowingCriteria() { return false; }
        public virtual bool GetIsLAIModel() { return false; }
        public virtual bool GetInFallow() { return (CropStatus == CropStatus.Fallow); }
        public virtual double GetPotentialSoilEvaporation() { return 0; }
        public virtual double GetTotalCover() { return 0; }
        public virtual double CalculatePotentialTranspiration() { return 0; }




      


        public bool ResetRotationCount()
        {
            RotationCount = 0;
            MissedRotationCount = 0;
            FirstRotationDate = Engine.TodaysDate;
            return true;
        }

        public bool GetIsPlanting()
        {
            //TOD:Check this ishould be double
            return (MathTools.DoublesAreEqual(DaysSincePlanting, 0));
        }

        public void CalculateTranspiration()
        {
            for (int i = 0; i < Engine.SoilModule.LayerCount; ++i)
            {
                Engine.SoilModule.LayerTranspiration[i] = 0;
            }
            PotTranspiration = CalculatePotentialTranspiration();

            if (PotTranspiration > 0)
            {
                double psup;

                double[] density = new double[10];
                double[] supply = new double[10];
                double[] rootPenetration = new double[10];


                // initialize transpiration array
                //Didn't we just do this !
                //for (int i = 0; i < Sim.SoilController.LayerCount; ++i)
                //{
                //    Sim.SoilController.LayerTranspiration[i] = 0;
                //}
                //  Calculate soil water supply index

                for (int i = 0; i < Engine.SoilModule.LayerCount; ++i)
                {
                    double denom = Engine.SoilModule.DrainUpperLimitRelWP[i];
                    if (!MathTools.DoublesAreEqual(denom, 0))
                    {
                        Engine.SoilModule.MCFC[i] = MathTools.CheckConstraints(Engine.SoilModule.SoilWaterRelWP[i] / denom, 1.0, 0.0);
                    }
                    else
                    {
                        Engine.SoilModule.MCFC[i] = 0;
                        //LogDivideByZeroError("CalculateTranspiration","sim.DrainUpperLimit_rel_wp[i]","sim.mcfc[i]");
                    }

                    //TODO: the 0.3 here is the SWPropForNoStress variable - need to implement

                    if (Engine.SoilModule.MCFC[i] >= 0.3)
                    {
                        supply[i] = 1.0;
                    }
                    else
                    {
                        supply[i] = Engine.SoilModule.MCFC[i] / 0.3;
                    }
                }

                //  Calculate root penetration per layer (root_penetration)
                //  Calculate root density per layer (density)

                rootPenetration[0] = 1.0;
                density[0] = 1.0;
                for (int i = 1; i < Engine.SoilModule.LayerCount; ++i)
                {
                    if (Engine.SoilModule.Depth[i + 1] - Engine.SoilModule.Depth[i] > 0)
                    {
                        rootPenetration[i] = Math.Min(1.0, Math.Max(RootDepth - Engine.SoilModule.Depth[i], 0) / (Engine.SoilModule.Depth[i + 1] - Engine.SoilModule.Depth[i]));
                        if (Engine.SoilModule.Depth[i + 1] > 300)
                        {
                            if (!MathTools.DoublesAreEqual(MaximumRootDepth, 300))
                            {
                                //density[i] = Math.Max(0.0, (1.0 - 0.50 * Math.Min(1.0, (Sim.SoilController.Depth[i + 1] - 300.0) / (MaximumRootDepth - 300.0))));
                                density[i] = Math.Max(0.0, (1.0 - 0.50 * Math.Min(1.0, (Engine.SoilModule.Depth[i + 1] - 300.0) / (RootDepth - 300.0))));

                            }
                            else
                            {
                                density[i] = 0.5;
                                //dont log this error
                            }
                        }
                        else
                        {
                            density[i] = 1.0;
                        }
                    }
                    else
                    {
                        rootPenetration[i] = 0;
                        density[i] = 1.0;
                        //string text1="sim.depth["+string(i+1)+"]-sim.depth["+string(i)+"] ("+FormatFloat("0.#",sim.depth[i+1])+"-"+FormatFloat("0.#",sim.depth[i])+")";
                        //LogDivideByZeroError("CalculateTranspiration",text1,"root_penetration["+String(i)+"]");
                    }
                }

                // Calculate transpiration from each layer

                psup = 0;
                for (int i = 0; i < Engine.SoilModule.LayerCount; ++i)
                {
                    if (rootPenetration[i] < 1.0 && Engine.SoilModule.MCFC[i] <= (1.0 - rootPenetration[i]))
                    {
                        Engine.SoilModule.LayerTranspiration[i] = 0.0;
                    }
                    else
                    {
                        Engine.SoilModule.LayerTranspiration[i] = density[i] * supply[i] * PotTranspiration;
                    }
                    if (Engine.SoilModule.LayerTranspiration[i] > Engine.SoilModule.SoilWaterRelWP[i])
                    {
                        Engine.SoilModule.LayerTranspiration[i] = Math.Max(0.0, Engine.SoilModule.SoilWaterRelWP[i]);
                    }
                    psup = psup + Engine.SoilModule.LayerTranspiration[i];
                }

                // reduce transpiration if more than potential transpiration
                if (!MathTools.DoublesAreEqual(psup, 0) && psup > PotTranspiration)
                {
                    for (int i = 0; i < Engine.SoilModule.LayerCount; ++i)
                    {
                        Engine.SoilModule.LayerTranspiration[i] *= PotTranspiration / psup;
                    }
                }
                TotalTranspiration = 0;
                for (int i = 0; i < Engine.SoilModule.LayerCount; ++i)
                {
                    TotalTranspiration += Engine.SoilModule.LayerTranspiration[i];
                }
            }
            else
            {
                for (int i = 0; i < Engine.SoilModule.LayerCount; ++i)
                {
                    Engine.SoilModule.LayerTranspiration[i] = 0;
                }
                TotalTranspiration = 0;
            }
            AccumulatedTranspiration += TotalTranspiration;

            CropTranspiration = TotalTranspiration;

            CropEvapoTranspiration = CropTranspiration + Engine.SoilModule.SoilEvap;

            Engine.SoilModule.EvapoTransp += CropTranspiration;
            Engine.SoilModule.Transpiration = CropTranspiration;
        }

        public double CalcFallowSoilWater()
        {
            if (SoilWaterAtHarvest > 0)
            {
                return SoilWaterAtPlanting - SoilWaterAtHarvest;
            }
            return 0;
        }


        public virtual void Plant()
        {
            ++Engine.TotalNumberPlantings;
            Engine.AccumulateCovDayBeforePlanting += Engine.TotalResidueCover;
            var lastcrop = Engine.CurrentCrop;
            TodayIsPlantDay=true;
            Engine.CurrentCrop = this;
            LastSowingDate = Engine.TodaysDate;
            if (lastcrop != this || RotationCount == 0)
            {
                lastcrop.ResetRotationCount();
                Engine.CurrentCrop.ResetRotationCount();
            }

            DaysSincePlanting = 0;
            ++PlantingCount;
            DryMatter = 0;
            DryMatter = 0;
            CropStage = 0;
            RootDepth = 0;
            Yield = 0;
            SoilWaterAtPlanting = Engine.SoilModule.TotalSoilWater;
            CropStatus = CropStatus.Growing;

        }

        public void ResetCover()
        {
            GreenCover = 0;
            CropCover = 0;
            CropCoverPercent = 0;
            TotalCover = 0;
        }


        public void CalculateResidue()
        {
            AccumulatedResidue += ResidueCover * 100.0;
        }

        public int GetCropIndex()
        {
            return Engine.GetCropIndex(this);
        }

        
       
    }
}
