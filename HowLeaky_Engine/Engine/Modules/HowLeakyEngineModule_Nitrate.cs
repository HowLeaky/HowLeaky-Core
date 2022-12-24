using HowLeaky_Engine.Outputs.Summaries;
using HowLeaky_SimulationEngine.Attributes;
using HowLeaky_SimulationEngine.Enums;
using HowLeaky_SimulationEngine.Errors;
using HowLeaky_SimulationEngine.Inputs;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HowLeaky_SimulationEngine.Engine
{
    public class HowLeakyEngineModule_Nitrate : _CustomHowLeakyEngineModule
    {
        public HowLeakyOutputSummary_Nitrate Summary { get; set; }
        public HowLeakyEngineModule_Nitrate(HowLeakyEngine sim, HowLeakyInputs_Nitrate inputs) : base(sim)
        {
            Name = inputs.Name;
            InputModel = inputs;

        }

        public HowLeakyEngineModule_Nitrate()
        {
        }

        public HowLeakyInputs_Nitrate InputModel { get; set; }

        [Internal] public int Nitratesdayindex1 { get; set; }
        [Internal] public int Nitratesdayindex2 { get; set; }
        [Internal] public int Nitratesdayindex3 { get; set; }
        [Internal] public double LastNAppliedRate { get; set; } = 0;
        [Internal] public double NitrateCumRain { get; set; } = 0;
        [Internal] public bool Saturated { get; set; } = false; //SAFEGAUGE MODEL
        [Internal] public double NApplication { get; set; } //SAFEGAUGE MODEL
        [Internal] public double YesterdaysRunoff { get; set; } = 0; //SAFEGAUGE MODEL
        [Internal] public StageType StageType { get; set; } //SAFEGAUGE MODEL


        //Reportable Outputs
        [Output] public double NO3NDissolvedInRunoff { get; set; }
        [Output] public double NO3NRunoffLoad { get; set; }
        [Output] public double NO3NDissolvedLeaching { get; set; }
        [Output] public double NO3NLeachingLoad { get; set; }
        [Output] public double ParticNInRunoff { get; set; }
        [Output] public double NO3NStoreTopLayer { get; set; }
        [Output] public double NO3NStoreBotLayer { get; set; }
        [Output] public double TotalNStoreTopLayer { get; set; }
        [Output] public double PNHLCa { get; set; }
        [Output] public double DrainageInNO3Period { get; set; }
        [Output] public double RunoffInNO3Period { get; set; }


        [Output] public double NitrogenApplication { get; set; } //SAFEGAUGE MODEL
        [Output] public double Mineralisation { get; set; } //SAFEGAUGE MODEL
        [Output] public double CropUsePlant { get; set; } //SAFEGAUGE MODEL
                                                          //  [Output] public double CropUseRatoon { get; set; } //SAFEGAUGE MODEL
        [Output] public double CropUseActual { get; set; } //SAFEGAUGE MODEL
        [Output] public double Denitrification { get; set; } //SAFEGAUGE MODEL
        [Output] public double ExcessN { get; set; } //SAFEGAUGE MODEL
        [Output] public double PropVolSat { get; set; } //SAFEGAUGE MODEL
                                                        // [Output] public double DINDrainage { get; set; } //SAFEGAUGE MODEL

        public List<BrowserDate> Cycle1ApplicationDates { get; set; }
        public List<BrowserDate> Cycle2ApplicationDates { get; set; }
        public override void Initialise()
        {
            Nitratesdayindex1 = 0;
            Nitratesdayindex2 = 0;
            Nitratesdayindex3 = 0;
            LastNAppliedRate = 0;
            NitrateCumRain = 0;
            Saturated = false;
            NApplication = 0;
            YesterdaysRunoff = 0;
            StageType = 0;

            NO3NDissolvedInRunoff = 0;
            NO3NRunoffLoad = 0;
            NO3NDissolvedLeaching = 0;
            NO3NLeachingLoad = 0;
            ParticNInRunoff = 0;
            NO3NStoreTopLayer = 0;
            NO3NStoreBotLayer = 0;
            TotalNStoreTopLayer = 0;
            PNHLCa = 0;
            DrainageInNO3Period = 0;
            RunoffInNO3Period = 0;

            NitrogenApplication = 0;
            Mineralisation = 0;
            CropUsePlant = 0;
            //  CropUseRatoon = 0;
            CropUseActual = 0;
            Denitrification = 0;
            ExcessN = InputModel.InitialExcessN;
            PropVolSat = 0;
            //DINDrainage = 0;

            Summary = new HowLeakyOutputSummary_Nitrate();

        }


        public override void Simulate()
        {
            try
            {
                if (InputModel.DissolvedNinRunoffOptions == DissolvedNinRunoffType.HowLeaky2012)
                {
                    CalculateDissolvedNInRunoff();
                }
                else if (InputModel.DissolvedNinRunoffOptions == DissolvedNinRunoffType.BananaEmpiricalModel)
                {
                    CalculateDissolvedNInRunoffBananas();
                }
                else if(InputModel.DissolvedNinRunoffOptions==DissolvedNinRunoffType.FixedEMC)
                {
                    CalculateDissolvedNInRunoffFixedEMC();
                }


                if (InputModel.DissolvedNinLeachingOptions == DissolvedNinLeachingType.HowLeaky2012)
                {
                    CalculateDissolvedNInLeaching_Method1();
                }
                else if (InputModel.DissolvedNinLeachingOptions == DissolvedNinLeachingType.ModifiedSafegaugeModel)
                {
                    CalculateDissolvedNInLeaching_Method2_SafeGauge();
                }
                else if (InputModel.DissolvedNinLeachingOptions == DissolvedNinLeachingType.FixedEMC)
                {
                    CalculateDissolvedNInLeaching_Method3_FixedEMC();
                }


                //if (InputModel.ParticulateNinRunoffOptions == ParticulateNinRunoffType.HowLeaky2012)
                //{
                CalculateParticulateNInRunoff();
                //}
                //else if(InputModel.ParticulateNinRunoffOptions==ParticulateNinRunoffType.Freebairn)
                //{

                //}

                Summary.Update(Engine);
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }



        private void CalculateDissolvedNInRunoffBananas()
        {
            try
            {
                //double NLKgHa = GetNO3NStoreTopLayerkgPerha(); //Nitrate load in surface layer (From Dairymod)
                //if (Math.Abs(NLKgHa - MathTools.MISSING_DATA_VALUE) > 0.00001)
                //{
                double a = InputModel.NAlpha_Disolved;
                double b = InputModel.NBeta_Disolved;
                double maxConc = InputModel.N_DanRat_MaxRunOffConc;
                double minConc = InputModel.N_DanRat_MinRunOffConc;
                NitrogenApplication = ExtractFertiliserRate();
                if (NitrogenApplication > 0 && Math.Abs(NitrogenApplication - MathTools.MISSING_DATA_VALUE) > 0.0001)
                {
                    NitrateCumRain = Engine.SoilModule.EffectiveRain;
                    LastNAppliedRate = NitrogenApplication;
                }
                else
                {
                    NitrateCumRain = NitrateCumRain + Engine.SoilModule.EffectiveRain;
                }


                double dinMgPerL = 0;
                if (Engine.SoilModule.Runoff > 0)
                {
                    if (NitrateCumRain > 0)
                    {
                        dinMgPerL = LastNAppliedRate / a * Math.Pow(NitrateCumRain, b);
                    }
                    else
                    {
                        dinMgPerL = 0;
                    }

                    dinMgPerL = Math.Min(maxConc, Math.Max(minConc, dinMgPerL));
                }

                NO3NStoreTopLayer = MathTools.MISSING_DATA_VALUE;
                NO3NDissolvedInRunoff = dinMgPerL;
                NO3NRunoffLoad = dinMgPerL * Engine.SoilModule.Runoff / 100.0;
                //}
                //else
                //{
                //    NO3NStoreTopLayer = MathTools.MISSING_DATA_VALUE;
                //    NO3NDissolvedInRunoff = MathTools.MISSING_DATA_VALUE;
                //    NO3NRunoffLoad = MathTools.MISSING_DATA_VALUE;
                //}
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        

        public void CalculateDissolvedNInRunoffFixedEMC()
        {
            try
            {

                NO3NStoreTopLayer = MathTools.MISSING_DATA_VALUE;
                NO3NDissolvedInRunoff = InputModel.FixedEMC_Runoff;
                NO3NRunoffLoad = InputModel.FixedEMC_Runoff * Engine.SoilModule.Runoff / 100.0;
               
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }


        //From Howleaky developers (Brett Robinson), based on the concept that soil and runoff water mixing increases up to a maximum of k.
        // DN = Nsurface * K(1- Math.Exp(-cvQ)
        // where DN is the Nitrate conc in the runoff (mg/L)
        // k is the parameter that regulates mixing of soil and runoff water, (suggested 0.5)
        // cv is parameter that describes the curvature of change in the soil and water runoff at increasing runoff values (initial guess is 0.2)
        // Q is runoff (mm)
        // Nsurface (mg N/kg) is the soil nitrate concentrate in teh survace layer (0-2cm), which in our approach is derived from nitrate load (NLsoil in kg/ha) in surface layer from DairyMod
        // NSurface = alpha*100*NLsoil/(depth*soildensity)
        // Then dissolved N load (NL, kg/ha) in runoff is
        // DL=ND*Q/100.0;
        // NOTATION USE HERE IS TO BE CONSISTENT WITH THAT USED BY VIC DPI
        /// <summary>
        /// 
        /// </summary>
        public void CalculateDissolvedNInRunoff()
        {
            try
            {
                double nlKgHa = GetNO3NStoreTopLayerkgPerha(); //Nitrate load in surface layer (From Dairymod)
                if (!MathTools.DoublesAreEqual(nlKgHa, MathTools.MISSING_DATA_VALUE))
                {
                    //nlKgHa = nlKgHa * InputModel.SoilNitrateLoadWeighting1; REMOVED DEC 2021
                    double k = InputModel.Nk; // INPUT parameter that regulates mixing of soil and runoff water
                    double
                        cv = InputModel
                            .Ncv; //INPUT parameter that describes the curvature of change in soil and water runoff at increasing runoff values
                    double Q = Engine.SoilModule.Runoff; //runoff amount
                    double d = InputModel.NDepthTopLayer1; //depth of surface soil layer mm
                    double phi =
                        Engine.SoilModule.InputModel
                            .BulkDensity[0]; //soil density t/m3       ( BulkDensity is in g/cm3)
                    double NSoil = 1.0 * 100.0 * nlKgHa / (d * phi); //mg/kg
                    double DN = NSoil * k * (1 - Math.Exp(-cv * Q));
                    double DL = DN * Q / 100.0;

                    NO3NStoreTopLayer = nlKgHa;
                    NO3NDissolvedInRunoff = DN;
                    NO3NRunoffLoad = DL;
                }
                else
                {
                    NO3NStoreTopLayer = MathTools.MISSING_DATA_VALUE;
                    NO3NDissolvedInRunoff = MathTools.MISSING_DATA_VALUE;
                    NO3NRunoffLoad = MathTools.MISSING_DATA_VALUE;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }


        // The nitrate concentrate in soil water contributing to leaching (mg/l) is
        // LN= NSoil/(totalsoilwater)
        // where NSoil is the nigrate concentration in the soil (kg/ha) and
        // totalsoilwater  is the soil water between air dry water content and saturated water content (mm) of the soil profile or the layer.
        // Nitrate concentration in soil can either be for the soil profile or for the deepest soil layer.
        // Concentration for the soil profile can be obtained from Math.Experiments or Math.Expert knowledge and soil nitrate concentration in the deepest soil layer can be informed by other nitrogen biophysical models (eg. DairyMod).
        // Nitrate leaching load LL (kg /ha) is then calcualted as
        // LL = LN*LE*D/100.0
        // Where LE is the leaching efficiency parameter portioning soil water nitrate concentration into various pathways (often taken as 0.5)
        // D is the daily drainage
        // NOTATION USE HERE IS TO BE CONSISTENT WITH THAT USED BY VIC DPI
        /// <summary>
        /// 
        /// </summary>
        public void CalculateDissolvedNInLeaching_Method1()
        {
            try
            {
                double nSoilKgPerHa = GetNO3NStoreBotLayerkgPerha(); //nitrate concentrate in the soil (kg/ha)
                if (!MathTools.DoublesAreEqual(nSoilKgPerHa, MathTools.MISSING_DATA_VALUE))
                {
                    //nSoilKgPerHa = nSoilKgPerHa * InputModel.SoilNitrateLoadWeighting2; REMVOED DEC 2021
                    double deltaDepth = InputModel.DepthBottomLayer;
                    if (deltaDepth > 0)
                    {
                        double soilWater =
                            (Engine.SoilModule.InputModel.Saturation[Engine.SoilModule.LayerCount - 1] -
                             Engine.SoilModule.InputModel.AirDryLimit[Engine.SoilModule.LayerCount - 1]) / 100.0 *
                            deltaDepth;
                        double LE = InputModel.NitrateLeachingEfficiency; //Leaching efficiency (INPUT)
                        double D = Engine.SoilModule.DeepDrainage; //Drainage (mm)
                        double LN = nSoilKgPerHa * 1000000.0 / (soilWater * 10000.0) * LE;// (* LE)added Rattray 1/09/22
                        double LL = (LN / 1000000.0) * D * 10000.0;// (* LE)removed  Rattray 1/09/22

                        NO3NStoreBotLayer = nSoilKgPerHa;
                        NO3NDissolvedLeaching = LN;
                        NO3NLeachingLoad = LL;
                    }
                    else
                    {
                        NO3NStoreBotLayer = MathTools.MISSING_DATA_VALUE;
                        NO3NDissolvedLeaching = MathTools.MISSING_DATA_VALUE;
                        NO3NLeachingLoad = MathTools.MISSING_DATA_VALUE;
                    }
                }
                else
                {
                    NO3NStoreBotLayer = MathTools.MISSING_DATA_VALUE;
                    NO3NDissolvedLeaching = MathTools.MISSING_DATA_VALUE;
                    NO3NLeachingLoad = MathTools.MISSING_DATA_VALUE;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        private void CalculateDissolvedNInLeaching_Method2_SafeGauge()
        {
            try
            {
                int das = Engine.CurrentCrop.DaysSincePlanting;


                //Excess N - calc from yesterdays values
                ExcessN = Math.Max(ExcessN + NitrogenApplication + Mineralisation - CropUseActual - Denitrification - NO3NLeachingLoad - NO3NRunoffLoad, 0);

                if(CanResetExcessNToday())
                {
                    ExcessN = InputModel.ExcessNResetValue;
                }

                //if (InputModel.DenitficationOption == DenitrificationOption.SafeGauge)
                //{
                    //Denitrification               
                    if (Engine.SoilModule.Runoff > 0 && YesterdaysRunoff > 0) //Runoff 2 days in a row, you get Denitrification 
                    {
                        Denitrification = InputModel.Denitrification * ExcessN;
                    }
                    else
                    {
                        Denitrification = 0;
                    }
                //}
                //else
                //{

                //}

                //if (das >= 0 && das < InputModel.MainStemDuration)
                //{
                //    StageType = StageType.Plant;
                //}
                //else
                //{
                //    StageType = StageType.Fallow;
                //}


                //Applied N
                NitrogenApplication = 0;
                //if (StageType == StageType.Plant)
                //{

                NitrogenApplication = ExtractFertiliserRate();
                    
                //}

                //Mineralisation
                if (InputModel.MineralisationOption == MineralisatinOption.Static)
                {
                    Mineralisation = Math.Min(Engine.SoilModule.InputModel.OrganicCarbon * InputModel.CMRSlope, InputModel.CMRMax) / 365.0;
                }
                else if (InputModel.MineralisationOption == MineralisatinOption.Dynamic)
                {
                    Mineralisation = CalculateMineralisationFreebairn();
                }



                //Crop use



                if (!Engine.InFallow())// && StageType == StageType.Plant)
                {
                    if (InputModel.CropUseOption == CropUseOption.LogisticCurve)
                    {
                        CropUsePlant = (1 / (1 + (Math.Exp((das - InputModel.PlantA) * (-InputModel.PlantB))))) * InputModel.PlantDaily;
                        CropUseActual = CropUsePlant;
                    }
                    else
                    {

                        CropUsePlant = Engine.TotalTranspiration * InputModel.NCropUseEfficiency;
                        CropUseActual = CropUsePlant;
                    }
                }
                else
                {
                    CropUsePlant = 0;
                    CropUseActual = 0;
                }

                //Vol of sat
                PropVolSat = Engine.SoilModule.DeepDrainage / Engine.SoilModule.VolSat;

                //DIN Drainage
                NO3NLeachingLoad = PropVolSat * ExcessN * InputModel.NitrateLeachingEfficiency;


                NO3NStoreBotLayer = MathTools.MISSING_DATA_VALUE;
                if (Math.Abs(Engine.SoilModule.DeepDrainage) > 0.000001)
                {
                    NO3NDissolvedLeaching = NO3NLeachingLoad / Engine.SoilModule.DeepDrainage * 100;
                }
                else
                {
                    NO3NDissolvedLeaching = 0;
                }


                YesterdaysRunoff = Engine.SoilModule.Runoff;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        private bool CanResetExcessNToday()
        {
            return InputModel.ResetExcessN == ResetExcessNType.True && InputModel.ExcessNResetDate.MatchesDate(Engine.TodaysDate);
        }

        public void CalculateDissolvedNInLeaching_Method3_FixedEMC()
        {
            try
            {

                NO3NStoreBotLayer = MathTools.MISSING_DATA_VALUE;
                NO3NDissolvedLeaching = InputModel.FixedEMC_Leaching;
                NO3NLeachingLoad = InputModel.FixedEMC_Leaching * Engine.SoilModule.DeepDrainage / 100.0;

            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }
        


        protected double CalculateMineralisationFreebairn()
        {
            double moistfactor;
            //Assign inputs
            double soilwater_percent = Engine.SoilModule.SoilWaterRelWP[0] / Engine.SoilModule.Depth[1] * 100.0;   // units of %
            double wiltingPoint_percent = Engine.SoilModule.InputModel.WiltingPoint[0];
            double fieldcapacity = Engine.SoilModule.InputModel.FieldCapacity[0]; // units of %
            var organicCarbon = Engine.SoilModule.InputModel.OrganicCarbon;
            var carbonNitrogenRatio = Engine.SoilModule.InputModel.CarbonNitrogenRatio;
            var mineralisationCoeff = InputModel.NitrateMineralisationCoefficient;

            //Calculate
            double Org_n = Math.Abs(carbonNitrogenRatio) > 0.000001 ? (organicCarbon / carbonNitrogenRatio) : 0;
            double potm = Org_n / 100.0 * 200.0 * 10.0 * 1000.0;
            if (soilwater_percent > 0)
                moistfactor = Math.Max(0, Math.Min(1, (soilwater_percent) / (fieldcapacity - wiltingPoint_percent)));
            else
                moistfactor = 0;
            var tempfactor = Math.Max(0, Math.Min(1, (0.035 * Engine.ClimateModule.Temperature - 0.1)));// exp(15.807 - 6350 / (temp + 273));

            double multiplier = Math.Min(moistfactor, tempfactor);
            return multiplier * mineralisationCoeff * potm;

        }



        //NO3NLeachingLoad = PropVolSat* ExcessN * InputModel.NitrateLeachingEfficiency;


        //        NO3NStoreBotLayer = MathTools.MISSING_DATA_VALUE;
        //        if (Math.Abs(Engine.SoilModule.DeepDrainage) > 0.000001)
        //        {
        //            NO3NDissolvedLeaching
        // Particulate N Losses in runoff are modelled in a similar way to particulate P
        // PN = beta*E*SDR*TNsoil*NER
        // where PN is the particulate N load (kg/ha)
        // TNsoil is the total N concentrate of the soil (mg/kg) and is the sum of the organica and inorganic N concentrations at 0-2cm from DairyMod.
        // As TNsoil will be derived from DairyMod in kg/ha, we need to convert this to mg.kg.
        // E is the gross errosion (kg/ha)
        // SDR is the sediment delivery ratio
        // NER is the Nitrogen enrichment ratio, which is unitless and defined similary to PER (for P)
        // Beta is a conversion factor to adjust units and that can be used as a calibration factor.
        // NOTATION USE HERE IS TO BE CONSISTENT WITH THAT USED BY VIC DPI
        /// <summary>
        /// 
        /// </summary>
        //public void CalculateParticulateNInRunoff()
        //{
        //    try
        //    {
        //        double
        //            tnSoilKgPerHa =
        //                GetTotalNStoreTopLayerkgPerha(); // Total N Concentration in soil (mg/kg) and is sum of organanic and inorgance conc at 0-2cm (Obtained from Dairymod)
        //        if (!MathTools.DoublesAreEqual(tnSoilKgPerHa, MathTools.MISSING_DATA_VALUE))
        //        {
        //            tnSoilKgPerHa = tnSoilKgPerHa * InputModel.SoilNitrateLoadWeighting3;
        //            double E = Engine.SoilModule.HillSlopeErosion * 1000.0; // Gross erosion (kg/ha)
        //            double SDR = Engine.SoilModule.InputModel.SedDelivRatio; // Sediment delivery ratio.
        //            double NER = InputModel.NEnrichmentRatio; // Nitrogen enrighment ratio
        //            double d = InputModel.NDepthTopLayer2; // depth of surface soil layer mm
        //            double phi =
        //                Engine.SoilModule.InputModel
        //                    .BulkDensity[0]; // soil density t/m3       ( BulkDensity is in g/cm3)
        //            double NSoil = InputModel.NAlpha_Particulate * 100.0 * tnSoilKgPerHa / (d * phi); // mg/kg
        //            double PN = InputModel.NBeta_Particulate * E * SDR * NSoil * NER / 1000000.0;

        //            ParticNInRunoff = PN;
        //            if (!MathTools.DoublesAreEqual(SDR, 0) &&
        //                !MathTools.DoublesAreEqual(Engine.SoilModule.UsleLsFactor, 0))
        //            {
        //                PNHLCa = PN / (SDR * Engine.SoilModule.UsleLsFactor);
        //            }
        //            else
        //            {
        //                PNHLCa = 0;
        //            }

        //            TotalNStoreTopLayer = tnSoilKgPerHa;
        //        }
        //        else
        //        {
        //            ParticNInRunoff = MathTools.MISSING_DATA_VALUE;
        //            PNHLCa = MathTools.MISSING_DATA_VALUE;
        //            TotalNStoreTopLayer = MathTools.MISSING_DATA_VALUE;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ErrorLogger.CreateException(ex);
        //    }
        //}

        public void CalculateParticulateNInRunoff()
        {
            try
            {

                var organicCarbon = Engine.SoilModule.InputModel.OrganicCarbon;
                var carbonNitrogenRatio = Engine.SoilModule.InputModel.CarbonNitrogenRatio;

                var hillslopeErosion = Engine.SoilModule.HillSlopeErosion;
                var seddelratio = Engine.SoilModule.InputModel.SedDelivRatio;
                var nenrichmentratio = InputModel.NEnrichmentRatio;
                if (Math.Abs(carbonNitrogenRatio) > 0.000001)
                {
                    ParticNInRunoff = ((organicCarbon / 100.0) / carbonNitrogenRatio) * hillslopeErosion * 1000.0 * seddelratio * nenrichmentratio;
                }
                else
                {
                    ParticNInRunoff = 0;
                }


            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        private double GetNO3NStoreTopLayerkgPerha()
        {
            try
            {
                if (InputModel.DissolvedNinRunoffOptions == DissolvedNinRunoffType.HowLeaky2012)
                {
                    return InputModel.SoilNLoadData1.ValueAtDate(Engine.TodaysDate);
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

            return 0;
        }


        private double GetNO3NStoreBotLayerkgPerha()
        {
            try
            {
                if (InputModel.DissolvedNinLeachingOptions == DissolvedNinLeachingType.HowLeaky2012)
                {
                    return InputModel.SoilNLoadData2.ValueAtDate(Engine.TodaysDate);
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

            return 0;
        }

        //removed December 2021
        //private double GetTotalNStoreTopLayerkgPerha()
        //{
        //    try
        //    {
        //        if (InputModel.ParticulateNinRunoffOptions == ParticulateNinRunoffType.HowLeaky2012)
        //        {
        //            return InputModel.SoilNLoadData3.ValueAtDate(Engine.TodaysDate);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ErrorLogger.CreateException(ex);
        //    }

        //    return 0;
        //}

        public bool CanSimulateNitrate()
        {
            try
            {
                return (InputModel.DissolvedNinRunoffOptions != 0 || InputModel.DissolvedNinLeachingOptions != 0 );//||
                       // InputModel.ParticulateNinRunoffOptions != 0);
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        private double ExtractFertiliserRate()
        {
            if (InputModel.FertAppOptions == FertiliserApplicationOption.SingleCycle)
            {
                return ExtractRateFromSingleCycle(Engine.TodaysDate);
            }
            else if (InputModel.FertAppOptions == FertiliserApplicationOption.DualCycle)
            {
                return ExtractRateFromDualCycle(Engine.TodaysDate);
            }
            else if (InputModel.FertAppOptions == FertiliserApplicationOption.DatesAndRates)
            {
                return InputModel.FertilizerInputDateSequences.ValueAtDate(Engine.TodaysDate);
            }
            return 0;
        }

        private double ExtractRateFromSingleCycle(BrowserDate date)
        {
            var delay1 = InputModel.FertAppCycle1_DelayStart_wks;
            var length1 = InputModel.FertAppCycle1_Length_wks;
            var noapps1 = InputModel.FertAppCycle1_NoApplications;
            var total1 = InputModel.FertAppCycle1_TotalN_kgPerha;
            CollateSingleCycleDates(delay1, length1, noapps1);
            if (DateExistsInCycle1(date))
            {
                return total1 / ((double)noapps1);
            }
            return 0;
        }

        private double ExtractRateFromDualCycle(BrowserDate date)
        {

            var delay1 = InputModel.FertAppCycle1_DelayStart_wks;
            var length1 = InputModel.FertAppCycle1_Length_wks;
            var noapps1 = InputModel.FertAppCycle1_NoApplications;
            var total1 = InputModel.FertAppCycle1_TotalN_kgPerha;
            var delay2 = InputModel.FertAppCycle2_DelayStart_wks;
            var length2 = InputModel.FertAppCycle2_Length_wks;
            var noapps2 = InputModel.FertAppCycle2_NoApplications;
            var total2 = InputModel.FertAppCycle2_TotalN_kgPerha;
            var repeats = InputModel.FertApp2Cycle_Repeats;
            CollateDualCycleDates(delay1, length1, noapps1, delay2, length2, noapps2, repeats);
            if (DateExistsInCycle1(date))
            {
                return total1 / ((double)noapps1);
            }
            if (DateExistsInCycle2(date))
            {
                return total2 / ((double)noapps2);
            }

            return 0;
        }
        private List<BrowserDate> BuildCycleApplicationDates(int totalPeriod, int delay, int length, int noapps, int repeats)
        {
            var dates = new List<BrowserDate>();
            var startDate = Engine.StartDate;
            var endDate = Engine.EndDate;
            var gap = noapps > 1 ? (length / (noapps)) : 0;
            var yearCycle = CalcYearCycle(totalPeriod);
            var totalDays = totalPeriod * 365;
            var totalapps = noapps * repeats;
            if (noapps > 0 && length > 0)
            {
                for (int year = startDate.Year; year <= endDate.Year; year = year + yearCycle)
                {
                    var firstDate = new BrowserDate(year, 1, 1);
                    var lastAllowableDate=firstDate.AddDays(totalDays);
                    var periodStart = firstDate.AddDays(delay * 7 );
                    dates.Add(periodStart);
                    for (var i = 1; i < totalapps; ++i)
                    {
                        var date = periodStart.AddDays((i*gap) * 7 );
                        if (date.DateInt <= lastAllowableDate.DateInt)
                        {
                            dates.Add(date);
                        }
                    }
                }
            }
            return dates;
        }
        private void CollateSingleCycleDates(int delay1, int length1, int noapps1)
        {
            if (Cycle1ApplicationDates == null)
            {
                var totalperiod = delay1 + length1;
                Cycle1ApplicationDates = BuildCycleApplicationDates(totalperiod,delay1, length1, noapps1, 1);
            }
        }

        private void CollateDualCycleDates(int delay1, int length1, int noapps1, int delay2, int length2, int noapps2, int repeat)
        {
            var totalPeriod = delay1 + length1 + (delay2 + length2) * repeat;
            if (Cycle1ApplicationDates == null)
            {
                Cycle1ApplicationDates = BuildCycleApplicationDates(totalPeriod, delay1, length1, noapps1,1);
            }
            if (Cycle2ApplicationDates == null)
            {
                var delay = delay1 + length1 + delay2;
                Cycle2ApplicationDates = BuildCycleApplicationDates(totalPeriod, delay, length2, noapps2, repeat);
            }
        }

        private bool DateExistsInCycle1(BrowserDate date)
        {
            return (Cycle1ApplicationDates.Any(x => x.DateInt == date.DateInt));
        }
        private bool DateExistsInCycle2(BrowserDate date)
        {
            return (Cycle2ApplicationDates.Any(x => x.DateInt == date.DateInt));
        }

       
        private int CalcYearCycle(int totalPeriod)
        {
            
            var yearsLength = (totalPeriod) * 7.0 / 365.0;

            return (int)Math.Ceiling(yearsLength);
            
        }

        
    }
}