using HowLeaky_SimulationEngine.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using HowLeaky_SimulationEngine.Tools;
using HowLeaky_SimulationEngine.Attributes;
using HowLeaky_SimulationEngine.Inputs;
using HowLeaky_SimulationEngine.Engine;
using HowLeaky_SimulationEngine.Errors;

namespace HowLeaky_SimulationEngine.Engine
{
    public class HowLeakyEngineModule_Irrigation : _CustomHowLeakyEngineModule
    {
        public HowLeakyEngineModule_Irrigation()
        {

        }


        public HowLeakyEngineModule_Irrigation(HowLeakyEngine sim, HowLeakyInputs_Irrigation inputs) : base(sim)
        {
            InputModel = inputs;
        }
        public HowLeakyInputs_Irrigation InputModel { get; set; }


        ////--------------------------------------------------------------------------
        ////Outputs
        ////--------------------------------------------------------------------------
        ////public IrrigationControllerOutput Output { get; set; } = new IrrigationControllerOutput();
        //public IrrigationControllerSummaryOutput Sum { get; set; } = new IrrigationControllerSummaryOutput();
        //public IrrigationControllerSO SO { get; set; } = new IrrigationControllerSO();

        //--------------------------------------------------------------------------
        //Internals
        //--------------------------------------------------------------------------
        [Internal] public int DaysSinceIrrigation { get; set; }                            // Number days since last irrigation
        [Internal] public bool FirstIrrigation { get; set; }                               // Switch to indicate it is the first irrigation 
        [Internal] public double IrrigationRunoffAmount { get; set; }                      // Amount of runoff (mm) from irrigation 
        [Internal] public double Overflow { get; set; }                                    // Amount of overflow (mm) from ringtank
        [Internal] public double IrrigationAmount { get; set; }                            // Amount of water required for irrigation
        [Internal] public double IrrigationAmountFromRingtank { get; set; }
        [Internal] public double StorageVolume { get; set; }
        [Internal] public double NumDaysOverflow { get; set; }
        [Internal] public double NumYearOverflow { get; set; }
        [Internal] public int LastOvertoppingYear { get; set; }



        //Reportbale Outputs
        [Output] public double IrrigationRunoff { get; set; }
        [Output] public double IrrigationApplied { get; set; }
        [Output] public double IrrigationInfiltration { get; set; }
        [Output] public double RingTankEvaporationLosses { get; set; }
        [Output] public double RingTankSeepageLosses { get; set; }
        [Output] public double RingTankOvertoppingLosses { get; set; }
        [Output] public double RingTankIrrigationLosses { get; set; }
        [Output] public double RingTankTotalLosses { get; set; }
        [Output] public double RingTankRunoffCaptureInflow { get; set; }
        [Output] public double RingTankRainfalInflow { get; set; }
        [Output] public double RingTankEffectiveAdditionalInflow { get; set; }
        [Output] public double RingTankTotalAdditionalInflow { get; set; }
        [Output] public double RingTankTotalInflow { get; set; }
        [Output] public double RingTankIneffectiveAdditionalInflow { get; set; }
        [Output] public double RingTankStorageVolume { get; set; }
        [Output] public double RingTankStorageLevel { get; set; }


