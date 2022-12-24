using HowLeaky_Engine.Outputs.Summaries;
using HowLeaky_SimulationEngine.Attributes;
using HowLeaky_SimulationEngine.Errors;
using HowLeaky_SimulationEngine.Inputs;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Engine
{
    public class HowLeakyEngineModule_Solutes : _CustomHowLeakyEngineModule
    {
        public HowLeakyEngineModule_Solutes(HowLeakyEngine sim, HowLeakyInputs_Solute inputs) : base(sim)
        {
            InputModel = inputs;
        }

        public HowLeakyEngineModule_Solutes()
        {
        }

        public HowLeakyInputs_Solute InputModel { get; set; }
        public HowLeakyOutputSummary_Solutes Summary { get; set; }

        [Output] public double total_soil_solute_mg_per_kg { get; set; }
        [Output] public double total_soil_solute_mg_per_L { get; set; }
        [Output] public double total_soil_solute_kg_per_ha { get; set; }
        [Output] public List<double> solute_conc_layer_mg_per_L { get; set; }
        [Output] public List<double> solute_conc_layer_mg_per_kg { get; set; }
        [Output] public List<double> solute_load_layer_kg_per_ha { get; set; }
        [Output] public double solute_leaching_conc_mg_per_L { get; set; }
        [Output] public double solute_leaching_load_kg_per_ha { get; set; }
      

        public override void Initialise()
        {
            solute_conc_layer_mg_per_L = new List<double>(new double[Engine.SoilModule.LayerCount]);
            solute_conc_layer_mg_per_kg = new List<double>(new double[Engine.SoilModule.LayerCount]);
            solute_load_layer_kg_per_ha = new List<double>(new double[Engine.SoilModule.LayerCount]);
            for (int i = 0; i < Engine.SoilModule.LayerCount; ++i)
            {
                solute_conc_layer_mg_per_kg[i] = InputModel.DefaultInitialConc;
                if (i == 0 && InputModel.StartConcOption  > 0)
                    solute_conc_layer_mg_per_kg[i] = InputModel.Layer1InitialConc;
                else if (i == 1 && InputModel.StartConcOption > 1)
                    solute_conc_layer_mg_per_kg[i] = InputModel.Layer2InitialConc;
                else if (i == 2 && InputModel.StartConcOption > 2)
                    solute_conc_layer_mg_per_kg[i] = InputModel.Layer3InitialConc;
                else if (i == 3 && InputModel.StartConcOption > 3)
                    solute_conc_layer_mg_per_kg[i] = InputModel.Layer4InitialConc;
                else if (i == 4 && InputModel.StartConcOption > 4)
                    solute_conc_layer_mg_per_kg[i] = InputModel.Layer5InitialConc;
                else if (i == 5 && InputModel.StartConcOption > 5)
                    solute_conc_layer_mg_per_kg[i] = InputModel.Layer6InitialConc;
                else if (i == 6 && InputModel.StartConcOption > 6)
                    solute_conc_layer_mg_per_kg[i] = InputModel.Layer7InitialConc;
                else if (i == 7 && InputModel.StartConcOption > 7)
                    solute_conc_layer_mg_per_kg[i] = InputModel.Layer8InitialConc;
                else if (i == 8 && InputModel.StartConcOption > 8)
                    solute_conc_layer_mg_per_kg[i] = InputModel.Layer9InitialConc;
                else if (i == 9 && InputModel.StartConcOption > 9)
                    solute_conc_layer_mg_per_kg[i] = InputModel.Layer10InitialConc;

            }
            Summary = new HowLeakyOutputSummary_Solutes();
        }

        public override void Simulate()
        {
            try
            {
                if (CanSimulateSolutes())
                {
                    var rain = Engine.ClimateModule.Rain;
                    var runoff = Engine.SoilModule.Runoff;
                    var irrigation_amount = Engine.SoilModule.Irrigation;
                    var solute_conc_rainfall_mg_per_kg = 0.0;
                    var solute_conc_irrig_mg_per_kg = 0.0;
                    var solute_conc_rainfall_kg_per_ha = 0.0;
                    var solute_conc_irrig_kg_per_ha = 0.0;
                    var SoluteRainfallConcentration_mg_per_L = InputModel.RainfallConcentration;
                    var SoluteIrrigationConcentration_mg_per_L = InputModel.IrrigationConcentration;
                    //calculte solute input loadings.
                    double kgs_soil_in_layer_1 = Engine.SoilModule.InputModel.BulkDensity[0] * 1000.0 * Engine.SoilModule.Depth[1] * 10000.0 / 1000.0;//per ha
                    if (rain > 0)
                    {
                        if (MathTools.NotZero(kgs_soil_in_layer_1))
                        {
                            solute_conc_rainfall_mg_per_kg = SoluteRainfallConcentration_mg_per_L * (rain - runoff) * 10000.0 / kgs_soil_in_layer_1;
                            solute_conc_layer_mg_per_kg[0] += solute_conc_rainfall_mg_per_kg;  //CHECKED OK
                            solute_conc_rainfall_kg_per_ha = solute_conc_rainfall_mg_per_kg * kgs_soil_in_layer_1 / 1000000.0;
                        }
                        else
                        {

                            MathTools.LogDivideByZeroError("CalculateSolutes", "kgs_soil_in_layer_1", "out_LayerSoluteConc_mg_per_kg[0]");
                        }
                    }
                    if (MathTools.NotZero(irrigation_amount))
                    {
                        if (MathTools.NotZero(kgs_soil_in_layer_1))
                        {
                            solute_conc_irrig_mg_per_kg = SoluteIrrigationConcentration_mg_per_L * irrigation_amount * 10000.0 / kgs_soil_in_layer_1;
                            solute_conc_layer_mg_per_kg[0] += solute_conc_irrig_mg_per_kg;
                            solute_conc_irrig_kg_per_ha = solute_conc_irrig_mg_per_kg * kgs_soil_in_layer_1 / 1000000.0;
                        }
                        else
                        {
                            MathTools.LogDivideByZeroError("CalculateSolutes", "kgs_soil_in_layer_1", "out_LayerSoluteConc_mg_per_kg[0]");
                        }
                    }
                    //initialise total solute count;
                    double total_soil_mass_kg = 0;
                    double total_SW_rel_OD = 0;
                    total_soil_solute_kg_per_ha = 0;
                    //double sum_actual_drained_solute_mg = 0;
                    //double sum_kgs_soil_in_next_layer = 0;
                    //Route solutes down through layer.
                    for (int i = 0; i < Engine.SoilModule.LayerCount; ++i)
                    {
                        double SW_rel_OD =  Engine.SoilModule.SoilWaterRelWP[i] + Engine.SoilModule.WiltingPointRelOD[i];
                        total_SW_rel_OD += SW_rel_OD;
                        double StartOfDay_SW_rel_OD = SW_rel_OD + Engine.SoilModule.Seepage[i + 1];

                        
                        if (MathTools.NotZero(StartOfDay_SW_rel_OD) && MathTools.NotZero(SW_rel_OD))
                        {
                            double kgs_soil_in_layer = Engine.SoilModule.InputModel.BulkDensity[i] * 1000.0 * (Engine.SoilModule.Depth[i + 1] - Engine.SoilModule.Depth[i]) * 10000.0 / 1000.0;
                            total_soil_mass_kg += kgs_soil_in_layer;
                            //calculate the potential drained loadings (doesn't take into account mixing effects)
                            double potential_drained_solute_mg = 0;
                            if (MathTools.NotZero(StartOfDay_SW_rel_OD ))
                            { 
                                potential_drained_solute_mg = (solute_conc_layer_mg_per_kg[i] * kgs_soil_in_layer / StartOfDay_SW_rel_OD) * Engine.SoilModule.Seepage[i + 1];       //checked ok
                            }
                            else
                            {
                            }
                            //calculate the actual drained loadings
                            double actual_drained_solute_mg = InputModel.MixingCoefficient * potential_drained_solute_mg;

                            //take the drained solute load away from the balance in the layer
                            if (MathTools.NotZero(kgs_soil_in_layer))
                            { 
                                /*OUTPUT*/
                                solute_conc_layer_mg_per_kg[i] -= actual_drained_solute_mg / kgs_soil_in_layer;
                            }
                            else
                            {
                                solute_conc_layer_mg_per_kg[i] = 0;
                            }

                            //calculate the solute load in the layer
                            /*OUTPUT*/
                            solute_load_layer_kg_per_ha[i] = solute_conc_layer_mg_per_kg[i] * kgs_soil_in_layer / 1000000.0;   //checked SUSPICIOUS

                            //keep track of total load
                            /*OUTPUT*/
                            total_soil_solute_kg_per_ha += solute_load_layer_kg_per_ha[i];

                            //calculate solute concentration in layer
                            double SW_Volumetric;
                            if (MathTools.NotZero(Engine.SoilModule.Depth[i + 1]) )
                            {
                                //SW_Volumetric=StartOfDay_SW_rel_OD/double(depth[i+1]);
                                SW_Volumetric = SW_rel_OD / (double)(Engine.SoilModule.Depth[i + 1]);
                            }
                            else
                            {
                                SW_Volumetric = 0;
                            }

                            if (MathTools.NotZero(SW_Volumetric) )
                            {
                                /*OUTPUT*/        //	solute_conc_layer_mg_per_L[i]	= solute_conc_layer_mg_per_kg[i] *BulkDensity[i]/SW_Volumetric;      //checked ok
                                solute_conc_layer_mg_per_L[i] = solute_load_layer_kg_per_ha[i] / (SW_rel_OD * 10.0) * 1000.0;

                            }
                            else
                            {
                                solute_conc_layer_mg_per_L[i] = 0;

                            }

                            //push solute into next layer OR calculate leaching (deep drainage) loadings
                            if (i + 1 < Engine.SoilModule.LayerCount)
                            {
                                double kgs_soil_in_next_layer = Engine.SoilModule.InputModel.BulkDensity[i + 1] * 1000.0 * (Engine.SoilModule.Depth[i + 2] - Engine.SoilModule.Depth[i + 1]) * 10000.0 / 1000.0;
                                if (MathTools.NotZero(kgs_soil_in_next_layer))
                                { 
                                    solute_conc_layer_mg_per_kg[i + 1] += actual_drained_solute_mg / kgs_soil_in_next_layer;
                                }
                                else
                                {
                                    solute_conc_layer_mg_per_kg[i + 1] = 0;

                                }
                            //    sum_actual_drained_solute_mg += actual_drained_solute_mg;
                            //    sum_kgs_soil_in_next_layer += kgs_soil_in_next_layer;
                            }
                            else
                            {

                                /*OUTPUT*/            //solute_leaching_load_kg_per_ha =	solute_conc_layer_mg_per_L[i]/1000000.0 * Seepage[i+1]*10000.0;      // CHECKED OK
                                solute_leaching_load_kg_per_ha = actual_drained_solute_mg / 1000000.0;
                                if (Engine.SoilModule.Seepage[i + 1] > 0)
                                    /*OUTPUT*/
                                    solute_leaching_conc_mg_per_L = solute_conc_layer_mg_per_L[i];  // ????
                                else
                                    solute_leaching_conc_mg_per_L = 0;
                                
                            }
                        }
                        else
                        {
                            solute_conc_layer_mg_per_kg[i] = 0;
                            solute_conc_layer_mg_per_L[i] = 0;
                        }
                    }
                    total_soil_solute_mg_per_kg = total_soil_solute_kg_per_ha / total_soil_mass_kg * 1000000.0;
                    total_soil_solute_mg_per_L = total_soil_solute_kg_per_ha / (total_SW_rel_OD * 10.0) * 1000.0;
                   


                    UpdateSolutesSummaryValues();
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }



        public void UpdateSolutesSummaryValues()
        {
            Summary.Update(Engine);
        }

        public bool CanSimulateSolutes()
        {
            return true;
        }
    }
}
