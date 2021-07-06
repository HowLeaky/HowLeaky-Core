using HowLeaky_SimulationEngine.Attributes;
using HowLeaky_SimulationEngine.Enums;
using HowLeaky_SimulationEngine.Errors;
using HowLeaky_SimulationEngine.Inputs;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Engine
{
    public class HowLeakyEngineModule_Climate : _CustomHowLeakyEngineModule
    {
        public HowLeakyEngineModule_Climate(HowLeakyEngine parent, HowLeakyInputs_Climate inputs) : base(parent)
        {
            InputModel = inputs;
        }

        public HowLeakyEngineModule_Climate()
        {
        }

        public HowLeakyInputs_Climate InputModel { get; set; }



        //public BrowserDate DataStartDate { get; set; }
        //public BrowserDate DataEndDate { get; set; }

        [Output] public double Rain { get; set; }
        [Output] public double MaxTemp { get; set; }
        [Output] public double MinTemp { get; set; }
        [Output] public double PanEvap { get; set; }
        [Output] public double SolarRadiation { get; set; }

        public double Temperature { get; set; }
        public double YesterdaysRain { get; set; }

        public int CurrentIndex { get; set; }




        public override void Simulate()
        {
            try
            {
                CurrentIndex = Engine.TodaysDate.DateInt - InputModel.StartDate.DateInt;
                if (CurrentIndex >= 0 && CurrentIndex < InputModel.Rain.Count)
                {

                    Temperature = (InputModel.MaxT[CurrentIndex] + InputModel.MinT[CurrentIndex]) / 2.0;
                    Rain = InputModel.Rain[CurrentIndex] * InputModel.RainfallMultiplier;
                    if (CurrentIndex > 0)
                    {
                        YesterdaysRain = InputModel.Rain[CurrentIndex - 1];
                    }
                    else
                    {
                        YesterdaysRain = 0;
                    }
                    MaxTemp = InputModel.MaxT[CurrentIndex];
                    MinTemp = InputModel.MinT[CurrentIndex];

                    PanEvap = InputModel.PanEvap[CurrentIndex] * InputModel.PanEvapMultiplier;
                    SolarRadiation = InputModel.Radiation[CurrentIndex];
                }
                else
                {
                    throw new ArgumentException($"Climate Index out of bounds",$"Date = {Engine.TodaysDate.ToString("dd/MM/yyyy")}");
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        public double RainOnDay(BrowserDate date)
        {
            try
            {
                int index = date.DateInt - InputModel.StartDate.DateInt;//(day - InputModel.StartDate.Value).Days;

                if (index > 0 && index <= InputModel.Rain.Count - 1)
                {
                    return InputModel.Rain[index];
                }
                
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return 0;
        }

        public double SumRain(int n, int delay)
        {
            try
            {
                double sumrain = 0;
                int index;
                for (int i = 0; i < n; ++i)
                {
                    index = CurrentIndex - i - delay;
                    if (index >= 0)
                    {
                        sumrain += InputModel.Rain[index];
                    }
                }
                return sumrain;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
         //   return 0;

        }
    }
}
