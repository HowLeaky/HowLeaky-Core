using HowLeaky_SimulationEngine.Engine;
using HowLeaky_SimulationEngine.Inputs;
using HowLeaky_SimulationEngine.Tools;

using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace HowLeakyEngineTests
{
    [TestFixture]
    public class HowLeakyEngineModule_ClimateTests
    {
        
        //user naming converntion  MethodName_Scenario_ExcectedBehaviour
        [Test]
        public void Simulate_DateInRange_NoException()
        {
    
            var engine = new HowLeakyEngine();
            var climatemodule = new HowLeakyEngineModule_Climate();
            var inputmodule = new HowLeakyInputs_Climate();
            inputmodule.Rain=new List<double>(new double[1000]);
            inputmodule.MaxT = new List<double>(new double[1000]);
            inputmodule.MinT = new List<double>(new double[1000]);
            inputmodule.PanEvap = new List<double>(new double[1000]);
            inputmodule.Radiation = new List<double>(new double[1000]);
            inputmodule.VP = new List<double>(new double[1000]);
            climatemodule.Engine = engine;
            climatemodule.InputModel = inputmodule;
   
            inputmodule.StartDate = new BrowserDate(2019, 1, 1);
            engine.TodaysDate = new BrowserDate(2019, 3, 4);

       
            Assert.DoesNotThrow(() => climatemodule.Simulate());
        }

        [Test]
        
        public void Simulate_DateOutOfRange_ThrowsException()
        {
            var engine=new HowLeakyEngine();
            var climatemodule=new HowLeakyEngineModule_Climate();
            var inputmodule=new HowLeakyInputs_Climate();
            climatemodule.Engine=engine;
            climatemodule.InputModel=inputmodule;
            inputmodule.StartDate=new BrowserDate(2019,1,1);
            engine.TodaysDate=new BrowserDate(2018,3,4);     

            Assert.Throws<Exception>(() => climatemodule.Simulate());
   
        }
        [Test]
        public void Simulate_IndexOutOfRange_ThrowsException()
        {
            var engine = new HowLeakyEngine();
            var climatemodule = new HowLeakyEngineModule_Climate();
            var inputmodule = new HowLeakyInputs_Climate();
            inputmodule.Rain = new List<double>(new double[10]);
            inputmodule.MaxT = new List<double>(new double[10]);
            inputmodule.MinT = new List<double>(new double[10]);
            inputmodule.PanEvap = new List<double>(new double[10]);
            inputmodule.Radiation = new List<double>(new double[10]);
            inputmodule.VP = new List<double>(new double[10]);
            climatemodule.Engine = engine;
            climatemodule.InputModel = inputmodule;
            inputmodule.StartDate = new BrowserDate(2019, 1, 1);
            engine.TodaysDate = new BrowserDate(2019, 3, 4);

            Assert.Throws<Exception>(() => climatemodule.Simulate());

        }

        [Test]
        public void RainOnDay_RainEquals10_ReturnsTrue()
        {
            
            var climatemodule = new HowLeakyEngineModule_Climate();
            var inputmodule = new HowLeakyInputs_Climate();
            climatemodule.InputModel= inputmodule;
            inputmodule.StartDate = new BrowserDate(2020, 1, 1);
            inputmodule.Rain = new List<double>(new double[10]);
            inputmodule.Rain[1]=10;
            var rain=climatemodule.RainOnDay(new BrowserDate(2020,1,2));

            Assert.IsTrue(ValuesAreEqual(rain,10));

        }
        [Test]
        public void RainOnDay_IndexOutOfRange_ThrowsException()
        {
            var climatemodule = new HowLeakyEngineModule_Climate();
            var inputmodule = new HowLeakyInputs_Climate();
            climatemodule.InputModel = inputmodule;
            inputmodule.StartDate = new BrowserDate(2020, 1, 1);
            inputmodule.Rain = new List<double>(new double[10]);
            inputmodule.Rain[1] = 10;
    

            Assert.Throws<Exception>(() => climatemodule.RainOnDay(new BrowserDate(2020, 12, 2)));
        }

        public bool ValuesAreEqual(double val1,double val2)
        {
            return Math.Abs(val1-val2)<0.000001;
        }
    }
}
