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
            var check2 = ValuesAreEqual(engine.SoilModule.Sse2, 21);
            var check3 = ValuesAreEqual(engine.SoilModule.Dsr, 27.5625);
            var check4 = ValuesAreEqual(engine.SoilModule.Sse1, 4);
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
            var check1 = ValuesAreEqual(result, 4);
            
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
            var check1 = ValuesAreEqual(engine.SoilModule.EffectiveRain, 4);
            var check2 = ValuesAreEqual(engine.SoilModule.Satd, 21);
            var check3 = ValuesAreEqual(engine.SoilModule.SoilWaterDeficit, 27.5625);
            
            Assert.IsTrue(check1 && check2 && check3 );
        }
        
        [Test]
        public void Test_SoilModule_4_CalculateResidue()
        {
            var engine = new HowLeakyEngine();
            LoadSoil(engine);
            LoadCoverVeg(engine);
            engine.VegetationModules[0].SetStartOfDayParameters();
            engine.SetStartOfDayParameters();
            var check1 = ValuesAreEqual(engine.SoilModule.TotalCropResidue, 4);
            var check2 = ValuesAreEqual(engine.SoilModule.TotalResidueCover, 21);
            var check3 = ValuesAreEqual(engine.SoilModule.TotalResidueCoverPercent, 27.5625);
            var check4 = ValuesAreEqual(engine.SoilModule.TotalCoverAllCrops, 4);
            var check5 = ValuesAreEqual(engine.SoilModule.CropCover, 4);
            var check6 = ValuesAreEqual(engine.SoilModule.TotalCoverPercent, 4);
            var check7 = ValuesAreEqual(engine.SoilModule.AccumulatedCover, 4);
           
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
            engine.SoilModule.EffectiveRain = 20;
            engine.SoilModule.CalculateRunoff();
            var check1 = ValuesAreEqual(engine.SoilModule.RunoffRetentionNumber, 4);
            var check2 = ValuesAreEqual(engine.SoilModule.RunoffCurveNo, 21);
            var check3 = ValuesAreEqual(engine.SoilModule.Runoff, 27.5625);
            var check4 = ValuesAreEqual(engine.SoilModule.Infiltration, 4);
            Assert.IsTrue(check1 && check2 && check3 && check4);
        }
        [Test]
        public void Test_SoilModule_7_CalculateSoilEvaporation()
        {
            var engine = new HowLeakyEngine();
            LoadSoil(engine);
            LoadClimate(engine);
            engine.ClimateModule.PanEvap = 15;
            engine.SoilModule.TotalCropResidue = 30;
            engine.SoilModule.Infiltration = 10;
            engine.SoilModule.CalculateSoilEvaporation();
            var check1 = ValuesAreEqual(engine.SoilModule.Sse1, 4);
            var check2 = ValuesAreEqual(engine.SoilModule.Dsr, 21);
            var check3 = ValuesAreEqual(engine.SoilModule.Se21, 27.5625);
            var check4 = ValuesAreEqual(engine.SoilModule.Se22, 4);
            var check5 = ValuesAreEqual(engine.SoilModule.Se2, 4);
            var check6 = ValuesAreEqual(engine.SoilModule.Sse2, 4);
            var check7 = ValuesAreEqual(engine.SoilModule.SoilEvap, 4);
            Assert.IsTrue(check1 && check2 && check3 && check4 && check5 && check6 && check7);
        }
        
        [Test]
        public void Test_SoilModule_8_UpdateWaterBalance()
        {
            var engine = new HowLeakyEngine();
            LoadSoil(engine);
            LoadClimate(engine);
            engine.ClimateModule.PanEvap = 15;
            engine.SoilModule.TotalCropResidue = 30;
            engine.SoilModule.Infiltration = 10;
            engine.SoilModule.UpdateWaterBalance();
            var checkSW1 = ValuesAreEqual(engine.SoilModule.SoilWaterRelWP[0], 4);
            var checkSW2 = ValuesAreEqual(engine.SoilModule.SoilWaterRelWP[1], 4);
            var checkSW3 = ValuesAreEqual(engine.SoilModule.SoilWaterRelWP[2], 4);
            var checkSW4 = ValuesAreEqual(engine.SoilModule.SoilWaterRelWP[3], 4);
            var checkSW5 = ValuesAreEqual(engine.SoilModule.SoilWaterRelWP[4], 4);
            var checkSW6 = ValuesAreEqual(engine.SoilModule.SoilWaterRelWP[5], 4);
            
            var checkD1 = ValuesAreEqual(engine.SoilModule.Drainage[0], 4);
            var checkD2 = ValuesAreEqual(engine.SoilModule.Drainage[1], 4);
            var checkD3 = ValuesAreEqual(engine.SoilModule.Drainage[2], 4);
            var checkD4 = ValuesAreEqual(engine.SoilModule.Drainage[3], 4);
            var checkD5 = ValuesAreEqual(engine.SoilModule.Drainage[4], 4);
            var checkD6 = ValuesAreEqual(engine.SoilModule.Drainage[5], 4);
            
            var checkWB1 = ValuesAreEqual(engine.SoilModule.TotalSoilWater, 4);
            var checkWB2 = ValuesAreEqual(engine.SoilModule.SaturationIndex, 21);
            var checkWB3 = ValuesAreEqual(engine.SoilModule.Overflow, 27.5625);
            var checkWB4 = ValuesAreEqual(engine.SoilModule.Runoff, 27.5625);
            var checkWB5 = ValuesAreEqual(engine.SoilModule.Infiltration, 27.5625);
            
            var check1=(checkSW1 && checkSW2 && checkSW3 && checkSW4 && checkSW5 && checkSW6);
            var check2=(checkD1 && checkD2 && checkD3 && checkD4 && checkD5 && checkD6);
            var check3=(checkWB1 && checkWB2 && checkWB3 && checkWB4 && checkWB5 );
            Assert.IsTrue(check1 && check2 && check3 );
        }
        
        [Test]
        public void Test_SoilModule_9_CalculateErosion()
        {
            var engine = new HowLeakyEngine();
            LoadSoil(engine);
            engine.SoilModule.Runoff = 20;
            engine.SoilModule.TotalCoverAllCrops = 0.3;
            engine.SoilModule.CalculateErosion();
            var check1 = ValuesAreEqual(engine.SoilModule.SedimentConc, 4);
            var check2 = ValuesAreEqual(engine.SoilModule.HillSlopeErosion, 21);
            var check3 = ValuesAreEqual(engine.SoilModule.PeakSedConc, 27.5625);
            var check4 = ValuesAreEqual(engine.SoilModule.OffSiteSedDelivery, 4);

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

        [Test]
        public void Test_NitrateModel_1_CalculateDissolvedNInRunoff()
        {
            var engine = new HowLeakyEngine();           
            LoadClimate(engine);
            LoadSoil(engine);                        
            LoadNitrate_N03NInRuoff(engine);
            InitialiseEngine(engine);
            engine.SoilModule.Runoff=5;
            engine.NitrateModule.CalculateDissolvedNInRunoff();
            bool check1 = ValuesAreEqual(engine.NitrateModule.NO3NStoreTopLayer, 10);
            bool check2 = ValuesAreEqual(engine.NitrateModule.NO3NDissolvedInRunoff, 1.896361676485673);
            bool check3 = ValuesAreEqual(engine.NitrateModule.NO3NRunoffLoad, 0.094818083824283653);
            Assert.IsTrue(check1 && check2 && check3 );
        }

        [Test]
        public void Test_NitrateModel_2_CalculateDissolvedNInLeaching()
        {
            var engine = new HowLeakyEngine();
            LoadClimate(engine);
            LoadSoil(engine);
            LoadNitrate_N03NInLeaching(engine);
            InitialiseEngine(engine);
            engine.SoilModule.DeepDrainage = 1;
            engine.NitrateModule.CalculateDissolvedNInLeaching();
            bool check1 = ValuesAreEqual(engine.NitrateModule.NO3NStoreBotLayer, 10);
            bool check2 = ValuesAreEqual(engine.NitrateModule.NO3NDissolvedLeaching, 58.823529411764703);
            bool check3 = ValuesAreEqual(engine.NitrateModule.NO3NLeachingLoad, 0.29411764705882348);
            Assert.IsTrue(check1 && check2 && check3);
        }
        [Test]
        public void Test_NitrateModel_3_CalculatParticulateNInRunoff()
        {
            var engine = new HowLeakyEngine();

            
            LoadClimate(engine);
            LoadSoil(engine);
            LoadNitrate_ParticNInRunoff(engine);
            InitialiseEngine(engine);
            engine.SoilModule.Runoff = 5;
            engine.SoilModule.HillSlopeErosion = 2;
            engine.NitrateModule.CalculateParticulateNInRunoff();
            bool check1 = ValuesAreEqual(engine.NitrateModule.TotalNStoreTopLayer, 10);
            bool check2 = ValuesAreEqual(engine.NitrateModule.PNHLCa, 0.031859621048828821);
            bool check3 = ValuesAreEqual(engine.NitrateModule.ParticNInRunoff, 0.0035);
            Assert.IsTrue(check1 && check2 && check3);
        }

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

    }
}