        public override void Simulate()
        {
            try
            {
                if (CanIrrigateToday())
                {
                    double requiredAmount = GetRequiredIrrigationAmount();
                    if (requiredAmount > 0)
                    {
                        double availableAmount = GetAvailableAmountFromSupply(requiredAmount);
                        if (availableAmount > 0)
                        {
                            Irrigate(availableAmount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }



        public override void Initialise()
        {
            try
            {
                StorageVolume = 0;
                DaysSinceIrrigation = -1;
                //	AdditionalInflowIndex=0;
                IrrigationRunoffAmount = 0;
                FirstIrrigation = true;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        public void Irrigate(double available_amount)
        {
            try
            {
                RecordIrrigationEvent();
                IrrigationApplied = available_amount;
                IrrigationAmount = RemoveRunoffLosses(available_amount);
                Engine.SoilModule.Irrigation = IrrigationAmount;
                if (IrrigationAmount > 0)
                {
                    DistributeWaterThroughSoilLayers(IrrigationAmount);
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        public override void SetStartOfDayParameters()
        {
            try
            {
                IrrigationAmount = 0;
                IrrigationApplied = 0;
                IrrigationRunoffAmount = 0;
                if (DaysSinceIrrigation != -1)
                {
                    ++DaysSinceIrrigation;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        public void RecordIrrigationEvent()
        {
            try
            {
                DaysSinceIrrigation = 0;
                //Sim.UpdateManagementEventHistory(ManagementEvent.Irrigation, 0);
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }


        public bool CanIrrigateToday()
        {
            try
            {
                switch (InputModel.IrrigationFormat)
                {
                    case IrrigationFormat.AutomaticDuringGrowthStage: return CropWantsIrrigating() && WaitingPeriodExceeded();
                    case IrrigationFormat.AutomaticDuringWindow: return IsDateinIrrigationWindow()  && WaitingPeriodExceeded();//&& CropWantsIrrigating()
                    case IrrigationFormat.FromSequenceFile: return IsDateInSequence();
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

            return false;
        }

        public bool CropWantsIrrigating()
        {
            try
            {
                return (Engine.CurrentCrop != null &&
                        Engine.CurrentCrop.IsGrowing() &&
                        Engine.CurrentCrop.StillRequiresIrrigating());
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }


        public bool IsDateinIrrigationWindow()
        {
            return Engine.IsDateInWindow(Engine.TodaysDate, InputModel.IrrigWindowStartDate, InputModel.IrrigWindowEndDate);
        }


        public bool IsDateInSequence()
        {
            return InputModel.IrrigSequence.ContainsDate(Engine.TodaysDate);
        }

        public bool WaitingPeriodExceeded()
        {
            return !(DaysSinceIrrigation != -1 && DaysSinceIrrigation < InputModel.IrrigationBufferPeriod);
        }

        public double GetRequiredIrrigationAmount()
        {
            try
            {
                if (InputModel.IrrigationFormat != IrrigationFormat.FromSequenceFile)
                {
                    if (Engine.SoilModule.EffectiveRain <= 0.1)
                    {
                        if (InputModel.SWDToIrrigate > 0.0 && Engine.SoilModule.SoilWaterDeficit > InputModel.SWDToIrrigate)
                        {
                            switch (InputModel.TargetAmountOptions)
                            {
                                case TargetAmountOptions.FieldCapacity: return Engine.SoilModule.SoilWaterDeficit;
                                case TargetAmountOptions.Saturation: return Engine.SoilModule.Satd;
                                case TargetAmountOptions.FixedAmount: return InputModel.FixedIrrigationAmount;
                                case TargetAmountOptions.DULplus25Percent: return Engine.SoilModule.SoilWaterDeficit + (Engine.SoilModule.Satd - Engine.SoilModule.SoilWaterDeficit) * 0.25;
                                case TargetAmountOptions.DULplus50Percent: return Engine.SoilModule.SoilWaterDeficit + (Engine.SoilModule.Satd - Engine.SoilModule.SoilWaterDeficit) * 0.50;
                                case TargetAmountOptions.DULplus75Percent: return Engine.SoilModule.SoilWaterDeficit + (Engine.SoilModule.Satd - Engine.SoilModule.SoilWaterDeficit) * 0.75;
                                default: return 0;
                            }
                        }
                    }
                }
                else
                {
                    InputModel.TargetAmountOptions = TargetAmountOptions.FixedAmount;
                    return InputModel.IrrigSequence.ValueAtDate(Engine.TodaysDate);
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

            return 0;
        }

        public double GetAvailableAmountFromSupply(double amount)
        {
            try
            {
                if (InputModel.UseRingTank)
                {
                    double irrigatedareaM2 = InputModel.IrrigatedArea * 10000.0;
                    if (InputModel.IrrigDeliveryEfficiency > 0)
                    {
                        double deliveffic = (InputModel.IrrigDeliveryEfficiency / 100.0);
                        double amountReqFromRingtankM3 = amount / 1000.0 * irrigatedareaM2 / InputModel.IrrigDeliveryEfficiency / 100.0;//m^3      //divide by zero checked above

                        if (amountReqFromRingtankM3 < StorageVolume)
                        {
                            StorageVolume -= amountReqFromRingtankM3;
                            //our irrigation amount as calculated above does not change
                        }
                        else  //if we dont have enough water in the tank
                        {
                            double irrigationAvailableM3 = StorageVolume * InputModel.IrrigDeliveryEfficiency;
                            amount = irrigationAvailableM3 / irrigatedareaM2 * 1000.0;
                            StorageVolume = 0;
                        }
                    }
                    else
                    {
                        //amount = 0;
                        StorageVolume = 0;
                    }
                }
                return amount;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            //return 0;
        }

        public double RemoveRunoffLosses(double amount)
        {
            try
            {
                if (InputModel.IrrigRunoffOptions == (int)IrrigationRunoffOptions.Proportional)
                {
                    double factor;
                    if (FirstIrrigation)
                    {
                        factor = InputModel.IrrigRunoffProportion1 / 100.0;
                    }
                    else
                    {
                        factor = InputModel.IrrigRunoffProportion2 / 100.0;
                    }
                    IrrigationRunoffAmount = factor * amount;

                    amount = amount - IrrigationRunoffAmount;

                    FirstIrrigation = false; //assign this to true when planting.
                }
                else if (InputModel.IrrigRunoffOptions == (int)IrrigationRunoffOptions.FromSequenceFile)
                {
                    IrrigationRunoffAmount = InputModel.IrrigRunoffSequence.ValueAtDate(Engine.TodaysDate);
                    if (IrrigationRunoffAmount < amount)
                        amount = amount - IrrigationRunoffAmount;
                    else if (IrrigationRunoffAmount >= amount)
                    {
                        IrrigationRunoffAmount = amount;
                        amount = 0;
                    }
                }
                if (amount < 0)
                {
                    amount = 0;
                }
                return amount;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            //return 0;

        }

        public void DistributeWaterThroughSoilLayers(double amount)
        {
            try
            {
                double targetlayeramount = 0;
                int layercount = Engine.SoilModule.LayerCount;
                for (int i = 0; i < layercount; ++i)
                {
                    double dul = Engine.SoilModule.DrainUpperLimitRelWP[i];
                    double sat = Engine.SoilModule.SaturationLimitRelWP[i];
                    switch (InputModel.TargetAmountOptions)
                    {
                        case TargetAmountOptions.FieldCapacity: targetlayeramount = dul; break;
                        case TargetAmountOptions.Saturation: targetlayeramount = sat; break;
                        case TargetAmountOptions.FixedAmount: targetlayeramount = sat; break;
                        case TargetAmountOptions.DULplus25Percent: targetlayeramount = dul + 0.25 * (sat - dul); break;
                        case TargetAmountOptions.DULplus50Percent: targetlayeramount = dul + 0.50 * (sat - dul); break;
                        case TargetAmountOptions.DULplus75Percent: targetlayeramount = dul + 0.75 * (sat - dul); break;
                    }
                    double layerdef = targetlayeramount - Engine.SoilModule.SoilWaterRelWP[i];
                    if (amount > layerdef)
                    {
                        Engine.SoilModule.SoilWaterRelWP[i] = targetlayeramount;
                    }
                    else
                    {
                        Engine.SoilModule.SoilWaterRelWP[i] = Engine.SoilModule.SoilWaterRelWP[i] + amount;
                    }
                    amount = amount - layerdef;
                    if (amount < 0)
                    {
                        i = layercount;
                    }
                }
                Engine.SoilModule.SoilWaterDeficit = 0;
                for (int i = 0; i < layercount; ++i)
                {
                    Engine.SoilModule.SoilWaterDeficit = Engine.SoilModule.SoilWaterDeficit + (Engine.SoilModule.DrainUpperLimitRelWP[i] - Engine.SoilModule.SoilWaterRelWP[i]);
                }
                Engine.SoilModule.Sse1 = Math.Max(0, Engine.SoilModule.Sse1 - Engine.SoilModule.SoilWaterDeficit);
                if (amount > 0)
                {
                    Engine.SoilModule.EffectiveRain += amount;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        public bool PondingExists()
        {
            try
            {
                if (Engine.CurrentCrop != null && Engine.CurrentCrop.IsGrowing())
                {
                    return (CansimulateIrrigation() && InputModel.UsePonding && Engine.CurrentCrop.StillRequiresIrrigating());
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

            return false;
        }

        public void ModelRingTank()
        {
            try
            {
                // NOTE- WE HAVE ALREADY IRRIGATED BY THIS STAGE
                if (CansimulateIrrigation() && InputModel.UseRingTank)
                {
                    double in_RingTankArea_ha = InputModel.RingTankArea * 10000.0;
                    if (InputModel.ResetRingTank && InputModel.ResetRingTankDate.MatchesDate(Engine.TodaysDate))
                        ResetRingTank();
                    else
                        SimulateDailyRingTankWaterBalance();
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
        public void ResetRingTank()
        {
            try
            {
                double maxvolume = InputModel.RingTankDepth * InputModel.RingTankArea; //m^3
                double oldvolume = StorageVolume;
                StorageVolume = maxvolume * InputModel.CapactityAtReset / 100.0;
                RingTankIrrigationLosses = 0;
                RingTankEvaporationLosses = 0;
                RingTankSeepageLosses = 0;
                RingTankOvertoppingLosses = 0;
                RingTankTotalLosses = 0;
                RingTankRunoffCaptureInflow = 0;
                RingTankRainfalInflow = 0;
                RingTankEffectiveAdditionalInflow = 0;
                RingTankTotalAdditionalInflow = 0;
                RingTankTotalInflow = 0;
                RingTankIneffectiveAdditionalInflow = 0;
                RingTankStorageVolume = StorageVolume / 1000.0;

                if (!MathTools.DoublesAreEqual(InputModel.RingTankDepth * InputModel.RingTankArea, 0))
                {
                    RingTankStorageLevel = StorageVolume / (InputModel.RingTankDepth * InputModel.RingTankArea) * 100.0;
                }
                else
                {
                    RingTankStorageLevel = 0;
                    MathTools.LogDivideByZeroError("ModelRingTank", "in_RingTankDepth_m*in_RingTankArea_ha", "out_RingTankStorageLevel_pc");
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        public void SimulateDailyRingTankWaterBalance()
        {
            try
            {
                //inflows
                double runoffCaptureInflowM3 = CalcRunoffCaptureInflow();
                double rainfallInflowM3 = Engine.ClimateModule.Rain / 1000.0 * InputModel.RingTankArea;
                double additionalInflowM3 = GetAdditionalTankInflow();
                double totalInflowM3 = runoffCaptureInflowM3 + rainfallInflowM3 + additionalInflowM3;

                //losses
                double seepageLossesM3 = InputModel.RingTankSeepageRate / 1000.0 * InputModel.RingTankArea;
                double evaporationLossesM3 = Engine.ClimateModule.PanEvap * InputModel.RingTankEvapCoefficient / 1000.0 * InputModel.RingTankArea;
                double seepagePlusEvapLossesM3 = seepageLossesM3 + evaporationLossesM3;

                if (seepagePlusEvapLossesM3 > StorageVolume)
                {
                    seepagePlusEvapLossesM3 = StorageVolume;
                    if (seepageLossesM3 < StorageVolume / 2.0)
                    {
                        evaporationLossesM3 = StorageVolume - seepageLossesM3;
                    }
                    else if (evaporationLossesM3 < StorageVolume / 2.0)
                    {
                        seepageLossesM3 = StorageVolume - evaporationLossesM3;
                    }
                    else
                    {
                        seepageLossesM3 = StorageVolume / 2.0;
                        evaporationLossesM3 = StorageVolume / 2.0;
                    }
                }
                //NOTE - Irrigation losses have already been extracted before we call this routine. Irrigation effectively
                // begins at the start of the day.

                //storage
                double potentialStorageVolumeM3 = CalcPotentialStorageVolume(totalInflowM3, seepagePlusEvapLossesM3);
                StorageVolume = CalcActualStorageVolume(totalInflowM3, seepagePlusEvapLossesM3, potentialStorageVolumeM3);

                //output variables
                RingTankIrrigationLosses = (IrrigationApplied / 1000.0 * InputModel.IrrigatedArea * 10000.0) / 1000.0 / (InputModel.IrrigDeliveryEfficiency / 100.0);
                RingTankEvaporationLosses = evaporationLossesM3 / 1000.0;
                RingTankSeepageLosses = seepageLossesM3 / 1000.0;
                RingTankOvertoppingLosses = CalcOvertoppingAmount(potentialStorageVolumeM3, StorageVolume);
                RingTankTotalLosses = RingTankIrrigationLosses + RingTankEvaporationLosses + RingTankSeepageLosses + RingTankOvertoppingLosses;
                RingTankRunoffCaptureInflow = runoffCaptureInflowM3 / 1000.0;
                RingTankRainfalInflow = rainfallInflowM3 / 1000.0;
                RingTankTotalAdditionalInflow = additionalInflowM3 / 1000.0;
                RingTankEffectiveAdditionalInflow = CalcEffectiveAdditionalInflow(RingTankOvertoppingLosses, additionalInflowM3);
                RingTankTotalInflow = RingTankRainfalInflow + RingTankRunoffCaptureInflow + additionalInflowM3 / 1000.0;
                RingTankIneffectiveAdditionalInflow = additionalInflowM3 / 1000.0 - RingTankEffectiveAdditionalInflow;
                RingTankStorageVolume = StorageVolume / 1000.0;
                RingTankStorageLevel = CalcStorageLevel(StorageVolume);
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        public double CalcOvertoppingAmount(double potentialM3, double actualM3)
        {
            try
            {
                if (potentialM3 > actualM3)
                {
                    return (potentialM3 - actualM3) / 1000.0;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }


        public double CalcEffectiveAdditionalInflow(double overtopping_ML, double additionalinflow_m3)
        {
            try
            {
                if (MathTools.DoublesAreEqual(overtopping_ML, 0))
                {
                    return additionalinflow_m3 / 1000.0; //convert to ML
                }
                double value = additionalinflow_m3 / 1000.0 - overtopping_ML;
                return (value > 0 ? value : 0);
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            //return 0;
        }


        public double CalcStorageLevel(double storage_volume_m3)
        {
            try
            {
                double level;
                double capacity_m3 = InputModel.RingTankDepth * InputModel.RingTankArea * 10000.0;
                if (!MathTools.DoublesAreEqual(capacity_m3, 0))
                {
                    level = storage_volume_m3 / capacity_m3 * 100.0;
                }
                else
                {
                    level = 0;
                    MathTools.LogDivideByZeroError("ModelRingTank", "in_RingTankDepth_m*in_RingTankArea_ha", "out_RingTankStorageLevel_pc");
                }
                return level;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            //return 0;
        }

        public double CalcPotentialStorageVolume(double inputs_m3, double outputs_m3)
        {
            try
            {
                double storagevolume = StorageVolume + inputs_m3 - outputs_m3;
                return (storagevolume > 0 ? storagevolume : 0);
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        public double CalcActualStorageVolume(double inputs_m3, double outputs_m3, double potentialvolume)
        {
            try
            {
                double capacity_m3 = InputModel.RingTankDepth * InputModel.RingTankArea * 10000.0; //m^3
                return (potentialvolume < capacity_m3 ? potentialvolume : capacity_m3);
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        public double CalcRunoffCaptureInflow()
        {
            try
            {
                double runoffinflow = Engine.SoilModule.Runoff / 1000.0 * InputModel.CatchmentArea * 10000;  //m^3
                double runoffcapturerate_m3 = InputModel.RunoffCaptureRate * 1000.0;
                return (runoffinflow < runoffcapturerate_m3 ? runoffinflow : runoffcapturerate_m3);
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        public double GetAdditionalTankInflow()
        {
            try
            {
                if (InputModel.AdditionalInflowFormat == (int)RingTankAdditionalInflowFormat.Constant)
                {
                    return InputModel.AdditionalInflow * 1000.0;         //converting to m3
                }
                else
                {
                    return InputModel.AdditionalInflowSequence.ValueAtDate(Engine.TodaysDate) * 1000.0;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            //return 0;

        }

        //public void UpdateRingtankWaterBalance()
        //{
        //    try
        //    {
        //        if (InputModel.UseRingTank)
        //        {
        //            //Sum.IrrigationLosses += RingTankIrrigationLosses;
        //            //Sum.EvaporationLosses += RingTankEvaporationLosses;
        //            //Sum.SeepageLosses += RingTankSeepageLosses;
        //            //Sum.OvertoppingLosses += RingTankOvertoppingLosses;
        //            //Sum.RunoffCaptureInflow += RingTankRunoffCaptureInflow;
        //            //Sum.RainfallInflow += RingTankRainfalInflow;
        //            //Sum.EffectiveAdditionalnflow += RingTankEffectiveAdditionalInflow;
        //            //Sum.TotalAdditionalInflow += RingTankTotalAdditionalInflow;
        //            //Sum.StorageLevel += RingTankStorageLevel;
        //            //if (RingTankOvertoppingLosses > 0)
        //            //{
        //            //    ++NumDaysOverflow;
        //            //    if (Sim.Year != LastOvertoppingYear)
        //            //    {
        //            //        ++NumYearOverflow;
        //            //        LastOvertoppingYear = Sim.Year;
        //            //    }
        //            //}
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //       throw new Exception("Exception in IRRIGATION:UpdateRingtankWaterBalance",ex);
        //    }
        //}

        public bool ConsiderCoverEffects()
        {
            try
            {
                return (InputModel.IrrigRunoffOptions > 0 &&
                        InputModel.IrrigCoverEffects > 0 &&
                        IrrigationAmount > 0);
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        public double GetCoverEffect(double crop_cover, double total_residue_cover)
        {
            try
            {
                double cover = 0;
                if (InputModel.IrrigCoverEffects == 0)
                {
                    cover = Math.Min(100.0, (crop_cover + total_residue_cover * (1 - crop_cover)) * 100.0);
                }
                else if (InputModel.IrrigCoverEffects == 1)
                {
                    cover = Math.Min(100.0, (0 + total_residue_cover * (1 - 0)) * 100.0);
                }
                else if (InputModel.IrrigCoverEffects == 2)
                {
                    cover = 0;
                }
                return cover;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            //return 0;
        }

        public bool CansimulateIrrigation()
        {
            return true;

        }

        public void CalculateMonthlyOutputs()
        {

        }

        public void CalculateSummaryOutputs()
        {
            //if (InputModel.UseRingTank)
            //{
            //    double simyears = Sim.NumberOfDaysInSimulation / 365.0;
            //    SO.RingTankIrrigationLosses = MathTools.Divide(Sum.IrrigationLosses, simyears);
            //    SO.RingTankIrrigationLossesDelivered = SO.RingTankIrrigationLosses * InputModel.IrrigDeliveryEfficiency / 100.0;
            //    SO.RingTankEvaporationLosses = MathTools.Divide(Sum.EvaporationLosses, simyears);
            //    SO.RingTankSeepageLosses = MathTools.Divide(Sum.SeepageLosses, simyears);
            //    SO.RingTankOvertoppingLosses = MathTools.Divide(Sum.OvertoppingLosses, simyears);
            //    SO.RingTankRunoffCaptureInflow = MathTools.Divide(Sum.RunoffCaptureInflow, simyears);
            //    SO.RingTankRainfallInflow = MathTools.Divide(Sum.RainfallInflow, simyears);
            //    SO.RingTankEffectiveAdditionalInflow = MathTools.Divide(Sum.EffectiveAdditionalnflow, simyears);
            //    SO.RingTankAdditionalInflow = MathTools.Divide(Sum.TotalAdditionalInflow, simyears);
            //    SO.RingTanksStorageLevel = MathTools.Divide(Sum.StorageLevel, Sim.NumberOfDaysInSimulation);
            //    SO.RingTankPropDaysOverflow = MathTools.Divide(NumDaysOverflow, Sim.NumberOfDaysInSimulation) * 100.0;
            //    SO.RingTankPropYearsOverflow = MathTools.Divide(NumYearOverflow, simyears) * 100.0;
            //}
        }
    }
}
