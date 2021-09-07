using HowLeaky_SimulationEngine.Errors;
using HowLeaky_SimulationEngine.Outputs;
using HowLeaky_SimulationEngine.Outputs.Definitions;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Engine
{
    public partial class HowLeakyEngine
    {
        public Dictionary<string, OutputAttributes> RemapDict { get; private set; }
        public string OutputsCSV { get; set; }
        public string SimulationName { get; set; }
        public HowLeakyEngineModule_Climate ClimateModule { get; set; } = null;
        public HowLeakyEngineModule_Soil SoilModule { get; set; } = null;
        public HowLeakyEngineModule_Irrigation IrrigationModule { get; set; } = null;
        public HowLeakyEngineModule_Phosphorus PhosphorusModule { get; set; } = null;
        public HowLeakyEngineModule_Nitrate NitrateModule { get; set; } = null;
        public HowLeakyEngineModule_Solutes SolutesModule { get; set; } = null;

        public List<_CustomHowLeakyEngine_VegModule> VegetationModules { get; set; } = null;
        public List<_CustomHowLeakyEngine_VegModule> SortedVegetationModules { get; set; }
        public List<HowLeakyEngineModule_Pesticide> PesticideModules { get; set; } = null;
        public List<HowLeakyEngineModule_Tillage> TillageModules { get; set; } = null;
        public _CustomHowLeakyEngine_VegModule VegetationModule(int? index) { return VegetationModules[(int)index]; }
        public HowLeakyEngineModule_Tillage TillageModule(int? index) { return TillageModules[(int)index]; }
        public HowLeakyEngineModule_Pesticide PesticideModule(int? index) { return PesticideModules[(int)index]; }
        public _CustomHowLeakyEngine_VegModule CurrentCrop { get; set; }

        public List<_CustomHowLeakyEngineModule> Modules { get; set; }

        public BrowserDate StartDate { get; set; }
        public BrowserDate EndDate { get; set; }
        public BrowserDate TodaysDate { get; set; }

        public HowLeakyOutputs Outputs { get; set; }

        //public bool Use2008CurveNoFn { get; set; }
        public bool Force2011CurveNoFn { get; set; } = false;
        //public int Index { get; set; }

        public int DaysSinceTillage { get; set; }
        public int DaysSinceHarvest { get; set; }
        public int TotalNumberPlantings { get; set; }
        public double AccumulateCovDayBeforePlanting { get; set; }
        public double TotalResidueCover { get; set; }
        public double TotalResidueCoverPercent { get; set; }
        public double NumberOfDaysInSimulation { get; set; }
        public double TotalTranspiration { get; set; }
        public double TotalEvapotranspiration { get; set; }

        public bool UseLAIModel()
        {
            return CurrentCrop.GetType() == typeof(HowLeakyEngineModule_LAIVeg);
        }
        public bool CanPlantCrop(_CustomHowLeakyEngine_VegModule crop)
        {
            // Here are a few notes to try and get your head around the logic here.
            // First of all check to see if we leave left sufficient gap between this new
            // rotation and the previous rotation of this crop.
            // Case 1:
            // First test to see if this is the CurrentCrop.
            // If so, then make sure that we HAVENT exceeded max rotation count. Then we must
            // see if actually meet the planting criteria. If any of these fail, then return
            // "false" so that we can test another crop. Otherwise return "true" to tell the
            // sim to replant the CurrentCrop
            // Case 2:
            // If this is NOT the CurrentCrop - we had better first check that the CurrentCrop
            // is still expected to be in rotation. Then check to see that the sowing criteria
            // has been met. Return "true" if all good, otherwise return "false" so we can test
            // another crop. If we run out of crops, then the CurrentCrop stays in fallow.
            try
            {
                if (crop.IsSequenceCorrect() && crop.DoesCropMeetSowingCriteria())
                {

                    if (crop == CurrentCrop)
                    {
                        return crop.IsCropUnderMaxContinuousRotations();
                    }
                    else if (CurrentCrop.HasCropHadSufficientContinuousRotations())
                    {
                        return crop.HasCropBeenAbsentForSufficientYears(TodaysDate);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return false;
        }

        internal bool IsDateInWindow(BrowserDate date, DayMonthData startWindow, DayMonthData endWindow)
        {
            try
            {
                int currYear = date.Year;
                if (startWindow.IsValid() && endWindow.IsValid())
                {
                    if (doesDateOverlapYear(startWindow, endWindow) == false)
                    {
                        var _startWindow = new BrowserDate(currYear, startWindow.Month, startWindow.Day);
                        var _endWindow = new BrowserDate(currYear, endWindow.Month, endWindow.Day);
                        return date.IsBetween(_startWindow, _endWindow);
                    }
                    else
                    {
                        var _startWindow = new BrowserDate(currYear, startWindow.Month, startWindow.Day);
                        if (_startWindow.DateInt > date.DateInt)
                        {
                            _startWindow = new BrowserDate(currYear - 1, startWindow.Month, startWindow.Day);
                            var _endWindow = new BrowserDate(currYear, endWindow.Month, endWindow.Day);
                            return date.IsBetween(_startWindow, _endWindow);
                        }
                        else
                        {
                            var _endWindow = new BrowserDate(currYear + 1, endWindow.Month, endWindow.Day);
                            return date.IsBetween(_startWindow, _endWindow);
                        }
                    }
                }
                return false;

            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            // return false;
        }

        private bool doesDateOverlapYear(DayMonthData startWindow, DayMonthData endWindow)
        {
            if (startWindow.Month < endWindow.Month) return false;
            else if (startWindow.Month == endWindow.Month)
            {
                if (startWindow.Day <= endWindow.Day) return false;
            }
            return true;
        }

        internal bool InFallow()
        {
            try
            {
                if (CurrentCrop != null)
                {
                    return CurrentCrop.GetInFallow();
                }

            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return true;
        }

        internal bool IsPlanting()
        {
            try
            {
                if (CurrentCrop != null)
                {
                    return CurrentCrop.GetIsPlanting();
                }

            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return false;
        }

        internal double CalcFallowSoilWater()
        {
            try
            {
                if (CurrentCrop != null)
                {
                    return CurrentCrop.CalcFallowSoilWater();
                }

            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return 0;
        }

        internal int GetCropIndex(_CustomHowLeakyEngine_VegModule module)
        {
            try
            {
                return VegetationModules.IndexOf(module);
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            //return 0;
        }




        internal _CustomHowLeakyEngine_VegModule GetCrop(int index)
        {
            try
            {
                if (index >= 0 && index < VegetationModules.Count)
                {
                    return VegetationModules[index];
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return null;
        }

        internal double GetTotalCover()
        {
            try
            {
                if (CurrentCrop != null)
                {
                    if (CurrentCrop.GetType() == typeof(HowLeakyEngineModule_LAIVeg))
                    {
                        return Math.Min(1.0, CurrentCrop.CropCover + SoilModule.TotalResidueCover * (1 - CurrentCrop.CropCover));
                    }
                    else
                    {
                        return CurrentCrop.GetTotalCover();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return 0;
        }

        internal double GetCropCover()
        {
            try
            {
                if (CurrentCrop != null)
                {
                    return CurrentCrop.CropCover;
                }

            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return 0;
        }

        internal double GetTotalTranspiration()
        {
            try
            {
                if (CurrentCrop != null)
                {
                    return CurrentCrop.TotalTranspiration;
                }

            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return 0;
        }

        internal double GetPotentialSoilEvaporation()
        {
            try
            {
                if (CurrentCrop != null)
                {
                    return CurrentCrop.GetPotentialSoilEvaporation();
                }

            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return 0;
        }

        internal double GetInitialPAW()
        {
            try
            {
                return InitialPAW;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            //return 0;
        }

        internal double GetCropCoverIfLAIModel(double current)
        {
            try
            {
                if (CurrentCrop != null)
                {
                    if (CurrentCrop.GetType() == typeof(HowLeakyEngineModule_LAIVeg))
                    {
                        return CurrentCrop.CropCover;
                        //LAI Model uses cover from the end of the previous day
                        //whereas Cover model predefines at the start of the day
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return current;
        }

        internal void CalculateTotalResidue()
        {
            try
            {
                //TODO: Make the crop residue function more generic -- remove type condition
                if (CurrentCrop != null && CurrentCrop.GetType() == typeof(HowLeakyEngineModule_LAIVeg))
                {
                    TotalCropResidue = 0;
                    TotalResidueCover = 0;
                    HowLeakyEngineModule_LAIVeg crop = (HowLeakyEngineModule_LAIVeg)CurrentCrop;
                    TotalResidueCover = crop.ResidueCover;
                    crop.CalculateResidue();
                    int count = VegetationModules.Count;
                    for (int i = 1; i < count; ++i)
                    {
                        int index = GetCropIndex(crop) + 1;
                        if (index == count)
                        {
                            index = 0;
                        }
                        crop = (HowLeakyEngineModule_LAIVeg)VegetationModules[index];
                        TotalResidueCover = Math.Min(1.0, TotalResidueCover + crop.ResidueCover * (1 - TotalResidueCover));
                    }
                    for (int i = 0; i < count; ++i)
                    {
                        TotalCropResidue += ((HowLeakyEngineModule_LAIVeg)VegetationModules[i]).ResidueAmount;
                    }

                }
                else
                {
                    TotalCropResidue = VegetationModules[0].CropResidue;
                    TotalResidueCover = VegetationModules[0].ResidueCover;
                }
                TotalResidueCoverPercent = TotalResidueCover * 100.0;

            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }



        internal int GetPesticideIndex(HowLeakyEngineModule_Pesticide pest)
        {
            try
            {
                if (PesticideModules != null)
                {
                    return PesticideModules.IndexOf(pest);
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return 0;
        }
    }
}
