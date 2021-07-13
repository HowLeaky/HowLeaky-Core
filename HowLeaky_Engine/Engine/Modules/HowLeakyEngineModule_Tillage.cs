using HowLeaky_SimulationEngine.Enums;
using HowLeaky_SimulationEngine.Errors;
using HowLeaky_SimulationEngine.Inputs;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Engine
{
    public class HowLeakyEngineModule_Tillage : _CustomHowLeakyEngineModule
    {
        public HowLeakyEngineModule_Tillage(HowLeakyEngine sim, HowLeakyInputs_Tillage inputs) : base(sim)
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

        public HowLeakyEngineModule_Tillage()
        {
        }

        public HowLeakyInputs_Tillage InputModel { get; set; }


        public override void Simulate()
        {
            try
            {
                if (CanTillToday())
                {
                    Engine.UpdateTillageParameters((ETillageType)InputModel.Type, InputModel.PrimaryCropResMultiplier, InputModel.PrimaryRoughnessRatio);
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }


        public bool CanTillToday()
        {
            try
            {
                switch (InputModel.Format)
                {
                    case (int)ETillageFormat.TillInWindow: return IsFallowAndInWindow();
                    case (int)ETillageFormat.FixedDate: return IsFallowAndDate();
                    case (int)ETillageFormat.AtPlantingAllCrop: return IsPlantDay();
                    case (int)ETillageFormat.AtPlantingCrop1: return IsPlantDayForCrop(0);
                    case (int)ETillageFormat.AtPlantingCrop2: return IsPlantDayForCrop(1);
                    case (int)ETillageFormat.AtPlantingCrop3: return IsPlantDayForCrop(2);
                    case (int)ETillageFormat.AtHarvestAllCrop: return IsHarvestDay();
                    case (int)ETillageFormat.AtHarvestCrop1: return IsHarvestDayForCrop(0);
                    case (int)ETillageFormat.AtHarvestCrop2: return IsHarvestDayForCrop(1);
                    case (int)ETillageFormat.AtHarvestCrop3: return IsHarvestDayForCrop(2);
                    case (int)ETillageFormat.FromSequenceFile: return IsFallowAndInSequence();
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

            return false;
        }

        public bool IsFallowAndInWindow()
        {
            try
            {
                bool check1 = Engine.InFallow();
                bool check2 = Engine.IsDateInWindow(Engine.TodaysDate, InputModel.StartTillWindow, InputModel.EndTillWindow);
                bool check3 = (Engine.DaysSinceTillage >= InputModel.MinDaysBetweenTills || Engine.DaysSinceTillage == -1);
                bool check4 = (Engine.ClimateModule.SumRain(InputModel.NoDaysToTotalRain, 0) >= InputModel.RainForPrimaryTill);
                return check1 && check2 && check3 && check4;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
           // return false;
        }

        public void AdjustKeyDatesForYear(int year)
        {

        }

        public bool IsFallowAndDate()
        {
            try
            {
                bool check1 = Engine.InFallow();
                bool check2 = ((InputModel.PrimaryTillDate!=null && InputModel.PrimaryTillDate.MatchesDate(Engine.TodaysDate))||
                                (InputModel.SecondaryTillDate1!=null && InputModel.SecondaryTillDate1.MatchesDate(Engine.TodaysDate) )|| 
                                (InputModel.SecondaryTillDate2!=null&&InputModel.SecondaryTillDate2.MatchesDate(Engine.TodaysDate))|| 
                                (InputModel.SecondaryTillDate3!=null&&InputModel.SecondaryTillDate3.MatchesDate(Engine.TodaysDate)));
                return check1 && check2;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
           

        }

        public bool IsPlantDay()
        {
            try
            {
                if (Engine.CurrentCrop != null)
                {
                    return Engine.CurrentCrop.TodayIsPlantDay;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        public bool IsPlantDayForCrop(int id)
        {
            try
            {
                if (Engine.CurrentCrop != null)
                {
                    return Engine.CurrentCrop.CropIndex == id && Engine.CurrentCrop.TodayIsPlantDay;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        public bool IsHarvestDay()
        {
            try
            {
                if (Engine.CurrentCrop != null)
                {
                    return Engine.CurrentCrop.TodayIsHarvestDay;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        public bool IsHarvestDayForCrop(int id)
        {
            try
            {

                if (Engine.CurrentCrop != null)
                {
                    return Engine.CurrentCrop.CropIndex == id && Engine.CurrentCrop.TodayIsHarvestDay;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }


        public bool IsFallowAndInSequence()
        {
            return Engine.InFallow() && InputModel.PrimaryTillageDates.ContainsDate(Engine.TodaysDate);
        }
    }
}
