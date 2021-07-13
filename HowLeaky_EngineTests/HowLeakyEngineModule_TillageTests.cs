using HowLeaky_SimulationEngine.Engine;
using HowLeaky_SimulationEngine.Inputs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_Engine.UnitTests
{
    [TestFixture]
    public class HowLeakyEngineModule_TillageTests:_CustomUnitTestClass
    {
        [Test]
        public void Simulate()
        {
            var engine = new HowLeakyEngine();
            var Tillagemodule = new HowLeakyEngineModule_Tillage();
            var Tillageinputs = new HowLeakyInputs_Tillage();
            Tillagemodule.Engine = engine;
            Tillagemodule.InputModel = Tillageinputs;
        }
        [Test]
        public void CanTillToday()
        {
            var engine = new HowLeakyEngine();
            var Tillagemodule = new HowLeakyEngineModule_Tillage();
            var Tillageinputs = new HowLeakyInputs_Tillage();
            Tillagemodule.Engine = engine;
            Tillagemodule.InputModel = Tillageinputs;
        }
        [Test]
        public void IsFallowAndInWindow()
        {
            var engine = new HowLeakyEngine();
            var Tillagemodule = new HowLeakyEngineModule_Tillage();
            var Tillageinputs = new HowLeakyInputs_Tillage();
            Tillagemodule.Engine = engine;
            Tillagemodule.InputModel = Tillageinputs;
        }
        [Test]
        public void IsFallowAndDate()
        {
            var engine = new HowLeakyEngine();
            var Tillagemodule = new HowLeakyEngineModule_Tillage();
            var Tillageinputs = new HowLeakyInputs_Tillage();
            Tillagemodule.Engine = engine;
            Tillagemodule.InputModel = Tillageinputs;
        }
    }
}
