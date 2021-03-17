using HowLeaky_SimulationEngine.Attributes;
using HowLeaky_SimulationEngine.Errors;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Engine
{
    public partial class HowLeakyEngine
    {
        public static int ROBINSON_CN { get; set; } = 0;
        public static int PERFECT_CN { get; set; } = 1;
        public static int DEFAULT_CN { get; set; } = 2;
        [ModelOption] public bool ResetSoilWaterAtDate { get; set; }
        [ModelOption] public bool ResetResidueAtDate { get; set; }
        [ModelOption] public bool ResetSoilWaterAtPlanting { get; set; }
        [ModelOption] public bool CanCalculateLateralFlow { get; set; }
        [ModelOption] public bool UsePERFECTGroundCovFn { get; set; }
        [ModelOption] public bool IgnoreCropKill { get; set; }
        [ModelOption] public bool UsePERFECTDryMatterFn { get; set; }
        [ModelOption] public bool UsePERFECTLeafAreaFn { get; set; }
        [ModelOption] public bool UsePERFECTUSLELSFn { get; set; }
        [ModelOption] public bool UsePERFECTResidueFn { get; set; }
        [ModelOption] public bool UsePERFECTSoilEvapFn { get; set; }
        [ModelOption] public int UsePERFECTCurveNoFn { get; set; }
        [ModelOption] public double ResetValueForSWAtPlanting { get; set; }
        [ModelOption] public double InitialPAW { get; set; }

        public bool UsePerfectCurveNoFn()
        {
            return ((UsePERFECTCurveNoFn == PERFECT_CN || UsePERFECTCurveNoFn == DEFAULT_CN) && !Force2011CurveNoFn);
        }

        public void ResetToDefault()
        {
            try
            {
                ResetSoilWaterAtDate = false;
                ResetResidueAtDate = false;
                ResetSoilWaterAtPlanting = false;
                CanCalculateLateralFlow = false;
                UsePERFECTGroundCovFn = false;
                IgnoreCropKill = false;
                UsePERFECTDryMatterFn = false;
                UsePERFECTLeafAreaFn = false;
                UsePERFECTUSLELSFn = true;
                UsePERFECTResidueFn = false;
                UsePERFECTSoilEvapFn = false;
                UsePERFECTCurveNoFn = DEFAULT_CN;
                InitialPAW = 0.5;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }


    }



}
