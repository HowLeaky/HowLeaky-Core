using HowLeaky_SimulationEngine.Attributes;
using HowLeaky_SimulationEngine.Inputs;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using HowLeaky_SimulationEngine.Enums;
using HowLeaky_SimulationEngine.Errors;

namespace HowLeaky_SimulationEngine.Engine
{
    class HowLeakyEngineModule_LAIVeg : _CustomHowLeakyEngine_VegModule
    {



        public HowLeakyEngineModule_LAIVeg(HowLeakyEngine sim, HowLeakyInputs_LAIVeg inputs) : base(sim)
        {
            try
            {
                Name = inputs.Name;
                PredefinedResidue = false;
                this.InputModel = inputs;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }
       

        public override string GetName()
        {
            return InputModel.Name;
        }
        public HowLeakyInputs_LAIVeg InputModel { get; set; }

        public static int UNCONTROLLED = 0;
        public static int OPPORTUNITY = 1;
        public static int INCROPORDER = 2;

        [Internal] public int LastPlantYear { get; set; }
        [Internal] public int DaysSinceFallow { get; set; }
        [Internal] public bool AllowMultiPlanting { get; set; }


        [Internal] public double Dhrlt { get; set; }
        [Internal] public double Hrltp { get; set; }
        [Internal] public double Hufp { get; set; }
        [Internal] public double Decompdays { get; set; }
        [Internal] public double LAICurveY1active { get; set; }
        [Internal] public double LAICurveY2active { get; set; }
        [Internal] public double MaxCalcLAI { get; set; }

        //public bool TodayIsPlantDay { get; set; }

        public override void Initialise()
        {
            try
            {
                base.Initialise();
                Scurve();
                HeatUnitIndex = 0;
                Hrltp = 0;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }


        public override void Simulate()
        {
            try
            {
                if (ExistsInTheGround)
                {
                    ++DaysSincePlanting;
                    CalculateTranspiration();

                    if (CheckCropSurvives())
                    {
                        //Sim.UpdateManagementEventHistory(ManagementEvent.CropGrowing, Sim.VegetationModule.GetCropIndex(this));
                        //if (TodayIsPlantDay) //remove this once debugging is done
                        if (DaysSincePlanting == 1)
                        {

                            LAI = 0.02;       // this is here just to replicate the old code... see Brett about it.
                            //TodayIsPlantDay = false;
                        }

                        if (!ReadyToHarvest())
                        {
                            RecordCropStage();
                            CalculateGrowthStressFactors();
                            CalculateLeafAreaIndex();
                            CalculateBioMass();
                            CalculateRootGrowth();
                            //CalculateResidue();
                            //	lai=0;
                        }
                        else
                        {
                            HarvestTheCrop();
                            //CalculateResidue();
                        }
                    }
                    else
                    {
                        SimulateCropDeath();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }


        }


        public override double GetPotentialSoilEvaporation()
        {
            try
            {
                if (Engine.UsePERFECTSoilEvapFn)
                {
                    if (LAI < 0.3)
                    {
                        return Engine.ClimateModule.PanEvap * (1.0 - GreenCover);
                    }
                }
                return Engine.ClimateModule.PanEvap * (1.0 - CropCover);
                //return Sim.ClimateModule.PanEvap * (1.0 - GetTotalCover());// TotalCover);
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            //return 0;
        }


        public override bool IsSequenceCorrect()
        {
            try
            {
                //if the cropindex is less than 2, the this is either the current crop or the next crop.
                //int index=sim.SortedCropModules.IndexOf(this);
                int index = 0;
                for (int i = 0; i < 10; i++)
                {
                    if (Engine.SortedVegetationModules[i] == this)
                    {
                        index = i;
                        break;
                    }

                }

                if (index == 2)
                {
                    return (InputModel.RotationFormat != INCROPORDER);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            //return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool DoesCropMeetSowingCriteria()
        {
            try
            {
                if (InputModel.PlantingRulesOptions == (int)EPlantingRules.FixedAnualPlaning)
                {
                    if (InputModel.PlantDate.MatchesDate(Engine.TodaysDate))
                    {
                        return true;
                    }
                }
                else if (InputModel.PlantingRulesOptions == (int)EPlantingRules.PlantInWindow)
                {
                    // run ALL planting tests up front before testing results so that results
                    // can be added to the annotations on the time-series charts.
                    bool satisifiesWindowConditions = SatisifiesWindowConditions();
                    bool satisifiesFallowConditions = SatisifiesFallowConditions();
                    bool satisifiesPlantingRainConditions = SatisifiesPlantingRainConditions();
                    bool satisifiesSoilWaterConditions = SatisifiesSoilWaterConditions();
                    bool satisifiesMultiPlantInWindow = SatisifiesMultiPlantInWindow();
                    if (satisifiesWindowConditions && satisifiesMultiPlantInWindow)
                    {
                        if (satisifiesFallowConditions)
                        {
                            if (satisifiesPlantingRainConditions && satisifiesSoilWaterConditions)
                            {
                                return true;
                            }
                        }
                        else if (InputModel.ForcePlantingAtEndOfWindow)
                        {
                            if (!HasAlreadyPlantedInThisWindow())
                            {
                                //DateTime endplantingdate = new DateTime(Sim.TodaysDate.Year, InputModel.PlantingWindowEndDate.Month, InputModel.PlantingWindowEndDate.Day);
                                //return (Sim.Today == endplantingdate);
                                if(InputModel.PlantingWindowEndDate!=null)
                                { 
                                    return InputModel.PlantingWindowEndDate.MatchesDate(Engine.TodaysDate);
                                }
                            }
                            return false;
                        }
                        else
                        {
                            ++MissedRotationCount;
                        }
                    }
                }
                else if (InputModel.PlantingRulesOptions == (int)EPlantingRules.PlantFromSequenceFile)
                {
                    if(InputModel.PlantingSequence!=null)
                    { 
                        return InputModel.PlantingSequence.ContainsDate(Engine.TodaysDate);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool SatisifiesMultiPlantInWindow()
        {
            try
            {
                if (!InputModel.MultiPlantInWindow && LastSowingDate != null)
                {
                    return !HasAlreadyPlantedInThisWindow();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasAlreadyPlantedInThisWindow()
        {
            //Note there was a possible error here in previous version where the wrong year could have been used.
            return Engine.IsDateInWindow(LastSowingDate, InputModel.PlantingWindowStartDate, InputModel.PlantingWindowEndDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool SatisifiesWindowConditions()
        {
            bool result = Engine.IsDateInWindow(Engine.TodaysDate, InputModel.PlantingWindowStartDate, InputModel.PlantingWindowEndDate);
            //if (result)
            //{
            //    Sim.UpdateManagementEventHistory(ManagementEvent.InPlantingWindow, Sim.VegetationModule.GetCropIndex(this));
            //}
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool SatisifiesPlantingRainConditions()
        {
            try
            {
                bool result = true;
                if (InputModel.PlantingRainSwitch)
                {
                    int actualSowingDelay = InputModel.SowingDelay;
                    double sumRain = 0;
                    int index;
                    int count_rainfreedays = 0;
                    int max = 3 * InputModel.SowingDelay;
                    for (int i = 0; i < max; ++i)
                    {
                        index = Engine.ClimateModule.CurrentIndex - i;
                        if (index >= 0)
                        {
                            if (Engine.ClimateModule.Rain < 5) //if ((Sim.ClimateModule.Rainfall)[index] < 5)
                            {
                                ++count_rainfreedays;
                            }
                        }
                        if (count_rainfreedays == InputModel.SowingDelay)
                        {
                            actualSowingDelay = i;
                            i = max;
                        }
                    }
                    if (count_rainfreedays == InputModel.SowingDelay)
                    {
                        int fallow_planting_rain = (int)Engine.ClimateModule.SumRain(InputModel.RainfallSummationDays, actualSowingDelay);
                        result = (fallow_planting_rain > InputModel.RainfallPlantingThreshold);
                    }
                    else
                    {
                        result = false;
                    }
                    if (result)
                    {
                        // Sim.UpdateManagementEventHistory(ManagementEvent.MeetsRainfallPlantCritera, Sim.VegetationModule.GetCropIndex(this));
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool SatisifiesFallowConditions()
        {
            bool result = true;
            if (InputModel.FallowSwitch)
            {
                result = (Engine.DaysSinceHarvest >= InputModel.MinimumFallowPeriod);
                if (result)
                {
                    //  Sim.UpdateManagementEventHistory(ManagementEvent.MeetsDaysSinceHarvestPlantCritera, Sim.VegetationModule.GetCropIndex(this));
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool SatisifiesSoilWaterConditions()
        {
            try
            {
                bool result = true;
                if (InputModel.SoilWaterSwitch)
                {
                    double SumSW = 0.0;
                    for (int i = 0; i < Engine.SoilModule.LayerCount; ++i)
                    {
                        if (Engine.SoilModule.Depth[i + 1] - Engine.SoilModule.Depth[i] > 0)
                        {
                            if (InputModel.DepthToSumPlantingWater > Engine.SoilModule.Depth[Engine.SoilModule.LayerCount])
                            {
                                InputModel.DepthToSumPlantingWater = Engine.SoilModule.Depth[Engine.SoilModule.LayerCount];
                            }
                            if (Engine.SoilModule.Depth[i + 1] < InputModel.DepthToSumPlantingWater)
                            {
                                SumSW += Engine.SoilModule.SoilWaterRelWP[i];
                            }
                            if (Engine.SoilModule.Depth[i] < InputModel.DepthToSumPlantingWater && Engine.SoilModule.Depth[i + 1] > InputModel.DepthToSumPlantingWater)
                            {
                                SumSW += Engine.SoilModule.SoilWaterRelWP[i] * (InputModel.DepthToSumPlantingWater - Engine.SoilModule.Depth[i]) / (Engine.SoilModule.Depth[i + 1] - Engine.SoilModule.Depth[i]);
                            }
                            if (!MathTools.DoublesAreEqual(Engine.SoilModule.DrainUpperLimitRelWP[i], 0))
                            {
                                Engine.SoilModule.MCFC[i] = Math.Max(Engine.SoilModule.SoilWaterRelWP[i] / Engine.SoilModule.DrainUpperLimitRelWP[i], 0.0);
                            }
                            else
                            {
                                Engine.SoilModule.MCFC[i] = 0;
                            }
                        }
                        else
                        {
                            SumSW = 0;
                            Engine.SoilModule.MCFC[i] = 0;
                            MathTools.LogDivideByZeroError("SatisifiesSoilWaterConditions", "sim.depth[i+1]-sim.depth[i]", "SumSW");
                        }
                    }
                    result = (SumSW > InputModel.SoilWaterReqToPlant && Engine.SoilModule.MCFC[0] >= InputModel.MinSoilWaterTopLayer && Engine.SoilModule.MCFC[0] <= InputModel.MaxSoilWaterTopLayer);

                    if (result)
                    {
                        // Sim.UpdateManagementEventHistory(ManagementEvent.MeetsSoilWaterPlantCritera, Sim.VegetationModule.GetCropIndex(this));
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
           // return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsCropUnderMaxContinuousRotations()
        {
            try
            {
                if (InputModel.PlantingRulesOptions != (int)EPlantingRules.PlantFromSequenceFile && InputModel.RotationFormat != UNCONTROLLED)
                {
                    if (FirstRotationDate != null)
                    {
                        return (RotationCount + MissedRotationCount < InputModel.MaxRotationCount);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool HasCropHadSufficientContinuousRotations()
        {
            try
            {
                if (InputModel.PlantingRulesOptions != (int)EPlantingRules.PlantFromSequenceFile && InputModel.RotationFormat != UNCONTROLLED)
                {
                    if (FirstRotationDate != null)
                    {
                        return (RotationCount + MissedRotationCount >= InputModel.MinRotationCount);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="today"></param>
        /// <returns></returns>
        public override bool HasCropBeenAbsentForSufficientYears(BrowserDate today)
        {
            try
            {
                if (InputModel.PlantingRulesOptions != (int)EPlantingRules.PlantFromSequenceFile && InputModel.RotationFormat != UNCONTROLLED)
                {
                    if (LastHarvestDate != null)
                    {
                        int months_since_last_sow = today.MonthsFrom(LastHarvestDate);//DateUtilities.MonthsBetween(today, LastHarvestDate);
                        return (months_since_last_sow >= InputModel.RestPeriodAfterChangingCrops);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Plant()
        {
            try
            {
                base.Plant();
                Engine.CurrentCrop.ResetCover();
                HeatUnitIndex = 0;
                HeatUnits = 0;
                MaxCalcLAI = 0;
                Hufp = 0;
                KillDays = 0;
                ++RotationCount;
                //TodayIsPlantDay = true;
                if (Engine.ResetSoilWaterAtPlanting)
                {
                    for (int i = 0; i < Engine.SoilModule.LayerCount; ++i)
                    {
                        Engine.SoilModule.SoilWaterRelWP[i] = (Engine.ResetValueForSWAtPlanting / 100.0) * Engine.SoilModule.DrainUpperLimitRelWP[i];
                    }
                }
                //Sim.FManagementEvent = ManagementEvent.Planting;
                //Sim.UpdateManagementEventHistory(ManagementEvent.Planting, Sim.VegetationModule.GetCropIndex(this));
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }


        public override double GetTotalCover()
        {
            try
            {
                //TotalCover = Math.Min(1.0, GreenCover + ResidueCover * (1 - GreenCover));


                TotalCover = Math.Min(1.0, CropCover + ResidueCover * (1 - CropCover));
                //REVIEW
                //TotalCoverPc = TotalCover * 100.0;
                return TotalCover;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }


        public bool CheckCropSurvives()
        {
            try
            {
                if (Engine.IgnoreCropKill == false)
                {
                    if (CropStage <= 2.0)
                    {
                        if (WaterStressIndex <= InputModel.SWPropForNoStress)
                        {
                            ++KillDays;
                        }
                        else
                        {
                            KillDays = 0;
                        }
                        if (KillDays >= InputModel.DaysOfStressToDeath)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        KillDays = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

            return true;
        }

        /// <summary>
        /// This subroutine calculates growth stress factors for todays_temp
        /// and water
        /// </summary>
        public void CalculateGrowthStressFactors()
        {
            try
            {
                // ******************************
                // *  Temperature stress index  *
                // ******************************
                // [Equation 2.235] from EPIC

                double ratio;
                if (!MathTools.DoublesAreEqual(InputModel.OptTemp, InputModel.BaseTemp))
                {
                    ratio = (Engine.ClimateModule.Temperature - InputModel.BaseTemp) / (InputModel.OptTemp - InputModel.BaseTemp);
                }
                else
                {
                    ratio = 1;
                    //dont log error for this one
                }
                TempStressIndex = Math.Sin(0.5 * Math.PI * ratio);
                TempStressIndex = Math.Max(TempStressIndex, 0.0);
                TempStressIndex = Math.Min(TempStressIndex, 1.0);

                // ************************
                // *  Water stress index  *
                // ************************

                WaterStressIndex = 1.0;
                if (PotTranspiration > 0.0)
                {
                    WaterStressIndex = TotalTranspiration / PotTranspiration;
                }
                WaterStressIndex = Math.Max(WaterStressIndex, 0.0);
                WaterStressIndex = Math.Min(WaterStressIndex, 1.0);

                // *************************************
                // *  calculate minimum stress factor  *
                // *************************************
                GrowthRegulator = 1;
                GrowthRegulator = Math.Min(GrowthRegulator, TempStressIndex);
                GrowthRegulator = Math.Min(GrowthRegulator, WaterStressIndex);
                GrowthRegulator = MathTools.CheckConstraints(GrowthRegulator, 1.0, 0);
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }


        }

        /// <summary>
        /// This subroutine fits an s curve to two points.  It was derived   
        /// from EPIC3270.
        /// </summary>
        public void Scurve()
        {
            try
            {
                if (MathTools.DoublesAreEqual(InputModel.PercentOfMaxLai1, 1))
                {
                    InputModel.PercentOfMaxLai1 = 0.99999;
                }
                if (MathTools.DoublesAreEqual(InputModel.PercentOfMaxLai2, 1))
                {
                    InputModel.PercentOfMaxLai2 = 0.99999;
                }
                if (InputModel.PercentOfMaxLai1 > 0 && InputModel.PercentOfMaxLai2 > 0)
                {
                    double value1 = (InputModel.PercentOfGrowSeason1 / 100) / (InputModel.PercentOfMaxLai1 / 100) - (InputModel.PercentOfGrowSeason1 / 100);
                    double value2 = (InputModel.PercentOfGrowSeason2 / 100) / (InputModel.PercentOfMaxLai2 / 100) - (InputModel.PercentOfGrowSeason2 / 100);
                    if (!MathTools.DoublesAreEqual(value1, 0) && !MathTools.DoublesAreEqual(value2, 0))
                    {
                        double x = Math.Log(value1);
                        LAICurveY2active = (x - Math.Log(value2)) / ((InputModel.PercentOfGrowSeason2 / 100) - (InputModel.PercentOfGrowSeason1 / 100));
                        LAICurveY1active = x + (InputModel.PercentOfGrowSeason1 / 100) * LAICurveY2active;
                    }
                    else
                    {
                        MathTools.LogDivideByZeroError("Scurve", "in_LAICurveX1/in_LAICurveY1-in_LAICurveX1 or in_LAICurveX2/in_LAICurveY2-in_LAICurveX2", "LAI Curves - Taking Logs");
                    }
                }
                else
                {
                    MathTools.LogDivideByZeroError("Scurve", "in_LAICurveY1 or in_LAICurveY2", "LAICurveY2active or LAICurveY1active");
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        /// <summary>
        /// This subroutine calculates leaf area index using the major             
        /// functions from the EPIC model
        /// </summary>
        public void CalculateLeafAreaIndex()
        {
            try
            {
                double HUF;   //Heat Unit Factor
                double dHUF;  //daily change in Heat Unit Factor
                double dlai;

                // ***************************
                // *  accumulate heat units  *
                // ***************************

                // accumulate heat units
                HeatUnits = HeatUnits + Math.Max(Engine.ClimateModule.Temperature - InputModel.BaseTemp, 0.0);

                // caluclate heat unit index [Equation 2.191] from EPIC
                if (!MathTools.DoublesAreEqual(InputModel.DegreeDaysPlantToHarvest, 0))
                {
                    HeatUnitIndex = HeatUnits / InputModel.DegreeDaysPlantToHarvest;
                }
                else
                {
                    HeatUnitIndex = 1;
                    MathTools.LogDivideByZeroError("CalculateLeafAreaIndex", "in_DegreeDaysToMaturity_days", "heat_unit_index");

                }

                // ***************************
                // *  calculate leaf growth  *
                // ***************************

                if (HeatUnitIndex < InputModel.PropGrowSeaForMaxLai)
                {
                    // heat unit factor, [Equation 2.198] from EPIC
                    double denom = (HeatUnitIndex + Math.Exp(LAICurveY1active - LAICurveY2active * HeatUnitIndex));
                    if (!MathTools.DoublesAreEqual(denom, 0))
                    {
                        HUF = HeatUnitIndex / denom;
                    }
                    else
                    {
                        HUF = 0;
                        //not sure I should log this one
                    }
                    dHUF = HUF - Hufp;
                    Hufp = HUF;

                    // leaf area index [Equation 2.197] from EPIC
                    //        Eqn 2.197 originally stated that
                    //
                    //        dlai = dHUF * MaxLAI * (1.0-Math.Exp(5.0*(laip-MaxLAI)))* sqrt(reg)
                    //
                    // This function NEVER allows lai to achieve MaxLAI under no stress
                    // conditions due to the exponential term.  This term was removed.  Therefore,
                    // lai development is governed by the S-Curve, Max lai, and stress factors
                    // only.


                    if (Engine.UsePERFECTLeafAreaFn)
                    {
                        dlai = dHUF * InputModel.PotMaxLAI * Math.Sqrt(GrowthRegulator);
                    }
                    else
                    {
                        dlai = dHUF * InputModel.PotMaxLAI * GrowthRegulator;
                    }
                    LAI = LAI + dlai;

                    // store maximum lai
                    if (LAI > MaxCalcLAI)
                    {
                        MaxCalcLAI = LAI;
                    }
                }

                // ******************************
                // *  calculate leaf senesence  *
                // ******************************

                if (HeatUnitIndex >= InputModel.PropGrowSeaForMaxLai && HeatUnitIndex <= 1)
                {
                    if (InputModel.SenesenceCoef > 0)
                    {
                        // leaf senesence [Equation 2.199] from EPIC
                        if (!MathTools.DoublesAreEqual(1.0 - InputModel.PropGrowSeaForMaxLai, 0))
                        {
                            LAI = MaxCalcLAI * Math.Pow((1.0 - HeatUnitIndex) / (1.0 - InputModel.PropGrowSeaForMaxLai), InputModel.SenesenceCoef);
                        }
                        else
                        {
                            LAI = 0;
                        }
                        LAI = Math.Max(LAI, 0);
                    }
                    else LAI = 0;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        /// <summary>
        /// Subroutine calculates biomass using EPIC type functions
        /// </summary>
        public void CalculateBioMass()
        {
            try
            {
                double rad, par, hrlt, dhrlt;
                // *********************************
                // *  intercepted radiation (PAR)  *
                // *********************************
                // Assumes PAR is 50% of solar radiation
                //         Extinction Coefficient = 0.65
                rad = Engine.ClimateModule.SolarRadiation;
                par = 0.5 * rad * (1.0 - Math.Exp(-0.65 * LAI));

                // **********************
                // *  daylength factor  *
                // **********************
                // daylength factor from PERFECT
                hrlt = GetDayLength();
                dhrlt = hrlt - Hrltp;
                if (Hrltp < 0.01)
                {
                    dhrlt = 0.0;
                }
                Hrltp = hrlt;

                double rue = InputModel.RadUseEffic;
                double effectiverue = rue;

                if (InputModel.WaterLoggingSwitch && IsWaterLogged())
                {
                    effectiverue = rue * InputModel.WaterLoggingFactor2;
                }
                // **************************
                // *  biomass accumulation  *
                // **************************
                // [Equation 2.193] from EPIC
                if (Engine.UsePERFECTDryMatterFn)
                {
                    DryMatter += GrowthRegulator * par * effectiverue * Math.Pow(1.0 + dhrlt, 3.0);
                    DryMatter = DryMatter * 10.0;
                }

                else
                {
                    par = 0.5 * rad;
                    DryMatter += (effectiverue * par * WaterStressIndex * TempStressIndex * GreenCover) * 10;
                    //DryMatter = DryMatter * 10.0;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsWaterLogged()
        {
            int i = 1;
            return (Engine.SoilModule.SoilWaterRelWP[i] > Engine.SoilModule.DrainUpperLimitRelWP[i]);
        }

        /// <summary>
        /// This subroutine calculates day length from latitude and day    *
        /// number in the year. 
        /// </summary>
        /// <returns></returns>
        public double GetDayLength()
        {
            try
            {
                double alat = (!MathTools.DoublesAreEqual(Engine.ClimateModule.InputModel.Latitude, 0) ? Engine.ClimateModule.InputModel.Latitude : -27);
                double sund = -2.2;
                int dayno = Engine.TodaysDate.GetJDay();//(Sim.Today - new DateTime(Sim.Today.Year, 1, 1 + 1)).Days;
                double theta = 0.0172142 * (dayno - 172.0);
                double sdcln = 0.00678 + 0.39762 * Math.Cos(theta) + 0.00613 * Math.Sin(theta) - 0.00661 * Math.Cos(2.0 * theta) - 0.00159 * Math.Sin(2.0 * theta);
                double dcln = Math.Asin(sdcln);
                double rlat = alat * 0.0174533;
                double dnlat = 0;
                if (!MathTools.DoublesAreEqual(Math.Cos(rlat), 0) && !MathTools.DoublesAreEqual(Math.Cos(dcln), 0))
                {
                    dnlat = -(Math.Sin(rlat) / Math.Cos(rlat)) * (Math.Sin(dcln) / Math.Cos(dcln));
                }
                else
                {
                    MathTools.LogDivideByZeroError("GetDayLength", "Math.Cos(rlat) or Math.Cos(dcln)", "dnlat");
                }
                double rsund = sund * 0.0174533;
                double twif = Math.Cos(rlat) * Math.Cos(dcln);
                double atwil = 0;
                if (!MathTools.DoublesAreEqual(twif, 0))
                {
                    atwil = Math.Sin(rsund) / twif;
                }
                else
                {
                    MathTools.LogDivideByZeroError("GetDayLength", "twif", "atwil");
                }
                double htwil = Math.Acos(atwil + dnlat);
                return 7.639437 * htwil;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateRootGrowth()
        {
            try
            {
                RootDepth = RootDepth + InputModel.DailyRootGrowth;
                RootDepth = MathTools.CheckConstraints(RootDepth, InputModel.MaxRootDepth, 0);
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ReadyToHarvest()
        {
            return (HeatUnitIndex >= 1);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ResetCropParametersAfterHarvest()
        {
            try
            {
                if (TodayIsHarvestDay) //if harvest day
                {
                    TodayIsHarvestDay = false;
                    Yield = 0;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void HarvestTheCrop()
        {
            try
            {
                LastHarvestDate = Engine.TodaysDate;
                TodayIsHarvestDay = true;
                //Sim.FManagementEvent = ManagementEvent.Harvest;
                //Sim.UpdateManagementEventHistory(ManagementEvent.Harvest, Sim.VegetationModule.GetCropIndex(this));
                ++HarvestCount;

                //	sim.days_since_harvest=0;    //should this also get reset at crop death
                //    soil_water_at_harvest=sim.total_soil_water;
                // ++number_of_fallows;
                //	CropStatus=csInFallow;
                //Yield = InputModel.HarvestIndex * DryMatter * 10.0;
                Yield = InputModel.HarvestIndex * DryMatter;
                //REVIEW
                //Output.Yield = Yield / 1000.0;
                //ResidueAmount = ResidueAmount + (DryMatter - Yield / 10.0) * 0.95 * 10.0;
                ResidueAmount = ResidueAmount + (DryMatter - Yield) * 0.95;

                GreenCover = 0;

                CalculateResidue();

                // Sim.VegetationModule.CalculateTotalResidue();

                CumulativeYield += Yield;
                ResetParametersForEndOfCrop();
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }


        }

        /// <summary>
        /// 
        /// </summary>
        public void SimulateCropDeath()
        {
            try
            {
                CalculateGrowthStressFactors();
                CalculateLeafAreaIndex();
                CalculateBioMass();
                CalculateRootGrowth();
                // CalculateBioMass();//only needs to be here to replicate previous results
                // CalculateResidue();//only needs to be here to replicate previous results
                ResidueAmount = ResidueAmount + (DryMatter - Yield) * 0.95;
                Engine.CalculateTotalResidue();
                WaterStressIndex = 1.0;
                ++CropDeathCount;
                Yield = 0;
                ResetParametersForEndOfCrop();

                //crop_stage=0;
                //dry_matter=0;
                //soil_water_at_harvest=sim.total_soil_water;

                //CropStatus=csInFallow;
                //	crop_cover=0;
                //	total_transpiration=0;
                //	for(int i=0;i<sim.LayerCount;++i)
                //		sim.layer_transpiration[i]=0;
                //   lai=0;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }


        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetParametersForEndOfCrop()
        {
            try
            {
                CropStatus = CropStatus.Fallow;
                ++FallowCount;
                SoilWaterAtHarvest = Engine.SoilModule.TotalSoilWater;
                Engine.DaysSinceHarvest = 0;    //should this also get reset at crop death
                DaysSincePlanting = 0;
                TotalTranspiration = 0;
                CropCover = 0;
                //CropCoverPercent = 0;
                CropStage = 0;
                LAI = 0;
                for (int i = 0; i < Engine.SoilModule.LayerCount; ++i)
                {
                    Engine.SoilModule.LayerTranspiration[i] = 0;
                }
                Engine.SoilModule.Transpiration = 0;
                CropTranspiration = 0;
                HeatUnitIndex = 0;
                RootDepth = 0;
                GreenCover = 0;
                GrowthRegulator = 0;
                DryMatter = 0;
                DryMatter = 0;
                PotTranspiration = 0;
                //CalculateResidue();
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }


        }

        /// <summary>
        /// 
        /// </summary>
        public void RecordCropStage()
        {
            try
            {
                //   CropAnthesis=false;
                if (HeatUnitIndex <= InputModel.PropGrowSeaForMaxLai && !MathTools.DoublesAreEqual(InputModel.PropGrowSeaForMaxLai, 0))
                {
                    CropStage = HeatUnitIndex * 2.0 / InputModel.PropGrowSeaForMaxLai;
                    //anth=0;
                }
                else
                {
                    if (MathTools.DoublesAreEqual(InputModel.PropGrowSeaForMaxLai, 0))
                    {
                        MathTools.LogDivideByZeroError("RecordCropStage", "in_PropSeasonForMaxLAI", "crop_stage");
                    }
                    //if(anth==0)
                    //		{
                    //			CropAnthesis=true;
                    //			anth=1;
                    //		}
                    if (!MathTools.DoublesAreEqual(InputModel.PropGrowSeaForMaxLai, 1))
                    {
                        CropStage = 2 + (HeatUnitIndex - InputModel.PropGrowSeaForMaxLai) / (1.0 - InputModel.PropGrowSeaForMaxLai);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override double CalculatePotentialTranspiration()
        {
            try
            {
                // Calculate potential transpiration
                if (Engine.UsePERFECTGroundCovFn)
                    GreenCover = Math.Min(LAI / 3.0, 1.0);
                else
                {
                    if (LAI > 0)
                    {
                        GreenCover = 1 - Math.Exp(-0.55 * (LAI + 0.1));  //  changed br on 9/12/2005
                    }
                    else
                    {
                        GreenCover = 0;
                    }
                }

                GreenCover = Math.Max(0.0, GreenCover);
                CropCover = Math.Max(CropCover, GreenCover);
                double value = Math.Min(Engine.ClimateModule.PanEvap * GreenCover, Engine.ClimateModule.PanEvap - Engine.SoilModule.SoilEvap);
                if (value > Engine.ClimateModule.PanEvap)
                {
                    value = Engine.ClimateModule.PanEvap;
                }
                //REVIEW
                //Output.GreenCover = GreenCover * 100.0;
                //CropCoverPercent = CropCover * 100.0;
                if (InputModel.WaterLoggingSwitch && IsWaterLogged())
                {
                    value = value * InputModel.WaterLoggingFactor1;
                }
                return value;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
           // return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool StillRequiresIrrigating()
        {
            try
            {
                return (HeatUnits < InputModel.PropGDDEnd / 100.0 * (double)(InputModel.DegreeDaysPlantToHarvest));
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public new void CalculateResidue()
        {
            try
            {
                if (Engine.UsePERFECTResidueFn)
                {
                    CalculateResiduePERFECT();
                }
                else
                {
                    CalculateResidueBR();
                }
                TotalCover = GetTotalCover();
                //TotalCover = TotalCover * 100.0;
                AccumulatedResidue += ResidueCover * 100.0;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateResidueBR()
        {
            try
            {
                int seriesindex = Engine.ClimateModule.CurrentIndex;
                double rain_yesterday = Engine.ClimateModule.RainOnDay(Engine.TodaysDate.AddDays(-1));// - new TimeSpan(1, 0, 0, 0));
                double rain_daybeforeyesterday = Engine.ClimateModule.RainOnDay(Engine.TodaysDate.AddDays(-2));//Sim.Today - new TimeSpan(2, 0, 0, 0));

                double mi = 4.0 / 7.0 * (Math.Min(Engine.SoilModule.EffectiveRain, 4) / 4.0
                            + Math.Min(rain_yesterday, 4) / 8.0
                            + Math.Min(rain_daybeforeyesterday, 4) / 16.0);  //moisture index
                                                                             //there is a minor problem here...doesn't take into consideration irrigation in the previous days.

                double ti = Math.Max(Engine.ClimateModule.Temperature / 32.0, 0);                   // temperature index
                Decompdays = Math.Min(Math.Min(mi, ti), 1);  //  min=0 days, max =1day

                // Will change this to a non-linear function in the near future. BR 14/09/2010

                ResidueAmount = Math.Max(0, ResidueAmount - ResidueAmount * InputModel.MaxResidueLoss / 100.0 * Decompdays);
                if (!MathTools.DoublesAreEqual(InputModel.BiomassAtFullCover, 0))
                    ResidueCover = Math.Min(1.0, ResidueAmount / InputModel.BiomassAtFullCover);
                else
                {
                    ResidueCover = 0;
                    MathTools.LogDivideByZeroError("CalculateResidue_BR", "in_BiomassAtFullCover", "residue_cover");
                }
                if (ResidueCover < 0)
                {
                    ResidueCover = 0;
                }
                //ResidueCover = ResidueCover * 100.0;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }


        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateResiduePERFECT()
        {
            try
            {
                // ************************************************************
                // *  Subroutine decays residue and calculates surface cover  *
                // ************************************************************
                //   Decay residue using Sallaway's functions
                if (CropStatus == CropStatus.Fallow)
                {
                    if (DaysSinceFallow < 60)
                    {
                        ResidueAmount = Math.Max(0, ResidueAmount - 15);
                    }
                    else if (DaysSinceFallow >= 60)
                    {
                        ResidueAmount = Math.Max(0, ResidueAmount - 3);
                    }
                    DaysSinceFallow += 1;

                }
                else
                {
                    ResidueAmount = Math.Max(0, ResidueAmount - 15);
                }
                //  Calculate proportion cover from residue weight
                //  using Sallaway type functions
                ResidueCover = InputModel.MaximumResidueCover * (1.0 - Math.Exp(-1.0 * ResidueAmount / 1000.0));
                if (ResidueCover < 0)
                {
                    ResidueCover = 0;
                }
                if (ResidueCover > 1)
                {
                    ResidueCover = 1;
                }
                //REVIEW
                //ResidueCoverPc = ResidueCover * 100.0;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }
    }
}
