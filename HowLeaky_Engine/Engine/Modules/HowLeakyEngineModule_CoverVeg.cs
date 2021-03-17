using HowLeaky_SimulationEngine.Inputs;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HowLeaky_SimulationEngine.Enums;
using HowLeaky_SimulationEngine.Errors;

namespace HowLeaky_SimulationEngine.Engine
{
    class HowLeakyEngineModule_CoverVeg : _CustomHowLeakyEngine_VegModule
    {
        public HowLeakyEngineModule_CoverVeg(HowLeakyEngine simulation, HowLeakyInputs_CoverVeg inputs) : base(simulation)
        {
            try
            {
                Name = inputs.Name;
                this.InputModel = inputs;
                CalculateMaxRootDepth();
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }
        public HowLeakyInputs_CoverVeg InputModel { get; set; }

        public int PanDayindex { get; set; }
        public int PanEvapFactor { get; set; }

        public override void Initialise()
        {
            DaysSincePlanting=365-InputModel.PlantDay;
            base.Initialise();
            InitialisedMeasuredInputs();
            
        }

        public override string GetName()
        {
            return InputModel.Name;
        }
        public void CalculateMaxRootDepth()
        {
            try
            {
                List<double> rootDepths = InputModel.CoverProfile.values["Root Depth"];
                if (rootDepths != null)
                {
                    MaximumRootDepth = rootDepths.Max();
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
        /// <returns></returns>
        public override bool StillRequiresIrrigating()
        {
            return (CropCover > 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool DoesCropMeetSowingCriteria()
        {
            return (Engine.TodaysDate.GetJDay() == InputModel.PlantDay && CropStatus != HowLeaky_SimulationEngine.Enums.CropStatus.Growing);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()
        {
            try
            {
                //InitialisedMeasuredInputs();
                if (DoesCropMeetSowingCriteria())
                {
                    Plant();


                }
                InitialisedMeasuredInputs();
                EtPanPhenology();
                //	CalculateRootGrowth();
                CalculateTranspiration();
                CalculateBiomass();
                if (DaysSincePlanting == InputModel.DaysPlantingToHarvest)
                {
                    CalculateYield();
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }


        }


        public override void Plant()
        {
            base.Plant();
            //Sim.FManagementEvent = ManagementEvent.Planting;
            //Sim.UpdateManagementEventHistory(ManagementEvent.Planting, Sim.VegetationModule.GetCropIndex(this));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override double GetTotalCover()
        {
            try
            {
                TotalCover = ResidueCover * (1 - GreenCover) + GreenCover;
                // was requested by VicDPI to account for animal trampling
                if (TotalCover > InputModel.MaxAllowTotalCover)
                {
                    TotalCover = InputModel.MaxAllowTotalCover;
                }
                return TotalCover;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            //return 0;

        }


        public override double GetPotentialSoilEvaporation()
        {
            return Engine.ClimateModule.PanEvap * (1.0 - TotalCover * 0.87);
            // Dan Rattray: 0.87 comes from APSIM
        }

        /// <returns></returns>
        public override bool InitialisedMeasuredInputs()
        {
            try
            {
                if (InputModel.CoverDataType == 0)
                {
                    //		UpdatePanDayIndex();
                    //GreenCover = DataModel.CoverProfile.GetValueForDayIndex("Green Cover", PanDayindex, Sim.Today) / 100.0 * DataModel.GreenCoverMultiplier;
                    //ResidueCover = DataModel.CoverProfile.GetValueForDayIndex("Residue Cover", PanDayindex, Sim.Today) / 100.0 * DataModel.ResidueCoverMultiplier;
                    //Output.RootDepth = DataModel.CoverProfile.GetValueForDayIndex("Root Depth", PanDayindex, Sim.Today) * DataModel.RootDepthMultiplier;
                    //   InputModel.CoverProfile.UpdateDayIndex(Engine.TodaysDate);
                    InputModel.CoverProfile.UpdateDayIndex(Engine.TodaysDate);
                    GreenCover = InputModel.CoverProfile.GetValueForDayIndex("Green Cover", Engine.TodaysDate) / 100.0 * InputModel.GreenCoverMultiplier;
                    ResidueCover = InputModel.CoverProfile.GetValueForDayIndex("Residue Cover", Engine.TodaysDate) / 100.0 * InputModel.ResidueCoverMultiplier;
                    RootDepth = InputModel.CoverProfile.GetValueForDayIndex("Root Depth", Engine.TodaysDate) * InputModel.RootDepthMultiplier;

                    CropCover = GreenCover;
                }
                else
                {
                    if (InputModel.GreenCoverTimeSeries.GetCount() > 0 && InputModel.ResidueCoverTimeSeries.GetCount() > 0 && InputModel.RootDepthTimeSeries.GetCount() > 0)
                    {
                        double greenbiomass = InputModel.GreenCoverTimeSeries.GetValueAtDate(Engine.TodaysDate);
                        double residuebiomass = InputModel.ResidueCoverTimeSeries.GetValueAtDate(Engine.TodaysDate);
                        double rootbiomass = InputModel.RootDepthTimeSeries.GetValueAtDate(Engine.TodaysDate);
                        if (!MathTools.DoublesAreEqual(greenbiomass, MathTools.MISSING_DATA_VALUE))
                        {
                            GreenCover = 1.0 * (1 - Math.Exp(-greenbiomass * InputModel.GreenCoverMultiplier));
                        }
                        else
                        {
                            GreenCover = 0;
                        }

                        CropCover = GreenCover;

                        if (!MathTools.DoublesAreEqual(residuebiomass, MathTools.MISSING_DATA_VALUE))
                        {
                            ResidueCover = 1.0 * (1 - Math.Exp(-residuebiomass * InputModel.ResidueCoverMultiplier));
                        }
                        else
                        {
                            ResidueCover = 0;
                        }
                        if (!MathTools.DoublesAreEqual(rootbiomass, MathTools.MISSING_DATA_VALUE))
                        {
                            RootDepth = InputModel.MaxRootDepth * (1 - Math.Exp(-rootbiomass * InputModel.RootDepthMultiplier));
                        }
                        else
                        {
                            RootDepth = 0;
                        }
                    }
                    else
                    {
                        GreenCover = 0;
                        CropCover = 0;
                        ResidueCover = 0;
                        RootDepth = 0;
                    }
                }
                if (ResidueCover > 1)
                {
                    ResidueCover = 1;
                }
                if (CropCover > 1)
                {
                    CropCover = 1;
                }
                if (GreenCover > 1)
                {
                    GreenCover = 1;
                }

                if (ResidueCover < 0)
                {
                    ResidueCover = 0;
                }
                if (GreenCover < 0)
                {
                    GreenCover = 0;
                }
                if (CropCover < 0)
                {
                    CropCover = 0;
                }

                Engine.SoilModule.TotalResidueCover = ResidueCover;
                Engine.SoilModule.TotalResidueCoverPercent = ResidueCover * 100.0;
                TotalCover = GetTotalCover();
                Yield = 0;
                if(CropCover>0)
                {
                    CropStatus=CropStatus.Growing;
                }
                else
                {
                    CropStatus=CropStatus.Fallow;
                }
                //REVIEW
                //Output.GreenCover = GreenCover * 100.0;
                //CropCoverPercent = CropCover * 100.0;
                //Output.TotalCover = TotalCover * 100.0;
                //Output.ResidueCover = ResidueCover * 100.0;

                return true;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            //return false;

        }

        public override double CalculatePotentialTranspiration()
        {
            try
            {
                double cf = 1;
                if (RootDepth > 0)
                {
                    return Math.Min(GreenCover * Engine.ClimateModule.PanEvap * cf, Engine.ClimateModule.PanEvap - Engine.SoilModule.SoilEvap);
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return 0;
        }


        public void EtPanPhenology()
        {
            try
            {
                if (CropStatus == CropStatus.Growing)
                {
                    ++DaysSincePlanting;
                    if (InputModel.DaysPlantingToHarvest != 0)
                    {
                        CropStage = 3.0 * DaysSincePlanting / (double)(InputModel.DaysPlantingToHarvest);
                    }
                    else
                    {
                        CropStage = 0;
                        MathTools.LogDivideByZeroError("EtPanPhenology", "in_DaysPlantToHarvest_days", "crop_stage");
                    }
                    if (CropStage >= 3.0)
                    {
                        CropStage = 3.0;
                        //CropHarvest=true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

        public void CalculateBiomass()
        {
            DryMatter += InputModel.TranspirationEfficiency * TotalTranspiration;
            //Output.DryMatter = DryMatter;   //this used to be multiplied by 10, removed 07/03/2013
        }

        public void CalculateYield()
        {
            try
            {
                Yield = InputModel.HarvestIndex * DryMatter; //this used to be multiplied by 10, removed
                //REVIEW
                //Output.Yield = Yield / 1000.0;
                CumulativeYield += Yield;
                ++HarvestCount;
                //CropStatus = CropStatus.Fallow;
                DaysSincePlanting = 0;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }
    }
}
