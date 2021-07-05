using HowLeaky_SimulationEngine.Engine;
using HowLeaky_SimulationEngine.Inputs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_Engine.UnitTests
{
    [TestFixture]
    public class HowLeakyEngineModule_NitrateTests:_CustomUnitTestClass
    {
        [Test]
        public void Simulate()
        {
            var engine = new HowLeakyEngine();
            var Nitratemodule = new HowLeakyEngineModule_Nitrate();
            var Nitrateinputs = new HowLeakyInputs_Nitrate();
            Nitratemodule.Engine = engine;
            Nitratemodule.InputModel = Nitrateinputs;
        }
        [Test]
        public void CanCalculateDissolvedNInRunoff()
        {
            var engine = new HowLeakyEngine();
            var Nitratemodule = new HowLeakyEngineModule_Nitrate();
            var Nitrateinputs = new HowLeakyInputs_Nitrate();
            Nitratemodule.Engine = engine;
            Nitratemodule.InputModel = Nitrateinputs;
        }
        [Test]
        public void CalculateDissolvedNInRunoffRattray()
        {
            var engine = new HowLeakyEngine();
            var Nitratemodule = new HowLeakyEngineModule_Nitrate();
            var Nitrateinputs = new HowLeakyInputs_Nitrate();
            Nitratemodule.Engine = engine;
            Nitratemodule.InputModel = Nitrateinputs;
        }
        [Test]
        public void CanCalculateDissolvedNInLeaching()
        {
            var engine = new HowLeakyEngine();
            var Nitratemodule = new HowLeakyEngineModule_Nitrate();
            var Nitrateinputs = new HowLeakyInputs_Nitrate();
            Nitratemodule.Engine = engine;
            Nitratemodule.InputModel = Nitrateinputs;
        }
        [Test]
        public void CanCalculateParticulateNInRunoff()
        {
            var engine = new HowLeakyEngine();
            var Nitratemodule = new HowLeakyEngineModule_Nitrate();
            var Nitrateinputs = new HowLeakyInputs_Nitrate();
            Nitratemodule.Engine = engine;
            Nitratemodule.InputModel = Nitrateinputs;
        }
        [Test]
        public void CalculateDissolvedNInRunoff()
        {
            var engine = new HowLeakyEngine();
            var Nitratemodule = new HowLeakyEngineModule_Nitrate();
            var Nitrateinputs = new HowLeakyInputs_Nitrate();
            Nitratemodule.Engine = engine;
            Nitratemodule.InputModel = Nitrateinputs;
        }
        [Test]
        public void CalculateDissolvedNInLeaching1()
        {
            var engine = new HowLeakyEngine();
            var Nitratemodule = new HowLeakyEngineModule_Nitrate();
            var Nitrateinputs = new HowLeakyInputs_Nitrate();
            Nitratemodule.Engine = engine;
            Nitratemodule.InputModel = Nitrateinputs;
        }
        [Test]
        public void CalculateDissolvedNInLeaching_SafeGauge()
        {
            var engine = new HowLeakyEngine();
            var Nitratemodule = new HowLeakyEngineModule_Nitrate();
            var Nitrateinputs = new HowLeakyInputs_Nitrate();
            Nitratemodule.Engine = engine;
            Nitratemodule.InputModel = Nitrateinputs;
        }
        [Test]
        public void CalculateParticulateNInRunoff()
        {
            var engine = new HowLeakyEngine();
            var Nitratemodule = new HowLeakyEngineModule_Nitrate();
            var Nitrateinputs = new HowLeakyInputs_Nitrate();
            Nitratemodule.Engine = engine;
            Nitratemodule.InputModel = Nitrateinputs;
        }
        [Test]
        public void GetNO3NStoreTopLayerkgPerha()
        {
            var engine = new HowLeakyEngine();
            var Nitratemodule = new HowLeakyEngineModule_Nitrate();
            var Nitrateinputs = new HowLeakyInputs_Nitrate();
            Nitratemodule.Engine = engine;
            Nitratemodule.InputModel = Nitrateinputs;
        }
        [Test]
        public void GetNO3NStoreBotLayerkgPerha()
        {
            var engine = new HowLeakyEngine();
            var Nitratemodule = new HowLeakyEngineModule_Nitrate();
            var Nitrateinputs = new HowLeakyInputs_Nitrate();
            Nitratemodule.Engine = engine;
            Nitratemodule.InputModel = Nitrateinputs;
        }
        [Test]
        public void GetTotalNStoreTopLayerkgPerha()
        {
            var engine = new HowLeakyEngine();
            var Nitratemodule = new HowLeakyEngineModule_Nitrate();
            var Nitrateinputs = new HowLeakyInputs_Nitrate();
            Nitratemodule.Engine = engine;
            Nitratemodule.InputModel = Nitrateinputs;
        }
    }
}
