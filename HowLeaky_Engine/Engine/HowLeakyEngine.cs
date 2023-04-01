using HowLeaky_SimulationEngine.Attributes;
using HowLeaky_SimulationEngine.Inputs;
using HowLeaky_SimulationEngine.Outputs;
using HowLeaky_SimulationEngine.Outputs.Definitions;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace HowLeaky_SimulationEngine.Engine
{
    //verion
    public partial class HowLeakyEngine
    {
        public HowLeakyEngine()
        {
            InitialPAW = 0.5;
            IncludeSummaries = true;
        }
       
        

        public static string GetAppVersion()
        {
            var appVersion = Assembly.GetAssembly(typeof(HowLeakyEngine)).GetName().Version;

            return string.Format("V{0}_{1}_{2}", appVersion.Major, appVersion.Minor, appVersion.Build, appVersion.Revision);
        }
         


        public bool InitialiseOutputs(string outputs, Dictionary<string, OutputAttributes> remapdict = null)
        {
            try
            {
               
                RemapDict = remapdict;
                OutputsCSV = outputs;
                BuildOutputDefinitions(OutputsCSV, RemapDict);
                InitialiseOutputObject();
                return true;
            }
            catch(Exception ex)
            {

            }
            

            return false;
        }


        public void Execute( Action<HowLeakyOutputs> onCompletion, Action<Exception> onError)
        {
            try
            {
               
                PrepareForNewSimulation();
                while (TodaysDate.DateInt <= EndDate.DateInt)
                {
                    SimulateDay();
                    TodaysDate.IncrementDay();
                }
                Outputs.LoadSummaries(this);
                onCompletion(Outputs);
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

