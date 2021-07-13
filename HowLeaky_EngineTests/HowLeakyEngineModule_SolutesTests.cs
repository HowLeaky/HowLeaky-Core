using HowLeaky_SimulationEngine.Engine;
using HowLeaky_SimulationEngine.Inputs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_Engine.UnitTests
{
    [TestFixture]
    public class HowLeakyEngineModule_SolutesTests : _CustomUnitTestClass
    {
        [Test]
        public void Simulate()
        {
            var engine = new HowLeakyEngine();
            var Solutesmodule = new HowLeakyEngineModule_Solutes();
            var Solutesinputs = new HowLeakyInputs_Solute();
            Solutesmodule.Engine = engine;
            Solutesmodule.InputModel = Solutesinputs;
        }
    }
}
