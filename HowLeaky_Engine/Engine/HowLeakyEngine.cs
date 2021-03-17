﻿using HowLeaky_SimulationEngine.Attributes;
using HowLeaky_SimulationEngine.Inputs;
using HowLeaky_SimulationEngine.Outputs;
using HowLeaky_SimulationEngine.Outputs.Definitions;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;

namespace HowLeaky_SimulationEngine.Engine
{
    public partial class HowLeakyEngine
    {


        public HowLeakyEngine(string outputs, Dictionary<string,OutputAttributes>remapdict=null)
        {
            RemapDict=remapdict;
            OutputsCSV = outputs;
            SortedVegetationModules=new List<_CustomHowLeakyEngine_VegModule>();
        }





        public void Execute(HowLeakyInputsModel inputs, Action<HowLeakyOutputs> onCompletion, Action<Exception> onError)
        {
            try
            {
                if (LoadInputs(inputs))
                {
                    PrepareForNewSimulation();
                    while (TodaysDate.DateInt <= EndDate.DateInt)
                    {
                        SimulateDay();
                        TodaysDate.IncrementDay();
                    }

                    onCompletion(Outputs);

                }
                else
                {
                    if(onError!=null)
                    {
                        onError(new Exception("Could not load inputs"));
                    }
                }

            }
            catch (Exception ex)
            {
                if(onError!=null)
                    {
                        onError(ex);
                    }
            }
        }
    }

}

