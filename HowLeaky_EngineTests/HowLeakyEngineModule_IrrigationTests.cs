using HowLeaky_SimulationEngine.Engine;
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
            var module = new HowLeakyEngineModule_Irrigation();
            Assert.DoesNotThrow(() => module.Simulate());
        }
        [Test]
        public void Irrigate()
        {
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void CanIrrigateToday()
        {
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void GetRequiredIrrigationAmount()
        {
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void GetAvailableAmountFromSupply()
        {
            //this one is a double
        }
        [Test]
        public void RemoveRunoffLosses()
        {
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void DistributeWaterThroughSoilLayers()
        {
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void PondingExists()
        {
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void ModelRingTank()
        {
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void ResetRingTank()
        {
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void SimulateDailyRingTankWaterBalance()
        {
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void CalcOvertoppingAmount()
        {
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void CalcEffectiveAdditionalInflow()
        {
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void CalcStorageLevel()
        {
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void CalcPotentialStorageVolume()
        {
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void CalcActualStorageVolume()
        {
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void CalcRunoffCaptureInflow()
        {
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void GetAdditionalTankInflow()
        {
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void ConsiderCoverEffects()
        {
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }
        [Test]
        public void GetCoverEffect()
        {
            //var module = new HowLeakyEngineModule_Irrigation();
            //Assert.DoesNotThrow(() => module.Irrigate_());
        }

    }
}
