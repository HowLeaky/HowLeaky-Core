using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_Engine.UnitTests
{
    [TestFixture]
    public class HowLeakyEngine_InitialiseTests:_CustomUnitTestClass
    {
        [Test]
        public void PrepareForNewSimulation()
        {
            var engine = new HowLeakyEngine();
        }
        [Test]
        public void InitialiseVegetationModules()
        {
            var engine = new HowLeakyEngine();
        }
        [Test]
        public void InitialisePesticideModules()
        {
            var engine = new HowLeakyEngine();
        }
        [Test]
        public void InitialiseTillageModules()
        {
            var engine = new HowLeakyEngine();
        }
    }
}
