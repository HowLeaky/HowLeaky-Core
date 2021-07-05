using HowLeaky_SimulationEngine.Engine;
using HowLeaky_SimulationEngine.Inputs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_Engine.UnitTests
{
    [TestFixture]
    public class HowLeakyEngineModule_PesticideTests:_CustomUnitTestClass
    {
        [Test]
        public void Simulate()
        {
            var engine = new HowLeakyEngine();
            var Pesticidemodule = new HowLeakyEngineModule_Pesticide();
            var Pesticideinputs = new HowLeakyInputs_Pesticide();
            Pesticidemodule.Engine = engine;
            Pesticidemodule.InputModel = Pesticideinputs;
        }
        [Test]
        public void ApplyAnyNewPesticides()
        {
            var engine = new HowLeakyEngine();
            var Pesticidemodule = new HowLeakyEngineModule_Pesticide();
            var Pesticideinputs = new HowLeakyInputs_Pesticide();
            Pesticidemodule.Engine = engine;
            Pesticidemodule.InputModel = Pesticideinputs;
        }
        [Test]
        public void CheckApplicationBasedOnGDD()
        {
            var engine = new HowLeakyEngine();
            var Pesticidemodule = new HowLeakyEngineModule_Pesticide();
            var Pesticideinputs = new HowLeakyInputs_Pesticide();
            Pesticidemodule.Engine = engine;
            Pesticidemodule.InputModel = Pesticideinputs;
        }
        [Test]
        public void CheckApplicationBasedOnDAS()
        {
            var engine = new HowLeakyEngine();
            var Pesticidemodule = new HowLeakyEngineModule_Pesticide();
            var Pesticideinputs = new HowLeakyInputs_Pesticide();
            Pesticidemodule.Engine = engine;
            Pesticidemodule.InputModel = Pesticideinputs;
        }
        [Test]
        public void CheckApplicationBasedOnDAH()
        {
            var engine = new HowLeakyEngine();
            var Pesticidemodule = new HowLeakyEngineModule_Pesticide();
            var Pesticideinputs = new HowLeakyInputs_Pesticide();
            Pesticidemodule.Engine = engine;
            Pesticidemodule.InputModel = Pesticideinputs;
        }
        [Test]
        public void ApplyPesticide()
        {
            var engine = new HowLeakyEngine();
            var Pesticidemodule = new HowLeakyEngineModule_Pesticide();
            var Pesticideinputs = new HowLeakyInputs_Pesticide();
            Pesticidemodule.Engine = engine;
            Pesticidemodule.InputModel = Pesticideinputs;
        }
        [Test]
        public void CalculateDegradingPestOnVeg()
        {
            var engine = new HowLeakyEngine();
            var Pesticidemodule = new HowLeakyEngineModule_Pesticide();
            var Pesticideinputs = new HowLeakyInputs_Pesticide();
            Pesticidemodule.Engine = engine;
            Pesticidemodule.InputModel = Pesticideinputs;
        }
        [Test]
        public void CalculateDegradingPestOnStubble()
        {
            var engine = new HowLeakyEngine();
            var Pesticidemodule = new HowLeakyEngineModule_Pesticide();
            var Pesticideinputs = new HowLeakyInputs_Pesticide();
            Pesticidemodule.Engine = engine;
            Pesticidemodule.InputModel = Pesticideinputs;
        }
        [Test]
        public void CalculateDegradingPestInSoil()
        {
            var engine = new HowLeakyEngine();
            var Pesticidemodule = new HowLeakyEngineModule_Pesticide();
            var Pesticideinputs = new HowLeakyInputs_Pesticide();
            Pesticidemodule.Engine = engine;
            Pesticidemodule.InputModel = Pesticideinputs;
        }
        [Test]
        public void CalculatePesticideRunoffConcentrations()
        {
            var engine = new HowLeakyEngine();
            var Pesticidemodule = new HowLeakyEngineModule_Pesticide();
            var Pesticideinputs = new HowLeakyInputs_Pesticide();
            Pesticidemodule.Engine = engine;
            Pesticidemodule.InputModel = Pesticideinputs;
        }
        [Test]
        public void CalculatePesticideLosses()
        {
            var engine = new HowLeakyEngine();
            var Pesticidemodule = new HowLeakyEngineModule_Pesticide();
            var Pesticideinputs = new HowLeakyInputs_Pesticide();
            Pesticidemodule.Engine = engine;
            Pesticidemodule.InputModel = Pesticideinputs;
        }
        [Test]
        public void CalculatePesticideDaysAboveCritical()
        {
            var engine = new HowLeakyEngine();
            var Pesticidemodule = new HowLeakyEngineModule_Pesticide();
            var Pesticideinputs = new HowLeakyInputs_Pesticide();
            Pesticidemodule.Engine = engine;
            Pesticidemodule.InputModel = Pesticideinputs;
        }
    }
}
