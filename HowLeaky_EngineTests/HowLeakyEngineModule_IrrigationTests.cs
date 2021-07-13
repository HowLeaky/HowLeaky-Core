using HowLeaky_SimulationEngine.Engine;
using HowLeaky_SimulationEngine.Inputs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_Engine.UnitTests
{
    [TestFixture]
    public class HowLeakyEngineModule_IrrigationTests:_CustomUnitTestClass
    {
        [Test]
        public void Simulate()
        {
            var engine = new HowLeakyEngine();
            var irrigationmodule = new HowLeakyEngineModule_Irrigation();
            var inputmodule = new HowLeakyInputs_Irrigation();
            irrigationmodule.Engine = engine;
            irrigationmodule.InputModel = inputmodule;
            var module = new HowLeakyEngineModule_Irrigation();
            Assert.DoesNotThrow(() => module.Simulate());
        }
        [Test]
        public void Irrigate()
        {
            var engine = new HowLeakyEngine();
            var irrigationmodule = new HowLeakyEngineModule_Irrigation();
            var inputmodule = new HowLeakyInputs_Irrigation();
            irrigationmodule.Engine = engine;
            irrigationmodule.InputModel = inputmodule;
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void CanIrrigateToday()
        {
            var engine = new HowLeakyEngine();
            var irrigationmodule = new HowLeakyEngineModule_Irrigation();
            var inputmodule = new HowLeakyInputs_Irrigation();
            irrigationmodule.Engine = engine;
            irrigationmodule.InputModel = inputmodule;
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void GetRequiredIrrigationAmount()
        {
            var engine = new HowLeakyEngine();
            var irrigationmodule = new HowLeakyEngineModule_Irrigation();
            var inputmodule = new HowLeakyInputs_Irrigation();
            irrigationmodule.Engine = engine;
            irrigationmodule.InputModel = inputmodule;
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void GetAvailableAmountFromSupply()
        {
            var engine = new HowLeakyEngine();
            var irrigationmodule = new HowLeakyEngineModule_Irrigation();
            var inputmodule = new HowLeakyInputs_Irrigation();
            irrigationmodule.Engine = engine;
            irrigationmodule.InputModel = inputmodule;
            //this one is a double
        }
        [Test]
        public void RemoveRunoffLosses()
        {
            var engine = new HowLeakyEngine();
            var irrigationmodule = new HowLeakyEngineModule_Irrigation();
            var inputmodule = new HowLeakyInputs_Irrigation();
            irrigationmodule.Engine = engine;
            irrigationmodule.InputModel = inputmodule;
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void DistributeWaterThroughSoilLayers()
        {
            var engine = new HowLeakyEngine();
            var irrigationmodule = new HowLeakyEngineModule_Irrigation();
            var inputmodule = new HowLeakyInputs_Irrigation();
            irrigationmodule.Engine = engine;
            irrigationmodule.InputModel = inputmodule;
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void PondingExists()
        {
            var engine = new HowLeakyEngine();
            var irrigationmodule = new HowLeakyEngineModule_Irrigation();
            var inputmodule = new HowLeakyInputs_Irrigation();
            irrigationmodule.Engine = engine;
            irrigationmodule.InputModel = inputmodule;
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void ModelRingTank()
        {
            var engine = new HowLeakyEngine();
            var irrigationmodule = new HowLeakyEngineModule_Irrigation();
            var inputmodule = new HowLeakyInputs_Irrigation();
            irrigationmodule.Engine = engine;
            irrigationmodule.InputModel = inputmodule;
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void ResetRingTank()
        {
            var engine = new HowLeakyEngine();
            var irrigationmodule = new HowLeakyEngineModule_Irrigation();
            var inputmodule = new HowLeakyInputs_Irrigation();
            irrigationmodule.Engine = engine;
            irrigationmodule.InputModel = inputmodule;
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void SimulateDailyRingTankWaterBalance()
        {
            var engine = new HowLeakyEngine();
            var irrigationmodule = new HowLeakyEngineModule_Irrigation();
            var inputmodule = new HowLeakyInputs_Irrigation();
            irrigationmodule.Engine = engine;
            irrigationmodule.InputModel = inputmodule;
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void CalcOvertoppingAmount()
        {
            var engine = new HowLeakyEngine();
            var irrigationmodule = new HowLeakyEngineModule_Irrigation();
            var inputmodule = new HowLeakyInputs_Irrigation();
            irrigationmodule.Engine = engine;
            irrigationmodule.InputModel = inputmodule;
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void CalcEffectiveAdditionalInflow()
        {
            var engine = new HowLeakyEngine();
            var irrigationmodule = new HowLeakyEngineModule_Irrigation();
            var inputmodule = new HowLeakyInputs_Irrigation();
            irrigationmodule.Engine = engine;
            irrigationmodule.InputModel = inputmodule;
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void CalcStorageLevel()
        {
            var engine = new HowLeakyEngine();
            var irrigationmodule = new HowLeakyEngineModule_Irrigation();
            var inputmodule = new HowLeakyInputs_Irrigation();
            irrigationmodule.Engine = engine;
            irrigationmodule.InputModel = inputmodule;
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void CalcPotentialStorageVolume()
        {
            var engine = new HowLeakyEngine();
            var irrigationmodule = new HowLeakyEngineModule_Irrigation();
            var inputmodule = new HowLeakyInputs_Irrigation();
            irrigationmodule.Engine = engine;
            irrigationmodule.InputModel = inputmodule;
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void CalcActualStorageVolume()
        {
            var engine = new HowLeakyEngine();
            var irrigationmodule = new HowLeakyEngineModule_Irrigation();
            var inputmodule = new HowLeakyInputs_Irrigation();
            irrigationmodule.Engine = engine;
            irrigationmodule.InputModel = inputmodule;
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void CalcRunoffCaptureInflow()
        {
            var engine = new HowLeakyEngine();
            var irrigationmodule = new HowLeakyEngineModule_Irrigation();
            var inputmodule = new HowLeakyInputs_Irrigation();
            irrigationmodule.Engine = engine;
            irrigationmodule.InputModel = inputmodule;
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void GetAdditionalTankInflow()
        {
            var engine = new HowLeakyEngine();
            var irrigationmodule = new HowLeakyEngineModule_Irrigation();
            var inputmodule = new HowLeakyInputs_Irrigation();
            irrigationmodule.Engine = engine;
            irrigationmodule.InputModel = inputmodule;
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void ConsiderCoverEffects()
        {
            var engine = new HowLeakyEngine();
            var irrigationmodule = new HowLeakyEngineModule_Irrigation();
            var inputmodule = new HowLeakyInputs_Irrigation();
            irrigationmodule.Engine = engine;
            irrigationmodule.InputModel = inputmodule;
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void GetCoverEffect()
        {
            var engine = new HowLeakyEngine();
            var irrigationmodule = new HowLeakyEngineModule_Irrigation();
            var inputmodule = new HowLeakyInputs_Irrigation();
            irrigationmodule.Engine = engine;
            irrigationmodule.InputModel = inputmodule;
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }

    }
}
