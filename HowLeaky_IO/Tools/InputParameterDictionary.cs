using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_IO.Tools
{

    // GenerateRemapDictionary creates a map of XMLNodeName:CodeName pairs. When reading in the XML datafiles,
    // we create parameters based on the Xml Element names, but these are often different to the parameter names
    // in the code.
    public class InputParameterDictionary
    {
        static public Dictionary<string,string> Create()
        {
            var dict=new Dictionary<string,string>();


            //SOIL PARAMETERS
            dict.Add("HorizonCount","LayerCount");
            dict.Add("LayerDepth","Depths");
            dict.Add("InSituAirDryMoist","AirDryLimit");
            dict.Add("WiltingPoint","WiltingPoint");
            dict.Add("FieldCapacity","FieldCapacity");
            dict.Add("SatWaterCont","Saturation");
            dict.Add("MaxDailyDrainRate","MaxDailyDrainRate");
            dict.Add("BulkDensity","BulkDensity");
            dict.Add("Stage2SoilEvap_Cona","Stage2SoilEvapCona");
            dict.Add("Stage1SoilEvap_U","Stage1SoilEvapU");
            dict.Add("RunoffCurveNumber","RunoffCurveNumber");
            dict.Add("RedInCNAtFullCover","RedInCNAtFullCover");
            dict.Add("MaxRedInCNDueToTill","MaxRedInCNDueToTill");
            dict.Add("RainToRemoveRough","RainToRemoveRough");
            dict.Add("USLE_K","USLEK");
            dict.Add("USLE_P","USLEP");
            dict.Add("SlopeLength","SlopeLength");
            dict.Add("FieldSlope","FieldSlope");
            dict.Add("RillRatio","RillRatio");            
            dict.Add("PAWC","PAWC");
            dict.Add("MaxDailyDrainVolume","MaxDailyDrainVolume");
            dict.Add("SoilCrack","SoilCrack");
            dict.Add("MaxInfiltIntoCracks","MaxInfiltIntoCracks");
            dict.Add("SedDelivRatio","SedDelivRatio");


            // CROP PARAMETERS
            dict.Add("PotMaxLai","PotMaxLAI");
            dict.Add("PropGrowSeaForMaxLai","PropGrowSeaForMaxLai");
            dict.Add("PercentOfMaxLai1","PercentOfMaxLai1");
            dict.Add("PercentOfGrowSeason1","PercentOfGrowSeason1");
            dict.Add("PercentOfMaxLai2","PercentOfMaxLai2");
            dict.Add("PercentOfGrowSeason2","PercentOfGrowSeason2");
            dict.Add("SWPropForNoStress","SWPropForNoStress");
            dict.Add("DegreeDaysPlantToHarvest","DegreeDaysPlantToHarvest");
            dict.Add("SenesenceCoef","SenesenceCoef");
            dict.Add("RadUseEffic","RadUseEffic");
            dict.Add("HarvestIndex","HarvestIndex");
            dict.Add("BaseTemp","BaseTemp");
            dict.Add("OptTemp","OptTemp");
            dict.Add("MaxRootDepth","MaxRootDepth");
            dict.Add("DailyRootGrowth","DailyRootGrowth");
            dict.Add("WatStressForDeath","WatStressForDeath");
            dict.Add("DaysOfStressToDeath","DaysOfStressToDeath");
            dict.Add("MaxResidueLoss","MaxResidueLoss");
            dict.Add("BiomassAtFullCover","BiomassAtFullCover");
            dict.Add("PropGGDEnd","PropGDDEnd");
            dict.Add("PlantingFormat","PlantingRulesOptions");
            dict.Add("StartPlantWindow","PlantingWindowStartDate");
            dict.Add("EndPlantWindow","PlantingWindowEndDate");
            dict.Add("PlantingWindowEndDate","PlantingWindowEndDate");
            dict.Add("PlantDate","PlantDate");
            dict.Add("ForcePlanting","ForcePlantingAtEndOfWindow");
            dict.Add("MultiPlantInWindow","MultiPlantInWindow");
            dict.Add("RotationOptions","RotationFormat");
            dict.Add("MinContinuousRotations","MinRotationCount");
            dict.Add("MaxContinuousRotations","MaxRotationCount");
            dict.Add("MinYearsBetweenSowing","RestPeriodAfterChangingCrops");
            dict.Add("PlantingDates","PlantingSequence");
            dict.Add("FallowSwitch","FallowSwitch");
            dict.Add("MinFallowLength","MinimumFallowPeriod");
            dict.Add("MinimumFallowPeriod","MinimumFallowPeriod");
            dict.Add("RainfallSwitch","PlantingRainSwitch");
            dict.Add("PlantingRain","RainfallPlantingThreshold");
            dict.Add("DaysToTotalRain","RainfallSummationDays");
            dict.Add("SowingDelay","SowingDelay");
            dict.Add("SoilWaterSwitch","SoilWaterSwitch");
            dict.Add("MinSoilWaterRatio","MinSoilWaterTopLayer");
            dict.Add("MaxSoilWaterRatio","MaxSoilWaterTopLayer");
            dict.Add("MaxSoilWaterTopLayer","MaxSoilWaterTopLayer");
            dict.Add("AvailSWAtPlanting","SoilWaterReqToPlant");
            dict.Add("SoilWaterReqToPlant","SoilWaterReqToPlant");
            dict.Add("SoilDepthToSumPlantingSW","DepthToSumPlantingWater");
            dict.Add("DepthToSumPlantingWater","DepthToSumPlantingWater");
            dict.Add("RatoonCrop","RatoonSwitch");
            dict.Add("RatoonCount","NumberOfRatoons");
            dict.Add("RatoonScaleFactor","ScalingFactorForRatoons");
            dict.Add("Waterlogging","WaterLoggingSwitch");
            dict.Add("WaterLoggingFactor1","WaterLoggingFactor1");
            dict.Add("WaterLoggingFactor2","WaterLoggingFactor2");


            dict.Add("PlantDay","PlantDay");
            dict.Add("DaysFromPlaningToHarvest","DaysFromPlaningToHarvest");
           // dict.Add("SWPropForNoStress","SWPropForNoStress");
            dict.Add("CoverInputOptions","CoverDataType");
            dict.Add("CropFactorMatrix","CoverProfile");
            //dict.Add("SourceData","SourceData
            dict.Add("GreenCoverTimeSeries","GreenCoverTimeSeries");
            dict.Add("ResidueCoverTimeSeries","ResidueCoverTimeSeries");
            dict.Add("RootDepthTimeSeries","RootDepthTimeSeries");
            dict.Add("WaterUseEffic","TranspirationEfficiency");            
            dict.Add("PanHarvestIndex","HarvestIndex");
            dict.Add("GreenBioMassToCoverFactor","GreenCoverMultiplier");
            dict.Add("ResidueBioMassToCoverFactor","ResidueCoverMultiplier");
            dict.Add("RootBioMassToDepthFactor","RootDepthMultiplier");
            dict.Add("PanPlantDay","PlantDay");
            dict.Add("DaysPlantingToHarvest","DaysPlantingToHarvest");
            dict.Add("MaxAllowTotalCover","MaxAllowTotalCover");
            
            
            dict.Add("LinkToGreenCover","GreenCoverTimeSeries");
            dict.Add("LinkToResidueCover","ResidueCoverTimeSeries");
            dict.Add("LinkToRootDepth","RootDepthTimeSeries");

            dict.Add("ModelType","ModelType");            
            dict.Add("SourceData","SourceData");
            dict.Add("PlantingSequence","PlantingSequence");


            //TILLAGE DATA
            dict.Add("TillageType","Type");
            dict.Add("TillageFormat","Format");
            dict.Add("PrimaryTillType", "PrimaryTillType");
            dict.Add("PrimaryCropResMultiplier","PrimaryCropResMultiplier");
            dict.Add("PrimaryRoughnessRatio","PrimaryRoughnessRatio");


            dict.Add("StartTillWindow","StartTillWindow");	
            dict.Add("EndTillWindow","EndTillWindow");   
           
            dict.Add("RainForPrimaryTill","RainForPrimaryTill");
            dict.Add("NoDaysToTotalRain","NoDaysToTotalRain");
            dict.Add("MinDaysBetweenTills","MinDaysBetweenTills");
            dict.Add("TillageDate1","PrimaryTillDate");
            dict.Add("TillageDate2","SecondaryTillDate1");
            dict.Add("TillageDate3","SecondaryTillDate2");
            dict.Add("TillageDate4","SecondaryTillDate3");

            dict.Add("PrimaryTillDate","PrimaryTillDate");
            dict.Add("SecondaryTillDate1","SecondaryTillDate1");
            dict.Add("SecondaryTillDate2","SecondaryTillDate2");
            dict.Add("SecondaryTillDate3","SecondaryTillDate3");


            dict.Add("TillageDates","PrimaryTillageDates");
           


            //IRRIGATION DATA
             dict.Add("IrrigationFormat","IrrigationFormat");
            dict.Add("IrrigationAmount","FixedIrrigationAmount");
            dict.Add("IrrigationBufferPeriod","IrrigationBufferPeriod");
            dict.Add("IrrigationRunoffOptions","IrrigRunoffOptions");
            dict.Add("IrrigationRunoffProportion1","IrrigRunoffProportion1");
            dict.Add("IrrigationRunoffProportion2","IrrigRunoffProportion2");
            dict.Add("IrrigationRunoffSequence","IrrigRunoffSequence");
            dict.Add("tbIrrigationCoverEffects","IrrigCoverEffects");
            dict.Add("AdditionalInflowFormat","AdditionalInflowFormat");
            dict.Add("AdditionalInflow","AdditionalInflow");
            dict.Add("AdditionalInflowSequence","AdditionalInflowSequence");
        
           
            
            dict.Add("StartIrrigationWindow","IrrigWindowStartDate");
            dict.Add("EndIrrigationWindow","IrrigWindowEndDate");
            dict.Add("IrrigationDates","IrrigSequence");
            dict.Add("SWDToIrrigate","SWDToIrrigate");
            dict.Add("TargetAmountOptions","TargetAmountOptions");
            dict.Add("Ponding","UsePonding");
            dict.Add("UseRingTank","UseRingTank");
            dict.Add("RingTankDepth","RingTankDepth");
            dict.Add("RingTankArea","RingTankArea");
            dict.Add("CatchmentArea","CatchmentArea");
            dict.Add("IrrigatedArea","IrrigatedArea");
            dict.Add("RunoffCaptureRate","RunoffCaptureRate");
            dict.Add("RingTankSeepage","RingTankSeepageRate");
            dict.Add("RingTankEvapCoeficient","RingTankEvapCoefficient");
            dict.Add("IrrigationDeliveryEfficiency","IrrigDeliveryEfficiency");
            dict.Add("ResetRingTank","ResetRingTank");
            dict.Add("RingTankResetDate","ResetRingTankDate");
            //dict.Add("RingTankResetDate","RingTankResetMonth");
            dict.Add("CapactityAtReset","CapactityAtReset");
            dict.Add("IrrigationEvaporationOptions","EvaporationOptions");
            dict.Add("IrrigationEvaporationProportion","EvaporationProportion");



            //PESTICIDE 
            dict.Add("PestApplicationTiming","ApplicationTiming");
            dict.Add("ApplicationDate","ApplicationDate");
            dict.Add("ProductRate","ProductRate");
            dict.Add("SubsequentProductRate","SubsequentProductRate");
            dict.Add("PesticideDatesAndRates","PestApplicationDateList");            
            dict.Add("TriggerGGDFirst","TriggerGGDFirst");
            dict.Add("TriggerGGDSubsequent","TriggerGGDSubsequent");
            dict.Add("TriggerDaysFirst","TriggerDaysFirst");
            dict.Add("TriggerDaysSubsequent","TriggerDaysSubsequent");
            dict.Add("PestApplicationPosition","ApplicationPosition");
            dict.Add("HalfLifeVeg","HalfLifeVeg");
            dict.Add("RefTempHalfLifeVeg","RefTempHalfLifeVeg");
            dict.Add("HalfLifeStubble","HalfLifeStubble");
            dict.Add("RefTempHalfLifeStubble","RefTempHalfLifeStubble");
            dict.Add("HalfLife","HalfLifeSoil");
            dict.Add("RefTempHalfLifeSoil","RefTempHalfLifeSoil");
            dict.Add("DegradationActivationEnergy","DegradationActivationEnergy");
            dict.Add("BandSpraying","BandSpraying");
            dict.Add("ConcActiveIngred","ConcActiveIngred");
            dict.Add("PestEfficiency","PestEfficiency");
            dict.Add("MixLayerThickness","MixLayerThickness");
            dict.Add("SorptionCoefficient","SorptionCoefficient");
            dict.Add("ExtractCoefficient","ExtractCoefficient");
            dict.Add("CoverWashoffFraction","CoverWashoffFraction");
            dict.Add("CritPestConc","CritPestConc");
            dict.Add("tbPestVegIndex1","tbPestVegIndex1");
             dict.Add("tbPestVegIndex2","tbPestVegIndex2");
            dict.Add("tbPestVegIndex3","tbPestVegIndex3");
            dict.Add("tbPestVegIndex4","tbPestVegIndex4");
            dict.Add("tbPestVegIndex5","tbPestVegIndex5");
            dict.Add("tbPestVegIndex6","tbPestVegIndex6");
            dict.Add("tbPestVegIndex7","tbPestVegIndex7");
            dict.Add("tbPestVegIndex8","tbPestVegIndex8");
            dict.Add("tbPestVegIndex9","tbPestVegIndex9");
            dict.Add("tbPestVegIndex10","tbPestVegIndex10");


            //PHOSPHORUS 
            dict.Add("TotalPConc","TotalPConc");
            dict.Add("ColwellP","ColwellP");
            //dict.Add("PhosphorusType","PhosphorusType");
            dict.Add("DissolvedPOption","DissolvedPOpt");
            dict.Add("PEnrichmentOption","PEnrichmentOpt");
            dict.Add("PBI","PBI");
            //dict.Add("DissolvedPOption","PEnrichmentOption");
            dict.Add("EnrichmentRatio","EnrichmentRatio");
            dict.Add("ClayPercentage","ClayPercentage");


            //NITRATE 
            dict.Add("FertilizerInputDateSequences", "FertilizerInputDateSequences");
            dict.Add("DissolvedNinRunoffOptions", "DissolvedNinRunoffOptions");
            dict.Add("SoilNLoadData1", "SoilNLoadData1");
            dict.Add("NDepthTopLayer1", "NDepthTopLayer1");
            dict.Add("SoilNitrateLoadWeighting1", "SoilNitrateLoadWeighting1");
            dict.Add("Nk", "Nk");
            dict.Add("Ncv", "Ncv");
            dict.Add("NAlpha_Disolved", "NAlpha_Disolved");
            dict.Add("N_DanRat_MaxRunOffConc", "N_DanRat_MaxRunOffConc");
            dict.Add("N_DanRat_MinRunOffConc", "N_DanRat_MinRunOffConc");
            dict.Add("NBeta_Disolved", "NBeta_Disolved");
            dict.Add("ParticulateNinRunoffOptions", "ParticulateNinRunoffOptions");
            dict.Add("SoilNLoadData3", "SoilNLoadData3");
            dict.Add("NDepthTopLayer2", "NDepthTopLayer2");
            dict.Add("SoilNitrateLoadWeighting3", "SoilNitrateLoadWeighting3");
            dict.Add("NAlpha_Particulate", "NAlpha_Particulate");
            dict.Add("NBeta_Particulate", "NBeta_Particulate");
            dict.Add("NEnrichmentRatio", "NEnrichmentRatio");
            dict.Add("DissolvedNinLeachingOptions", "DissolvedNinLeachingOptions");
            dict.Add("SoilNLoadData2", "SoilNLoadData2");
            dict.Add("DepthBottomLayer", "DepthBottomLayer");
            dict.Add("SoilNitrateLoadWeighting2", "SoilNitrateLoadWeighting2");
            dict.Add("NitrateLeachingEfficiency", "NitrateLeachingEfficiency");
            dict.Add("PlantAnnual", "PlantAnnual");
            dict.Add("PlantA", "PlantA");
            dict.Add("PlantB", "PlantB");
            dict.Add("MainStemDuration", "MainStemDuration");
            dict.Add("CMRMax", "CMRMax");
            dict.Add("CMRSlope", "CMRSlope");
            dict.Add("Denitrification", "Denitrification");
            dict.Add("NitrateDrainageRetention", "NitrateDrainageRetention");
            

            //dict.Add("Mineralisation","Mineralisation");

            // dict.Add("VolSat","VolSat");

            //  dict.Add("NitrogenApplication","NitrogenApplication");

            // dict.Add("NitrogenFrequency","NitrogenFrequency");
            //dict.Add("SoilCarbon","SoilCarbon");

            // dict.Add("InitialExcessN","InitialExcessN");



            //dict.Add("DepthBottomLayer","DepthBottomLayer");
            //dict.Add("NitrateLeachingEfficiency","NitrateLeachingEfficiency");

            //dict.Add("NLoadInSurfaceLayerTimeSeries","NLoadInSurfaceLayerTimeSeries");
            //dict.Add("NLoadInLowerLayersTimeSeries","NLoadInLowerLayersTimeSeries");

            //dict.Add("NDepthTopLayer2","NDepthTopLayer2");
            //dict.Add("NEnrichmentRatio","NEnrichmentRatio");
            // dict.Add("NAlpha","NAlpha");
            //dict.Add("SoilNitrateLoadWeighting1","SoilNitrateLoadWeighting1");
            //dict.Add("DissolvedNinLeachingOptions","DissolvedNinLeaching");
            //dict.Add("SoilNitrateLoadWeighting2","SoilNitrateLoadWeighting2");
            //dict.Add("ParticulateNinRunoffOptions","ParticulateNinRunoff");
            //dict.Add("NBeta","NBeta");
            //dict.Add("SoilNitrateLoadWeighting3","SoilNitrateLoadWeighting3");
            //dict.Add("NitrateSourceData","NitrateSourceData");
            //dict.Add("SoilNitrateTimeseries","SoilNitrateTimeseries");
            //dict.Add("InorganicNitrateNTimeseries","InorganicNitrateNTimeseries");
            //dict.Add("InorganicAmmoniumNTimeseries","InorganicAmmoniumNTimeseries");
            //dict.Add("OrganicNTimeseries","OrganicNTimeseries");
            //dict.Add("OrganicCarbon","OrganicCarbon");
            //dict.Add("Nbeta","NBeta"); 
            //dict.Add("PlantA","PlantA"); 
            //dict.Add("PlantB","PlantB"); 
            //dict.Add("PlantAnnual","PlantAnnual"); // This is something that Al did. 
            //dict.Add("RatoonA","RatoonA"); 
            //dict.Add("RatoonB","RatoonB"); 
            //dict.Add("RatoonAnnual","RatoonAnnual");// This is something that Al did. 
            //dict.Add("Denitrification","Denitrification"); 
            //dict.Add("NitrateDrainageRetention","NitrateDrainageRetention"); 
            //dict.Add("CNSlope","CNSlope"); 
            //dict.Add("CNMax","CNMax"); 
            //dict.Add("MainStemDuration","MainStemDuration"); 
            //dict.Add("SoilNLoadData3","SoilNLoadData3");


            //SOLUTES 
            dict.Add("InitialStartingConditionsOptions","StartConcOption");
            dict.Add("InitialSoilSoluteConcDefault","DefaultInitialConc");
            dict.Add("InitialSoilSoluteConc1","Layer1InitialConc");
            dict.Add("InitialSoilSoluteConc2","Layer2InitialConc");
            dict.Add("InitialSoilSoluteConc3","Layer3InitialConc");
            dict.Add("InitialSoilSoluteConc4","Layer4InitialConc");
            dict.Add("InitialSoilSoluteConc5","Layer5InitialConc");
            //dict.Add("InitialSoilSoluteConc6","SoluteLayerInitialConc6");
            //dict.Add("InitialSoilSoluteConc7","SoluteLayerInitialConc7");
            //dict.Add("InitialSoilSoluteConc8","SoluteLayerInitialConc8");
            //dict.Add("InitialSoilSoluteConc9","SoluteLayerInitialConc9");
            //dict.Add("InitialSoilSoluteConc10","SoluteLayerInitialConc10");
            dict.Add("SoluteRainfallConcentration","RainfallConcentration");
            dict.Add("SoluteIrrigaitonConcentration","IrrigationConcentration");
            dict.Add("SoluteMixingCoefficient","MixingCoefficient");
            
            
            //OPTIONS            
            dict.Add("ResetResidueMass","ResetResidueAtDate");
            dict.Add("ResetDateForResidue","ResetDayForResidue,ResetMonthForResidue");
            dict.Add("CropResResetValue","CropResidueResetValue");
            dict.Add("ResetSoilWater","ResetSoilWaterAtDate");
            dict.Add("ResetDateForSoilWater","ResetDayForSoilWater,ResetMonthForSoilWater");
            dict.Add("PercentPAWCAtDate","SoilWaterResetValueAtDate");
            dict.Add("UpdateSWAfterPlanting","UpdateSWAfterPlanting");
            dict.Add("PercentPAWCAtPlanting","SoilWaterResetValueAfterPlanting");
            dict.Add("CalculateLateralFlow","CanCalculateLateralFlow");
            dict.Add("IgnoreCropDeath","IgnoreCropKill");
            dict.Add("Use_PERFECT_DryMatter","Use_PERFECT_DryMatter");
            dict.Add("Use_PERFECT_GCovEqn","Use_PERFECT_GCovEqn");
            dict.Add("Use_PERFECT_PotSE","Use_PERFECT_SoilEvapFn");
            dict.Add("Use_PERFECT_DLAI","Use_PERFECT_DLAI");
            dict.Add("Use_PERFECT_Residue","Use_PERFECT_ResidueFunction");
            dict.Add("Use_PERFECT_USLE_LSFactor","Use_PERFECT_USLE_LSFactor");
            dict.Add("Use_PERFECT_CN","Use_PERFECT_CNFunction");
            dict.Add("InitialPAWC","InitialPAWC");

            return dict;
        }
    }
}
