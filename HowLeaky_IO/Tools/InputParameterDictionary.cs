using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_IO.Tools
{

    // GenerateRemapDictionary creates a map of XMLNodeName:CodeName pairs. When reading in the XML datafiles,
    // we create parameters based on the Xml Element names, but these are often different to the parameter names
    // in the code.

    //NOTE: ONLY THOSE PARAMETERS WHOSE XML NAME DIFFERS FROM ITS CODENAME ARE LISTED HERE
    public class InputParameterDictionary
    {
        static public Dictionary<string,string> Create()
        {
            var dict=new Dictionary<string,string>();
            //Note - this was changed on 23 March 2022 so that only parameters with differnent names in the XML file are listed

            //SOIL PARAMETERS
            dict.Add("HorizonCount","LayerCount");
            dict.Add("LayerDepth","Depths");
            dict.Add("InSituAirDryMoist","AirDryLimit");
            dict.Add("SatWaterCont","Saturation");
            dict.Add("Stage2SoilEvap_Cona","Stage2SoilEvapCona");
            dict.Add("Stage1SoilEvap_U","Stage1SoilEvapU");
            dict.Add("USLE_K","USLEK");
            dict.Add("USLE_P","USLEP");

            // CROP PARAMETERS
            dict.Add("PotMaxLai","PotMaxLAI");
            dict.Add("WatStressForDeath", "WaterStressThreshold");
            dict.Add("PlantingFormat","PlantingRulesOptions");
            dict.Add("StartPlantWindow","PlantingWindowStartDate");
            dict.Add("EndPlantWindow","PlantingWindowEndDate");
            dict.Add("ForcePlanting","ForcePlantingAtEndOfWindow");
            dict.Add("RotationOptions","RotationFormat");
            dict.Add("MinContinuousRotations","MinRotationCount");
            dict.Add("MaxContinuousRotations","MaxRotationCount");
            dict.Add("MinYearsBetweenSowing","RestPeriodAfterChangingCrops");
            dict.Add("PlantingDates","PlantingSequence");
            dict.Add("MinFallowLength","MinimumFallowPeriod");
            dict.Add("PropGGDEnd","PropGDDEnd");
            dict.Add("RainfallSwitch","PlantingRainSwitch");
            dict.Add("PlantingRain","RainfallPlantingThreshold");
            dict.Add("DaysToTotalRain","RainfallSummationDays");
            dict.Add("MinSoilWaterRatio","MinSoilWaterTopLayer");
            dict.Add("MaxSoilWaterRatio","MaxSoilWaterTopLayer");
            dict.Add("AvailSWAtPlanting","SoilWaterReqToPlant");
            dict.Add("SoilDepthToSumPlantingSW","DepthToSumPlantingWater");
            dict.Add("RatoonCrop","RatoonSwitch");
            dict.Add("RatoonCount","NumberOfRatoons");
            dict.Add("RatoonScaleFactor","ScalingFactorForRatoons");
            dict.Add("Waterlogging","WaterLoggingSwitch");
            dict.Add("CoverInputOptions","CoverDataType");
            dict.Add("CropFactorMatrix","CoverProfile");
            dict.Add("WaterUseEffic","TranspirationEfficiency");            
            dict.Add("PanHarvestIndex","HarvestIndex");
            dict.Add("GreenBioMassToCoverFactor","GreenCoverMultiplier");
            dict.Add("ResidueBioMassToCoverFactor","ResidueCoverMultiplier");
            dict.Add("RootBioMassToDepthFactor","RootDepthMultiplier");
            dict.Add("PanPlantDay","PlantDay");
            dict.Add("LinkToGreenCover","GreenCoverTimeSeries");
            dict.Add("LinkToResidueCover","ResidueCoverTimeSeries");
            dict.Add("LinkToRootDepth","RootDepthTimeSeries");

            //TILLAGE DATA
            dict.Add("TillageType","Type");
            dict.Add("TillageFormat","Format");
            dict.Add("TillageDate1","PrimaryTillDate");
            dict.Add("TillageDate2","SecondaryTillDate1");
            dict.Add("TillageDate3","SecondaryTillDate2");
            dict.Add("TillageDate4","SecondaryTillDate3");
            dict.Add("TillageDates","PrimaryTillageDates");

            //IRRIGATION DATA
            dict.Add("IrrigationAmount","FixedIrrigationAmount");
            dict.Add("IrrigationRunoffOptions","IrrigRunoffOptions");
            dict.Add("IrrigationRunoffProportion1","IrrigRunoffProportion1");
            dict.Add("IrrigationRunoffProportion2","IrrigRunoffProportion2");
            dict.Add("IrrigationRunoffSequence","IrrigRunoffSequence");
            dict.Add("tbIrrigationCoverEffects","IrrigCoverEffects");
            dict.Add("StartIrrigationWindow","IrrigWindowStartDate");
            dict.Add("EndIrrigationWindow","IrrigWindowEndDate");
            dict.Add("IrrigationDates","IrrigSequence");
            dict.Add("Ponding","UsePonding");
            dict.Add("RingTankSeepage","RingTankSeepageRate");
            dict.Add("RingTankEvapCoeficient","RingTankEvapCoefficient");
            dict.Add("IrrigationDeliveryEfficiency","IrrigDeliveryEfficiency");
            dict.Add("RingTankResetDate","ResetRingTankDate");
            dict.Add("IrrigationEvaporationOptions","EvaporationOptions");
            dict.Add("IrrigationEvaporationProportion","EvaporationProportion");

            //PESTICIDE 
            dict.Add("PestApplicationTiming","ApplicationTiming");
            dict.Add("PesticideDatesAndRates","PestApplicationDateList");            
            dict.Add("PestApplicationPosition","ApplicationPosition");
            dict.Add("HalfLife","HalfLifeSoil");
         
            //PHOSPHORUS 
            dict.Add("DissolvedPOption","DissolvedPOpt");
            dict.Add("PEnrichmentOption","PEnrichmentOpt");

            //SOLUTES 
            dict.Add("InitialStartingConditionsOptions","StartConcOption");
            dict.Add("InitialSoilSoluteConcDefault","DefaultInitialConc");
            dict.Add("InitialSoilSoluteConc1","Layer1InitialConc");
            dict.Add("InitialSoilSoluteConc2","Layer2InitialConc");
            dict.Add("InitialSoilSoluteConc3","Layer3InitialConc");
            dict.Add("InitialSoilSoluteConc4","Layer4InitialConc");
            dict.Add("InitialSoilSoluteConc5","Layer5InitialConc");
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
            dict.Add("PercentPAWCAtPlanting","SoilWaterResetValueAfterPlanting");
            dict.Add("CalculateLateralFlow","CanCalculateLateralFlow");
            dict.Add("IgnoreCropDeath","IgnoreCropKill");
            dict.Add("Use_PERFECT_PotSE","Use_PERFECT_SoilEvapFn");
            dict.Add("Use_PERFECT_Residue","Use_PERFECT_ResidueFunction");
            dict.Add("Use_PERFECT_CN","Use_PERFECT_CNFunction");
            return dict;
        }
    }
}
