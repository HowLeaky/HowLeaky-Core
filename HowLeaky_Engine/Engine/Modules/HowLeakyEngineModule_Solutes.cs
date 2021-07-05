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

        [Output] public double TotalSoilSolute { get; set; }
        [Output] public List<double> LayerSoluteLoad { get; set; }
        [Output] public List<double> LayerSoluteConcmgPerL { get; set; }
        [Output] public List<double> LayerSoluteConcmgPerkg { get; set; }
        [Output] public double LeachateSoluteConcmgPerL { get; set; }
        [Output] public double LeachateSoluteLoadkgPerha { get; set; }


        public override void Initialise()
        {
            LayerSoluteLoad = new List<double>(new double[Engine.SoilModule.LayerCount]);
            LayerSoluteConcmgPerL = new List<double>(new double[Engine.SoilModule.LayerCount]);
            LayerSoluteConcmgPerkg = new List<double>(new double[Engine.SoilModule.LayerCount]);
        }

        public override void Simulate()
        {
            try
            {
                if (CanSimulateSolutes())
                {
                    double rain = Engine.ClimateModule.Rain;
                    double runoff = Engine.SoilModule.Runoff;
                    double irrigationAmountmm = Engine.SoilModule.Irrigation;
                    //calculte solute input loadings.
                    double kgsSoilinLayer1 = Engine.SoilModule.InputModel.BulkDensity[0] * 1000.0 * Engine.SoilModule.Depth[1] * 10000.0 / 1000.0;//per ha
                    if (rain > 0)
                    {
                        if (!MathTools.DoublesAreEqual(kgsSoilinLayer1, 0))
                        {
                            LayerSoluteConcmgPerkg[0] += InputModel.RainfallConcentration * (rain - runoff) * 10000.0 / kgsSoilinLayer1;
                        }
                        else
                        {

                            MathTools.LogDivideByZeroError("CalculateSolutes", "kgs_soil_in_layer_1", "out_LayerSoluteConc_mg_per_kg[0]");
                        }
                    }
                    if (!MathTools.DoublesAreEqual(irrigationAmountmm, 0))
                    {
                        if (!MathTools.DoublesAreEqual(kgsSoilinLayer1, 0))
                        {
                            LayerSoluteConcmgPerkg[0] += InputModel.IrrigationConcentration * irrigationAmountmm * 10000.0 / kgsSoilinLayer1;
                        }
                        else
                        {
                            MathTools.LogDivideByZeroError("CalculateSolutes", "kgs_soil_in_layer_1", "out_LayerSoluteConc_mg_per_kg[0]");
                        }
                    }
                    //initialise total solute count;
                    TotalSoilSolute = 0;
                    //Route solutes down through layer.
                    for (int i = 0; i < Engine.SoilModule.LayerCount; ++i)
                    {
                        double SWRelOD = Engine.SoilModule.SoilWaterRelWP[i] + Engine.SoilModule.WiltingPointRelOD[i];
                        double StartOfDaySWRelOD = SWRelOD + Engine.SoilModule.Seepage[i + 1];

                        if (!MathTools.DoublesAreEqual(StartOfDaySWRelOD, 0) && !MathTools.DoublesAreEqual(SWRelOD, 0))
                        {
                            double kgsSoilInLayer = Engine.SoilModule.InputModel.BulkDensity[i] * 1000.0 * (Engine.SoilModule.Depth[i + 1] - Engine.SoilModule.Depth[i]) * 10000.0 / 1000.0;

                            //calculate the potential drained loadings (doesn't take into account mixing effects)
                            double potentialDrainedSolutemg = 0;
                            if (!MathTools.DoublesAreEqual(StartOfDaySWRelOD, 0))
                                potentialDrainedSolutemg = (LayerSoluteConcmgPerkg[i] * kgsSoilInLayer / StartOfDaySWRelOD) * Engine.SoilModule.Seepage[i + 1];
                            else
                            {
                                MathTools.LogDivideByZeroError("CalculateSolutes", "StartOfDay_SW_rel_OD", "potential_drained_solute_mg");
                            }
                            //calculate the actual drained loadings
                            double actualDrainedSolutemg = InputModel.MixingCoefficient * potentialDrainedSolutemg;

                            //take the drained solute load away from the balance in the layer
                            if (!MathTools.DoublesAreEqual(kgsSoilInLayer, 0))
                            {
                                /*OUTPUT*/
                                LayerSoluteConcmgPerkg[i] -= actualDrainedSolutemg / kgsSoilInLayer;
                            }
                            else
                            {
                                LayerSoluteConcmgPerkg[i] = 0;
                                MathTools.LogDivideByZeroError("CalculateSolutes", "kgs_soil_in_layer", "out_LayerSoluteConc_mg_per_kg[i]");
                            }

                            //calculate the solute load in the layer
                            /*OUTPUT*/
                            LayerSoluteLoad[i] = LayerSoluteConcmgPerkg[i] * kgsSoilInLayer / 1000000.0;

                            //keep track of total load
                            /*OUTPUT*/
                            TotalSoilSolute += LayerSoluteLoad[i];

                            //calculate solute concentration in layer
                            double SWVolumetric;
                            if (!MathTools.DoublesAreEqual(Engine.SoilModule.Depth[i + 1], 0))
                                SWVolumetric = SWRelOD / (double)(Engine.SoilModule.Depth[i + 1]);
                            else
                            {
                                SWVolumetric = 0;

                                MathTools.LogDivideByZeroError("CalculateSolutes", "depth[i+1]", "SW_Volumetric");
                            }
                            if (!MathTools.DoublesAreEqual(SWVolumetric, 0))
                                /*OUTPUT*/
                                LayerSoluteConcmgPerL[i] = LayerSoluteConcmgPerkg[i] * Engine.SoilModule.InputModel.BulkDensity[i] / SWVolumetric;
                            else
                            {
                                LayerSoluteConcmgPerL[i] = 0;

                                MathTools.LogDivideByZeroError("CalculateSolutes", "SW_Volumetric", "out_LayerSoluteConc_mg_per_L[i]");
                            }

                            //push solute into next layer OR calculate leaching (deep drainage) loadings
                            if (i + 1 < Engine.SoilModule.LayerCount)
                            {
                                double kgsSoilInNextLayer = Engine.SoilModule.InputModel.BulkDensity[i + 1] * 1000.0 * (Engine.SoilModule.Depth[i + 2] - Engine.SoilModule.Depth[i + 1]) * 10000.0 / 1000.0;
                                if (!MathTools.DoublesAreEqual(kgsSoilInNextLayer, 0))
                                {
                                    LayerSoluteConcmgPerkg[i + 1] += actualDrainedSolutemg / kgsSoilInNextLayer;
                                }
                                else
                                {
                                    LayerSoluteConcmgPerkg[i + 1] = 0;
                                    MathTools.LogDivideByZeroError("CalculateSolutes", "kgs_soil_in_next_layer", "out_LayerSoluteConc_mg_per_kg[i+1]");
                                }
                            }
                            else
                            {

                                /*OUTPUT*/
                                LeachateSoluteLoadkgPerha = LayerSoluteConcmgPerL[i] / 1000000.0 * Engine.SoilModule.Seepage[i + 1] * 10000.0;
                                if (Engine.SoilModule.Seepage[i + 1] > 0)
                                /*OUTPUT*/
                                {
                                    LeachateSoluteConcmgPerL = LayerSoluteConcmgPerL[i];      //CHECKTHIS
                                }
                                else
                                {
                                    LeachateSoluteConcmgPerL = 0;
                                }
                            }
                        }
                        else
                        {
                            LayerSoluteConcmgPerkg[i] = 0;
                            LayerSoluteConcmgPerL[i] = 0;
                        }
                    }

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
        }

        public bool CanSimulateSolutes()
        {
            return true;
        }
    }
}
