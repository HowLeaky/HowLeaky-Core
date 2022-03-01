using HowLeaky_SimulationEngine.Engine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_Engine.UnitTests
{
    [TestFixture]
    public class HowLeakyEngineTests : _CustomUnitTestClass
    {

        [Test]
        public void Test_SoilModule_1_Initialise()
        {
            var engine = new HowLeakyEngine();
            LoadSoil(engine);
            var check1 = ValuesAreEqual(engine.SoilModule.Sse1, 4);
            var check2 = ValuesAreEqual(engine.SoilModule.Sse2, 8.5);
            var check3 = ValuesAreEqual(engine.SoilModule.Dsr, 4.515625);
            var check4 = ValuesAreEqual(engine.SoilModule.PAWC, 300);
            //var check5 = ValuesAreEqual(engine.SoilModule.WF[0], 4);
            //var check5 = ValuesAreEqual(engine.SoilModule.WF[1], 4);
            //var check5 = ValuesAreEqual(engine.SoilModule.WF[2], 4);
            //var check5 = ValuesAreEqual(engine.SoilModule.WF[3], 4);
            //var check5 = ValuesAreEqual(engine.SoilModule.WF[4], 4);
            //var check5 = ValuesAreEqual(engine.SoilModule.WF[5], 4);
            Assert.IsTrue(check1 && check2 && check3 && check4);
        }
        
        [Test]
        public void Test_SoilModule_2_VolSat()
        {
            var engine = new HowLeakyEngine();
            LoadSoil(engine);
            var result = engine.SoilModule.VolSat;
            var check1 = ValuesAreEqual(result, 1034);
            
            Assert.IsTrue(check1 );
        }
        
        [Test]
        public void Test_SoilModule_3_SetStartOfDayParameters()
        {
            var engine = new HowLeakyEngine();
      
            LoadSoil(engine);
            LoadClimate(engine);
            engine.ClimateModule.Rain = 10;
            engine.SetStartOfDayParameters();
            var check1 = ValuesAreEqual(engine.SoilModule.EffectiveRain,10);
            var check2 = ValuesAreEqual(engine.SoilModule.Satd, 275);
            var check3 = ValuesAreEqual(engine.SoilModule.SoilWaterDeficit, 150);
            
            Assert.IsTrue(check1 && check2 && check3 );
        }
        
        [Test]
        public void Test_SoilModule_4_CalculateResidue()
        {
            var engine = new HowLeakyEngine();
            LoadSoil(engine);
            LoadCoverVeg(engine);
            LoadClimate(engine);
            engine.ClimateModule.Rain = 10;
            InitialiseEngine(engine);
            engine.SetStartOfDayParameters();           
            engine.VegetationModules[0].SetStartOfDayParameters();
            engine.CalculateResidue();
           
            var check1 = ValuesAreEqual(engine.SoilModule.TotalCropResidue, 0);
            var check2 = ValuesAreEqual(engine.SoilModule.TotalResidueCover, 0.3);
            var check3 = ValuesAreEqual(engine.SoilModule.TotalResidueCoverPercent, 30);
            var check4 = ValuesAreEqual(engine.SoilModule.TotalCoverAllCrops, 0.58000000000000007);
            var check5 = ValuesAreEqual(engine.SoilModule.CropCover, 0.4);
            var check6 = ValuesAreEqual(engine.SoilModule.TotalCoverPercent, 58.000000000000007);
            var check7 = ValuesAreEqual(engine.SoilModule.AccumulatedCover, 58.000000000000007);
           
            Assert.IsTrue(check1 && check2 && check3 && check4 && check5 && check6 && check7);
        }
        // [Test]
        // public void Test_SoilModule_5_TryModelSoilCracking()
        // {
        //     var engine = new HowLeakyEngine();
        //     LoadSoil(engine);
        //     
        //     var check1 = ValuesAreEqual(engine.SoilModule.Sse1, 4);
        //     var check2 = ValuesAreEqual(engine.SoilModule.Sse2, 21);
        //     var check3 = ValuesAreEqual(engine.SoilModule.Dsr, 27.5625);
        //     var check4 = ValuesAreEqual(engine.SoilModule.Sse1, 4);
        //     //var check5 = ValuesAreEqual(engine.SoilModule.WF[0], 4);
        //     //var check5 = ValuesAreEqual(engine.SoilModule.WF[1], 4);
        //     //var check5 = ValuesAreEqual(engine.SoilModule.WF[2], 4);
        //     //var check5 = ValuesAreEqual(engine.SoilModule.WF[3], 4);
        //     //var check5 = ValuesAreEqual(engine.SoilModule.WF[4], 4);
        //     //var check5 = ValuesAreEqual(engine.SoilModule.WF[5], 4);
        //     Assert.IsTrue(check1 && check2 && check3 && check4);
        // }
        [Test]
        public void Test_SoilModule_6_CalculateRunoff()
        {
            var engine = new HowLeakyEngine();
            LoadSoil(engine);
            LoadClimate(engine);

            engine.SoilModule.EffectiveRain = 100;
            engine.SoilModule.CalculateRunoff();
            var check1 = ValuesAreEqual(engine.SoilModule.RunoffRetentionNumber, 71);
            var check2 = ValuesAreEqual(engine.SoilModule.RunoffCurveNo, 82);
            var check3 = ValuesAreEqual(engine.SoilModule.Runoff, 46.949234693877543);
            var check4 = ValuesAreEqual(engine.SoilModule.Infiltration, 53.050765306122457);
            Assert.IsTrue(check1 && check2 && check3 && check4);
        }
        [Test]
        public void Test_SoilModule_7_CalculateSoilEvaporation()
        {
            var engine = new HowLeakyEngine();
            engine.InitialPAW=0.8;
            LoadSoil(engine);
            LoadClimate(engine);
            LoadCoverVeg(engine);
            InitialiseEngine(engine);
            engine.SetStartOfDayParameters();
            engine.VegetationModules[0].SetStartOfDayParameters();
            engine.ClimateModule.PanEvap = 15;
            engine.SoilModule.TotalCropResidue = 30;
            engine.SoilModule.Infiltration = 10;
            engine.SoilModule.CalculateSoilEvaporation();
            var check1 = ValuesAreEqual(engine.SoilModule.Sse1, 4);
            var check2 = ValuesAreEqual(engine.SoilModule.Dsr, 0.257371080055702);
            var check3 = ValuesAreEqual(engine.SoilModule.Se21, 2.02927013502176);
            var check4 = ValuesAreEqual(engine.SoilModule.Se22, 0);
            var check5 = ValuesAreEqual(engine.SoilModule.Se2, 2.02927013502176);
            var check6 = ValuesAreEqual(engine.SoilModule.Sse2, 2.02927013502176);
            var check7 = ValuesAreEqual(engine.SoilModule.SoilEvap, 6.0292701350217595);
            var check8 = ValuesAreEqual(engine.SoilModule.PotSoilEvap, 7.3821168917029336);
            Assert.IsTrue(check1 && check2 && check3 && check4 && check5 && check6 && check7 && check8);
        }
        
        [Test]
        public void Test_SoilModule_8a_UpdateWaterBalance()
        {
            var engine = new HowLeakyEngine();
            engine.InitialPAW = 0.8;
            LoadSoil(engine);
            LoadClimate(engine);
            LoadCoverVeg(engine);
            InitialiseEngine(engine);
            engine.SetStartOfDayParameters();
            engine.VegetationModules[0].SetStartOfDayParameters();
            engine.ClimateModule.PanEvap = 15;
            engine.SoilModule.TotalCropResidue = 30;
            engine.SoilModule.Infiltration = 90;
            engine.SoilModule.EffectiveRain= 100;
            engine.SoilModule.CalculateSoilEvaporation();
            engine.SoilModule.UpdateWaterBalance();
            var checkSW1 = ValuesAreEqual(engine.SoilModule.SoilWaterRelWP[0], 25);
            var checkSW2 = ValuesAreEqual(engine.SoilModule.SoilWaterRelWP[1], 68.970729864978239);
            var checkSW3 = ValuesAreEqual(engine.SoilModule.SoilWaterRelWP[2], 63);
            var checkSW4 = ValuesAreEqual(engine.SoilModule.SoilWaterRelWP[3], 45);
            var checkSW5 = ValuesAreEqual(engine.SoilModule.SoilWaterRelWP[4], 45);
            var checkSW6 = ValuesAreEqual(engine.SoilModule.SoilWaterRelWP[5], 75.571428571428569);
            
            var checkD1 = ValuesAreEqual(engine.SoilModule.Drainage[0], 78.970729864978239);
            var checkD2 = ValuesAreEqual(engine.SoilModule.Drainage[1], 50);
            var checkD3 = ValuesAreEqual(engine.SoilModule.Drainage[2], 37.400000000000006);
            var checkD4 = ValuesAreEqual(engine.SoilModule.Drainage[3], 28.400000000000006);
            var checkD5 = ValuesAreEqual(engine.SoilModule.Drainage[4], 19.400000000000006);
            var checkD6 = ValuesAreEqual(engine.SoilModule.Drainage[5], 1.4285714285714284);
            
            var checkWB1 = ValuesAreEqual(engine.SoilModule.TotalSoilWater, 322.54215843640679);
            var checkWB2 = ValuesAreEqual(engine.SoilModule.SaturationIndex, 0);
            var checkWB3 = ValuesAreEqual(engine.SoilModule.Overflow, 0);
            var checkWB4 = ValuesAreEqual(engine.SoilModule.Runoff, 0);
            var checkWB5 = ValuesAreEqual(engine.SoilModule.Infiltration, 90);
            
            var check1=(checkSW1 && checkSW2 && checkSW3 && checkSW4 && checkSW5 && checkSW6);
            var check2=(checkD1 && checkD2 && checkD3 && checkD4 && checkD5 && checkD6);
            var check3=(checkWB1 && checkWB2 && checkWB3 && checkWB4 && checkWB5 );
            Assert.IsTrue(check1 && check2 && check3 );
        }


        [Test]
        public void Test_SoilModule_8b_UpdateWaterBalance()
        {
            var engine = new HowLeakyEngine();
            engine.InitialPAW = 0.8;
            LoadSoil(engine);
            LoadClimate(engine);
            LoadCoverVeg(engine);
            InitialiseEngine(engine);
            engine.SetStartOfDayParameters();
            engine.VegetationModules[0].SetStartOfDayParameters();
            engine.ClimateModule.PanEvap = 15;
            engine.SoilModule.TotalCropResidue = 30;
            engine.SoilModule.Infiltration = 180;
            engine.SoilModule.EffectiveRain = 200;
            engine.SoilModule.CalculateSoilEvaporation();
            engine.SoilModule.UpdateWaterBalance();
            var checkSW1 = ValuesAreEqual(engine.SoilModule.SoilWaterRelWP[0], 40);
            var checkSW2 = ValuesAreEqual(engine.SoilModule.SoilWaterRelWP[1], 70);
            var checkSW3 = ValuesAreEqual(engine.SoilModule.SoilWaterRelWP[2], 63);
            var checkSW4 = ValuesAreEqual(engine.SoilModule.SoilWaterRelWP[3], 45);
            var checkSW5 = ValuesAreEqual(engine.SoilModule.SoilWaterRelWP[4], 45);
            var checkSW6 = ValuesAreEqual(engine.SoilModule.SoilWaterRelWP[5], 75.571428571428569);

            var checkD1 = ValuesAreEqual(engine.SoilModule.Drainage[0], 100);
            var checkD2 = ValuesAreEqual(engine.SoilModule.Drainage[1], 50);
            var checkD3 = ValuesAreEqual(engine.SoilModule.Drainage[2], 37.400000000000006);
            var checkD4 = ValuesAreEqual(engine.SoilModule.Drainage[3], 28.400000000000006);
            var checkD5 = ValuesAreEqual(engine.SoilModule.Drainage[4], 19.400000000000006);
            var checkD6 = ValuesAreEqual(engine.SoilModule.Drainage[5], 1.4285714285714284);

            var checkWB1 = ValuesAreEqual(engine.SoilModule.TotalSoilWater, 338.57142857142856);
            var checkWB2 = ValuesAreEqual(engine.SoilModule.SaturationIndex, 1);
            var checkWB3 = ValuesAreEqual(engine.SoilModule.Overflow, 73.970729864978239);
            var checkWB4 = ValuesAreEqual(engine.SoilModule.Runoff, 73.970729864978239);
            var checkWB5 = ValuesAreEqual(engine.SoilModule.Infiltration, 106.02927013502176);

            var check1 = (checkSW1 && checkSW2 && checkSW3 && checkSW4 && checkSW5 && checkSW6);
            var check2 = (checkD1 && checkD2 && checkD3 && checkD4 && checkD5 && checkD6);
            var check3 = (checkWB1 && checkWB2 && checkWB3 && checkWB4 && checkWB5);
            Assert.IsTrue(check1 && check2 && check3);
        }


        [Test]
        public void Test_SoilModule_9_CalculateErosion()
        {
            var engine = new HowLeakyEngine();
            LoadSoil(engine);
            LoadCoverVeg(engine);
            LoadClimate(engine);
            engine.ClimateModule.Rain = 30;
            engine.SoilModule.Runoff=10;
            InitialiseEngine(engine);
            engine.SetStartOfDayParameters();
            engine.VegetationModules[0].SetStartOfDayParameters();
            engine.CalculateResidue();
            engine.SoilModule.CalculateErosion();
            var check1 = ValuesAreEqual(engine.SoilModule.SedimentConc, 0.41018378655449816);
            var check2 = ValuesAreEqual(engine.SoilModule.HillSlopeErosion, 0.29298841896749866);
            var check3 = ValuesAreEqual(engine.SoilModule.PeakSedConc, 0.41018378655449816);
            var check4 = ValuesAreEqual(engine.SoilModule.OffSiteSedDelivery, 0.041018378655449814);

            Assert.IsTrue(check1 && check2 && check3 && check4);
        }
        
        // [Test]
        // public void Test_SoilModule_10_TryModelLateralFlow()
        // {
        //     var engine = new HowLeakyEngine();
        //     LoadSoil(engine);
        //     var check1 = ValuesAreEqual(engine.SoilModule.Sse1, 4);
        //     var check2 = ValuesAreEqual(engine.SoilModule.Sse2, 21);
        //     var check3 = ValuesAreEqual(engine.SoilModule.Dsr, 27.5625);
        //     var check4 = ValuesAreEqual(engine.SoilModule.Sse1, 4);
        //     //var check5 = ValuesAreEqual(engine.SoilModule.WF[0], 4);
        //     //var check5 = ValuesAreEqual(engine.SoilModule.WF[1], 4);
        //     //var check5 = ValuesAreEqual(engine.SoilModule.WF[2], 4);
        //     //var check5 = ValuesAreEqual(engine.SoilModule.WF[3], 4);
        //     //var check5 = ValuesAreEqual(engine.SoilModule.WF[4], 4);
        //     //var check5 = ValuesAreEqual(engine.SoilModule.WF[5], 4);
        //     Assert.IsTrue(check1 && check2 && check3 && check4);
        // }        
        
        [Test]
        public void Test_SoluteModel_1_Constant()
        {
            var engine = new HowLeakyEngine();
            engine.InitialPAW = 1;
            LoadSoil(engine);
            LoadClimate(engine);
            LoadSolutes_Constant(engine);
           
            
            engine.ClimateModule.Rain=100;
            engine.SoilModule.Seepage[0]=1;
            engine.SoilModule.Seepage[1] = 1;
            engine.SoilModule.Seepage[2] = 1;
            engine.SoilModule.Seepage[3] = 1;
            engine.SoilModule.Seepage[4] = 1;
            engine.SoilModule.Seepage[5] = 1;
            engine.SoilModule.Seepage[6] = 1;
            engine.SolutesModule.Initialise();
            engine.TryModelSolutes();
            bool check1 = ValuesAreEqual(engine.SolutesModule.total_soil_solute_mg_per_kg, 82.000440517424522);
            bool check2 = ValuesAreEqual(engine.SolutesModule.total_soil_solute_mg_per_L, 220.11119346811424);
            bool check3 = ValuesAreEqual(engine.SolutesModule.solute_leaching_load_kg_per_ha, 1.1892513748417488);
            bool check4 = ValuesAreEqual(engine.SolutesModule.solute_leaching_conc_mg_per_L, 238.27199531403838);
            Assert.IsTrue(check1 && check2 && check3 && check4);
        }

        //[Test]
        //public void Test_NitrateModel_1_CalculateDissolvedNInRunoff()
        //{
        //    var engine = new HowLeakyEngine();           
        //    LoadClimate(engine);
        //    LoadSoil(engine);                        
        //    LoadNitrate_N03NInRuoff(engine);
        //    InitialiseEngine(engine);
        //    engine.SoilModule.Runoff=5;
        //    engine.NitrateModule.CalculateDissolvedNInRunoff();
        //    bool check1 = ValuesAreEqual(engine.NitrateModule.NO3NStoreTopLayer, 10);
        //    bool check2 = ValuesAreEqual(engine.NitrateModule.NO3NDissolvedInRunoff, 1.896361676485673);
        //    bool check3 = ValuesAreEqual(engine.NitrateModule.NO3NRunoffLoad, 0.094818083824283653);
        //    Assert.IsTrue(check1 && check2 && check3 );
        //}

        //[Test]
        //public void Test_NitrateModel_2_CalculateDissolvedNInLeaching()
        //{
        //    var engine = new HowLeakyEngine();
        //    LoadClimate(engine);
        //    LoadSoil(engine);
        //    LoadNitrate_N03NInLeaching(engine);
        //    InitialiseEngine(engine);
        //    engine.SoilModule.DeepDrainage = 1;
        //    engine.NitrateModule.CalculateDissolvedNInLeaching();
        //    bool check1 = ValuesAreEqual(engine.NitrateModule.NO3NStoreBotLayer, 10);
        //    bool check2 = ValuesAreEqual(engine.NitrateModule.NO3NDissolvedLeaching, 58.823529411764703);
        //    bool check3 = ValuesAreEqual(engine.NitrateModule.NO3NLeachingLoad, 0.29411764705882348);
        //    Assert.IsTrue(check1 && check2 && check3);
        //}
        //[Test]
        //public void Test_NitrateModel_3_CalculatParticulateNInRunoff()
        //{
        //    var engine = new HowLeakyEngine();

            
        //    LoadClimate(engine);
        //    LoadSoil(engine);
        //    LoadNitrate_ParticNInRunoff(engine);
        //    InitialiseEngine(engine);
        //    engine.SoilModule.Runoff = 5;
        //    engine.SoilModule.HillSlopeErosion = 2;
        //    engine.NitrateModule.CalculateParticulateNInRunoff();
        //    bool check1 = ValuesAreEqual(engine.NitrateModule.TotalNStoreTopLayer, 10);
        //    bool check2 = ValuesAreEqual(engine.NitrateModule.PNHLCa, 0.031859621048828821);
        //    bool check3 = ValuesAreEqual(engine.NitrateModule.ParticNInRunoff, 0.0035);
        //    Assert.IsTrue(check1 && check2 && check3);
        //}

        [Test]
        public void Test_Irrigation_1_Constant()
        {
            var engine = new HowLeakyEngine();
            engine.InitialPAW = 0.5;
            engine.UsePERFECTUSLELSFn = true;
            LoadClimate(engine);
            LoadSoil(engine);
            LoadIrrigation_Constant(engine);
            InitialiseEngine(engine);
           
            engine.IrrigationModule.Simulate();
            bool check1 = ValuesAreEqual(engine.SoilModule.SoilWaterDeficit, 50);
       
            Assert.IsTrue(check1);
        }

        [Test]
        public void Test_Irrigation_2_RingTank()
        {
            var engine = new HowLeakyEngine();
            LoadIrrigation_RingTank(engine);
            LoadSoil(engine);
            LoadClimate(engine);
            InitialiseEngine(engine);

            engine.ClimateModule.Rain = 100;
            engine.ClimateModule.PanEvap=10;
            engine.IrrigationModule.IrrigationApplied = 100;
            engine.IrrigationModule.StorageVolume = 20000;
            engine.IrrigationModule.ModelRingTank();
            bool check1 = ValuesAreEqual(engine.IrrigationModule.RingTankStorageVolume, 39.3);
            bool check2 = ValuesAreEqual(engine.IrrigationModule.RingTankTotalLosses, 22.2);
            bool check3 = ValuesAreEqual(engine.IrrigationModule.RingTankTotalInflow, 21.5);

            Assert.IsTrue(check1&&check2&&check3);
        }



        //CalculateDegradingPestOnVeg();
        //CalculateDegradingPestOnStubble();
        //CalculateDegradingPestInSoil();

        ////generate output values
        //CalculatePesticideRunoffConcentrations();
        //CalculatePesticideLosses();
        //CalculatePesticideDaysAboveCritical();

        [Test]
        public void Test_Pesticide_1_ApplyAnyNewPesticides()
        {
            var engine = new HowLeakyEngine();

            LoadSoil(engine);
            LoadClimate(engine);
            LoadPesticide_FixedDate(engine);
            InitialiseEngine(engine);
            var pestmodule=engine.PesticideModules[0];
            engine.ClimateModule.MaxTemp = 30;
            engine.ClimateModule.MinTemp = 15;
            engine.ClimateModule.Rain = 25;
           
            pestmodule.ApplyAnyNewPesticides();
            bool check1 = ValuesAreEqual(pestmodule.ProductRateApplied, 2);
 
            Assert.IsTrue(check1);
        }

        [Test]
        public void Test_Pesticide_2_CalculateDegradingPestOnVeg()
        {
            var engine = new HowLeakyEngine();

            LoadSoil(engine);
            LoadClimate(engine);
            LoadPesticide_FixedDate(engine);
            var pestmodule = engine.PesticideModules[0];
            InitialiseEngine(engine);

            engine.ClimateModule.MaxTemp = 30;
            engine.ClimateModule.MinTemp = 15;
            engine.ClimateModule.Rain = 25;
            engine.SoilModule.CropCover = 0.3;
            engine.SoilModule.TotalResidueCover = 0.2;
            pestmodule.ProductRateApplied=2;
            pestmodule.ApplyPesticide();
            pestmodule.CalculateDegradingPestOnVeg();
            bool check1 = ValuesAreEqual(pestmodule.PestOnVeg, 165);
            Assert.IsTrue(check1);
        }

        [Test]
        public void Test_Pesticide_3_CalculateDegradingPestOnStubble()
        {
            var engine = new HowLeakyEngine();

            LoadSoil(engine);
            LoadClimate(engine);
            LoadPesticide_FixedDate(engine);
            var pestmodule = engine.PesticideModules[0];
            InitialiseEngine(engine);

            engine.ClimateModule.MaxTemp = 30;
            engine.ClimateModule.MinTemp = 15;
            engine.ClimateModule.Rain = 25;
            engine.SoilModule.Runoff = 15;
            engine.SoilModule.CropCover=0.3;
            engine.SoilModule.TotalResidueCover=0.2;
            pestmodule.ProductRateApplied = 2;
            pestmodule.ApplyPesticide();
            pestmodule.CalculateDegradingPestOnStubble();
            bool check1 = ValuesAreEqual(pestmodule.PestOnStubble, 76.999999999999986);

            Assert.IsTrue(check1 );
        }

        [Test]
        public void Test_Pesticide_4_CalculateDegradingPestInSoil()
        {
            var engine = new HowLeakyEngine();
            LoadIrrigation_RingTank(engine);
            LoadSoil(engine);
            LoadClimate(engine);
            LoadPesticide_FixedDate(engine);
            var pestmodule = engine.PesticideModules[0];
            InitialiseEngine(engine);

            engine.ClimateModule.MaxTemp = 30;
            engine.ClimateModule.MinTemp = 15;
            engine.ClimateModule.Rain = 25;
            engine.SoilModule.Runoff = 15;
            engine.SoilModule.CropCover = 0.3;
            engine.SoilModule.TotalResidueCover = 0.2;
            pestmodule.ProductRateApplied = 2;
            pestmodule.ApplyPesticide();
            pestmodule.CalculateDegradingPestInSoil();
            bool check1 = ValuesAreEqual(pestmodule.PestInSoil, 560);
            bool check2 = ValuesAreEqual(pestmodule.PestSoilConc, 2.8);
            bool check3 = ValuesAreEqual(pestmodule.ConcSoilAfterLeach, 2.3999934317960676);

            Assert.IsTrue(check1 && check2 && check3);
        }

        [Test]
        public void Test_Pesticide_5_CalculatePesticideRunoffConcentrations()
        {
            var engine = new HowLeakyEngine();

            LoadSoil(engine);
            LoadClimate(engine);
            LoadPesticide_FixedDate(engine);
            var pestmodule = engine.PesticideModules[0];
            InitialiseEngine(engine);
            engine.ClimateModule.MaxTemp = 30;
            engine.ClimateModule.MinTemp = 15;
            engine.ClimateModule.Rain = 25;
            engine.SoilModule.Runoff=15;
            engine.SoilModule.CropCover = 0.3;
            engine.SoilModule.TotalResidueCover = 0.2;
            pestmodule.ProductRateApplied = 2;
            pestmodule.ApplyPesticide();
            pestmodule.CalculateDegradingPestInSoil();
            pestmodule.CalculatePesticideRunoffConcentrations();
            bool check1 = ValuesAreEqual(pestmodule.PestWaterPhaseConc, 46.322976873114605);
            bool check2 = ValuesAreEqual(pestmodule.PestSedPhaseConc, 0.083844588140337434);
            bool check3 = ValuesAreEqual(pestmodule.PestRunoffConc, 46.322976873114605);

            Assert.IsTrue(check1 && check2 && check3);
        }

        [Test]
        public void Test_Pesticide_6_CalculatePesticideLosses()
        {
            var engine = new HowLeakyEngine();

            LoadSoil(engine);
            LoadClimate(engine);
            LoadPesticide_FixedDate(engine);
            var pestmodule = engine.PesticideModules[0];
            InitialiseEngine(engine);

            engine.ClimateModule.MaxTemp = 30;
            engine.ClimateModule.MinTemp = 15;
            engine.ClimateModule.Rain = 25;
            engine.SoilModule.Runoff = 15;
            engine.SoilModule.CropCover = 0.3;
            engine.SoilModule.TotalResidueCover = 0.2;
            engine.SoilModule.HillSlopeErosion = 1;
            pestmodule.ProductRateApplied = 2;
            pestmodule.ApplyPesticide();
            pestmodule.CalculateDegradingPestInSoil();
            pestmodule.CalculatePesticideRunoffConcentrations();
            pestmodule.CalculatePesticideLosses();
            bool check1 = ValuesAreEqual(pestmodule.PestLostInRunoffWater, 6.9484465309671908);
            bool check2 = ValuesAreEqual(pestmodule.PestLostInRunoffSediment, 0.011738242339647242);
            bool check3 = ValuesAreEqual(pestmodule.TotalPestLostInRunoff, 6.960184773306838);
            bool check4 = ValuesAreEqual(pestmodule.PestLostInLeaching, 0.80001313640786442);

            Assert.IsTrue(check1 && check2 && check3&&check4);
        }
       
        //[Test]
        //public void Test_Pesticide_7_CalculatePesticideDaysAboveCritical()
        //{
        //    var engine = new HowLeakyEngine();
      
        //    LoadSoil(engine);
        //    LoadClimate(engine);
        //    LoadPesticide_FixedDate(engine);
        //    var pestmodule = engine.PesticideModules[0];
        //    InitialiseEngine(engine);

        //    engine.ClimateModule.Rain = 100;
        //    engine.ClimateModule.PanEvap = 10;
        //    engine.IrrigationModule.IrrigationApplied = 100;
        //    engine.IrrigationModule.StorageVolume = 20000;
        //    pestmodule.CalculatePesticideDaysAboveCritical();
        //    bool check1 = ValuesAreEqual(pestmodule.RingTankStorageVolume, 39.3);
        //    bool check2 = ValuesAreEqual(pestmodule.RingTankTotalLosses, 22.2);
        //    bool check3 = ValuesAreEqual(pestmodule.RingTankTotalInflow, 21.5);

        //    Assert.IsTrue(check1 && check2 && check3);
        //}
    }
}
