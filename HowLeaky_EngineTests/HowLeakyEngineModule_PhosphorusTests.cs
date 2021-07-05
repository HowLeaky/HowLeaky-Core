using HowLeaky_SimulationEngine.Engine;
using HowLeaky_SimulationEngine.Inputs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_Engine.UnitTests
{
    [TestFixture]
    public class HowLeakyEngineModule_PhosphorusTests:_CustomUnitTestClass
    {
        [Test]
        public void Simulate()
        {
            var engine = new HowLeakyEngine();
            var Phosphorusmodule = new HowLeakyEngineModule_Phosphorus();
            var Phosphorusinputs = new HowLeakyInputs_Phosphorus();
            Phosphorusmodule.Engine = engine;
            Phosphorusmodule.InputModel = Phosphorusinputs;
        }
        [Test]
        public void CalculateDissolvedPhosphorus()
        {
            var engine = new HowLeakyEngine();
            var Phosphorusmodule = new HowLeakyEngineModule_Phosphorus();
            var Phosphorusinputs = new HowLeakyInputs_Phosphorus();
            Phosphorusmodule.Engine = engine;
            Phosphorusmodule.InputModel = Phosphorusinputs;
        }
        [Test]
        public void CalculateParticulatePhosphorus()
        {
            var engine = new HowLeakyEngine();
            var Phosphorusmodule = new HowLeakyEngineModule_Phosphorus();
            var Phosphorusinputs = new HowLeakyInputs_Phosphorus();
            Phosphorusmodule.Engine = engine;
            Phosphorusmodule.InputModel = Phosphorusinputs;
        }
        [Test]
        public void CalculatePhosphorusEnrichmentRatio()
        {
            var engine = new HowLeakyEngine();
            var Phosphorusmodule = new HowLeakyEngineModule_Phosphorus();
            var Phosphorusinputs = new HowLeakyInputs_Phosphorus();
            Phosphorusmodule.Engine = engine;
            Phosphorusmodule.InputModel = Phosphorusinputs;
        }
        [Test]
        public void CalculateBioavailableParticulatePhosphorus()
        {
            var engine = new HowLeakyEngine();
            var Phosphorusmodule = new HowLeakyEngineModule_Phosphorus();
            var Phosphorusinputs = new HowLeakyInputs_Phosphorus();
            Phosphorusmodule.Engine = engine;
            Phosphorusmodule.InputModel = Phosphorusinputs;
        }
        [Test]
        public void CalculateCATCHMODSOutputs()
        {
            var engine = new HowLeakyEngine();
            var Phosphorusmodule = new HowLeakyEngineModule_Phosphorus();
            var Phosphorusinputs = new HowLeakyInputs_Phosphorus();
            Phosphorusmodule.Engine = engine;
            Phosphorusmodule.InputModel = Phosphorusinputs;
        }
        [Test]
        public void ResetPhosphorusOutputParameters()
        {
            var engine = new HowLeakyEngine();
            var Phosphorusmodule = new HowLeakyEngineModule_Phosphorus();
            var Phosphorusinputs = new HowLeakyInputs_Phosphorus();
            Phosphorusmodule.Engine = engine;
            Phosphorusmodule.InputModel = Phosphorusinputs;
        }
        [Test]
        public void TestMaximumPhosphorusConcentrations()
        {
            var engine = new HowLeakyEngine();
            var Phosphorusmodule = new HowLeakyEngineModule_Phosphorus();
            var Phosphorusinputs = new HowLeakyInputs_Phosphorus();
            Phosphorusmodule.Engine = engine;
            Phosphorusmodule.InputModel = Phosphorusinputs;
        }
    }
}
