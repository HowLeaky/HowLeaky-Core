using HowLeaky_SimulationEngine.Attributes;
using HowLeaky_SimulationEngine.Errors;
using HowLeaky_SimulationEngine.Inputs;
using HowLeaky_SimulationEngine.Outputs;
using HowLeaky_SimulationEngine.Tools;
using System;

namespace HowLeaky_SimulationEngine.Engine
{
    public class HowLeakyEngineModule_Phosphorus : _CustomHowLeakyEngineModule
    {

        public HowLeakyOutputSummary_Phosphorus Summary { get; set; }
        public HowLeakyEngineModule_Phosphorus(HowLeakyEngine sim, HowLeakyInputs_Phosphorus inputs) : base(sim)
        {
            Name = inputs.Name;
            InputModel = inputs;
           

        }

        public HowLeakyEngineModule_Phosphorus()
        {
        }



        //TODO: Change to enums
        static int ENRICHMENT_RATIO = 0;
        static int ENRICHMENT_CLAY = 1;
        static int DISSOLVED_P_VICDPI = 0;
        static int DISSOLVED_P_QLDREEF = 1;

        //public	double so_DissolvePExport_kg_per_ha{get;set;}		// Dissolved P export(kg/ha)
        //public	double so_EMC_mg_per_l{get;set;}						// Phosphorus EMC(mg/L)

        public double MaxPhosConcBioParticmgPerL { get; set; }
        public double MaxPhosConcBiomgPerL { get; set; }
        public double MaxPhosConcParticmgPerL { get; set; }
        public double MaxPhosConcTotalmgPerL { get; set; }
        public double MaxPhosConcDissolvemgPerL { get; set; }

        public HowLeakyInputs_Phosphorus InputModel { get; set; }
        //public PhosphorusOutputDataModel Output { get; set; }

        //Reportable Outputs
        [Output] public double ParticulateConc { get; set; }
        [Output] public double DissolvedConc { get; set; }
        [Output] public double BioAvailParticPConc { get; set; }
        [Output] public double BioAvailPConc { get; set; }
        [Output] public double TotalPConc { get; set; }
        [Output] public double ParticPExport { get; set; }
        [Output] public double BioAvailParticPExport { get; set; }
        [Output] public double TotalBioAvailExport { get; set; }
        [Output] public double TotalP { get; set; }
        [Output] public double PPHLC { get; set; }
        [Output] public double PhosExportDissolve { get; set; }

        //public double CKQ { get; set; }
        //  [Output("", "kg/ha")]


