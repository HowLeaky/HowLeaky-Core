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
    public class HowLeakyEngineModule_Nitrate : _CustomHowLeakyEngineModule
    {
        public HowLeakyEngineModule_Nitrate(HowLeakyEngine sim, HowLeakyInputs_Nitrate inputs) : base(sim)
        {
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
        [Internal] public double NApplication { get; set; }  //SAFEGAUGE MODEL
        [Internal] public double YesterdaysRunoff { get; set; } = 0; //SAFEGAUGE MODEL
        [Internal] public StageType StageType { get; set; }  //SAFEGAUGE MODEL




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
        [Output] public double CropUseRatoon { get; set; } //SAFEGAUGE MODEL
        [Output] public double CropUseActual { get; set; } //SAFEGAUGE MODEL
        [Output] public double Denitrification { get; set; } //SAFEGAUGE MODEL
        [Output] public double ExcessN { get; set; } //SAFEGAUGE MODEL
        [Output] public double PropVolSat { get; set; } //SAFEGAUGE MODEL
        [Output] public double DINDrainage { get; set; } //SAFEGAUGE MODEL







        public override void Initialise()
        {
            ExcessN = InputModel.InitialExcessN;
        }

        public void InitialiseNitrateParameters()
        {
            //Nitratesdayindex1 = 0;
            //Nitratesdayindex2 = 0;
            //Nitratesdayindex3 = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()
        {
            try
            {
                bool CanSimulateNitrate = (InputModel.DissolvedNinRunoff != DissolvedNinRunoffType.None || InputModel.DissolvedNinLeaching != DissolvedNinLeachingType.None || InputModel.ParticulateNinRunoff != ParticulateNinRunoffType.None);
                if (CanSimulateNitrate)
                {
                    if (CanCalculateDissolvedNInRunoff())
                    {
                        CalculateDissolvedNInRunoff();
                    }
                    if (CanCalculateDissolvedNInLeaching())
                    {
                        if (InputModel.DissolvedNinLeaching == DissolvedNinLeachingType.ModifiedSafegaugeModel)
                        {
                            CalculateDissolvedNInLeaching_SafeGauge();
                        }
                        else
                        {
                            CalculateDissolvedNInLeaching1();
                        }
                    }
                    if (CanCalculateParticulateNInRunoff())
                    {
                        CalculateParticulateNInRunoff();
                    }

                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }



        public bool CanCalculateDissolvedNInRunoff()
        {
            try
            {

                if (InputModel.DissolvedNinRunoff == DissolvedNinRunoffType.ImportedTimeSeries)
                {
                    return InputModel.NLoadInSurfaceLayerTimeSeries.GetCount() != 0;
                }
                if (InputModel.DissolvedNinRunoff == DissolvedNinRunoffType.UserDefinedProfile)
                {
                    return true;
                }
                else if (InputModel.DissolvedNinRunoff == DissolvedNinRunoffType.RattrayEmpiricalModel || InputModel.DissolvedNinRunoff == DissolvedNinRunoffType.FraserEmpiricalModel)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }


        //        //From Howleaky developers (Brett Robinson), based on the concept that soil and runoff water mixing increases up to a maximum of k.
        //        // DN = Nsurface * K(1- exp(-cvQ)
        //        // where DN is the Nitrate conc in the runoff (mg/L)
        //        // k is the parameter that regulates mixing of soil and runoff water, (suggested 0.5)
        //        // cv is parameter that describes the curvature of change in the soil and water runoff at increasing runoff values (initial guess is 0.2)
        //        // Q is runoff (mm)
        //        // Nsurface (mg N/kg) is the soil nitrate concentrate in teh survace layer (0-2cm), which in our approach is derived from nitrate load (NLsoil in kg/ha) in surface layer from DairyMod
        //        // NSurface = alpha*100*NLsoil/(depth*soildensity)
        //        // Then dissolved N load (NL, kg/ha) in runoff is
        //        // DL=ND*Q/100.0;
        //        // NOTATION USE HERE IS TO BE CONSISTENT WITH THAT USED BY VIC DPI
        //        void CalculateDissolvedNInRunoff_VicDPI()
        //{
        //	try
        //	{
        //		double NL_kg_ha = GetNO3_N_Store_TopLayer_kg_per_ha();  //Nitrate load in surface layer (From Dairymod)
        //		if(NL_kg_ha!=-32768)
        //		{
        //			NL_kg_ha=NL_kg_ha* SoilNitrateLoadWeighting1;
        //        double k = Nk; // INPUT parameter that regulates mixing of soil and runoff water
        //        double cv = Ncv;  //INPUT parameter that describes the curvature of change in soil and water runoff at increasing runoff values
        //        double Q = runoff;   //runoff amount

        //        double d = NDepthTopLayer1;    //depth of surface soil layer mm
        //        double phi = BulkDensity[0]; //soil density t/m3       ( BulkDensity is in g/cm3)

        //        //double kgs_soil_in_layer_1=BulkDensity[0]*1000.0*depth[1]*10000.0/1000.0;//per ha

        //        double NSoil = NAlpha * 100.0 * NL_kg_ha / (d * phi);     //mg/kg
        //        double DN = NSoil * k * (1 - exp(-cv * Q));
        //        double DL = DN * Q / 100.0;

        //        NO3_N_Store_TopLayer_kg_per_ha=NL_kg_ha;
        //			NO3_N_Dissolved_Runoff_mg_per_L=DN;
        //			NO3_N_Load_Runoff_kg_per_ha=DL;
        //		}
        //		else
        //		{
        //			NO3_N_Store_TopLayer_kg_per_ha	=-32768;
        //			NO3_N_Dissolved_Runoff_mg_per_L	=-32768;
        //			NO3_N_Load_Runoff_kg_per_ha		=-32768;
        //		}
        //	}
        //	catch(...)
        //	{
        //		throw;
        //	}
        //}

        void CalculateDissolvedNInRunoffRattray()
        {
            try
            {
                double NLKgHa = GetNO3NStoreTopLayerkgPerha();  //Nitrate load in surface layer (From Dairymod)
                if (NLKgHa != MathTools.MISSING_DATA_VALUE)
                {
                    double a = InputModel.N_DanRat_Alpha;
                    double b = -InputModel.N_DanRat_Beta;
                    double maxconc = InputModel.N_DanRat_MaxRunOffConc;
                    double minconc = InputModel.N_DanRat_MinRunOffConc;
                    double rate = InputModel.FertilizerInputDateSequences.ValueAtDate(Engine.TodaysDate);
                    if (rate > 0 && rate != MathTools.MISSING_DATA_VALUE)
                    {
                        NitrateCumRain = Engine.SoilModule.EffectiveRain;
                        LastNAppliedRate = rate;
                    }
                    else
                    {
                        NitrateCumRain = NitrateCumRain + Engine.SoilModule.EffectiveRain;
                    }


                    double DINMgPerL = 0;
                    if (Engine.SoilModule.Runoff > 0)
                    {

                        if (NitrateCumRain > 0)
                        {
                            DINMgPerL = LastNAppliedRate / a * Math.Pow(NitrateCumRain, b);
                        }
                        else
                        {
                            DINMgPerL = 0;
                        }
                        DINMgPerL = Math.Min(maxconc, Math.Max(minconc, DINMgPerL));
                    }

                    NO3NStoreTopLayer = MathTools.MISSING_DATA_VALUE;
                    NO3NDissolvedInRunoff = DINMgPerL;
                    NO3NRunoffLoad = DINMgPerL* Engine.SoilModule.Runoff/100.0;
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

        //void __fastcall TPERFECTSimulation::CalculateDissolvedNInRunoff_Fraser(void)
        //{
        //	try
        //	{
        //		double NL_kg_ha = GetNO3_N_Store_TopLayer_kg_per_ha();  //Nitrate load in surface layer (From Dairymod)
        //		if(NL_kg_ha!=-32768)
        //		{
        //			double invk = 1.0 / Nk;
        //double DL = N_GraFraz_DL;
        //double RL = N_GraFraz_RL;
        //double lowlimit = N_GraFraz_LowLimitDINConc;

        //double fertapp_kg_per_ha = GetFertilizerInputValue();
        //double DINinput = invk * fertapp_kg_per_ha;

        //double solid = DINinput + GetMaximum(lowlimit, solid_yesterday - GetMaximum(solid_yesterday * DL, effective_rain * RL));

        //NO3_N_Store_TopLayer_kg_per_ha=0;
        //			NO3_N_Dissolved_Runoff_mg_per_L=solid;
        //			NO3_N_Load_Runoff_kg_per_ha=0;
        //			solid_yesterday=solid;
        //		}
        //		else
        //		{
        //			NO3_N_Store_TopLayer_kg_per_ha	=-32768;
        //			NO3_N_Dissolved_Runoff_mg_per_L	=-32768;
        //			NO3_N_Load_Runoff_kg_per_ha		=-32768;
        //		}
        //	}
        //	catch(...)
        //	{

        //		throw;
        //	}
        //}

        //double __fastcall TPERFECTSimulation::GetFertilizerInputValue(void)
        //{
        //	int index = IsDateInSequenceList(today, FertilizerDateList);
        //	if(index>=0&&index<FertilizerValueList.size())
        //		return FertilizerValueList[index];
        //	return 0;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanCalculateDissolvedNInLeaching()
        {
            try
            {
                if (InputModel.DissolvedNinLeaching == DissolvedNinLeachingType.ImportedTimeSeries)
                {
                    return InputModel.NLoadInLowerLayersTimeSeries.GetCount() != 0;
                }
                else if (InputModel.DissolvedNinLeaching == DissolvedNinLeachingType.UserDefinedProfile)
                {
                    return true;
                }
                else if (InputModel.DissolvedNinLeaching == DissolvedNinLeachingType.ModifiedSafegaugeModel)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }


            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanCalculateParticulateNInRunoff()
        {
            try
            {
                if (InputModel.ParticulateNinRunoff == ParticulateNinRunoffType.ImportedTimeSeries)
                {
                    return (InputModel.InorganicNitrateNTimeseries.GetCount() != 0 &&
                              InputModel.InorganicAmmoniumNTimeseries.GetCount() != 0);
                }
                else if (InputModel.ParticulateNinRunoff == ParticulateNinRunoffType.UserDefinedProfile)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

            return false;
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
                if (InputModel.DissolvedNinRunoff == DissolvedNinRunoffType.RattrayEmpiricalModel)
                {
                    CalculateDissolvedNInRunoffRattray();
                    return;
                }


                double NLKgHa = GetNO3NStoreTopLayerkgPerha();  //Nitrate load in surface layer (From Dairymod)
                if (!MathTools.DoublesAreEqual(NLKgHa, MathTools.MISSING_DATA_VALUE))
                {
                    NLKgHa = NLKgHa * InputModel.SoilNitrateLoadWeighting1;
                    double k = InputModel.Nk;                               // INPUT parameter that regulates mixing of soil and runoff water
                    double cv = InputModel.Ncv;                             //INPUT parameter that describes the curvature of change in soil and water runoff at increasing runoff values
                    double Q = Engine.SoilModule.Runoff;                   //runoff amount
                    double d = InputModel.NDepthTopLayer1;                  //depth of surface soil layer mm
                    double phi = Engine.SoilModule.InputModel.BulkDensity[0];   //soil density t/m3       ( BulkDensity is in g/cm3)
                    double NSoil = InputModel.NAlpha * 100.0 * NLKgHa / (d * phi);      //mg/kg
                    double DN = NSoil * k * (1 - Math.Exp(-cv * Q));
                    double DL = DN * Q / 100.0;

                    NO3NStoreTopLayer = NLKgHa;
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
        public void CalculateDissolvedNInLeaching1()
        {
            try
            {
                double NSoilKgPerHa = GetNO3NStoreBotLayerkgPerha();       //nitrate concentrate in the soil (kg/ha)
                if (!MathTools.DoublesAreEqual(NSoilKgPerHa, MathTools.MISSING_DATA_VALUE))
                {
                    NSoilKgPerHa = NSoilKgPerHa * InputModel.SoilNitrateLoadWeighting2;
                    double deltadepth = InputModel.DepthBottomLayer;
                    if (deltadepth > 0)
                    {
                        double soilwater = (Engine.SoilModule.InputModel.Saturation[Engine.SoilModule.LayerCount - 1] - Engine.SoilModule.InputModel.AirDryLimit[Engine.SoilModule.LayerCount - 1]) / 100.0 * deltadepth;
                        double LE = InputModel.NitrateLeachingEfficiency;                      //Leaching efficiency (INPUT)
                        double D = Engine.SoilModule.DeepDrainage;                  //Drainage (mm)
                        double LN = NSoilKgPerHa * 1000000.0 / (soilwater * 10000.0);
                        double LL = (LN / 1000000.0) * D * 10000.0 * LE;

                        NO3NStoreBotLayer = NSoilKgPerHa;
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

        public void CalculateDissolvedNInLeaching_SafeGauge()
        {
            try
            {
                int das = Engine.CurrentCrop.DaysSincePlanting;

                //if (das == 1)
                //{
                //    ExcessN = InputModel.InitialExcessN;
                //}

                //Saturated
                Saturated = false;
                if (Engine.TodaysDate.DateInt > Engine.StartDate.DateInt && Engine.SoilModule.Runoff > 0 && YesterdaysRunoff > 0)
                {
                    Saturated = true;
                }

                if (Engine.TodaysDate.DateInt > Engine.StartDate.DateInt)
                {
                    //Excess N - calc from yesterdays values
                    ExcessN = Math.Max(ExcessN + NitrogenApplication + Mineralisation - CropUseActual - Denitrification - DINDrainage, 0);

                    //Denitrification
                    Denitrification = 0;
                    if (Saturated)
                    {
                        Denitrification = InputModel.Denitrification * ExcessN;
                    }
                }

                //Stage
                if (das == 0)
                {
                    StageType = StageType.Fallow;
                }
                else if (das > 0 && das < InputModel.MainStemDuration)
                {
                    StageType = StageType.Plant;
                }
                else if (das > 0 && das > InputModel.MainStemDuration)
                {
                    StageType = StageType.Ratoon;
                }

                //Applied N
                NitrogenApplication = 0;
                //if (StageType != StageType.Fallow && (Sim.Today - Sim.StartDate).Days % (int)InputModel.NitrogenFrequency == 0)
                //{
                //    NitrogenApplication = NApplication;
                //}
                if (InputModel.FertilizerInputDateSequences.ContainsDate(Engine.TodaysDate))
                {
                    NitrogenApplication = InputModel.FertilizerInputDateSequences.ValueAtDate(Engine.TodaysDate);
                }

                //Mineralisation
                Mineralisation = 0;
                if (StageType == StageType.Fallow)
                {                    
                    Mineralisation = Math.Min(Engine.SoilModule.InputModel.OrganicCarbon * InputModel.CNSlope, InputModel.CNMax) / 365.0;
                }

                //Crop use
                CropUseActual = 0;
                CropUsePlant = (1 / (1 + (Math.Exp((das - InputModel.PlantA) * (-InputModel.PlantB))))) * InputModel.PlantDaily;
                CropUseRatoon = (1 / (1 + (Math.Exp((das - InputModel.RatoonA) * (-InputModel.RatoonB))))) * InputModel.RatoonDaily;

                if (StageType == StageType.Plant)
                {
                    CropUseActual = CropUsePlant;
                }
                else if (StageType == StageType.Ratoon)
                {
                    CropUseActual = CropUseRatoon;
                }

                //Vol of sat
                PropVolSat = Engine.SoilModule.DeepDrainage / Engine.SoilModule.VolSat;

                //DIN Drainage
                DINDrainage = PropVolSat * ExcessN * InputModel.NitrateDrainageRetention;

                YesterdaysRunoff = Engine.SoilModule.Runoff;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }


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
        public void CalculateParticulateNInRunoff()
        {
            try
            {
                double TNSoilKgPerHa = GetTotalNStoreTopLayerkgPerha();     // Total N Concentration in soil (mg/kg) and is sum of organanic and inorgance conc at 0-2cm (Obtained from Dairymod)
                if (!MathTools.DoublesAreEqual(TNSoilKgPerHa, MathTools.MISSING_DATA_VALUE))
                {
                    TNSoilKgPerHa = TNSoilKgPerHa * InputModel.SoilNitrateLoadWeighting3;
                    double E = Engine.SoilModule.HillSlopeErosion * 1000.0;// Gross erosion (kg/ha)
                    double SDR = Engine.SoilModule.InputModel.SedDelivRatio;                         // Sediment delivery ratio.
                    double NER = InputModel.NEnrichmentRatio;                           // Nitrogen enrighment ratio
                    double d = InputModel.NDepthTopLayer2;                       // depth of surface soil layer mm
                    double phi = Engine.SoilModule.InputModel.BulkDensity[0];          // soil density t/m3       ( BulkDensity is in g/cm3)
                    double NSoil = InputModel.NAlpha * 100.0 * TNSoilKgPerHa / (d * phi);    // mg/kg
                    double PN = InputModel.NBeta * E * SDR * NSoil * NER / 1000000.0;

                    ParticNInRunoff = PN;
                    if (!MathTools.DoublesAreEqual(SDR, 0) && !MathTools.DoublesAreEqual(Engine.SoilModule.UsleLsFactor, 0))
                    {
                        PNHLCa = PN / (SDR * Engine.SoilModule.UsleLsFactor);
                    }
                    else
                    {
                        PNHLCa = 0;
                    }
                    TotalNStoreTopLayer = TNSoilKgPerHa;
                }
                else
                {
                    ParticNInRunoff = MathTools.MISSING_DATA_VALUE;
                    PNHLCa = MathTools.MISSING_DATA_VALUE;
                    TotalNStoreTopLayer = MathTools.MISSING_DATA_VALUE;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        // Called from  CalculateDissolvedNInRunoff
        // extracts value directly from input time-series;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetNO3NStoreTopLayerkgPerha()
        {
            try
            {
                if (InputModel.DissolvedNinRunoff == DissolvedNinRunoffType.ImportedTimeSeries && InputModel.NLoadInSurfaceLayerTimeSeries.GetCount() != 0)
                {
                    return InputModel.NLoadInSurfaceLayerTimeSeries.GetValueAtDate(Engine.TodaysDate);
                }
                else if (InputModel.DissolvedNinRunoff == DissolvedNinRunoffType.UserDefinedProfile)
                {
                    InputModel.SoilNLoadData1.UpdateDayIndex(Engine.TodaysDate);
                    //return DataModel.SoilNLoadData1.GetValueForDayIndex("SoilNLoadData1", Nitratesdayindex1, Sim.Today);
                    return InputModel.SoilNLoadData1.GetValueForDayIndex("SoilNLoadData1", Engine.TodaysDate);
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return 0;
        }


        // Called from  CalculateDissolvedNInLeaching
        // extracts value directly from input time-series OR can interpolate from user-defined values;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetNO3NStoreBotLayerkgPerha()
        {
            try
            {
                if (InputModel.DissolvedNinLeaching == DissolvedNinLeachingType.ImportedTimeSeries && InputModel.NLoadInLowerLayersTimeSeries.GetCount() != 0)
                {
                    return InputModel.NLoadInLowerLayersTimeSeries.GetValueAtDate(Engine.TodaysDate);
                }
                else if (InputModel.DissolvedNinLeaching == DissolvedNinLeachingType.UserDefinedProfile)
                {
                    InputModel.SoilNLoadData2.UpdateDayIndex(Engine.TodaysDate);
                    //return DataModel.SoilNLoadData2.GetValueForDayIndex("SoilNLoadData2", Nitratesdayindex2, Sim.Today);
                    return InputModel.SoilNLoadData2.GetValueForDayIndex("SoilNLoadData2", Engine.TodaysDate);
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return 0;
        }

        // Called from  CalculateParticulateNInRunoff
        // extracts value directly from input time-series;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetTotalNStoreTopLayerkgPerha()
        {
            try
            {
                if (InputModel.ParticulateNinRunoff == ParticulateNinRunoffType.ImportedTimeSeries)
                {
                    double value1 = 0;
                    double value2 = 0;
                    double value3 = 0;
                    if (InputModel.InorganicNitrateNTimeseries.GetCount() != 0)
                    {
                        value1 = InputModel.InorganicNitrateNTimeseries.GetValueAtDate(Engine.TodaysDate);
                    }
                    if (InputModel.InorganicAmmoniumNTimeseries.GetCount() != 0)
                    {
                        value2 = InputModel.InorganicAmmoniumNTimeseries.GetValueAtDate(Engine.TodaysDate);
                    }
                    if (InputModel.OrganicNTimeseries.GetCount() != 0)
                    {
                        value3 = InputModel.OrganicNTimeseries.GetValueAtDate(Engine.TodaysDate);
                    }
                    if (MathTools.DoublesAreEqual(value1, MathTools.MISSING_DATA_VALUE) || MathTools.DoublesAreEqual(value2, MathTools.MISSING_DATA_VALUE) || MathTools.DoublesAreEqual(value3, MathTools.MISSING_DATA_VALUE))
                    {
                        return MathTools.MISSING_DATA_VALUE;
                    }
                    return value1 + value1 + value3;
                }
                else if (InputModel.ParticulateNinRunoff == ParticulateNinRunoffType.UserDefinedProfile)
                {
                    //return DataModel.SoilNLoadData3.GetValueForDayIndex("SoilNLoadData3", Nitratesdayindex3, Sim.Today);
                    return InputModel.SoilNLoadData3.GetValueForDayIndex("SoilNLoadData3", Engine.TodaysDate);
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanSimulateNitrate()
        {
            try
            {
                return (InputModel.DissolvedNinRunoff != 0 || InputModel.DissolvedNinLeaching != 0 || InputModel.ParticulateNinRunoff != 0);
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

    }
}
