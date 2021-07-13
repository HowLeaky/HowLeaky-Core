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
            var engine = new HowLeakyEngine();
            var covervegmodule = new HowLeakyEngineModule_CoverVeg();
            var inputmodule = new HowLeakyInputs_CoverVeg();
            covervegmodule.Engine = engine;
            covervegmodule.InputModel = inputmodule;
            covervegmodule.CropCover=100;
            Assert.IsTrue(covervegmodule.StillRequiresIrrigating());
        }

        [Test]
        public void StillRequiresIrrigating_CropCoverEquals0_ReturnsFalse()
        {
            var engine = new HowLeakyEngine();
            var covervegmodule = new HowLeakyEngineModule_CoverVeg();
            var inputmodule = new HowLeakyInputs_CoverVeg();
            covervegmodule.Engine = engine;
            covervegmodule.InputModel = inputmodule;
            covervegmodule.CropCover = 0;
            Assert.IsFalse(covervegmodule.StillRequiresIrrigating());
        }

       
        [Test]
        public void GetTotalCover_TotalCovGTMax_ReturnsCorrectValue()
        {
            var engine = new HowLeakyEngine();
            var covervegmodule = new HowLeakyEngineModule_CoverVeg();
            var inputmodule = new HowLeakyInputs_CoverVeg();
            covervegmodule.Engine = engine;
            covervegmodule.InputModel = inputmodule;
            covervegmodule.InputModel=new HowLeakyInputs_CoverVeg();
            covervegmodule.InputModel.MaxAllowTotalCover=0.8;
            covervegmodule.CropCover = 0;
            covervegmodule.ResidueCover=0.20;
            covervegmodule.GreenCover=0.40;
            var result=covervegmodule.GetTotalCover();
            Assert.IsTrue(ValuesAreEqual(result,0.52));
        }


        [Test]
        public void GetTotalCover_TotalCovLTMax_ReturnsCorrectValue()
        {
            var engine = new HowLeakyEngine();
            var covervegmodule = new HowLeakyEngineModule_CoverVeg();
            var inputmodule = new HowLeakyInputs_CoverVeg();
            covervegmodule.Engine = engine;
            covervegmodule.InputModel = inputmodule;
            covervegmodule.InputModel = new HowLeakyInputs_CoverVeg();
            covervegmodule.InputModel.MaxAllowTotalCover =0.4;
            covervegmodule.CropCover = 0;
            covervegmodule.ResidueCover = 0.20;
            covervegmodule.GreenCover = 0.40;
            var result = covervegmodule.GetTotalCover();
            Assert.IsTrue(ValuesAreEqual(result, 0.4));
        }
        [Test]
        public void InitialisedMeasuredInputs()
        {
            var engine = new HowLeakyEngine();
            var covervegmodule = new HowLeakyEngineModule_CoverVeg();
            var inputmodule = new HowLeakyInputs_CoverVeg();
            covervegmodule.Engine = engine;
            covervegmodule.InputModel = inputmodule;
        }
        [Test]
        public void EtPanPhenology()
        {
            var engine = new HowLeakyEngine();
            var covervegmodule = new HowLeakyEngineModule_CoverVeg();
            var inputmodule = new HowLeakyInputs_CoverVeg();
            covervegmodule.Engine = engine;
            covervegmodule.InputModel = inputmodule;
        }
        [Test]
        public void CalculateYield()
        {
            var engine = new HowLeakyEngine();
            var covervegmodule = new HowLeakyEngineModule_CoverVeg();
            var inputmodule = new HowLeakyInputs_CoverVeg();
            covervegmodule.Engine = engine;
            covervegmodule.InputModel = inputmodule;
        }

    }
}