        public override void Initialise()
        {
            try
            {
                MaxPhosConcBioParticmgPerL = 0;
                MaxPhosConcBiomgPerL = 0;
                MaxPhosConcParticmgPerL = 0;
                MaxPhosConcTotalmgPerL = 0;
                MaxPhosConcDissolvemgPerL = 0;

                ParticulateConc = 0;
                DissolvedConc = 0;
                BioAvailParticPConc = 0;
                BioAvailPConc = 0;
                TotalPConc = 0;
                ParticPExport = 0;
                BioAvailParticPExport = 0;
                TotalBioAvailExport = 0;
                TotalP = 0;
                PPHLC = 0;
                PhosExportDissolve = 0;
                Summary = new HowLeakyOutputSummary_Phosphorus(Engine);
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        

       
        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()
        {
            try
            {
                
                if (Engine.SoilModule.Runoff > 0)
                {
                    CalculateDissolvedPhosphorus();
                    CalculateParticulatePhosphorus();
                    CalculateTotalPhosphorus();
                    CalculateBioavailableParticulatePhosphorus();
                    CalculateBioavailablePhosphorus();
                    TestMaximumPhosphorusConcentrations();
                }
                else
                {
                    ResetPhosphorusOutputParameters();

                }
                CalculateCATCHMODSOutputs();                                  
                Summary.Update(Engine);

            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }


      

        public void CalculateDissolvedPhosphorus()
        {
            try
            {
                double phosSaturationIndex = 0;
                double pMaxSorption = 0;
                if (InputModel.DissolvedPOpt == DISSOLVED_P_VICDPI)
                {
                    pMaxSorption = 1447 * (1 - Math.Exp(-0.001 * InputModel.PBI));
                }
                else
                {
                    pMaxSorption = 5.84 * InputModel.PBI - 0.0096 * Math.Pow(InputModel.PBI, 2);
                    if (pMaxSorption < 50)
                    {
                        pMaxSorption = 50;
                    }
                }

                double p_enrich = CalculatePhosphorusEnrichmentRatio();
                if (!MathTools.DoublesAreEqual(pMaxSorption, 0))
                {
                    phosSaturationIndex = (InputModel.ColwellP * p_enrich) / (pMaxSorption) * 100.0;
                }
                else
                {
                    phosSaturationIndex = 0;
                    MathTools.LogDivideByZeroError("CalculateDissolvedPhosphorus", "p_max_sorption", "phos_saturation_index");
                }

                if (InputModel.DissolvedPOpt == DISSOLVED_P_VICDPI)
                {
                    //The following is the original fn published at MODSIM11:
                    if (phosSaturationIndex < 5)
                    {
                        DissolvedConc = 10.0 * phosSaturationIndex / 1000.0;
                    }
                    else
                    {
                        DissolvedConc = (-100.0 + 30 * phosSaturationIndex) / 1000.0;
                    }
                }
                else
                {
                    if (phosSaturationIndex < 10)
                    {
                        DissolvedConc = 7.5 * phosSaturationIndex / 1000.0;
                    }
                    else
                    {
                        DissolvedConc = ((-200.0 + 27.5 * phosSaturationIndex) / 1000.0);
                    }
                }
                PhosExportDissolve = (DissolvedConc / 1000000.0 * Engine.SoilModule.Runoff * 10000.0);//CHECK - this wasn't marked as an output parameter
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateParticulatePhosphorus()
        {
            try
            {
                double pEnrich = CalculatePhosphorusEnrichmentRatio();
                double pSedConcGPerL = 0;
                if (!MathTools.DoublesAreEqual(Engine.SoilModule.Runoff, 0))//&&sim.in_SedDelivRatio!=0)
                {
                    // convert t/ha to g/ha and sim.out_WatBal_Runoff_mm(mm) to L/ha.  Then the division yields g/L of sediment.
                    pSedConcGPerL = Engine.SoilModule.HillSlopeErosion * 1000000.0 / (Engine.SoilModule.Runoff * 10000.0) * Engine.SoilModule.InputModel.SedDelivRatio;
                }
                // convert sed conc from g/L to mg/L and totalPconc from mg/kg (I assume it is in mg/kg) to g/g
                ParticulateConc = (pSedConcGPerL * 1000.0 * InputModel.TotalPConc / 1000000.0 * pEnrich);
                ParticPExport = (ParticulateConc / 1000000.0 * Engine.SoilModule.Runoff * 10000.0);
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double CalculatePhosphorusEnrichmentRatio()
        {
            try
            {
                if (InputModel.PEnrichmentOpt == ENRICHMENT_RATIO)
                {
                    //input a constant (range = 1 to ~10).
                    return InputModel.EnrichmentRatio;
                }
                else if (InputModel.PEnrichmentOpt == ENRICHMENT_CLAY)
                {
                    //Clay%.  This function is based on a few data from Qld field experiments.
                    //Penrichment=MIN(10,MAX(1,15-0.33*clay)). The results of this funtion are:
                    return Math.Min(10.0, Math.Max(1, 15 - 0.33 * InputModel.ClayPercentage));
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

            return 0;
        }

        public void CalculateTotalPhosphorus()
        {
            TotalPConc = (DissolvedConc + ParticulateConc);
            TotalP = (PhosExportDissolve + ParticPExport); //CHECK Phos_Export_Dissolve wasn't marked as out;

        }

        public void CalculateBioavailableParticulatePhosphorus()
        {
            try
            {
                double pA;
                if (!MathTools.DoublesAreEqual(InputModel.TotalPConc, 0))
                {
                    pA = InputModel.ColwellP * 1.2 / InputModel.TotalPConc;
                }
                else
                {
                    pA = 0;
                    //	LogDivideByZeroError("CalculateBioavailableParticulatePhosphorus","in_TotalPConc_mg_per_kg","pA");
                }
                BioAvailParticPConc = (ParticulateConc * pA);
                BioAvailParticPExport = (ParticPExport * pA);
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }


        public void CalculateBioavailablePhosphorus()
        {
            try
            {
                BioAvailPConc = (0.8 * DissolvedConc + BioAvailParticPConc);
                TotalBioAvailExport = (0.8 * PhosExportDissolve + BioAvailParticPExport);
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        public void CalculateCATCHMODSOutputs()
        {
            try
            {
                if (Engine.SoilModule.Runoff > 0 && Engine.SoilModule.HillSlopeErosion > 0)
                {
                    if (!MathTools.DoublesAreEqual(Engine.SoilModule.InputModel.SedDelivRatio, 0) && !MathTools.DoublesAreEqual(Engine.SoilModule.UsleLsFactor, 0))
                    {
                        PPHLC = (ParticPExport / (Engine.SoilModule.InputModel.SedDelivRatio * Engine.SoilModule.UsleLsFactor));
                    }
                    else
                    {
                        PPHLC = 0;
                    }
                }
                else
                {
                    PPHLC = 0;
                }
                //CKQ = (Sim.SoilModule.SedCatchmod);
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }


        public void ResetPhosphorusOutputParameters()
        {
            try
            {
                MaxPhosConcBioParticmgPerL = 0;
                MaxPhosConcBiomgPerL = 0;
                MaxPhosConcParticmgPerL = 0;
                MaxPhosConcTotalmgPerL = 0;
                MaxPhosConcDissolvemgPerL = 0;

                ParticulateConc = 0;
                DissolvedConc = 0;
                BioAvailParticPConc = 0;
                BioAvailPConc = 0;
                TotalPConc = 0;
                ParticPExport = 0;
                BioAvailParticPExport = 0;
                TotalBioAvailExport = 0;
                TotalP = 0;
                PPHLC = 0;
                PhosExportDissolve = 0;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }


        public void TestMaximumPhosphorusConcentrations()
        {
            try
            {
                if (MaxPhosConcParticmgPerL < ParticulateConc)
                {
                    MaxPhosConcParticmgPerL = ParticulateConc;
                }
                if (MaxPhosConcTotalmgPerL < TotalPConc)
                {
                    MaxPhosConcTotalmgPerL = TotalPConc;
                }
                if (MaxPhosConcDissolvemgPerL < DissolvedConc)
                {
                    MaxPhosConcDissolvemgPerL = DissolvedConc;
                }
                if (MaxPhosConcBioParticmgPerL < BioAvailParticPConc)
                {
                    MaxPhosConcBioParticmgPerL = BioAvailParticPConc;
                }
                if (MaxPhosConcBiomgPerL < TotalPConc)
                {
                    MaxPhosConcBiomgPerL = BioAvailPConc;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }


    }
}
