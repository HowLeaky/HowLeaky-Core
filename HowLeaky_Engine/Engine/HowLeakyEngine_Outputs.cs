using HowLeaky_SimulationEngine.Errors;
using HowLeaky_SimulationEngine.Outputs;
using HowLeaky_SimulationEngine.Outputs.Definitions;
using HowLeaky_SimulationEngine.Outputs.maries;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HowLeaky_SimulationEngine.Engine
{
    public partial class HowLeakyEngine
    {
        public List<HowLeakyOutputDefinition> Definitions { get; set; }
        public HowLeakyOutputSummary_WaterBalance WaterBalanceSummary { get; set; }



        public bool IncludeSummaries { get; set; }

        public void InitialiseOutputObject()
        {
            try
            {
                Outputs = new HowLeakyOutputs(StartDate, EndDate);
                foreach (var outputtype in Definitions.Where(x => x.IsActive()).ToList())
                {

                    foreach (var action in outputtype.Actions)
                    {
                        Outputs.TimeSeries.Add(new HowLeakyOutputTimeseriesActive(outputtype, StartDate, EndDate));
                    }
                }
                if (IncludeSummaries)
                {
                    WaterBalanceSummary = new HowLeakyOutputSummary_WaterBalance();
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }



        //This function is called from the constructor - only need to call it once per HowLeaky Instance.
        public void BuildOutputDefinitions(string timeseriescsv, Dictionary<string, OutputAttributes> remapdict)
        {
            try
            {
                Definitions = new List<HowLeakyOutputDefinition>();
                if (ClimateModule != null) LoadDefaultDefinitions(GetOutputProp(ClimateModule), "ClimateModule", remapdict);
                if (SoilModule != null) LoadDefaultDefinitions(GetOutputProp(SoilModule), "SoilModule", remapdict);
                if (IrrigationModule != null) LoadDefaultDefinitions(GetOutputProp(IrrigationModule), "IrrigationModule", remapdict);
                if (PhosphorusModule != null) LoadDefaultDefinitions(GetOutputProp(PhosphorusModule), "PhosphorusModule", remapdict);
                if (NitrateModule != null) LoadDefaultDefinitions(GetOutputProp(NitrateModule), "NitrateModule", remapdict);
                if (SolutesModule != null) LoadDefaultDefinitions(GetOutputProp(SolutesModule), "SolutesModule", remapdict);
                if (VegetationModules != null && VegetationModules.Count > 0) foreach (var veg in VegetationModules) LoadDefaultDefinitions2(GetOutputProp(veg), "VegetationModule", veg.GetName(), VegetationModules.IndexOf(veg), remapdict);
                if (PesticideModules != null && PesticideModules.Count > 0) foreach (var pest in PesticideModules) LoadDefaultDefinitions2(GetOutputProp(pest), "PesticideModule", pest.GetName(), PesticideModules.IndexOf(pest), remapdict);
                if (TillageModules != null && TillageModules.Count > 0) foreach (var till in TillageModules) LoadDefaultDefinitions2(GetOutputProp(till), "TillageModule", till.InputModel.Name, TillageModules.IndexOf(till), remapdict);
                SelectTimeSeries(timeseriescsv);
                AssignDelegates();
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }


        public void LoadDefaultDefinitions(List<System.Reflection.PropertyInfo> props, string module, Dictionary<string, OutputAttributes> remapdict)
        {
            try
            {
                foreach (var prop in props)
                {
                    Definitions.Add(new HowLeakyOutputDefinition(prop, module, remapdict, "", null));
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        public void LoadDefaultDefinitions2(List<System.Reflection.PropertyInfo> props, string module, string prefix, int index, Dictionary<string, OutputAttributes> remapdict)
        {
            try
            {
                foreach (var prop in props)
                {
                    Definitions.Add(new HowLeakyOutputDefinition(prop, module, remapdict, prefix, index));
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }
        public void SelectTimeSeries(string csvlist)
        {
            try
            {
                if (!string.IsNullOrEmpty(csvlist))
                {
                    var items = csvlist.Split(',').ToList();
                    foreach (var item in items)
                    {
                        ActivateOutput(item.Trim());
                    }
                }
                //var checkcount=Definitions.Count(x=>x.IsActive());
                //if(items.Count!=checkcount)
                //{
                //    var list=new List<string>();
                //    list.Add($"The folowing output timeseries could note be connected:");
                //    foreach(var item in items)
                //    {
                //        var defn=Definitions.FirstOrDefault(x=>x.CodeName==item);
                //        if(defn==null)
                //        {
                //            list.Add(item);
                //        }
                //    }
                //    if(list.Count>1)
                //    {
                //        throw new Exception(string.Join("\n",list));
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }


        public bool ActivateOutput(string codename)
        {
            try
            {

                var definitions = Definitions.Where(x => x.CodeName == codename).ToList();
                if (definitions.Count > 0)
                {
                    foreach (var definition in definitions)
                    {
                        definition.UserDefinedActive = true;
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }


        public void AssignDelegates()
        {
            try
            {

                foreach (var definition in Definitions.Where(x => x.IsActive()).ToList())
                {
                    var engineType = this.GetType();
                    var methodInfo = engineType.GetMethod(definition.GetMethodName());
                    if (methodInfo != null)
                    {
                        try
                        {
                            var action = (Action<HowLeakyOutputTimeseriesActive, int>)Delegate.CreateDelegate(typeof(Action<HowLeakyOutputTimeseriesActive, int>), this, methodInfo);


                            if (definition.VectorType == OutputVectorType.None)
                            {
                                //list.Add(new HowLeakyOutputTimeseries(outputtype,null,action));
                                definition.Actions.Add(action);
                            }
                            else if (definition.VectorType == OutputVectorType.Crop)
                            {
                                //var index=0;
                                foreach (var crop in VegetationModules)
                                {
                                    definition.Actions.Add(action);
                                    // list.Add(new HowLeakyOutputTimeseries(outputtype,index,action));
                                    // ++index;
                                }
                            }
                            else if (definition.VectorType == OutputVectorType.Pesticide)
                            {
                                //var index=0;
                                foreach (var pest in PesticideModules)
                                {
                                    definition.Actions.Add(action);
                                    //list.Add(new HowLeakyOutputTimeseries(outputtype,index,action));
                                    //++index;
                                }
                            }
                            else if (definition.VectorType == OutputVectorType.SoilLayer)
                            {
                                var layercount = SoilModule.LayerCount;
                                for (var index = 0; index < layercount; ++index)
                                {
                                    definition.Actions.Add(action);
                                    //list.Add(new HowLeakyOutputTimeseries(outputtype,index,action));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ErrorLogger.CreateException(ex);
                        }

                    }
                    else
                    {
                        var name = definition.GetMethodName();
                        throw new Exception($"Couldn't find {name} in HowLeakyEngine:AssignDelegates");

                    }
                }
                // TimeSeries=list;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        public List<System.Reflection.PropertyInfo> GetOutputProp(_CustomHowLeakyEngineModule module)
        {
            return module.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(Attributes.Output))).ToList();
        }

        public void UpdateOutputs()
        {
            try
            {
                var index = TodaysDate.DateInt - StartDate.DateInt;
                Outputs.UpdateDailyTimeseries(index);
                if (IncludeSummaries)
                {
                    WaterBalanceSummary.Update(this);
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        public void UpdateClimateModule_Rain(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, ClimateModule.Rain);
        }

        public void UpdateClimateModule_MaxTemp(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, ClimateModule.MaxTemp);
        }

        public void UpdateClimateModule_MinTemp(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, ClimateModule.MinTemp);
        }

        public void UpdateClimateModule_PanEvap(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, ClimateModule.PanEvap);
        }

        public void UpdateClimateModule_SolarRadiation(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, ClimateModule.SolarRadiation);
        }

        public void UpdateIrrigationModule_IrrigationRunoff(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, IrrigationModule.IrrigationRunoff);
        }

        public void UpdateIrrigationModule_IrrigationApplied(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, IrrigationModule.IrrigationApplied);
        }

        public void UpdateIrrigationModule_IrrigationInfiltration(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, IrrigationModule.IrrigationInfiltration);
        }

        public void UpdateIrrigationModule_RingTankEvaporationLosses(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, IrrigationModule.RingTankEvaporationLosses);
        }

        public void UpdateIrrigationModule_RingTankSeepageLosses(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, IrrigationModule.RingTankSeepageLosses);
        }

        public void UpdateIrrigationModule_RingTankOvertoppingLosses(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, IrrigationModule.RingTankOvertoppingLosses);
        }

        public void UpdateIrrigationModule_RingTankIrrigationLosses(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, IrrigationModule.RingTankIrrigationLosses);
        }

        public void UpdateIrrigationModule_RingTankTotalLosses(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, IrrigationModule.RingTankTotalLosses);
        }

        public void UpdateIrrigationModule_RingTankRunoffCaptureInflow(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, IrrigationModule.RingTankRunoffCaptureInflow);
        }

        public void UpdateIrrigationModule_RingTankRainfalInflow(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, IrrigationModule.RingTankRainfalInflow);
        }

        public void UpdateIrrigationModule_RingTankEffectiveAdditionalInflow(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, IrrigationModule.RingTankEffectiveAdditionalInflow);
        }

        public void UpdateIrrigationModule_RingTankTotalAdditionalInflow(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, IrrigationModule.RingTankTotalAdditionalInflow);
        }

        public void UpdateIrrigationModule_RingTankTotalInflow(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, IrrigationModule.RingTankTotalInflow);
        }

        public void UpdateIrrigationModule_RingTankIneffectiveAdditionalInflow(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, IrrigationModule.RingTankIneffectiveAdditionalInflow);
        }

        public void UpdateIrrigationModule_RingTankStorageVolume(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, IrrigationModule.RingTankStorageVolume);
        }

        public void UpdateIrrigationModule_RingTankStorageLevel(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, IrrigationModule.RingTankStorageLevel);
        }

        public void UpdatePhosphorusModule_ParticulateConc(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PhosphorusModule.ParticulateConc);
        }

        public void UpdatePhosphorusModule_DissolvedConc(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PhosphorusModule.DissolvedConc);
        }

        public void UpdatePhosphorusModule_BioAvailParticPConc(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PhosphorusModule.BioAvailParticPConc);
        }

        public void UpdatePhosphorusModule_BioAvailPConc(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PhosphorusModule.BioAvailPConc);
        }

        public void UpdatePhosphorusModule_TotalPConc(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PhosphorusModule.TotalPConc);
        }

        public void UpdatePhosphorusModule_ParticPExport(HowLeakyOutputTimeseriesActive output, int index)
        {

            output.Update(index, PhosphorusModule.ParticPExport);
        }

        public void UpdatePhosphorusModule_BioAvailParticPExport(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PhosphorusModule.BioAvailParticPExport);
        }

        public void UpdatePhosphorusModule_TotalBioAvailExport(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PhosphorusModule.TotalBioAvailExport);
        }

        public void UpdatePhosphorusModule_TotalP(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PhosphorusModule.TotalP);
        }

        //public void UpdatePhosphorusModule_CKQ(HowLeakyOutputTimeseries output, int index){ 
        //     output.Update(index,PhosphorusModule.CKQ);
        //}

        public void UpdatePhosphorusModule_PPHLC(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PhosphorusModule.PPHLC);
        }

        public void UpdatePhosphorusModule_PhosExportDissolve(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PhosphorusModule.PhosExportDissolve);
        }

        public void UpdateSoilModule_Irrigation(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.Irrigation);
        }

        public void UpdateSoilModule_Runoff(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.Runoff);
        }

        public void UpdateSoilModule_RunoffFromIrrigation(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.RunoffFromIrrigation);
        }

        public void UpdateSoilModule_RunoffFromRainfall(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.RunoffFromRainfall);
        }

        public void UpdateSoilModule_SoilEvap(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.SoilEvap);
        }

        public void UpdateSoilModule_PotSoilEvap(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.PotSoilEvap);
        }

        public void UpdateSoilModule_Transpiration(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.Transpiration);
        }

        public void UpdateSoilModule_EvapoTransp(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.EvapoTransp);
        }

        public void UpdateSoilModule_DeepDrainage(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.DeepDrainage);
        }

        public void UpdateSoilModule_Overflow(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.Overflow);
        }

        public void UpdateSoilModule_LateralFlow(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.LateralFlow);
        }

        public void UpdateSoilModule_VBE(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.VBE);
        }

        public void UpdateSoilModule_RunoffCurveNo(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.RunoffCurveNo);
        }

        public void UpdateSoilModule_RunoffRetentionNumber(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.RunoffRetentionNumber);
        }

        public void UpdateSoilModule_HillSlopeErosion(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.HillSlopeErosion);
        }

        public void UpdateSoilModule_OffSiteSedDelivery(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.OffSiteSedDelivery);
        }

        public void UpdateSoilModule_TotalSoilWater(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.TotalSoilWater);
        }

        public void UpdateSoilModule_SoilWaterDeficit(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.SoilWaterDeficit);
        }

        public void UpdateSoilModule_Layer1Satindex(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.Layer1SatIndex);
        }

        public void UpdateSoilModule_TotalCropResidue(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.TotalCropResidue);
        }

        public void UpdateSoilModule_TotalResidueCover(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.TotalResidueCover * 100.0);
        }

        public void UpdateSoilModule_TotalCoverAllCrops(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.TotalCoverAllCrops * 100.0);
        }

        public void UpdateSoilModule_SoilWater(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.SoilWater[(int)output.Index]);
        }

        public void UpdateSoilModule_Drainage(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SoilModule.Drainage[(int)output.Index]);
        }

        //[Output] public double total_soil_solute_mg_per_kg { get; set; }
        //[Output] public double total_soil_solute_mg_per_L { get; set; }
        //[Output] public double total_soil_solute_kg_per_ha { get; set; }
        //[Output] public List<double> solute_conc_layer_mg_per_L { get; set; }
        //[Output] public List<double> solute_conc_layer_mg_per_kg { get; set; }
        //[Output] public List<double> solute_load_layer_kg_per_ha { get; set; }
        //[Output] public double solute_leaching_conc_mg_per_L { get; set; }
        //[Output] public double solute_leaching_load_kg_per_ha { get; set; }


        public void UpdateSolutesModule_total_soil_solute_mg_per_kg(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SolutesModule.total_soil_solute_mg_per_kg);
        }

        public void UpdateSolutesModule_total_soil_solute_mg_per_L(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SolutesModule.total_soil_solute_mg_per_L);
        }

        public void UpdateSolutesModule_total_soil_solute_kg_per_ha(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SolutesModule.total_soil_solute_kg_per_ha);
        }

        public void UpdateSolutesModule_solute_conc_layer_mg_per_L(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SolutesModule.solute_conc_layer_mg_per_L[(int)output.Index]);
        }

        public void UpdateSolutesModule_solute_conc_layer_mg_per_kg(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SolutesModule.solute_conc_layer_mg_per_kg[(int)output.Index]);
        }

        public void UpdateSolutesModule_solute_load_layer_kg_per_ha(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SolutesModule.solute_load_layer_kg_per_ha[(int)output.Index]);
        }

        public void UpdateSolutesModule_solute_leaching_conc_mg_per_L(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SolutesModule.solute_leaching_conc_mg_per_L);
        }

        public void UpdateSolutesModule_solute_leaching_load_kg_per_ha(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, SolutesModule.solute_leaching_load_kg_per_ha);
        }

        public void UpdateNitrateModule_NitrogenApplication(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, NitrateModule.NitrogenApplication);
        }

        public void UpdateNitrateModule_Mineralisation(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, NitrateModule.Mineralisation);
        }

        public void UpdateNitrateModule_CropUsePlant(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, NitrateModule.CropUsePlant);
        }

        public void UpdateNitrateModule_CropUseRatoon(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, NitrateModule.CropUseRatoon);
        }

        public void UpdateNitrateModule_CropUseActual(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, NitrateModule.CropUseActual);
        }

        public void UpdateNitrateModule_Denitrification(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, NitrateModule.Denitrification);
        }

        public void UpdateNitrateModule_ExcessN(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, NitrateModule.ExcessN);
        }

        public void UpdateNitrateModule_PropVolSat(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, NitrateModule.PropVolSat);
        }

        public void UpdateNitrateModule_DINDrainage(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, NitrateModule.DINDrainage);
        }

        public void UpdateNitrateModule_NO3NDissolvedInRunoff(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, NitrateModule.NO3NDissolvedInRunoff);
        }

        public void UpdateNitrateModule_NO3NRunoffLoad(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, NitrateModule.NO3NRunoffLoad);
        }

        public void UpdateNitrateModule_NO3NDissolvedLeaching(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, NitrateModule.NO3NDissolvedLeaching);
        }

        public void UpdateNitrateModule_NO3NLeachingLoad(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, NitrateModule.NO3NLeachingLoad);
        }

        public void UpdateNitrateModule_ParticNInRunoff(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, NitrateModule.ParticNInRunoff);
        }

        public void UpdateNitrateModule_NO3NStoreTopLayer(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, NitrateModule.NO3NStoreTopLayer);
        }

        public void UpdateNitrateModule_NO3NStoreBotLayer(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, NitrateModule.NO3NStoreBotLayer);
        }

        public void UpdateNitrateModule_TotalNStoreTopLayer(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, NitrateModule.TotalNStoreTopLayer);
        }

        public void UpdateNitrateModule_PNHLCa(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, NitrateModule.PNHLCa);
        }

        public void UpdateNitrateModule_DrainageInNO3Period(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, NitrateModule.DrainageInNO3Period);
        }

        public void UpdateNitrateModule_RunoffInNO3Period(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, NitrateModule.RunoffInNO3Period);
        }

        public void UpdatePesticideModule_AppliedPestOnVeg(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).AppliedPestOnVeg);
        }

        public void UpdatePesticideModule_AppliedPestOnStubble(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).AppliedPestOnStubble);
        }

        public void UpdatePesticideModule_AppliedPestOnSoil(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).AppliedPestOnSoil);
        }

        public void UpdatePesticideModule_PestOnVeg(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).PestOnVeg);
        }

        public void UpdatePesticideModule_PestOnStubble(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).PestOnStubble);
        }

        public void UpdatePesticideModule_PestInSoil(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).PestInSoil);
        }


        public void UpdatePesticideModule_PestSoilConc(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).PestSoilConc);
        }

        public void UpdatePesticideModule_PestSedPhaseConc(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).PestSedPhaseConc);
        }

        public void UpdatePesticideModule_PestWaterPhaseConc(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).PestWaterPhaseConc);
        }

        public void UpdatePesticideModule_PestRunoffConc(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).PestRunoffConc);
        }

        //public void UpdatePesticideModule_SedimentDelivered(HowLeakyOutputTimeseries output, int index){ 
        //     output.Update(index,PesticideModule(output.Index).SedimentDelivered);
        //}

        public void UpdatePesticideModule_PestLostInRunoffWater(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).PestLostInRunoffWater);
        }

        public void UpdatePesticideModule_PestLostInRunoffSediment(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).PestLostInRunoffSediment);
        }

        public void UpdatePesticideModule_TotalPestLostInRunoff(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).TotalPestLostInRunoff);
        }

        public void UpdatePesticideModule_PestLostInLeaching(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).PestLostInLeaching);
        }

        public void UpdatePesticideModule_PestLossesPercentOfInput(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).PestLossesPercentOfInput);
        }

        public void UpdatePesticideModule_ApplicationCount(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).ApplicationCount);
        }

        public void UpdatePesticideModule_ProductApplication(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).ProductApplication);
        }

        public void UpdatePesticideModule_AvgBoundPestConcInRunoff(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).AvgBoundPestConcInRunoff);
        }

        public void UpdatePesticideModule_AvgUnboundPestConcInRunoff(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).AvgUnboundPestConcInRunoff);
        }

        public void UpdatePesticideModule_AvgCombinedPestConcInRunoff(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).AvgCombinedPestConcInRunoff);
        }

        public void UpdatePesticideModule_AvgPestLoadWater(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).AvgPestLoadWater);
        }

        public void UpdatePesticideModule_AvgPestLoadSediment(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).AvgPestLoadSediment);
        }

        public void UpdatePesticideModule_AvgTotalPestLoad(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).AvgTotalPestLoad);
        }

        public void UpdatePesticideModule_ApplicationLossRatio(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).ApplicationLossRatio);
        }

        public void UpdatePesticideModule_DaysGreaterCrit1(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).DaysGreaterCrit1);
        }

        public void UpdatePesticideModule_DaysGreaterCrit2(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).DaysGreaterCrit2);
        }

        public void UpdatePesticideModule_DaysGreaterCrit3(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).DaysGreaterCrit3);
        }

        public void UpdatePesticideModule_DaysGreaterCrit4(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).DaysGreaterCrit4);
        }

        public void UpdatePesticideModule_PestEMCL(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, PesticideModule(output.Index).PestEMCL);
        }

        public void UpdateVegetationModule_DaysSincePlanting(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).DaysSincePlanting);
        }

        public void UpdateVegetationModule_LAI(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).LAI);
        }

        public void UpdateVegetationModule_GreenCover(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).GreenCover * 100.0);
        }

        public void UpdateVegetationModule_ResidueCover(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).ResidueCover * 100.0);
        }

        public void UpdateVegetationModule_TotalCover(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).TotalCover * 100.0);
        }

        public void UpdateVegetationModule_ResidueAmount(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).ResidueAmount);
        }

        public void UpdateVegetationModule_DryMatter(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).DryMatter);
        }

        public void UpdateVegetationModule_RootDepth(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).RootDepth);
        }

        public void UpdateVegetationModule_Yield(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).Yield);
        }

        public void UpdateVegetationModule_PotTranspiration(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).PotTranspiration);
        }

        public void UpdateVegetationModule_GrowthRegulator(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).GrowthRegulator);
        }

        public void UpdateVegetationModule_WaterStressIndex(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).WaterStressIndex);
        }

        public void UpdateVegetationModule_TempStressIndex(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).TempStressIndex);
        }

        public void UpdateVegetationModule_CropRainfall(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).CropRainfall);
        }

        public void UpdateVegetationModule_CropIrrigation(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).CropIrrigation);
        }

        public void UpdateVegetationModule_CropRunoff(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).CropRunoff);
        }

        public void UpdateVegetationModule_SoilEvaporation(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).SoilEvaporation);
        }

        public void UpdateVegetationModule_CropTranspiration(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).CropTranspiration);
        }

        public void UpdateVegetationModule_CropEvapoTranspiration(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).CropEvapoTranspiration);
        }
        //Not currently set
        public void UpdateVegetationModule_CropDrainage(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).CropDrainage);
        }

        public void UpdateVegetationModule_CropLateralFlow(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).CropLateralFlow);
        }

        public void UpdateVegetationModule_CropOverflow(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).CropOverflow);
        }

        public void UpdateVegetationModule_CropSoilErosion(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).CropSoilErosion);
        }

        public void UpdateVegetationModule_CropSedimentDelivery(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).CropSedimentDelivery);
        }

        public void UpdateVegetationModule_PlantingCount(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).PlantingCount);
        }

        public void UpdateVegetationModule_HarvestCount(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).HarvestCount);
        }

        public void UpdateVegetationModule_CropDeathCount(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).CropDeathCount);
        }

        public void UpdateVegetationModule_FallowCount(HowLeakyOutputTimeseriesActive output, int index)
        {
            output.Update(index, VegetationModule(output.Index).FallowCount);
        }



    }
}
