using HowLeaky_SimulationEngine.Enums;
using HowLeaky_SimulationEngine.Errors;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Engine
{
    public partial class HowLeakyEngine
    {
        public double RoughnessRatio { get; set; }
        public double TotalCropResidue { get; private set; }
        public double TillageResidueReduction { get; private set; }

        public bool SimulateDay()
        {
            bool result = true;
            try
            {
                LoadTodaysClimateData();
                AdjustKeyDatesForYear();
                SetStartOfDayParameters();
                ApplyResetsIfAny();
                TryModelIrrigation();
               // TryModelSoilCracking();//Removed Mar 2022
                CalculateRunoff();
                CalculatSoilEvaporation();
                TryModelVegetation();
                UpdateWaterBalance();
                TryModelTillage();
                CalculateResidue();
                CalculateErosion();
                TryModelRingTank();
                TryModelPesticide();
                TryModelPhosphorus();
                TryModelNitrate();
                TryModelSolutes();
                TryModelLateralFlow();
       
                UpdateOutputs();
                ResetAnyParametersIfRequired();
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return result;

        }

        internal void UpdateTillageParameters(ETillageType type, double cresmultiplier, double roughnessratio)
        {
            try
            {
                double initialCropResidue = TotalCropResidue;
                RoughnessRatio = roughnessratio;
                DaysSinceTillage = 0;
                //Sim.UpdateManagementEventHistory(ManagementEvent.Tillage, 0);        
                foreach (var crop in VegetationModules)
                {
                    crop.ResidueAmount = crop.ResidueAmount * cresmultiplier;
                }
                CalculateTotalResidue();
                TillageResidueReduction = initialCropResidue - TotalCropResidue;
                if (RoughnessRatio > 0.0)
                {
                    SoilModule.RainSinceTillage = 0.0;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        public void LoadTodaysClimateData()
        {
            try
            {
                ClimateModule.Simulate();
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        public void AdjustKeyDatesForYear()
        {
            try
            {
                //Day = Today.Day;
                //Month = Today.Month;
                //Year = Today.Year;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }


        public void SetStartOfDayParameters()
        {
            try
            {
                if (SoilModule != null)
                {
                    SoilModule.SetStartOfDayParameters();
                }
                if (IrrigationModule != null)
                {
                    IrrigationModule.SetStartOfDayParameters();
                }
                if (VegetationModules != null)
                {
                    
                     SortCurrentVegList();   //resort crop list to put the current crop first.        
                    //VegetationModules.SetStartOfDayParameters();
                }
                if (TillageModules != null)
                {
                    // TillageModules.SetStartOfDayParameters();
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
        public void ApplyResetsIfAny()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            // ModelOptionsController.ApplyResetsIfAny(Today);
        }

        /// <summary>
        /// 
        /// </summary>
        public void TryModelIrrigation()
        {
            try
            {
                if (IrrigationModule != null)
                {
                    IrrigationModule.Simulate();
                    SoilModule.Irrigation = IrrigationModule.IrrigationApplied;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        //Removed Mar 2022
        //public void TryModelSoilCracking()
        //{
        //    try
        //    {
        //        if (SoilModule != null)
        //        {
        //            SoilModule.TryModelSoilCracking();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ErrorLogger.CreateException(ex);
        //    }

        //}

        public void CalculateRunoff()
        {
            try
            {
                if (SoilModule != null)
                {
                    SoilModule.CalculateRunoff();
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        public void CalculatSoilEvaporation()
        {
            try
            {
                if (SoilModule != null)
                {
                    SoilModule.CalculateSoilEvaporation();
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        public void TryModelVegetation()
        {
            try
            {
                if (VegetationModules != null)
                {
                    if (CurrentCrop != null)
                    {
                        if (UseLAIModel())
                        {
                            if (CurrentCrop.GetInFallow())
                            {
                                foreach (var crop in SortedVegetationModules)
                                {
                                    if (CanPlantCrop(crop))
                                    {
                                        crop.Plant();
                                        if (IrrigationModule != null)
                                        {
                                            IrrigationModule.FirstIrrigation = true;
                                        }
                                        return;
                                    }
                                }
                            }
                            else if (CurrentCrop.IsGrowing())
                            {
                                CurrentCrop.Simulate();
                            }
                        }
                        else
                        {
                            CurrentCrop.Simulate();
                        }
                        ++DaysSinceHarvest;
                        TotalTranspiration = CurrentCrop.TotalTranspiration;
                        TotalEvapotranspiration = CurrentCrop.TotalTranspiration + SoilModule.SoilEvap;
                        //UpdateCropWaterBalanceParameters();
                    }




                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }
        public void UpdateWaterBalance()
        {
            try
            {
                if (SoilModule != null)
                {
                    SoilModule.UpdateWaterBalance();
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        public void TryModelTillage()
        {
            try
            {
                if (TillageModules != null)
                {
                    if (DaysSinceTillage != -1)
                    {
                        ++DaysSinceTillage;
                    }
                    foreach (var tillage in TillageModules)
                    {
                        tillage.Simulate();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        public void CalculateResidue()
        {
            try
            {
                foreach (var crop in VegetationModules)
                {
                    crop.CalculateResidue();
                }
                CalculateTotalResidue();
                SoilModule.CalculateResidue();
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }


        }


        public void CalculateErosion()
        {
            try
            {
                SoilModule.CalculateErosion();
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        public void TryModelRingTank()
        {
            try
            {
                if (IrrigationModule != null)
                {
                    IrrigationModule.ModelRingTank();
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        public void TryModelPesticide()
        {
            try
            {
                if (PesticideModules != null)
                {
                    foreach (var pest in PesticideModules)
                    {
                        pest.Simulate();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        public void TryModelPhosphorus()
        {
            try
            {
                if (PhosphorusModule != null)
                {
                    PhosphorusModule.Simulate();
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        public void TryModelNitrate()
        {
            try
            {
                if (NitrateModule != null)
                {
                    NitrateModule.Simulate();
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        public void TryModelSolutes()
        {
            try
            {
                if (SolutesModule != null)
                {
                    SolutesModule.Simulate();
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }
        public void TryModelLateralFlow()
        {
            try
            {
                SoilModule.TryModelLateralFlow();
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }
        public void UpdateCropWaterBalance()
        {

        }

        public void UpdateFallowWaterBalance()
        {
            //SoilModule.UpdateFallowWaterBalance();
        }

        public void UpdateTotalWaterBalance()
        {
            //SoilModule.UpdateTotalWaterBalance();
        }

        public void TryUpdateRingTankWaterBalance()
        {

        }

        public void CalculateVolumeBalanceError()
        {
            SoilModule.CalculateVolumeBalanceError();
        }

        public void ResetAnyParametersIfRequired()
        {
            try
            {
                foreach (var module in Modules)
                {
                    module.ResetAnyParametersIfRequired();
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }


        
        private void SortCurrentVegList()
        {
           int cropCount = VegetationModules.Count;
            SortedVegetationModules.Clear();
            int startindex = GetCropIndex(CurrentCrop);
            for (int i = 0; i < cropCount; ++i)
            {
                int index = startindex + i;
                index = (index < cropCount ? index : index - cropCount);
                var crop = GetCrop(index);
                SortedVegetationModules.Add(crop);
            }
        }
    }
}
