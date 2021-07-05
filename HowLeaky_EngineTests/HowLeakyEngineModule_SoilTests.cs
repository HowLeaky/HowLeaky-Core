using HowLeaky_SimulationEngine.Engine;
using HowLeaky_SimulationEngine.Inputs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_Engine.UnitTests
{
    [TestFixture]
    public class HowLeakyEngineModule_SoilTests:_CustomUnitTestClass
    {
        [Test]
        public void Initialise()
        {
            var engine = new HowLeakyEngine();
            var soilmodule = new HowLeakyEngineModule_Soil();
            var inputmodule = new HowLeakyInputs_Soil();
            soilmodule.Engine = engine;
            soilmodule.InputModel = inputmodule;
        }
        [Test]
        public void VolSat()
        {
            var engine = new HowLeakyEngine();
            var soilmodule = new HowLeakyEngineModule_Soil();
            var inputmodule = new HowLeakyInputs_Soil();
            soilmodule.Engine = engine;
            soilmodule.InputModel = inputmodule;
        }
        [Test]
        public void SetStartOfDayParameters()
        {
            var engine = new HowLeakyEngine();
            var soilmodule = new HowLeakyEngineModule_Soil();
            var inputmodule = new HowLeakyInputs_Soil();
            soilmodule.Engine = engine;
            soilmodule.InputModel = inputmodule;
        }
        [Test]
        public void CalculateResidue()
        {
            var engine = new HowLeakyEngine();
            var soilmodule = new HowLeakyEngineModule_Soil();
            var inputmodule = new HowLeakyInputs_Soil();
            soilmodule.Engine = engine;
            soilmodule.InputModel = inputmodule;
           

            Assert.Throws<Exception>(() => climatemodule.Simulate());
        }
        [Test]
        public void TryModelSoilCracking()
        {
            var engine = new HowLeakyEngine();
            var soilmodule = new HowLeakyEngineModule_Soil();
            var inputmodule = new HowLeakyInputs_Soil();
            soilmodule.Engine = engine;
            soilmodule.InputModel = inputmodule;
        }
        [Test]
        public void CalculateRunoff()
        {
            var engine = new HowLeakyEngine();
            var soilmodule = new HowLeakyEngineModule_Soil();
            var inputmodule = new HowLeakyInputs_Soil();
            soilmodule.Engine = engine;
            soilmodule.InputModel = inputmodule;
        }
        [Test]
        public void CalculatSoilEvaporation()
        {
            var engine = new HowLeakyEngine();
            var soilmodule = new HowLeakyEngineModule_Soil();
            var inputmodule = new HowLeakyInputs_Soil();
            soilmodule.Engine = engine;
            soilmodule.InputModel = inputmodule;
        }
        [Test]
        public void UpdateWaterBalance()
        {
            var engine = new HowLeakyEngine();
            var soilmodule = new HowLeakyEngineModule_Soil();
            var inputmodule = new HowLeakyInputs_Soil();
            soilmodule.Engine = engine;
            soilmodule.InputModel = inputmodule;
        }
        [Test]
        public void CalculateErosion()
        {
            var engine = new HowLeakyEngine();
            var soilmodule = new HowLeakyEngineModule_Soil();
            var inputmodule = new HowLeakyInputs_Soil();
            soilmodule.Engine = engine;
            soilmodule.InputModel = inputmodule;
        }
        [Test]
        public void TryModelLateralFlow()
        {
            var engine = new HowLeakyEngine();
            var soilmodule = new HowLeakyEngineModule_Soil();
            var inputmodule = new HowLeakyInputs_Soil();
            soilmodule.Engine = engine;
            soilmodule.InputModel = inputmodule;
        }
        [Test]
        public void CalculateInitialValuesOfCumulativeSoilEvaporation()
        {
            var engine = new HowLeakyEngine();
            var soilmodule = new HowLeakyEngineModule_Soil();
            var inputmodule = new HowLeakyInputs_Soil();
            soilmodule.Engine = engine;
            soilmodule.InputModel = inputmodule;
        }
        [Test]
        public void CalculateVolumeBalanceError()
        {
            var engine = new HowLeakyEngine();
            var soilmodule = new HowLeakyEngineModule_Soil();
            var inputmodule = new HowLeakyInputs_Soil();
            soilmodule.Engine = engine;
            soilmodule.InputModel = inputmodule;
        }
        [Test]
        public void CalculateUSLE_LSFactor()
        {
            var engine = new HowLeakyEngine();
            var soilmodule = new HowLeakyEngineModule_Soil();
            var inputmodule = new HowLeakyInputs_Soil();
            soilmodule.Engine = engine;
            soilmodule.InputModel = inputmodule;
        }
        [Test]
        public void CalculateDepthRetentionWeightFactors()
        {
            var engine = new HowLeakyEngine();
            var soilmodule = new HowLeakyEngineModule_Soil();
            var inputmodule = new HowLeakyInputs_Soil();
            soilmodule.Engine = engine;
            soilmodule.InputModel = inputmodule;
        }
        [Test]
        public void CalculateDrainageFactors()
        {
            var engine = new HowLeakyEngine();
            var soilmodule = new HowLeakyEngineModule_Soil();
            var inputmodule = new HowLeakyInputs_Soil();
            soilmodule.Engine = engine;
            soilmodule.InputModel = inputmodule;
        }
    }
}
