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


        [Test]
        public void RainOnDay_SumRainDelayEquals0_ReturnsCorrectSum()
        {

            var climatemodule = new HowLeakyEngineModule_Climate();
            var inputmodule = new HowLeakyInputs_Climate();
            climatemodule.InputModel = inputmodule;
            inputmodule.StartDate = new BrowserDate(2020, 1, 1);
            inputmodule.Rain = new List<double>(new double[10]);
            inputmodule.Rain[0] = 1;
            inputmodule.Rain[1] = 2;
            inputmodule.Rain[2] = 3;
            inputmodule.Rain[3] = 4;
            inputmodule.Rain[4] = 5;//THIS ONe
            inputmodule.Rain[5] = 6;//THIS ONe
            inputmodule.Rain[6] = 7;//THIS ONe
            inputmodule.Rain[7] = 8;
            inputmodule.Rain[8] = 9;
            inputmodule.Rain[9] = 10;
            climatemodule.CurrentIndex=6;
            var rain = climatemodule.SumRain(3,0);

            Assert.IsTrue(ValuesAreEqual(rain, 5+6+7));

        }
        [Test]
        public void RainOnDay_SumRainDelayEqual2_ReturnsCorrectSum()
        {

            var climatemodule = new HowLeakyEngineModule_Climate();
            var inputmodule = new HowLeakyInputs_Climate();
            climatemodule.InputModel = inputmodule;
            inputmodule.StartDate = new BrowserDate(2020, 1, 1);
            inputmodule.Rain = new List<double>(new double[10]);
            inputmodule.Rain[0] = 1;
            inputmodule.Rain[1] = 2;
            inputmodule.Rain[2] = 3;//THIS ON
            inputmodule.Rain[3] = 4;//THIS ON
            inputmodule.Rain[4] = 5;//THIS ON
            inputmodule.Rain[5] = 6;
            inputmodule.Rain[6] = 7;
            inputmodule.Rain[7] = 8;
            inputmodule.Rain[8] = 9;
            inputmodule.Rain[9] = 10;
            climatemodule.CurrentIndex = 6;
            var rain = climatemodule.SumRain(3, 2);

            Assert.IsTrue(ValuesAreEqual(rain, 3+ 4 + 5));

        }



        public bool ValuesAreEqual(double val1,double val2)
        {
            return Math.Abs(val1-val2)<0.000001;
        }
    }
}
