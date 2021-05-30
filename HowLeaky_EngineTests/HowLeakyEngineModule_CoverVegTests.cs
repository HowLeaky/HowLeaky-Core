using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using HowLeaky_SimulationEngine.Engine;
using HowLeaky_SimulationEngine.Inputs;

namespace HowLeaky_Engine.UnitTests
{



    [TestFixture]
    public class HowLeakyEngineModule_CoverVegTests : _CustomUnitTestClass
    {


        [Test]
        public void StillRequiresIrrigating_CropCoverEquals100_ReturnsTrue()
        {
            var module=new HowLeakyEngineModule_CoverVeg();
            module.CropCover=100;
            Assert.IsTrue(module.StillRequiresIrrigating());
        }

        [Test]
        public void StillRequiresIrrigating_CropCoverEquals0_ReturnsFalse()
        {
            var module = new HowLeakyEngineModule_CoverVeg();
            module.CropCover = 0;
            Assert.IsFalse(module.StillRequiresIrrigating());
        }

       
        [Test]
        public void GetTotalCover_TotalCovGTMax_ReturnsCorrectValue()
        {
            var module = new HowLeakyEngineModule_CoverVeg();
            module.InputModel=new HowLeakyInputs_CoverVeg();
            module.InputModel.MaxAllowTotalCover=0.8;
            module.CropCover = 0;
            module.ResidueCover=0.20;
            module.GreenCover=0.40;
            var result=module.GetTotalCover();
            Assert.IsTrue(ValuesAreEqual(result,0.52));
        }


        [Test]
        public void GetTotalCover_TotalCovLTMax_ReturnsCorrectValue()
        {
            var module = new HowLeakyEngineModule_CoverVeg();
            module.InputModel = new HowLeakyInputs_CoverVeg();
            module.InputModel.MaxAllowTotalCover =0.4;
            module.CropCover = 0;
            module.ResidueCover = 0.20;
            module.GreenCover = 0.40;
            var result = module.GetTotalCover();
            Assert.IsTrue(ValuesAreEqual(result, 0.4));
        }

    }
}
