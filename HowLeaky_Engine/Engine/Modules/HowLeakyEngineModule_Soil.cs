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
    public class HowLeakyEngineModule_Soil : _CustomHowLeakyEngineModule
    {
        public HowLeakyEngineModule_Soil(HowLeakyEngine sim, HowLeakyInputs_Soil inputs) : base(sim)
        {
            InputModel = inputs;
        }

        public HowLeakyEngineModule_Soil()
        {
        }

        public HowLeakyInputs_Soil InputModel { get; set; }


        //--------------------------------------------------------------------------
        // intermediate variables
        //--------------------------------------------------------------------------
        //public bool InRunoff { get; set; }
        [Internal] public bool InRunoff2 { get; set; }
        [Internal] public int RunoffEventCount2 { get; set; }
        [Internal] public double PreviousTotalSoilWater { get; set; }
        [Internal] public double EffectiveRain { get; set; }
        [Internal] public double TotalCoverPercent { get; set; }
        [Internal] public double TotalResidueCoverPercent { get; set; }
        [Internal] public double CropCover { get; set; }
        [Internal] public double AccumulatedCover { get; set; }
        [Internal] public double SedimentConc { get; set; }
        [Internal] public double CumSedConc { get; set; }
        [Internal] public double PeakSedConc { get; set; }
        [Internal] public double Satd { get; set; }
        [Internal] public double Sse1 { get; set; }
        [Internal] public double Sse2 { get; set; }
        [Internal] public double Se1 { get; set; }
        [Internal] public double Se2 { get; set; }
        [Internal] public double Se21 { get; set; }
        [Internal] public double Se22 { get; set; }
        [Internal] public double Dsr { get; set; }
        [Internal] public double SedCatchmod { get; set; }
        [Internal] public double SaturationIndex { get; set; }
        [Internal] public double RainSinceTillage { get; set; }
        [Internal] public double Infiltration { get; set; }
        [Internal] public double PotentialSoilEvaporation { get; set; }
        
        [Internal] public double UsleLsFactor { get; set; }
        [Internal] public double PredRh { get; set; }


        [Internal]public double PAWC { get;set;}
        [Internal] public List<double> MCFC { get; set; }
        [Internal] public List<double> SoilWaterRelWP { get; set; }
        [Internal] public List<double> DrainUpperLimitRelWP { get; set; }
        [Internal] public List<double> Depth { get; set; }
        [Internal] public List<double> LayerTranspiration { get; set; }
        [Internal] public List<double> Red { get; set; }
        [Internal] public List<double> WF { get; set; }
        [Internal] public List<double> SaturationLimitRelWP { get; set; }
        [Internal] public List<double> WiltingPointRelOD { get; set; }
        [Internal] public List<double> DULRelOD { get; set; }
        [Internal] public List<double> AirDryLimitRelWP { get; set; }
        [Internal] public List<double> KSat { get; set; }
        [Internal] public List<double> SWCon { get; set; }
        [Internal] public List<double> Seepage { get; set; }
        [Internal] public List<double> MaxDrainage { get; set; }
        [Internal] public int LayerCount { get { return InputModel.LayerCount; } }



        [Output] public double Irrigation { get; set; }
        [Output] public double Runoff { get; set; }
        [Output] public double RunoffFromIrrigation { get; set; }
        [Output] public double RunoffFromRainfall { get; set; }
        [Output] public double SoilEvap { get; set; }
        [Output] public double PotSoilEvap { get; set; }
        [Output] public double Transpiration { get; set; }
        [Output] public double EvapoTransp { get; set; }
        [Output] public double DeepDrainage { get; set; }
        [Output] public double Overflow { get; set; }
        [Output] public double LateralFlow { get; set; }
        [Output] public double VBE { get; set; }
        [Output] public double RunoffCurveNo { get; set; }
       
        [Output] public double RunoffRetentionNumber { get; set; }
        [Output] public double HillSlopeErosion { get; set; }
        [Output] public double OffSiteSedDelivery { get; set; }
        [Output] public double TotalSoilWater { get; set; }
        [Output] public double SoilWaterDeficit { get; set; }
        [Output] public double Layer1SatIndex { get; set; }
        [Output] public double TotalCropResidue { get; set; }
        [Output] public double TotalResidueCover { get; set; }
        [Output] public double TotalCoverAllCrops { get; set; }
        [Output] public List<double> SoilWater { get; set; }
        [Output] public List<double> Drainage { get; set; }



        public override void Initialise()
        {
            try
            {
                Depth = new List<double>(new double[LayerCount + 1]);
                Red = new List<double>(new double[LayerCount]);
                WF = new List<double>(new double[LayerCount]);
                SoilWaterRelWP = new List<double>(new double[LayerCount]);
                DrainUpperLimitRelWP = new List<double>(new double[LayerCount]);
                SaturationLimitRelWP = new List<double>(new double[LayerCount]);
                AirDryLimitRelWP = new List<double>(new double[LayerCount]);
                WiltingPointRelOD = new List<double>(new double[LayerCount]);
                DULRelOD = new List<double>(new double[LayerCount]);
                KSat = new List<double>(new double[LayerCount]);
                SWCon = new List<double>(new double[LayerCount]);
                Seepage = new List<double>(new double[LayerCount + 1]);
                LayerTranspiration = new List<double>(new double[LayerCount]);
                MCFC = new List<double>(new double[LayerCount]);
                SoilWater = new List<double>(new double[LayerCount]);
                Drainage = new List<double>(new double[LayerCount]);

                SoilWaterDeficit = 0;
                Sse1 = 0;
                Sse2 = 0;
                Se1 = 0;
                Se2 = 0;
                Se21 = 0;
                Se22 = 0;
                Dsr = 0;
                CumSedConc = 0;
                PeakSedConc = 0;
                Depth[0] = 0;
                for (int i = 0; i < LayerCount; ++i)
                {
                    Depth[i + 1] = (int)(InputModel.Depths[i] + 0.5);
                    Red[i] = 0;
                    //TODO: Use MaxDailyDrainRate instead of ksat - ksat is misleading
                    KSat[i] = InputModel.MaxDailyDrainRate[i];
                }

                TotalSoilWater = 0.0;
                PreviousTotalSoilWater = 0.0;
                for (int i = 0; i < LayerCount; ++i)
                {
                    if (Depth[i + 1] - Depth[i] > 0)
                    {
                        //PERFECT soil water alorithms relate all values to wilting point.
                        double deltadepth = (Depth[i + 1] - Depth[i]) * 0.01;
                        WiltingPointRelOD[i] = InputModel.WiltingPoint[i] * deltadepth;
                        DULRelOD[i] = InputModel.FieldCapacity[i] * deltadepth;
                        if (i == 0)
                        {
                            AirDryLimitRelWP[0] = WiltingPointRelOD[i] - InputModel.AirDryLimit[i] * deltadepth;
                        }
                        else if (i == 1)
                        {
                            AirDryLimitRelWP[1] = 0.5 * (WiltingPointRelOD[i] - InputModel.AirDryLimit[i] * deltadepth);
                        }
                        else
                        {
                            AirDryLimitRelWP[i] = 0;
                        }
                        DrainUpperLimitRelWP[i] = (InputModel.FieldCapacity[i] * deltadepth) - WiltingPointRelOD[i];
                        SaturationLimitRelWP[i] = (InputModel.Saturation[i] * deltadepth) - WiltingPointRelOD[i];
                    }
                    else
                    {

                        DrainUpperLimitRelWP[i] = 0;
                        SaturationLimitRelWP[i] = 0;
                        AirDryLimitRelWP[0] = 0;
                    }
                }
                var initpawc= Engine.GetInitialPAW();
                for (int i = 0; i < LayerCount; ++i)
                {
                    SoilWaterRelWP[i] = initpawc * DrainUpperLimitRelWP[i];

                    if (SoilWaterRelWP[i] > SaturationLimitRelWP[i])
                    {
                        SoilWaterRelWP[i] = SaturationLimitRelWP[i];
                    }
                    else if (SoilWaterRelWP[i] < 0)
                    {
                        SoilWaterRelWP[i] = 0;
                    }
                    TotalSoilWater += SoilWaterRelWP[i];
                }


                PAWC = 0.0;
                for (int i = 0; i < LayerCount; ++i)
                    PAWC += DrainUpperLimitRelWP[i];


                TotalCropResidue = 0;
                TotalResidueCover = 0;  //0.707*(1.0-exp(-1.0*total_crop_residue/1000.0));
                TotalResidueCoverPercent = 0;


                CalculateInitialValuesOfCumulativeSoilEvaporation();

                CalculateDepthRetentionWeightFactors();

                CalculateDrainageFactors();

                CalculateUSLE_LSFactor();
                RunoffEventCount2 = 0;
                PredRh = 0;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }



        public double VolSat
        {
            get
            {
                try
                {
                    double volSat = 0;
                    for (int i = 0; i < InputModel.LayerCount; i++)
                    {
                        volSat += InputModel.Saturation[i] *
                            (i > 0 ? InputModel.Depths[i] - InputModel.Depths[i - 1] : InputModel.Depths[i]);
                    }
                    return volSat / 100;
                }
                catch (Exception ex)
                {
                    throw ErrorLogger.CreateException(ex);
                }
            }
        }


        public override void SetStartOfDayParameters()
        {
            try
            {
                EffectiveRain = Engine.ClimateModule.Rain;
                SoilWaterDeficit = 0;
                Satd = 0;
                for (int i = 0; i < LayerCount; ++i)
                {
                    Satd = Satd + (SaturationLimitRelWP[i] - SoilWaterRelWP[i]);
                    SoilWaterDeficit = SoilWaterDeficit + (DrainUpperLimitRelWP[i] - SoilWaterRelWP[i]);
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }





        public void CalculateResidue()
        {
            try
            {
                //Sim.CalculateResidue();
                //we already estimated these in the Runoff function- but will recalculate here.
                TotalCropResidue = Engine.TotalCropResidue;
                TotalResidueCover = Engine.TotalResidueCover;
                TotalResidueCoverPercent = Engine.TotalResidueCoverPercent;
                TotalCoverAllCrops = Engine.GetTotalCover();
                CropCover = Engine.GetCropCover();
                TotalCoverPercent = TotalCoverAllCrops * 100.0;
                AccumulatedCover += TotalCoverPercent;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        //Removed Mar 2022
        //public void TryModelSoilCracking()
        //{
        //    try
        //    {
        //        if (InputModel.SoilCrack)
        //        {
        //            //************************************************************************
        //            //*                                                                      *
        //            //*  This function allows for water to directly enter lower layers     *
        //            //*  of the soil profile through cracks. For cracks to occur the top     *
        //            //*  and second profile layers must be less than 30% and 50%             *
        //            //*  respectively of field capacity. Cracks can extend down the          *
        //            //*  profile using similar criteria. This subroutine assumes all         *
        //            //*  cracks must exist at the surface. Water is placed into the          *
        //            //*  lowest accessable layer first.                                      *
        //            //*                                                                      *
        //            //************************************************************************
        //            int nod;
        //            //  Initialise total water redistributed through cracks
        //            double tred = 0;
        //            for (int i = 0; i < LayerCount; ++i)
        //            {
        //                Red[i] = 0;
        //                if (!MathTools.DoublesAreEqual(DrainUpperLimitRelWP[i], 0))
        //                {
        //                    MCFC[i] = SoilWaterRelWP[i] / DrainUpperLimitRelWP[i];
        //                }
        //                else
        //                {
        //                    MCFC[i] = 0;

        //                    LogDivideByZeroError("ModelSoilCracking", "DrainUpperLimit_rel_wp[i]", "mcfc[i]");
        //                }
        //                if (MCFC[i] < 0)
        //                {
        //                    MCFC[i] = 0;
        //                }
        //                else if (MCFC[i] > 1)
        //                {
        //                    MCFC[i] = 1;
        //                }
        //            }

        //            //  Don't continue if rainfall is less than 10mm
        //            if (EffectiveRain < 10)
        //            {
        //                return;
        //            }
        //            //  Check if profile is dry enough for cracking to occur.
        //            if (MCFC[0] >= 0.3 || MCFC[1] >= 0.3)
        //            {
        //                return;
        //            }
        //            //  Calculate number of depths to which cracks extend
        //            nod = 1;
        //            for (int i = 1; i < LayerCount; ++i)
        //            {
        //                if (MCFC[i] >= 0.3)
        //                {
        //                    i = LayerCount;
        //                }
        //                else
        //                {
        //                    ++nod;
        //                }
        //            }
        //            //  Fill cracks from lowest cracked layer first to a maximum of 50% of
        //            //  field capacity.
        //            tred = Math.Min(InputModel.MaxInfiltIntoCracks, EffectiveRain);
        //            for (int i = nod - 1; i >= 0; --i)
        //            {
        //                Red[i] = Math.Min(tred, DrainUpperLimitRelWP[i] / 2.0 - SoilWaterRelWP[i]);
        //                tred -= Red[i];
        //                if (tred <= 0)
        //                {
        //                    i = -1;
        //                }
        //            }

        //            //  calculate effective rainfall after infiltration into cracks.
        //            //  Note that redistribution of water into layer 1 is ignored.
        //            EffectiveRain = EffectiveRain + Red[0] - Math.Min(InputModel.MaxInfiltIntoCracks, EffectiveRain);
        //            Red[0] = 0.0;

        //            //  calculate total amount of water in cracks
        //            for (int i = 0; i < LayerCount; ++i)
        //            {
        //                tred += Red[i];
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ErrorLogger.CreateException(ex);
        //    }
        //}


        public void CalculateRunoff()
        {
            //int progress = 0;
            try
            {
                //  *********************************************************************
                //  *  This subroutine calculates surface runoff using a modified form  *
                //  *  of USDA Curve numbers from CREAMS.  The input value of Curve     *
                //  *  Number for AMC II is adjusted to account for the effects of crop *
                //  *  and residue cover.  The magnitude of the reduction in CNII due   *
                //  *  to cover is governed by the user defined CNRED parameter.        *
                //  *                                                                   *
                //  *  Knisel, W.G. editor. CREAMS: A field-scale model for chemical,   *
                //  *  runoff and erosion from agricultural management systems.         *
                //  *  United States Department of Agriculture, Conservation Research   *
                //  *  Report no. 26.                                                   *
                //  *********************************************************************
                double sumh20;
                Infiltration = 0.0;
                Runoff = 0.0;
                RunoffRetentionNumber = 0;
                double cn1, smx;

                //  ***************************************************
                //  *  Calculate cover effect on curve number (cn2).  *
                //  ***************************************************}
                CropCover = Engine.GetCropCoverIfLAIModel(CropCover);  //LAI Model uses cover from the end of the previous day whereas Cover model predefines at the start of the day
                //Note that RedInCNAtFullCover used to be capped at 30 in the C++ version of HowLeaky.
                //if(InputModel.RedInCNAtFullCover>30)
                //    InputModel.RedInCNAtFullCover=30;
                RunoffCurveNo = InputModel.RunoffCurveNumber - InputModel.RedInCNAtFullCover /* Check this -- in_CurveNumberReduction*/ * Math.Min(1.0, CropCover + TotalResidueCover * (1 - CropCover));
                //progress = 1;

                //this could need attention!!!! Danny Rattray
                //  *******************************************************
                //  *  Calculate roughness effect on curve number (cn2).  *
                //  *******************************************************

                RainSinceTillage += EffectiveRain;
                if (!MathTools.DoublesAreEqual(InputModel.RainToRemoveRough, 0))
                {
                    if (RainSinceTillage < InputModel.RainToRemoveRough && Engine.TillageModules != null)
                    {
                        RunoffCurveNo += Engine.RoughnessRatio * InputModel.MaxRedInCNDueToTill * (RainSinceTillage / InputModel.RainToRemoveRough - 1);
                    }
                }

                if (EffectiveRain < 0.1)
                {
                    if (Engine.IrrigationModule == null)
                    {
                        Runoff = 0;
                    }
                    else
                    {
                        Runoff = Engine.IrrigationModule.IrrigationRunoff;
                    }
                    return;
                }
                //progress = 2;
                if (Engine.UsePerfectCurveNoFn())
                {
                    //  *******************************************************
                    //  *  Calculate smx (CREAMS p14, equations i-3 and i-4)  *
                    //  *******************************************************
                    cn1 = -16.91 + 1.348 * RunoffCurveNo - 0.01379 * RunoffCurveNo * RunoffCurveNo + 0.0001177 * RunoffCurveNo * RunoffCurveNo * RunoffCurveNo;
                    if (!MathTools.DoublesAreEqual(cn1, 0))
                    {
                        smx = 254.0 * ((100.0 / cn1) - 1.0);
                    }
                    else
                    {
                        smx = 0;

                        LogDivideByZeroError("CalculateRunoff", "cn1", "smx");
                    }
                    //progress = 3;
                    //  ***************************************
                    //  *  Calculate retention parameter,  runoff_retention_number  *
                    //  ***************************************
                    sumh20 = 0.0;
                    for (int i = 0; i < LayerCount - 1; ++i)
                    {
                        if (!MathTools.DoublesAreEqual(SaturationLimitRelWP[i], 0))
                        {
                            sumh20 += WF[i] * (Math.Max(SoilWaterRelWP[i], 0) / SaturationLimitRelWP[i]);
                        }
                        else
                        {
                            LogDivideByZeroError("CalculateRunoff", "SaturationLimit_rel_wp[i]", "sumh20");
                        }
                    }
                    RunoffRetentionNumber = (int)(smx * (1.0 - sumh20));
                    //REMOVE INT STATEMENT AFTER VALIDATION
                    //progress = 4;
                }
                else
                {
                    // ******************************************************************
                    // *  MODIFIED Calculate smx (CREAMS p14, equations i-3 and i-4)  	*
                    // *  Fix" for oversize Smx at low CN                     			*
                    // *  e.g. >254mm for cn2<70                              			*
                    // *  Brett Robinson May 2011                             			*
                    // ******************************************************************
                    double temp = 265.0 + (Math.Exp(0.17 * (RunoffCurveNo - 50)) + 1);
                    if (!MathTools.DoublesAreEqual(temp, 0))
                    {
                        if (RunoffCurveNo > 83) // linear above cn2=83
                        {
                            smx = 6 + (100 - RunoffCurveNo) * 6.66;
                        }
                        else            // logistic for cn2<=83
                        {
                            smx = 254.0 - (265.0 * Math.Exp(0.17 * (RunoffCurveNo - 50))) / temp;
                        }
                    }
                    else
                    {
                        smx = 0;

                        LogDivideByZeroError("CalculateRunoff", "(265.0+(exp(cn2)+1)", "smx");
                    }
                    //progress = 3;
                    //  ***************************************
                    //  *  Calculate retention parameter,  runoff_retention_number  *
                    //  ***************************************
                    sumh20 = 0.0;
                    // * CREAMS and other model discount S for water content (linear from air dry to sat) *
                    // * old code = relative to WP, new code = rel to air dry                             *
                    // * Changes by Brett Robinson May 2011                                               *
                    for (int i = 0; i < LayerCount - 1; ++i)
                    {
                        double deno = SaturationLimitRelWP[i] + AirDryLimitRelWP[i];
                        if (!MathTools.DoublesAreEqual(deno, 0))
                        {
                            sumh20 = sumh20 + WF[i] * (SoilWaterRelWP[i] + AirDryLimitRelWP[i]) / deno;
                        }
                        else
                        {
                            LogDivideByZeroError("CalculateRunoff", "SaturationLimit_rel_wp[i]+AirDryLimit_rel_wp[i]", "sumh20");
                        }
                    }
                    RunoffRetentionNumber = (int)(smx * (1.0 - sumh20));
                    //REMOVE INT STATEMENT AFTER VALIDATION
                    //progress = 4;
                }

                //  *************************************************
                //  *  Calculate runoff (creams p14, equation i-1)  *
                //  *************************************************
                double denom = EffectiveRain + 0.8 * RunoffRetentionNumber;
                double bas = EffectiveRain - 0.2 * RunoffRetentionNumber;
                if (!MathTools.DoublesAreEqual(denom, 0) && bas > 0)
                {
                    Runoff = Math.Pow(bas, 2.0) / denom;
                    Infiltration = EffectiveRain - Runoff;
                }
                else
                {
                    Runoff = 0;
                    Infiltration = EffectiveRain;
                }

                //add any runoff from irrigation.
                if (Engine.IrrigationModule != null)
                {
                    Runoff += Engine.IrrigationModule.IrrigationRunoff;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }





        public void CalculateSoilEvaporation()
        {
            try
            {
                //  ********************************************************************
                //  *  This function calculates soil evaporation using the Ritchie     *
                //  *  model.                                                          *
                //  ********************************************************************

                //  Calculate potential soil evaporation
                //  From proportion of bare soil
                PotSoilEvap = Engine.GetPotentialSoilEvaporation();

                if (Engine.IrrigationModule != null && Engine.IrrigationModule.PondingExists())
                {
                    SoilEvap = PotSoilEvap;
                }
                else
                {
                    //  Add crop residue effects
                    ////NOTE THAT THIS USED TO ONLY BE FOR THE LAI MODEL -  I"VE NOW MADE IT FOR EITHER

                    if (TotalCropResidue > 1.0)
                    {
                        PotSoilEvap = PotSoilEvap * (Math.Exp(-0.22 * TotalCropResidue / 1000.0));
                    }

                    //  *******************************
                    //  *  initialize daily variables
                    //  ******************************
                    Se1 = 0.0;
                    SoilEvap = 0.0;
                    Se2 = 0.0;
                    Se21 = 0.0;
                    Se22 = 0.0;
                    //  **************************************************
                    //  * If infiltration has occurred then reset sse1.  *
                    //  * Reset sse2 if infiltration exceeds sse1.       *
                    //  **************************************************
                    if (Infiltration > 0.0)
                    {
                        Sse2 = Math.Max(0, Sse2 - Math.Max(0, Infiltration - Sse1));
                        Sse1 = Math.Max(0, Sse1 - Infiltration);
                        if (!MathTools.DoublesAreEqual(InputModel.Stage2SoilEvapCona, 0))
                        {
                            Dsr = Math.Pow(Sse2 / InputModel.Stage2SoilEvapCona, 2);
                        }
                        else
                        {
                            Dsr = 0;

                            LogDivideByZeroError("CalculatSoilEvaporation", "in_Cona_mm_per_sqrroot_day", "dsr");
                        }
                    }
                    //  ********************************
                    //  *  Test for 1st stage drying.  *
                    //  ********************************
                    if (Sse1 < InputModel.Stage1SoilEvapU)
                    {
                        //  *****************************************************************
                        //  *  1st stage evaporation for today. Set se1 equal to potential  *
                        //  *  soil evaporation but limited by U.                           *
                        //  *****************************************************************
                        Se1 = Math.Min(PotSoilEvap, InputModel.Stage1SoilEvapU - Sse1);
                        Se1 = Math.Max(0.0, Math.Min(Se1, SoilWaterRelWP[0] + AirDryLimitRelWP[0]));

                        //  *******************************
                        //  *  Accumulate stage 1 drying  *
                        //  *******************************
                        Sse1 = Sse1 + Se1;
                        //  ******************************************************************
                        //  *  Check if potential soil evaporation is satisfied by 1st stage *
                        //  *  drying.  If not, calculate some stage 2 drying(se2).          *
                        //  ******************************************************************
                        if (PotSoilEvap > Se1)
                        {
                            //  *****************************************************************************
                            //  * If infiltration on day, and potential_soil_evaporation.gt.se1 (ie. a deficit in evap) .and. sse2.gt.0 *
                            //  * than that portion of potential_soil_evaporation not satisfied by se1 should be 2nd stage. This *
                            //  * can be determined by Math.Sqrt(time)*in_Cona_mm_per_sqrroot_day with any remainder ignored.          *
                            //  * If sse2 is zero, then use Ritchie's empirical transition constant (0.6).  *
                            //  *****************************************************************************
                            if (Sse2 > 0.0)
                            {
                                Se2 = Math.Min(PotSoilEvap - Se1, InputModel.Stage2SoilEvapCona * Math.Pow(Dsr, 0.5) - Sse2);
                            }
                            else
                            {
                                Se2 = 0.6 * (PotSoilEvap - Se1);
                            }
                            //  **********************************************************
                            //  *  Calculate stage two evaporation from layers 1 and 2.  *
                            //  **********************************************************

                            //  Any 1st stage will equal infiltration and therefore no net change in
                            //  soil water for layer 1 (ie can use SoilWater_rel_wp(1)+AirDryLimit_rel_wp(1) to determine se21.
                            Se21 = Math.Max(0.0, Math.Min(Se2, SoilWaterRelWP[0] + AirDryLimitRelWP[0]));
                            Se22 = Math.Max(0.0, Math.Min(Se2 - Se21, SoilWaterRelWP[1] + AirDryLimitRelWP[1]));
                            //  ********************************************************
                            //  *  Re-Calculate se2 for when se2-se21 > SoilWater_rel_wp(2)+AirDryLimit_rel_wp(2)  *
                            //  ********************************************************
                            Se2 = Se21 + Se22;
                            //  ************************************************
                            //  *  Update 1st and 2nd stage soil evaporation.  *
                            //  ************************************************
                            Sse1 = InputModel.Stage1SoilEvapU;
                            Sse2 += Se2;
                            if (!MathTools.DoublesAreEqual(InputModel.Stage2SoilEvapCona, 0))
                                Dsr = Math.Pow(Sse2 / InputModel.Stage2SoilEvapCona, 2);
                            else
                            {
                                Dsr = 0;
                                LogDivideByZeroError("CalculatSoilEvaporation", "in_Cona_mm_per_sqrroot_day", "dsr");
                            }
                        }
                        else
                        {
                            Se2 = 0.0;
                        }
                    }
                    else
                    {
                        Sse1 = InputModel.Stage1SoilEvapU;
                        //  ************************************************************************
                        //  *  No 1st stage drying. Calc. 2nd stage and remove from layers 1 & 2.  *
                        //  ************************************************************************
                        Dsr = Dsr + 1.0;
                        Se2 = Math.Min(PotSoilEvap, InputModel.Stage2SoilEvapCona * Math.Pow(Dsr, 0.5) - Sse2);
                        Se21 = Math.Max(0.0, Math.Min(Se2, SoilWaterRelWP[0] + AirDryLimitRelWP[0]));
                        Se22 = Math.Max(0.0, Math.Min(Se2 - Se21, SoilWaterRelWP[1] + AirDryLimitRelWP[1]));
                        //  ********************************************************
                        //  *  Re-calculate se2 for when se2-se21 > SoilWater_rel_wp(2)+AirDryLimit_rel_wp(2)  *
                        //  ********************************************************
                        Se2 = Se21 + Se22;
                        //  *****************************************
                        //  *   Update 2nd stage soil evaporation.  *
                        //  *****************************************
                        Sse2 = Sse2 + Se2;
                        //  **************************************
                        //  *  calculate total soil evaporation  *
                        //  **************************************
                    }
                    SoilEvap = Se1 + Se2;

                    EvapoTransp = SoilEvap;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }


        public void UpdateWaterBalance()
        {
            try
            {
                //***********************************************************************
                //*  This subroutine performs the water balance. New nested loop        *
                //*  algorithm infiltrates and redistributes water in one pass.  This   *
                //*  new algorithm has many advantages over the previous one.  Firstly, *
                //*  it is more biophysically realistic; secondly, it considers the     *
                //*  effects of a restricted Ksat on both infiltration and              *
                //*  redistribution.   Previously, only redistribution was considered.  *
                //*  It should also bettern explain water movemnet under saturated      *
                //*  conditions.                                                        *
                //***********************************************************************
                double oflow = 0.0;
                Overflow = 0;
                double drain = Infiltration;

                //  1.  Add all infiltration/drainage and extract ET.
                //  2.  Cascade a proportion of all water greater than drained upper limit (FC)
                //  3.  If soil water content is still greater than upper limit (SWMAX), add
                //      all excess above upper limit to runoff

                for (int i = 0; i < LayerCount; ++i)
                {
                    Seepage[i] = drain;
                    if (i == 0)
                    {
                        SoilWaterRelWP[i] += Seepage[i] - (SoilEvap - Se22) - LayerTranspiration[i];
                    }
                    else if (i == 1)
                    {
                        SoilWaterRelWP[i] += Seepage[i] - LayerTranspiration[i] + Red[i] - Se22;
                    }
                    else
                    {
                        SoilWaterRelWP[i] += Seepage[i] - LayerTranspiration[i] + Red[i];
                    }

                    if (SoilWaterRelWP[i] > DrainUpperLimitRelWP[i])
                    {
                        drain = SWCon[i] * (SoilWaterRelWP[i] - DrainUpperLimitRelWP[i]);
                        //if (drain > (KSat[i] * 12.0))
                        //    drain = KSat[i] * 12.0;
                        if (drain > KSat[i])
                        {
                            drain = KSat[i];
                        }
                        else if (drain < 0)
                        {
                            drain = 0;
                        }
                        SoilWaterRelWP[i] -= drain;
                    }
                    else
                    {
                        drain = 0;
                    }
                    if (SoilWaterRelWP[i] > SaturationLimitRelWP[i])
                    {
                        oflow = SoilWaterRelWP[i] - SaturationLimitRelWP[i];
                        SoilWaterRelWP[i] = SaturationLimitRelWP[i];
                    }

                    int j = 0;
                    while (oflow > 0)
                    {
                        if (i - j == 0)    //look at first layer
                        {
                            Overflow += oflow;
                            Runoff = Runoff + oflow;
                            Infiltration -= oflow;
                            Seepage[0] -= oflow;         //drainage in first layer
                            oflow = 0;
                        }
                        else           //look at other layersException e
                        {
                            SoilWaterRelWP[i - j] += oflow;
                            Seepage[i - j + 1] -= oflow;
                            if (SoilWaterRelWP[i - j] > SaturationLimitRelWP[i - j])
                            {
                                oflow = SoilWaterRelWP[i - j] - SaturationLimitRelWP[i - j];
                                SoilWaterRelWP[i - j] = SaturationLimitRelWP[i - j];
                            }
                            else
                            {
                                oflow = 0;
                            }
                            ++j;

                        }
                    }
                }
                double satrange = SaturationLimitRelWP[0] - DrainUpperLimitRelWP[0];
                double satamount = SoilWaterRelWP[0] - DrainUpperLimitRelWP[0];
                if (satamount > 0 && satrange > 0)
                {
                    SaturationIndex = satamount / satrange;
                }
                else
                {
                    SaturationIndex = 0;
                }
                Seepage[LayerCount] = drain;
                DeepDrainage = drain;
                TotalSoilWater = 0;
                for (int i = 0; i < LayerCount; ++i)
                {
                    TotalSoilWater += SoilWaterRelWP[i];

                }

                for (int i = 0; i < LayerCount; i++)
                {
                    SoilWater[i] = SoilWaterRelWP[i];
                    Drainage[i] = Seepage[i+1];
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        public void CalculateErosion()
        {
            try
            {
                //  ***********************************************************************
                //  *  This subroutine calculates sediment yield in tonnes/ha using the   *
                //  *  Dave Freebairn method                                              *
                //  ***********************************************************************

                HillSlopeErosion = 0;
                SedCatchmod = 0;
                if (Runoff <= 1)
                {
                    SedimentConc = 0;
                }
                else
                {
                    double conc = 0;
                    double cover = TotalCoverAllCrops * 100;

                    if (Engine.IrrigationModule != null)
                    {
                        if (!Engine.IrrigationModule.ConsiderCoverEffects())
                        {
                            cover = Math.Min(100.0, (CropCover + TotalResidueCover * (1 - CropCover)) * 100.0);
                        }
                        else
                        {
                            cover = Engine.IrrigationModule.GetCoverEffect(CropCover, TotalResidueCover);
                        }
                    }

                    if (cover < 50.0)
                    {
                        conc = 16.52 - 0.46 * cover + 0.0031 * cover * cover;  //% sediment concentration Exception e max g/l is 165.2 when cover =0;
                    }
                    else if (cover >= 50.0)
                    {
                        conc = -0.0254 * cover + 2.54;
                    }
                    conc = Math.Max(0.0, conc);
                    HillSlopeErosion = conc * UsleLsFactor * InputModel.USLEK * InputModel.USLEP * Runoff / 10.0;
                    SedCatchmod = conc * InputModel.USLEK * InputModel.USLEP * Runoff / 10.0;
                }
                if (!MathTools.DoublesAreEqual(Runoff, 0))
                {
                    if (!InRunoff2)
                    {
                        ++RunoffEventCount2;
                    }
                    InRunoff2 = true;

                    SedimentConc = HillSlopeErosion * 100.0 / Runoff * InputModel.SedDelivRatio;    //sediment concentration in g/l
                    if (SedimentConc > PeakSedConc)
                    {
                        PeakSedConc = SedimentConc;
                    }
                }
                else
                {
                    // dont log a divide by zero error for this one
                    if (InRunoff2)
                    {
                        CumSedConc += PeakSedConc;
                    }
                    PeakSedConc = 0;
                    InRunoff2 = false;
                    SedimentConc = 0;
                }

                OffSiteSedDelivery = HillSlopeErosion * InputModel.SedDelivRatio;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }


        public void TryModelLateralFlow()
        {
            try
            {
                if (Engine.CanCalculateLateralFlow)
                {

                    // Calculate most limiting Kratio
                    double kr;
                    double kratio = 1.0;
                    for (int i = 1; i < LayerCount; ++i)
                    {
                        if (!MathTools.DoublesAreEqual(KSat[i], 0))
                            kr = KSat[i - 1] / KSat[i];
                        else
                            kr = 0;
                        if (kr > kratio)
                            kratio = kr;
                    }

                    //  Convert in_FieldSlope_pc from percent to degrees

                    double slopedeg = Math.Atan(InputModel.FieldSlope / 100.0) * 180.0 / 3.14159;

                    // Calculate PredRH - lateral flow partitioning

                    double LN_kratio = Math.Log(kratio);
                    double LN_angle = Math.Log(slopedeg);
                    double LN_kratio2 = LN_kratio * LN_kratio;
                    double LN_angle2 = LN_angle * LN_angle;
                    double LNK_lnang = LN_kratio * LN_angle;

                    double numer = 0.04487067 + (0.019797884 * LN_kratio) - (0.020606403 * LN_angle)
                           + (0.01010285 * LN_kratio2) + (0.01415831 * LN_angle2)
                           - (0.011046881 * LNK_lnang);
                    double denom = 1 - (0.11431376 * LN_kratio) - (0.35073561 * LN_angle)
                           + (0.013044911) * (LN_kratio2) + (0.040556192 * LN_angle2)
                           + (0.015858813 * LNK_lnang);
                    if (!MathTools.DoublesAreEqual(denom, 0))
                        PredRh = numer / denom;
                    else
                    {
                        PredRh = 0;

                    }
                    LateralFlow = Seepage[LayerCount] * PredRh;
                    Seepage[LayerCount] = Seepage[LayerCount] * (1 - PredRh);
                }
                else
                    LateralFlow = 0;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }



        public void CalculateInitialValuesOfCumulativeSoilEvaporation()
        {
            try
            {
                //  Calculate initial values of cumulative soil evaporation
                if (DrainUpperLimitRelWP[0] - SoilWaterRelWP[0] > InputModel.Stage1SoilEvapU)
                {
                    Sse1 = InputModel.Stage1SoilEvapU;
                    Sse2 = Math.Max(0.0, DrainUpperLimitRelWP[0] - SoilWaterRelWP[0]) - InputModel.Stage1SoilEvapU;
                }
                else
                {
                    Sse1 = Math.Max(0.0, DrainUpperLimitRelWP[0] - SoilWaterRelWP[0]);
                    Sse2 = 0.0;
                }
                if (!MathTools.DoublesAreEqual(InputModel.Stage2SoilEvapCona, 0))
                    Dsr = Math.Pow(Sse2 / InputModel.Stage2SoilEvapCona, 2.0);
                else
                {
                    Dsr = 0;

                    LogDivideByZeroError("InitialiseSoilParameters", "in_Cona_mm_per_sqrroot_day", "dsr");
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        public void CalculateVolumeBalanceError()
        {
            try
            {
                double sse;
                double deltasw = TotalSoilWater - PreviousTotalSoilWater;
                if (Engine.CanCalculateLateralFlow)
                {
                    if (!MathTools.DoublesAreEqual(Engine.ClimateModule.Rain, MathTools.MISSING_DATA_VALUE))
                        sse = (Irrigation + Engine.ClimateModule.Rain) - (deltasw + Runoff + SoilEvap + Engine.GetTotalTranspiration() + Seepage[LayerCount] + LateralFlow);
                    else
                        sse = (Irrigation + 0) - (deltasw + Runoff + SoilEvap + Engine.GetTotalTranspiration() + Seepage[LayerCount] + LateralFlow);
                }
                else
                {
                    if (!MathTools.DoublesAreEqual(Engine.ClimateModule.Rain, MathTools.MISSING_DATA_VALUE))
                        sse = (Irrigation + Engine.ClimateModule.Rain) - (deltasw + Runoff + SoilEvap + Engine.GetTotalTranspiration() + Seepage[LayerCount]);
                    else
                        sse = (Irrigation + 0) - (deltasw + Runoff + SoilEvap + Engine.GetTotalTranspiration() + Seepage[LayerCount]);
                }

                VBE = (int)(sse * 1000000) / 100000.0;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        public void CalculateUSLE_LSFactor()
        {
            try
            {
                if (Engine.UsePERFECTUSLELSFn)
                {
                    double aht = InputModel.FieldSlope * InputModel.SlopeLength / 100.0;
                    double lambda = 3.281 * (Math.Sqrt(InputModel.SlopeLength * InputModel.SlopeLength + aht * aht));
                    double theta;
                    if (!MathTools.DoublesAreEqual(InputModel.SlopeLength, 0))
                        theta = Math.Asin(aht / InputModel.SlopeLength);
                    else
                    {
                        theta = 0;

                        LogDivideByZeroError("CalculateUSLE_LSFactor", "in_SlopeLength_m", "theta");
                    }
                    if (!MathTools.DoublesAreEqual(1.0 + InputModel.RillRatio, 0))
                    {
                        if (InputModel.FieldSlope < 9.0)

                            UsleLsFactor = Math.Pow(lambda / 72.6, InputModel.RillRatio / (1.0 + InputModel.RillRatio)) * (10.8 * Math.Sin(theta) + 0.03);
                        else
                            UsleLsFactor = Math.Pow(lambda / 72.6, InputModel.RillRatio / (1.0 + InputModel.RillRatio)) * (16.8 * Math.Sin(theta) - 0.5);
                    }
                    else
                    {
                        UsleLsFactor = 0;

                        LogDivideByZeroError("CalculateUSLE_LSFactor", "1.0+in_RillRatio", "usle_ls_factor");
                    }
                }
                else
                {
                    if (!MathTools.DoublesAreEqual(1.0 + InputModel.RillRatio, 0))
                    {
                        UsleLsFactor = Math.Pow(InputModel.SlopeLength / 22.1, InputModel.RillRatio / (1.0 + InputModel.RillRatio)) * (0.065 + 0.0456 * InputModel.FieldSlope + 0.006541 * Math.Pow(InputModel.FieldSlope, 2));
                    }
                    else
                    {
                        UsleLsFactor = 0;

                        LogDivideByZeroError("CalculateUSLE_LSFactor", "1.0+in_RillRatio", "usle_ls_factor");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateDepthRetentionWeightFactors()
        {
            try
            {
                double a, b;
                for (int i = 0; i < LayerCount - 1; ++i)
                {
                    if (Depth[LayerCount - 1] > 0)
                    {
                        a = -4.16 * (Depth[i] / Depth[LayerCount - 1]);
                        b = -4.16 * (Depth[i + 1] / Depth[LayerCount - 1]);
                        WF[i] = 1.016 * (Math.Exp(a) - Math.Exp(b));
                    }
                    else
                    {
                        WF[i] = 0;

                        LogDivideByZeroError("CalculateDepthRetentionWeightFactors", "depth[in_LayerCount-1]", "wf[i]");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }




        /// <summary>
        /// 
        /// </summary>
        public void CalculateDrainageFactors()
        {
            try
            {
                for (int i = 0; i < LayerCount; ++i)
                {
                    if (KSat[i] > 0)
                    {
                        // I've got rid of the old PERFECTism regarding Ksat
                        // the commented bits below was just my testing algorithms,
                        // to make sure the results are identical to the reworked equations.
                        //	double oldksat=ksat[i]/12.0;
                        //	double val1 = 48.0/(2.0*(SaturationLimit_rel_wp[i]-DrainUpperLimit_rel_wp[i])/oldksat+24.0);
                        //	double val2 = 2.0*ksat[i]/(SaturationLimit_rel_wp[i]-DrainUpperLimit_rel_wp[i]+ksat[i]);
                        //	if(int(val1*1000000)!=int(val2*1000000))
                        //		throw(new Exception("error!", mtError, TMsgDlgButtons() << mbOK, 0);
                        //	swcon[i]=val2;
                        double temp = (SaturationLimitRelWP[i] - DrainUpperLimitRelWP[i] + KSat[i]);
                        if (!MathTools.DoublesAreEqual(temp, 0))
                        {
                            SWCon[i] = 2.0 * KSat[i] / temp;
                        }
                        else
                        {
                            SWCon[i] = 0;

                            LogDivideByZeroError("CalculateDrainageFactors", "SaturationLimit_rel_wp[i]-DrainUpperLimit_rel_wp[i]+ksat[i]", "swcon[i]");
                        }
                    }
                    else
                    {
                        SWCon[i] = 0;
                    }

                    if (SWCon[i] < 0)
                    {
                        SWCon[i] = 0;
                    }
                    else if (SWCon[i] > 1)
                    {
                        SWCon[i] = 1;
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
