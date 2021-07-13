using HowLeaky_SimulationEngine.Attributes;
using HowLeaky_SimulationEngine.Enums;
using HowLeaky_SimulationEngine.Errors;
using HowLeaky_SimulationEngine.Inputs;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HowLeaky_SimulationEngine.Engine
{
    public class HowLeakyEngineModule_Pesticide : _CustomHowLeakyEngineModule
    {
        public HowLeakyEngineModule_Pesticide(HowLeakyEngine sim, HowLeakyInputs_Pesticide inputs) : base(sim)
        {
            try
            {
                Name = inputs.Name;
                InputModel = inputs;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        public HowLeakyEngineModule_Pesticide()
        {
        }

        public HowLeakyInputs_Pesticide InputModel { get; set; }

        internal string GetName()
        {
            return InputModel.GetName();
        }

        [Internal] public int DaysSinceApplication { get; set; }
        [Internal] public int PestApplicCount { get; set; }
        [Internal] public int ApplicationIndex { get; set; }
        [Internal] public double ProductRateApplied { get; set; }
        [Internal] public double ConcSoilAfterLeach { get; set; }
        [Internal] public double LastPestInput { get; set; }



        //Reportable Outputs
        [Output] public double AppliedPestOnVeg { get; set; }
        [Output] public double AppliedPestOnStubble { get; set; }
        [Output] public double AppliedPestOnSoil { get; set; }
        [Output] public double PestOnVeg { get; set; }
        [Output] public double PestOnStubble { get; set; }
        [Output] public double PestInSoil { get; set; }
        [Output] public double PestSoilConc { get; set; }
        [Output] public double PestSedPhaseConc { get; set; }
        [Output] public double PestWaterPhaseConc { get; set; }
        [Output] public double PestRunoffConc { get; set; }
        [Output] public double PestLostInRunoffWater { get; set; }
        [Output] public double PestLostInRunoffSediment { get; set; }
        [Output] public double TotalPestLostInRunoff { get; set; }
        [Output] public double PestLostInLeaching { get; set; }
        [Output] public double PestLossesPercentOfInput { get; set; }
        [Output] public double ApplicationCount { get; set; }
        [Output] public double ProductApplication { get; set; }
        [Output] public double AvgBoundPestConcInRunoff { get; set; }
        [Output] public double AvgUnboundPestConcInRunoff { get; set; }

        

        [Output] public double AvgCombinedPestConcInRunoff { get; set; }
        [Output] public double AvgPestLoadWater { get; set; }
        [Output] public double AvgPestLoadSediment { get; set; }
        [Output] public double AvgTotalPestLoad { get; set; }
        [Output] public double ApplicationLossRatio { get; set; }
        [Output] public int DaysGreaterCrit1 { get; set; }
        [Output] public int DaysGreaterCrit2 { get; set; }
        [Output] public int DaysGreaterCrit3 { get; set; }
        [Output] public int DaysGreaterCrit4 { get; set; }
        [Output] public double PestEMCL { get; set; }

        public int PesticideIndex
        {

            get
            {
                return Engine.PesticideModules.IndexOf(this);
            }
        }

        public override void Simulate()
        {
            try
            {
                if (PestApplicCount > 0)
                {
                    ++DaysSinceApplication;
                }
                //check inputs 
                ApplyAnyNewPesticides();

                //calculate pest store
                CalculateDegradingPestOnVeg();
                CalculateDegradingPestOnStubble();
                CalculateDegradingPestInSoil();

                //generate output values
                CalculatePesticideRunoffConcentrations();
                CalculatePesticideLosses();
                CalculatePesticideDaysAboveCritical();
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }


        public void ApplyAnyNewPesticides()
        {
            try
            {
                AppliedPestOnVeg = 0;
                AppliedPestOnStubble = 0;
                AppliedPestOnSoil = 0;

                ResetPesticideInputs();
                if (InputModel.ApplicationTiming == (int)EPestApplicationTiming.FixedDate)
                {
                    if (Engine.TodaysDate.Day == InputModel.ApplicationDate.Day && Engine.TodaysDate.Month == InputModel.ApplicationDate.Month)
                    {
                        ProductRateApplied = InputModel.ProductRate;
                    }
                }
                else if (InputModel.ApplicationTiming == (int)EPestApplicationTiming.FromSequenceFile)
                {
                    //int index = DateUtilities.isDateInSequenceList(Sim.Today, InputModel.PestApplicationTiming.PesticideDatesAndRates);
                    var item = InputModel.PestApplicationDateList.Dates.FirstOrDefault(x => x.DateInt == Engine.TodaysDate.DateInt);
                    if (item != null)
                    {
                        var index = InputModel.PestApplicationDateList.Dates.IndexOf(item);
                        ProductRateApplied = InputModel.PestApplicationDateList.Values[index];
                    }
                }
                else
                {
                    var crop = Engine.CurrentCrop;
                    if (crop != null)
                    {
                        if (crop.CropStatus != CropStatus.Fallow)
                        {
                            if (MathTools.DoublesAreEqual(crop.HeatUnitIndex, 0))
                            {
                                ApplicationIndex = 0;
                            }
                            if (InputModel.ApplicationTiming == (int)EPestApplicationTiming.GDDCrop1 && crop == Engine.GetCrop(0))
                            {
                                CheckApplicationBasedOnGDD(crop);
                            }
                            else if (InputModel.ApplicationTiming == (int)EPestApplicationTiming.GDDCrop2 && crop == Engine.GetCrop(1))
                            {
                                CheckApplicationBasedOnGDD(crop);
                            }
                            else if (InputModel.ApplicationTiming == (int)EPestApplicationTiming.GDDCrop3 && crop == Engine.GetCrop(2))
                            {
                                CheckApplicationBasedOnGDD(crop);
                            }
                            else if (InputModel.ApplicationTiming == (int)EPestApplicationTiming.DASCrop1 && crop == Engine.GetCrop(0))
                            {
                                CheckApplicationBasedOnDAS(crop);
                            }
                            else if (InputModel.ApplicationTiming == (int)EPestApplicationTiming.DASCrop2 && crop == Engine.GetCrop(1))
                            {
                                CheckApplicationBasedOnDAS(crop);
                            }
                            else if (InputModel.ApplicationTiming == (int)EPestApplicationTiming.DASCrop3 && crop == Engine.GetCrop(2))
                            {
                                CheckApplicationBasedOnDAS(crop);
                            }
                        }
                        else if (crop.CropStatus == CropStatus.Fallow && InputModel.ApplicationTiming == (int)EPestApplicationTiming.DaysSinceFallow)
                        {
                            CheckApplicationBasedOnDAH();
                        }
                    }
                }
                if (ProductRateApplied > 0)
                    ApplyPesticide();
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        public void CheckApplicationBasedOnGDD(_CustomHowLeakyEngine_VegModule crop)
        {
            try
            {
                if (ApplicationIndex == 0 && crop.HeatUnits >= InputModel.TriggerGGDFirst)
                {
                    ProductRateApplied = InputModel.ProductRate;
                    ++ApplicationIndex;
                }
                else if (ApplicationIndex > 0 && crop.HeatUnits >= InputModel.TriggerGGDFirst + InputModel.TriggerGGDSubsequent * ApplicationIndex)
                {
                    ProductRateApplied = InputModel.SubsequentProductRate;
                    ++ApplicationIndex;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }


        public void CheckApplicationBasedOnDAS(_CustomHowLeakyEngine_VegModule crop)
        {
            try
            {
                if (ApplicationIndex == 0 && crop.DaysSincePlanting >= InputModel.TriggerDaysFirst)
                {
                    ProductRateApplied = InputModel.ProductRate;
                    ++ApplicationIndex;
                }
                else if (ApplicationIndex > 0 && crop.DaysSincePlanting >= InputModel.TriggerDaysFirst + InputModel.TriggerDaysSubsequent * ApplicationIndex)
                {
                    ProductRateApplied = InputModel.SubsequentProductRate;
                    ++ApplicationIndex;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }


        public void CheckApplicationBasedOnDAH()
        {
            try
            {
                int days_since_harvest = Engine.DaysSinceHarvest;
                if (days_since_harvest == 0) ApplicationIndex = 0;
                if (ApplicationIndex == 0 && days_since_harvest >= InputModel.TriggerDaysFirst)
                {
                    ProductRateApplied = InputModel.ProductRate;
                    ++ApplicationIndex;
                }
                else if (ApplicationIndex > 0 && days_since_harvest >= InputModel.TriggerDaysFirst + InputModel.TriggerDaysSubsequent * ApplicationIndex)
                {
                    ProductRateApplied = InputModel.SubsequentProductRate;
                    ++ApplicationIndex;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        public void ResetPesticideInputs()
        {
            ProductRateApplied = 0;
        }


        public void ApplyPesticide()
        {
            try
            {
                //   Sim.UpdateManagementEventHistory(ManagementEvent.Pesticide, PesticideIndex);
                DaysSinceApplication = 0;
                EPestApplicationPosition pos = (EPestApplicationPosition)InputModel.ApplicationPosition;
                double pest_application = InputModel.ConcActiveIngred * ProductRateApplied * InputModel.PestEfficiency / 100.0 * InputModel.BandSpraying / 100.0;
                LastPestInput = pest_application;

                if (pos == EPestApplicationPosition.ApplyToVegetationLayer)
                {
                    AppliedPestOnVeg = pest_application * Engine.SoilModule.CropCover;
                }
                else
                {
                    AppliedPestOnVeg = 0;
                }
                if (pos == EPestApplicationPosition.ApplyToVegetationLayer || pos == EPestApplicationPosition.ApplyToStubbleLayer)
                {
                    double stubble_cover = (1 - Engine.SoilModule.CropCover) * Engine.SoilModule.TotalResidueCover;
                    AppliedPestOnStubble = pest_application * stubble_cover;
                }
                else
                {
                    AppliedPestOnStubble = 0;
                }


                AppliedPestOnSoil = pest_application - AppliedPestOnVeg - AppliedPestOnStubble;
                ++PestApplicCount;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateDegradingPestOnVeg()
        {
            try
            {
                double halfLifeVegAdjusted = 0;
                double universalGasConstant = 8.314472;
                double refAirTempVegKelvin = InputModel.RefTempHalfLifeVeg + 273.15;
                double airTempKelvin = ((Engine.ClimateModule.MaxTemp + Engine.ClimateModule.MinTemp) / 2.0) + 273.15;
                if (!MathTools.DoublesAreEqual(airTempKelvin, 0) && !MathTools.DoublesAreEqual(refAirTempVegKelvin, 0))
                    halfLifeVegAdjusted = InputModel.HalfLifeVeg * Math.Exp((InputModel.DegradationActivationEnergy / universalGasConstant) * (1.0 / airTempKelvin - 1.0 / refAirTempVegKelvin));
                else
                {
                    MathTools.LogDivideByZeroError("CalculateDegradingPestOnVeg", "AirTemperature_kelvin!=0 or Ref_AirTemperatureVeg_kelvin", "HalfLifeVeg_adjusted");
                }
                double vegdegrate;
                if (!MathTools.DoublesAreEqual(halfLifeVegAdjusted, 0))
                {
                    vegdegrate = Math.Exp(-0.693 / halfLifeVegAdjusted);
                }
                else
                {
                    vegdegrate = 0;
                    MathTools.LogDivideByZeroError("CalculateDegradingPestOnVeg", "HalfLifeVeg_adjusted", "vegdegrate");
                }
                if (Engine.ClimateModule.YesterdaysRain < 5.0)
                {
                    PestOnVeg = PestOnVeg * vegdegrate + AppliedPestOnVeg;
                    if (Engine.ClimateModule.Rain >= 5) //rain over 5mm will wash part of pest of veg
                    {
                        PestOnVeg = PestOnVeg * (1 - InputModel.CoverWashoffFraction);
                    }
                }
                else
                {
                    PestOnVeg = 0;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }


        public void CalculateDegradingPestOnStubble()
        {
            try
            {
                double halfLifeStubbleAdjusted = 0;
                double universalGasConstant = 8.314472;
                double refAirTempStubbleKelvin = InputModel.RefTempHalfLifeStubble + 273.15;
                double airTempKelvin = ((Engine.ClimateModule.MaxTemp + Engine.ClimateModule.MinTemp) / 2.0) + 273.15;
                if (!MathTools.DoublesAreEqual(airTempKelvin, 0) && !MathTools.DoublesAreEqual(refAirTempStubbleKelvin, 0))
                {
                    halfLifeStubbleAdjusted = InputModel.HalfLifeStubble * Math.Exp((InputModel.DegradationActivationEnergy / universalGasConstant) * (1.0 / airTempKelvin - 1.0 / refAirTempStubbleKelvin));
                }
                else
                {
                    MathTools.LogDivideByZeroError("CalculateDegradingPestOnStubble", "AirTemperature_kelvin or Ref_AirTemperatureStubble_kelvin", "HalfLifeStubble_adjusted");
                }

                double stubdegrate;
                if (!MathTools.DoublesAreEqual(halfLifeStubbleAdjusted, 0))
                {
                    stubdegrate = Math.Exp(-0.693 / halfLifeStubbleAdjusted);
                }
                else
                {
                    stubdegrate = 0;
                    MathTools.LogDivideByZeroError("CalculateDegradingPestOnStubble", "HalfLifeStubble_adjusted", "stubdegrate");
                }
                if (Engine.ClimateModule.YesterdaysRain < 5.0)
                {
                    PestOnStubble = PestOnStubble * stubdegrate + AppliedPestOnStubble;
                    if (Engine.ClimateModule.Rain >= 5) //rain over 5mm will wash part of pest of stubble
                    {
                        PestOnStubble = PestOnStubble * (1 - InputModel.CoverWashoffFraction);
                    }
                }
                else
                {
                    PestOnStubble = 0;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }


        public void CalculateDegradingPestInSoil()
        {
            try
            {
                double halflifesoil_adjusted = 0;
                double universalgasconstant = 8.314472;
                double ref_airtempsoil_kelvin = InputModel.RefTempHalfLifeSoil + 273.15;
                double airtemp_kelvin = ((Engine.ClimateModule.MaxTemp + Engine.ClimateModule.MinTemp) / 2.0) + 273.15;
                if (!MathTools.DoublesAreEqual(airtemp_kelvin, 0) && !MathTools.DoublesAreEqual(ref_airtempsoil_kelvin, 0))
                {
                    halflifesoil_adjusted = InputModel.HalfLifeSoil * Math.Exp((InputModel.DegradationActivationEnergy / universalgasconstant) * (1.0 / airtemp_kelvin - 1.0 / ref_airtempsoil_kelvin));
                }
                else
                {
                    MathTools.LogDivideByZeroError("CalculateDegradingPestInSoil", "AirTemperature_kelvin or Ref_AirTemperatureSoil_kelvin", "HalfLifeSoil_adjusted");
                }
                double denom;
                double soildegrate;
                if (halflifesoil_adjusted > 0)
                {
                    soildegrate = Math.Exp(-0.693 / halflifesoil_adjusted);
                }
                else
                {
                    soildegrate = 0;
                    MathTools.LogDivideByZeroError("CalculateDegradingPestInSoil", "HalfLifeSoil_adjusted", "soildegrate");
                }

                PestInSoil = PestInSoil * soildegrate + AppliedPestOnSoil - PestLostInLeaching - TotalPestLostInRunoff;
                if (Engine.ClimateModule.Rain >= 5.0)
                {
                    PestInSoil = PestInSoil + (PestOnStubble + PestOnVeg) * InputModel.CoverWashoffFraction;
                }


                denom = (Engine.SoilModule.InputModel.BulkDensity[0] * InputModel.MixLayerThickness * 10);
                if (!MathTools.DoublesAreEqual(denom, 0))
                {
                    PestSoilConc = PestInSoil / denom;
                }
                else
                {
                    PestSoilConc = 0;
                    MathTools.LogDivideByZeroError("CalculateDegradingPestInSoil", "sim.in_BulkDensity_g_per_cm3[0]*in_MixLayerThickness_mm*10", "out_PestSoilConc_mg_per_kg");
                }


                double porosity = 1 - Engine.SoilModule.InputModel.BulkDensity[0] / 2.65;

                //calculate the denominator of the PestConcInSoilAfterLeaching Equation - need to test for denom=0
                denom = InputModel.MixLayerThickness * (InputModel.SorptionCoefficient * Engine.SoilModule.InputModel.BulkDensity[0] + porosity);

                double availwaterstorageinmixing;
                if (!MathTools.DoublesAreEqual(Engine.SoilModule.Depth[1], 0))
                {
                    availwaterstorageinmixing = (Engine.SoilModule.DrainUpperLimitRelWP[0] - Engine.SoilModule.SoilWaterRelWP[0]) * InputModel.MixLayerThickness / Engine.SoilModule.Depth[1];
                }
                else
                {
                    availwaterstorageinmixing = 0;
                    MathTools.LogDivideByZeroError("CalculateDegradingPestInSoil", "sim.depth[1]", "availwaterstorageinmixing");
                }
                if (!MathTools.DoublesAreEqual(denom, 0))
                {
                    double infiltration = Engine.ClimateModule.Rain - Engine.SoilModule.Runoff - availwaterstorageinmixing;
                    if (infiltration < 0)
                    {
                        infiltration = 0;
                    }
                    ConcSoilAfterLeach = PestSoilConc * Math.Exp(-infiltration / (denom));
                }
                else
                {
                    ConcSoilAfterLeach = 0;
                    MathTools.LogDivideByZeroError("CalculateDegradingPestInSoil", "sim.in_BulkDensity_g_per_cm3[0]*in_MixLayerThickness_mm*10", "conc_in_soil_after_leach_mg_per_kg");
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }


        }


        public void CalculatePesticideRunoffConcentrations()
        {
            try
            {
                double sorpBYext = InputModel.SorptionCoefficient * InputModel.ExtractCoefficient;
                double denom1 = (1 + sorpBYext);

                if (Engine.SoilModule.Runoff > 0 && PestSoilConc > 0 && !MathTools.DoublesAreEqual(denom1, 0))
                {
                    PestWaterPhaseConc = ConcSoilAfterLeach * InputModel.ExtractCoefficient / denom1 * 1000.0;
                    PestSedPhaseConc = ConcSoilAfterLeach * sorpBYext / denom1;
                    PestRunoffConc = PestWaterPhaseConc + PestSedPhaseConc * Engine.SoilModule.SedimentConc;
                }
                else
                {
                    if (MathTools.DoublesAreEqual(1 + sorpBYext, 0))
                    {
                        MathTools.LogDivideByZeroError("CalculatePesticideRunoffConcentrations", "1+sorpBYext", "3 x pest-concs");
                    }
                    PestWaterPhaseConc = 0;
                    PestSedPhaseConc = 0;
                    PestRunoffConc = 0;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }


        public void CalculatePesticideLosses()
        {
            try
            {
                if (Engine.SoilModule.Runoff > 0)
                {
                    PestLostInRunoffWater = PestWaterPhaseConc * Engine.SoilModule.Runoff * 0.01;
                    PestLostInRunoffSediment = PestSedPhaseConc * Engine.SoilModule.HillSlopeErosion * Engine.SoilModule.InputModel.SedDelivRatio;// spreadsheet uses runoff instead of erosion*SelDelivRatio
                    TotalPestLostInRunoff = PestLostInRunoffWater + PestLostInRunoffSediment;

                }
                else
                {
                    PestLostInRunoffWater = 0;
                    PestLostInRunoffSediment = 0;
                    TotalPestLostInRunoff = 0;
                }
                PestLostInLeaching = (PestSoilConc - ConcSoilAfterLeach) * Engine.SoilModule.InputModel.BulkDensity[0] * InputModel.MixLayerThickness / 10.0;
                if (PestLostInLeaching < 0)
                {
                    PestLostInLeaching = 0;
                }
                if (!MathTools.DoublesAreEqual(LastPestInput, 0))
                {
                    PestLossesPercentOfInput = (TotalPestLostInRunoff + PestLostInLeaching) / LastPestInput * 100.0;
                }
                else
                {
                    PestLossesPercentOfInput = 0;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }


        public void CalculatePesticideDaysAboveCritical()
        {
            try
            {
                if (PestRunoffConc > InputModel.CritPestConc * 1)
                {
                    DaysGreaterCrit1++;
                }
                if (PestRunoffConc > InputModel.CritPestConc * 0.5)
                {
                    DaysGreaterCrit2++;
                }
                if (PestRunoffConc > InputModel.CritPestConc * 2)
                {
                    DaysGreaterCrit3++;
                }
                if (PestRunoffConc > InputModel.CritPestConc * 10)
                {
                    DaysGreaterCrit4++;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        public int GetPesticideIndex()
        {
            return Engine.GetPesticideIndex(this);
        }
    }
}
