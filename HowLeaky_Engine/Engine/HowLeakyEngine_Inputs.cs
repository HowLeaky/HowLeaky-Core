using HowLeaky_SimulationEngine.Errors;
using HowLeaky_SimulationEngine.Inputs;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Engine
{
    public partial class HowLeakyEngine
    {
        public void PreloadClimateFile(HowLeakyInputs_Climate climateInputs)
        {
            if (ClimateModule == null)
            {
                ClimateModule = new HowLeakyEngineModule_Climate(this, climateInputs);
                if(Modules==null)
                {
                    Modules = new List<_CustomHowLeakyEngineModule>();
                }
                Modules.Add(ClimateModule);
            }
        }
        public bool LoadInputs(HowLeakyInputsModel model)
        {
            try
            {
                if(model.Climate==null&&ClimateModule==null)
                {
                    return false;
                }
                if(model.Soil==null)
                {
                    return false;
                }
                if(model.Crops==null||model.Crops.Count==0)
                {
                    return false;
                }
                SimulationName = model.Name;
                if (Modules == null)
                {
                    Modules = new List<_CustomHowLeakyEngineModule>();
                }
                if (model.Climate != null)
                {
                    ClimateModule = new HowLeakyEngineModule_Climate(this, model.Climate);
                    Modules.Add(ClimateModule);
                }
                if (model.Soil != null)
                {
                    SoilModule = new HowLeakyEngineModule_Soil(this, model.Soil);
                    Modules.Add(SoilModule);
                }
                if (model.Crops != null)
                {
                    VegetationModules = new List<_CustomHowLeakyEngine_VegModule>();
                    foreach (var crop in model.Crops)
                    {
                        if (crop.IsLAI())
                        {
                            VegetationModules.Add(new HowLeakyEngineModule_LAIVeg(this, (HowLeakyInputs_LAIVeg)crop));
                        }
                        else
                        {
                            VegetationModules.Add(new HowLeakyEngineModule_CoverVeg(this, (HowLeakyInputs_CoverVeg)crop));
                        }
                    }
                    Modules.AddRange(VegetationModules);
                }
                if (model.Tillage != null)
                {
                    TillageModules = new List<HowLeakyEngineModule_Tillage>();
                    foreach (var till in model.Tillage)
                    {
                        TillageModules.Add(new HowLeakyEngineModule_Tillage(this, till));
                    }
                    Modules.AddRange(TillageModules);
                }
                if (model.Pesticides != null)
                {
                    PesticideModules = new List<HowLeakyEngineModule_Pesticide>();
                    foreach (var pest in model.Pesticides)
                    {
                        PesticideModules.Add(new HowLeakyEngineModule_Pesticide(this, pest));
                    }
                    Modules.AddRange(PesticideModules);
                }
                if (model.Irrigation != null)
                {
                    IrrigationModule = new HowLeakyEngineModule_Irrigation(this, model.Irrigation);
                    Modules.Add(IrrigationModule);

                }
                if (model.Phosphorus != null)
                {
                    PhosphorusModule = new HowLeakyEngineModule_Phosphorus(this, model.Phosphorus);
                    Modules.Add(PhosphorusModule);
                }
                if (model.Solutes != null)
                {
                    SolutesModule = new HowLeakyEngineModule_Solutes(this, model.Solutes);
                    Modules.Add(SolutesModule);
                }
                if (model.Nitrate != null)
                {
                    NitrateModule = new HowLeakyEngineModule_Nitrate(this, model.Nitrate);
                    Modules.Add(NitrateModule);
                }

                StartDate = model.StartDate;
                EndDate = model.EndDate;
                if (ClimateModule != null && ClimateModule.InputModel != null)
                {
                    if (StartDate == null || StartDate.DateInt < ClimateModule.InputModel.StartDate.DateInt)
                    {
                        StartDate = new BrowserDate(ClimateModule.InputModel.StartDate);
                    }

                    if (EndDate == null || EndDate.DateInt > ClimateModule.InputModel.EndDate.DateInt)
                    {
                        EndDate = new BrowserDate(ClimateModule.InputModel.EndDate);
                    }
                }
                if (StartDate != null)
                {
                    TodaysDate = new BrowserDate(StartDate);
                }
                if (ClimateModule == null)
                {
                    throw new Exception("Climate Module could not be loaded");
                }
                if (SoilModule == null)
                {
                    throw new Exception("Soil Module could not be loaded");
                }
                else if (VegetationModules == null || VegetationModules.Count == 0)
                {
                    throw new Exception("Vegetation Modules could not be loaded");
                }



                //ResetResidueMassDay=model.ResetResidueMassDay;
                //ResetResidueMassMonth=model.ResetResidueMassMonth;
                //ResetResidueMassValue=model.ResetResidueMassValue;
                //ResetSoilWaterAtDate=model.ResetSoilWaterAtDefinedDate;


                //ResetSoilWaterDay=model.ResetSoilWaterDay;
                //ResetSoilWaterMonth=model.ResetSoilWaterMonth;

                //                ResetValueForSWAtPlanting=model.ResetSoilWaterValue;
                //ResetValueForSWAtPlanting=model.ResetSoilWaterValue;
                //ResetSoilWaterAtPlanting=model.ResetSoilWaterAtPlanting;
                //public bool CalculateLateralFlow{get;set;}
                //public bool IgnoreCropDepth{get;set;}
                //public bool UsePERFECTDryMatterFn{get;set;}
                //public bool UsePERFECTGroundCoverFn{get;set;}
                //public bool UsePERFECTSoilEvapFn{get;set;}
                //public bool UsePERFECTLeafAreaFn{get;set;}
                //public bool UsePERFECTResidueFn{get;set;}
                //public bool UsePERFECTUSLELSFactor{get;set;}
                //public bool UsePERFECTCNFn{get;set;}
                //public double PAWCStart{get;set;}
                //public int EvaporationOptions{get;set;}
                ClimateModule.InputModel.PanEvapMultiplier = model.EPanMultiplier;
                ClimateModule.InputModel.RainfallMultiplier = model.RainfallMultiplier;

                return ClimateModule != null && SoilModule != null && VegetationModules != null && VegetationModules.Count > 0;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            // return false;

        }
    }
}
