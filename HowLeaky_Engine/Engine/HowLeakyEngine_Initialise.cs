using HowLeaky_SimulationEngine.Attributes;
using HowLeaky_SimulationEngine.Errors;
using HowLeaky_SimulationEngine.Inputs;
using HowLeaky_SimulationEngine.Outputs;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace HowLeaky_SimulationEngine.Engine
{
    public partial class HowLeakyEngine
    {

        public bool PrepareForNewSimulation()
        {
            try
            {
                InitialiseGlobals();
                InitialiseClimateModule();
                InitialiseSoilModule();
                InitialiseIrrigationModule();
                InitialisePhosphorusModule();
                InitialiseNitrateModule();
                InitialiseSolutesModule();
                InitialiseVegetationModules();
                InitialisePesticideModules();
                InitialiseTillageModules();
                
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return false;
        }

        private void InitialiseGlobals()
        {
            ResetToDefault();
        }

        public void InitialiseClimateModule()
        {
            if (ClimateModule != null) ClimateModule.Initialise();
        }
        public void InitialiseSoilModule()
        {
            if (SoilModule != null) SoilModule.Initialise();
        }

        public void InitialiseIrrigationModule()
        {
            if (IrrigationModule != null) IrrigationModule.Initialise();
        }

        public void InitialisePhosphorusModule()
        {
            if (PhosphorusModule != null) PhosphorusModule.Initialise();
        }

        public void InitialiseNitrateModule()
        {
            if (NitrateModule != null) NitrateModule.Initialise();
        }

        public void InitialiseSolutesModule()
        {
            if (SolutesModule != null) SolutesModule.Initialise();
        }

        public void InitialiseVegetationModules()
        {
            try
            {
                if (VegetationModules != null)
                {
                    SortedVegetationModules=new List<_CustomHowLeakyEngine_VegModule>();
                    CurrentCrop = GetCrop(0);
                    DaysSinceHarvest = 0;
                    TotalTranspiration = 0;
                    TotalEvapotranspiration = 0;
                    foreach (var veg in VegetationModules)
                    {
                        veg.Initialise();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        public void InitialisePesticideModules()
        {
            try
            {
                if (PesticideModules != null)
                {
                    foreach (var pest in PesticideModules)
                    {
                        pest.Initialise();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }
        public void InitialiseTillageModules()
        {
            try
            {
                DaysSinceTillage = -1;
                if (TillageModules != null)
                {
                    foreach (var till in TillageModules)
                    {
                        till.Initialise();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }





    }
}
