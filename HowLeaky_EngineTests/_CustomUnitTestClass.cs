using HowLeaky_SimulationEngine.Engine;
using HowLeaky_SimulationEngine.Enums;
using HowLeaky_SimulationEngine.Inputs;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HowLeaky_Engine.UnitTests
{
    public class _CustomUnitTestClass
    {
        public bool ValuesAreEqual(double val1, double val2)
        {
            return Math.Abs(val1 - val2) < 0.000001;
        }



        public void InitialiseEngine(HowLeakyEngine engine)
        {
            engine.StartDate = new BrowserDate(2001, 1, 1);
            engine.EndDate = new BrowserDate(2003, 12, 31);
            engine.TodaysDate = engine.StartDate;
            engine.PrepareForNewSimulation();

        }

        public HowLeakyInputs_Climate FetchClimateInputs(HowLeakyEngine engine)
        {
            if (engine.ClimateModule is null)
            {
                engine.ClimateModule = new HowLeakyEngineModule_Climate()
                {
                    Engine = engine
                };
            }
            if (engine.ClimateModule.InputModel is null)
            {
                engine.ClimateModule.InputModel = new HowLeakyInputs_Climate();
            }
            if (engine.Modules is null)
            {
                engine.Modules = new List<_CustomHowLeakyEngineModule>();
            }
            engine.Modules.Add(engine.ClimateModule);
            return engine.ClimateModule.InputModel;
        }

        public HowLeakyInputs_Soil FetchSoilInputs(HowLeakyEngine engine)
        {
            if (engine.SoilModule is null)
            {
                engine.SoilModule = new HowLeakyEngineModule_Soil()
                {
                    Engine = engine
                };
            }
            if (engine.SoilModule.InputModel is null)
            {
                engine.SoilModule.InputModel = new HowLeakyInputs_Soil();
            }
            if (engine.Modules is null)
            {
                engine.Modules = new List<_CustomHowLeakyEngineModule>();
            }
            engine.Modules.Add(engine.SoilModule);
            return engine.SoilModule.InputModel;
        }

        public HowLeakyInputs_Solute FetchSolutesInputs(HowLeakyEngine engine)
        {
            if (engine.SolutesModule is null)
            {
                engine.SolutesModule = new HowLeakyEngineModule_Solutes()
                {
                    Engine = engine
                };
            }
            if (engine.SolutesModule.InputModel is null)
            {
                engine.SolutesModule.InputModel = new HowLeakyInputs_Solute();
            }
            if (engine.Modules is null)
            {
                engine.Modules = new List<_CustomHowLeakyEngineModule>();
            }
            engine.Modules.Add(engine.SolutesModule);
            return engine.SolutesModule.InputModel;
        }

        public HowLeakyInputs_Phosphorus FetchPhosphorusInputs(HowLeakyEngine engine)
        {
            if (engine.PhosphorusModule is null)
            {
                engine.PhosphorusModule = new HowLeakyEngineModule_Phosphorus()
                {
                    Engine = engine
                };
            }
            if (engine.PhosphorusModule.InputModel is null)
            {
                engine.PhosphorusModule.InputModel = new HowLeakyInputs_Phosphorus();
            }
            if (engine.Modules is null)
            {
                engine.Modules = new List<_CustomHowLeakyEngineModule>();
            }
            engine.Modules.Add(engine.PhosphorusModule);
            return engine.PhosphorusModule.InputModel;
        }

        public HowLeakyInputs_Nitrate FetchNitrateInputs(HowLeakyEngine engine)
        {
            if (engine.NitrateModule is null)
            {
                engine.NitrateModule = new HowLeakyEngineModule_Nitrate()
                {
                    Engine = engine
                };
            }
            if (engine.NitrateModule.InputModel is null)
            {
                engine.NitrateModule.InputModel = new HowLeakyInputs_Nitrate();
            }
            return engine.NitrateModule.InputModel;
        }

        public HowLeakyInputs_Irrigation FetchIrrigationInputs(HowLeakyEngine engine)
        {
            if (engine.IrrigationModule is null)
            {
                engine.IrrigationModule = new HowLeakyEngineModule_Irrigation()
                {
                    Engine = engine
                };
            }
            if (engine.IrrigationModule.InputModel is null)
            {
                engine.IrrigationModule.InputModel = new HowLeakyInputs_Irrigation();
            }
            return engine.IrrigationModule.InputModel;
        }

        public HowLeakyInputs_LAIVeg FetchLAIVegInputs(HowLeakyEngine engine)
        {
            if (engine.VegetationModules is null)
            {
                engine.VegetationModules = new List<_CustomHowLeakyEngine_VegModule>();
                var laivegmodule = new HowLeakyEngineModule_LAIVeg()
                {
                    Engine = engine
                };
                engine.VegetationModules.Add(laivegmodule);

            }
            var vegmodel = (HowLeakyEngineModule_LAIVeg)engine.VegetationModules.FirstOrDefault();
            if (vegmodel.InputModel is null)
            {
                vegmodel.InputModel = new HowLeakyInputs_LAIVeg();
            }
            if (engine.Modules is null)
            {
                engine.Modules = new List<_CustomHowLeakyEngineModule>();
            }
            engine.Modules.Add(vegmodel);
            return vegmodel.InputModel;
        }

        public HowLeakyInputs_CoverVeg FetchCoverVegInputs(HowLeakyEngine engine)
        {
            if (engine.VegetationModules is null)
            {
                engine.VegetationModules = new List<_CustomHowLeakyEngine_VegModule>();
                var laivegmodule = new HowLeakyEngineModule_CoverVeg()
                {
                    Engine = engine
                };
                engine.VegetationModules.Add(laivegmodule);

            }
            var vegmodel = (HowLeakyEngineModule_CoverVeg)engine.VegetationModules.FirstOrDefault();
            if (vegmodel.InputModel is null)
            {
                vegmodel.InputModel = new HowLeakyInputs_CoverVeg();
            }
            if (engine.Modules is null)
            {
                engine.Modules = new List<_CustomHowLeakyEngineModule>();
            }
            engine.Modules.Add(vegmodel);
            return vegmodel.InputModel;
        }

        public HowLeakyInputs_Pesticide FetchPesticideInputs(HowLeakyEngine engine)
        {
            if (engine.PesticideModules is null)
            {
                engine.PesticideModules = new List<HowLeakyEngineModule_Pesticide>();
                var pestmodule = new HowLeakyEngineModule_Pesticide()
                {
                    Engine = engine
                };
                engine.PesticideModules.Add(pestmodule);

            }
            var pestmodel = (HowLeakyEngineModule_Pesticide)engine.PesticideModules.FirstOrDefault();
            if (pestmodel.InputModel is null)
            {
                pestmodel.InputModel = new HowLeakyInputs_Pesticide();
            }
            if (engine.Modules is null)
            {
                engine.Modules = new List<_CustomHowLeakyEngineModule>();
            }
            engine.Modules.Add(pestmodel);
            return pestmodel.InputModel;
        }

        public void LoadClimate(HowLeakyEngine engine)
        {
            var inputs = FetchClimateInputs(engine);
            //  J   F   M   A   M   J   J   A   S   O   N   D
            var DaysInMonth = new List<int> { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            var Rain = new List<double> { 50, 52, 54, 56, 58, 60, 60, 58, 56, 54, 52, 50 };
            var Evap = new List<double> { 20, 18, 15, 12, 8, 4, 4, 8, 12, 15, 18, 20 };
            var MaxTemp = new List<double> { 30, 28, 24, 20, 16, 14, 14, 16, 20, 24, 28, 30 };
            var MinTemp = new List<double> { 15, 14, 12, 10, 8, 7, 7, 8, 10, 12, 14, 15 };
            var Radiation = new List<double> { 15, 14, 12, 10, 8, 7, 7, 8, 10, 12, 14, 15 };

            List<double> RainValues = new List<double>();
            List<double> EvapValues = new List<double>();
            List<double> MaxTempValues = new List<double>();
            List<double> MinTempValues = new List<double>();
            List<double> RadiationValues = new List<double>();
            for (int year = 2001; year < 2004; ++year)
            {
                for (int month = 1; month <= 12; ++month)
                {
                    int max = DaysInMonth[month - 1];
                    double rain = Rain[month - 1];
                    double evap = Evap[month - 1];
                    double maxtemp = MaxTemp[month - 1];
                    double mintemp = MinTemp[month - 1];
                    double radiation = Radiation[month - 1];
                    for (int day = 1; day <= max; ++day)
                    {
                        if (day == 1) { RainValues.Add(rain); }
                        else { RainValues.Add(0); }
                        EvapValues.Add(evap);
                        MinTempValues.Add(mintemp);
                        MaxTempValues.Add(maxtemp);
                        RadiationValues.Add(radiation);
                    }
                }
            }
            inputs.Rain = RainValues;
            inputs.PanEvap = EvapValues;
            inputs.MinT = MinTempValues;
            inputs.MaxT = MaxTempValues;
            inputs.Radiation = RadiationValues;
            inputs.StartDate = new BrowserDate(2001, 1, 1);
            inputs.EndDate = new BrowserDate(2003, 12, 31);
            engine.ClimateModule.Initialise();

        }

        public void LoadPesticide_FixedDate(HowLeakyEngine engine)
        {
            var inputs = FetchPesticideInputs(engine);
            inputs.Name="Atrazine";
            inputs.ApplicationTiming = 0;
            inputs.ApplicationDate = new DayMonthData(1,1);
            inputs.ProductRate = 2;
            inputs.ConcActiveIngred = 500;
            inputs.CritPestConc=500;
            inputs.SubsequentProductRate = 2;
            inputs.ApplicationPosition = 0;
            inputs.HalfLifeVeg = 14.5;
            inputs.HalfLifeSoil = 24.5;
            inputs.HalfLifeStubble = 29;
            inputs.RefTempHalfLifeVeg = 24.5;            
            inputs.RefTempHalfLifeStubble = 25;           
            inputs.RefTempHalfLifeSoil = 25;                        
            inputs.PestEfficiency = 100;
            inputs.DegradationActivationEnergy = 54900;
            inputs.MixLayerThickness = 20;
            inputs.SorptionCoefficient = 1.81;
            inputs.ExtractCoefficient = 0.02;
            inputs.CoverWashoffFraction = 0.45;
            inputs.BandSpraying = 100;
            inputs.PestApplicationDateList = null;
            inputs.TriggerGGDFirst = 0;
            inputs.TriggerGGDSubsequent = 0;
            inputs.TriggerDaysFirst = 0;
            inputs.TriggerDaysSubsequent = 0;
            

        }

        public void LoadSoil(HowLeakyEngine engine)
        {
            var inputs = FetchSoilInputs(engine);
            inputs.LayerCount = 6;
            inputs.Depths = new List<double> { 100, 300, 600, 900, 1200, 1800 };
            inputs.AirDryLimit = new List<double> { 12, 12, 33, 35, 35, 35 };
            inputs.WiltingPoint = new List<double> { 30, 30, 33, 35, 35, 35 };
            inputs.FieldCapacity = new List<double> { 55, 55, 54, 50, 50, 47 };
            inputs.Saturation = new List<double> { 70, 65, 62, 56, 56, 52 };
            inputs.BulkDensity = new List<double> { 1, 1.2, 1.4, 1.4, 1.4, 1.4 };
            inputs.MaxDailyDrainRate = new List<double> { 100, 50, 50, 50, 50, 5 };
            inputs.Stage2SoilEvapCona = 4;
            inputs.Stage1SoilEvapU = 4;
            inputs.RunoffCurveNumber = 82;
            inputs.RedInCNAtFullCover = 15;
            inputs.MaxRedInCNDueToTill = 0;
            inputs.RainToRemoveRough = 0;
            inputs.USLEK = 0.35;
            inputs.USLEP = 1;
            inputs.FieldSlope = 7;
            inputs.SlopeLength = 22;
            inputs.RillRatio = 1;
            //inputs.SoilCrack = false;
            //inputs.MaxInfiltIntoCracks = 0;
            inputs.SedDelivRatio = 0.14;
            engine.SoilModule.Initialise();
        }
        
        public void LoadCoverVeg(HowLeakyEngine engine)
        {
            var inputs = FetchCoverVegInputs(engine);
            //inputs.ModelType = 0;
            //inputs.SourceData = 0;
            inputs.PlantDay = 1;
            inputs.CoverDataType = 0;
            inputs.CoverProfile = new ProfileData("1,40,30,100|365,1,40,30,100");
            inputs.GreenCoverTimeSeries = null;
            inputs.ResidueCoverTimeSeries = null;
            inputs.RootDepthTimeSeries = null;
            inputs.TranspirationEfficiency = 0;
            inputs.HarvestIndex = 0;
            inputs.DaysPlantingToHarvest = 10000;
            inputs.GreenCoverMultiplier = 1;
            inputs.ResidueCoverMultiplier = 1;
            inputs.RootDepthMultiplier = 1;
          //  inputs.MaxAllowTotalCover = 100;
            inputs.MaxRootDepth = 2000;
            inputs.SWPropForNoStress = 0.3;
           // inputs.MaximumRootDepth = 2000;
            // engine.VegetationModules[0].Initialise();
        }
        public void LoadLAIVeg_FixedPlant(HowLeakyEngine engine)
        {
            var inputs = FetchLAIVegInputs(engine);
            inputs.Name = "Cotton Dalby";
            inputs.PotMaxLAI = 3.5;
            inputs.PropGrowSeaForMaxLai = 0.8;
            inputs.PercentOfGrowSeason1 = 5;
            inputs.PercentOfMaxLai1 = 20;
            inputs.PercentOfGrowSeason2 = 70;
            inputs.PercentOfMaxLai2 = 40;
            inputs.SWPropForNoStress = 0.3;
            inputs.DegreeDaysPlantToHarvest = 2100;
            inputs.SenesenceCoef = 0.2;
            inputs.RadUseEffic = 2;
            inputs.HarvestIndex = 0.1;
            inputs.BaseTemp = 10;
            inputs.OptTemp = 32;
            inputs.MaxRootDepth = 900;
            inputs.DailyRootGrowth = 15;
            inputs.WaterStressThreshold = 0.1;
            inputs.DaysOfStressToDeath = 21;
            inputs.PlantingRulesOptions = 1; //FIXED
            inputs.PlantDate = new DayMonthData(7, 10);
            inputs.RatoonSwitch = false;
            inputs.MaxResidueLoss = 5; //Was called Decomponsition Rate in old version
            inputs.BiomassAtFullCover = 10000;
            inputs.PropGDDEnd = 75;
            inputs.MaxRotationCount = 0;
            inputs.MinRotationCount = 0;
            inputs.WaterLoggingSwitch = false;
            inputs.WaterLoggingFactor1 = 0;
            inputs.WaterLoggingFactor2 = 0;
            inputs.RotationFormat = 0;
            inputs.PlantingWindowStartDate = new DayMonthData(1, 1);
            inputs.PlantingWindowEndDate = new DayMonthData(1, 1);

            inputs.MinimumFallowPeriod = 0;
            inputs.RainfallPlantingThreshold = 0;
            inputs.RainfallSummationDays = 0;
            inputs.SowingDelay = 0;
            inputs.RestPeriodAfterChangingCrops = 0;
            inputs.MinSoilWaterTopLayer = 0;
            inputs.MaxSoilWaterTopLayer = 0;
            inputs.SoilWaterReqToPlant = 0;
            inputs.DepthToSumPlantingWater = 0;
            inputs.FallowSwitch = false;
            inputs.PlantingRainSwitch = false;
            inputs.SoilWaterSwitch = false;
            inputs.NumberOfRatoons = 0;
            inputs.ScalingFactorForRatoons = 0;
            inputs.ForcePlantingAtEndOfWindow = false;
            inputs.MultiPlantInWindow = false;
            engine.VegetationModule(0).Initialise();
        }

        public void LoadSolutes_Constant(HowLeakyEngine engine)
        {
            var inputs = FetchSolutesInputs(engine);
            inputs.StartConcOption = 0;
            inputs.DefaultInitialConc = 80;
            inputs.RainfallConcentration = 50;
            inputs.IrrigationConcentration = 200;
            inputs.MixingCoefficient = 0.5;
            inputs.Layer1InitialConc = 0;
            inputs.Layer2InitialConc = 0;
            inputs.Layer3InitialConc = 0;
            inputs.Layer4InitialConc = 0;
            inputs.Layer5InitialConc = 0;
            //inputs.Layer6InitialConc = 0;
            //inputs.Layer7InitialConc = 0;
            //inputs.Layer8InitialConc = 0;
            //inputs.Layer9InitialConc = 0;
            //inputs.Layer10InitialConc = 0;
            engine.SolutesModule.Initialise();
        }

        //public void LoadNitrate_N03NInRuoff(HowLeakyEngine engine)
        //{
        //    var inputs = FetchNitrateInputs(engine);
        //    inputs.DissolvedNinRunoffOptions = DissolvedNinRunoffType.HowLeaky2012;
        //    inputs.NDepthTopLayer1 = 100;
        //    inputs.Nk = 0.3;
        //    inputs.Ncv = 0.2;
        //    inputs.NAlpha_Disolved = 1;
        //    inputs.NBeta_Disolved = 0;
        //    inputs.SoilNLoadData1 = new Sequence("1,10");
        //    inputs.SoilNitrateLoadWeighting1 = 1;

        //}

        //public void LoadNitrate_N03NInLeaching(HowLeakyEngine engine)
        //{
        //    var inputs = FetchNitrateInputs(engine);
        //    inputs.DissolvedNinLeachingOptions = DissolvedNinLeachingType.HowLeaky2012;
        //    inputs.DepthBottomLayer = 100;
        //    inputs.NitrateLeachingEfficiency = 0.5;

        //    inputs.SoilNLoadData2 = new Sequence("1,10");
        //    inputs.SoilNitrateLoadWeighting2 = 1;

        //}

        //public void LoadNitrate_ParticNInRunoff(HowLeakyEngine engine)
        //{
        //    var inputs = FetchNitrateInputs(engine);
        //    inputs.ParticulateNinRunoffOptions = ParticulateNinRunoffType.HowLeaky2012;
        //    inputs.NDepthTopLayer2 = 20;
        //    inputs.NEnrichmentRatio = 0.5;
        //    inputs.NAlpha_Particulate = 1;
        //    inputs.NBeta_Particulate = 0.5;
        //    inputs.SoilNLoadData3 = new Sequence("1,10");
        //    inputs.SoilNitrateLoadWeighting3 = 1;


        //}

        public void LoadIrrigation_Constant(HowLeakyEngine engine)
        {
            var inputs = FetchIrrigationInputs(engine);

            inputs.IrrigationFormat = IrrigationFormat.FromSequenceFile;
            inputs.IrrigWindowStartDate = new DayMonthData(1, 1);
            inputs.IrrigWindowEndDate = new DayMonthData(1, 12);
            inputs.IrrigSequence = new Sequence("1,100",false);

            inputs.SWDToIrrigate = 0;
            inputs.TargetAmountOptions = TargetAmountOptions.FixedAmount;
            inputs.FixedIrrigationAmount = 100;
            inputs.IrrigationBufferPeriod = 0;
            inputs.AdditionalInflowFormat = 0;
            inputs.UsePonding = false;
            inputs.UseRingTank = false;
            inputs.RingTankDepth = 0;
            inputs.RingTankArea = 0;
            inputs.CatchmentArea = 0;
            inputs.IrrigatedArea = 0;
            inputs.AdditionalInflow = 0;
            inputs.RunoffCaptureRate = 0;
            inputs.RingTankSeepageRate = 0;
            inputs.RingTankEvapCoefficient = 0;
            inputs.IrrigDeliveryEfficiency = 0;
            inputs.ResetRingTank = false;
            inputs.ResetRingTankDate = new DayMonthData(1, 1);

            inputs.CapactityAtReset = 0;

            inputs.AdditionalInflowSequence = new Sequence("1,1",false);
            inputs.IrrigRunoffOptions = 0;
            inputs.IrrigRunoffProportion1 = 0;
            inputs.IrrigRunoffProportion2 = 0;
            inputs.IrrigCoverEffects = 0;
            inputs.IrrigRunoffSequence = new Sequence("1,1",false);


            inputs.EvaporationProportion = 0;
            inputs.EvaporationOptions = IrrigationEvaporationOptions.Option1;
        }

        public void LoadIrrigation_RingTank(HowLeakyEngine engine)
        {
            var inputs = FetchIrrigationInputs(engine);

            inputs.IrrigationFormat = IrrigationFormat.FromSequenceFile;
            inputs.IrrigWindowStartDate = new DayMonthData(1, 1);
            inputs.IrrigWindowEndDate = new DayMonthData(1, 12);
            inputs.IrrigSequence = new Sequence("1,100",false);

            inputs.SWDToIrrigate = 0;
            inputs.TargetAmountOptions = TargetAmountOptions.FixedAmount;
            inputs.FixedIrrigationAmount = 100;
            inputs.IrrigationBufferPeriod = 0;
            inputs.AdditionalInflowFormat = 0;
            inputs.UsePonding = false;
            inputs.UseRingTank = true;
            inputs.RingTankDepth = 1;
            inputs.RingTankArea = 20;
            inputs.CatchmentArea = 0;
            inputs.IrrigatedArea = 10;
            inputs.AdditionalInflow = 1.5;
            inputs.RunoffCaptureRate = 0;
            inputs.RingTankSeepageRate = 1;
            inputs.RingTankEvapCoefficient = 1;
            inputs.IrrigDeliveryEfficiency = 50;
            inputs.ResetRingTank = false;
            inputs.ResetRingTankDate = new DayMonthData(1, 1);

            inputs.CapactityAtReset = 0;

            inputs.AdditionalInflowSequence = new Sequence("1,1",false);
            inputs.IrrigRunoffOptions = 0;
            inputs.IrrigRunoffProportion1 = 0;
            inputs.IrrigRunoffProportion2 = 0;
            inputs.IrrigCoverEffects = 0;
            inputs.IrrigRunoffSequence = new Sequence("1,1",false);


            inputs.EvaporationProportion = 0;
            inputs.EvaporationOptions = IrrigationEvaporationOptions.Option1;
        }
    }
}
