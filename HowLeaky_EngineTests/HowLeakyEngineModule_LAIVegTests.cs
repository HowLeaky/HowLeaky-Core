using HowLeaky_SimulationEngine.Engine;
using HowLeaky_SimulationEngine.Inputs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_Engine.UnitTests
{
    [TestFixture]
    public class HowLeakyEngineModule_LAIVegTests:_CustomUnitTestClass
    {
        [Test]
        public void Initialise()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void Simulate()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void GetPotentialSoilEvaporation()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void IsSequenceCorrect()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void DoesCropMeetSowingCriteria()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void SatisifiesPlantingRainConditions()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void SatisifiesFallowConditions()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void SatisifiesSoilWaterConditions()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void IsCropUnderMaxContinuousRotations()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void HasCropHadSufficientContinuousRotations()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void HasCropBeenAbsentForSufficientYears()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void Plant()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void GetTotalCover()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void CheckCropSurvives()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void CalculateGrowthStressFactors()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void Scurve()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void CalculateLeafAreaIndex()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void CalculateBioMass()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void GetDayLength()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void CalculateRootGrowth()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void ResetCropParametersAfterHarvest()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void HarvestTheCrop()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void SimulateCropDeath()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void ResetParametersForEndOfCrop()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void RecordCropStage()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void CalculatePotentialTranspiration()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void CalculateResidue()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void CalculateResidueBR()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }
        [Test]
        public void CalculateResiduePERFECT()
        {
            var engine = new HowLeakyEngine();
            var LAIVegmodule = new HowLeakyEngineModule_LAIVeg();
            var LAIVeginputs = new HowLeakyInputs_LAIVeg();
            LAIVegmodule.Engine = engine;
            LAIVegmodule.InputModel = LAIVeginputs;
        }

    }
}
