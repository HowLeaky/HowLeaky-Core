using HowLeaky_SimulationEngine.Enums;
using HowLeaky_SimulationEngine.Tools;
using System;

namespace HowLeaky_SimulationEngine.Inputs
{




    public class HowLeakyInputs_Irrigation:_CustomHowLeakyInputsModel
    {
        public HowLeakyInputs_Irrigation():base(null,null)
        {

        }

        public HowLeakyInputs_Irrigation(Guid? id, string name):base(id,name)
        {
        }

        public double SWDToIrrigate { get; set; }                       // Soil water deficit amount for which to trigger an irrigation (mm).      
        public double FixedIrrigationAmount { get; set; }               // Irrigation amount to apply during each irrigation (mm).        
        public int IrrigationBufferPeriod { get; set; }                 // Minimum days that should elapse between irrigations (days)        
        public IrrigationFormat IrrigationFormat { get; set; }          // Option to choose to irrigate when Soil-Water Reqires it ( while crop is growing, or in a predefined window) or at predefined dates and amounts               
        public TargetAmountOptions TargetAmountOptions { get; set; }                    
                               
        public DayMonthData IrrigWindowStartDate { get;set;}            // Start date of window in which to consider irrigating.                
        public DayMonthData IrrigWindowEndDate { get;set; }             // End date of window in which to consider irrigating.                
        public Sequence IrrigSequence { get;set; }                      // Predefined irrigation dates and amounts.              
        //public int TargetAmountOpt {get;set; }                          // Select how much water to apply from: Field Capacity (DUL), Saturation, Fixed amount, DUL+25%, DUL+50%, DUL+75%        
        public int IrrigRunoffOptions { get;set; }                      // How runoff is calculated during an irrigation including "Ignore Runoff", "Proportial to Application" and "Predefined Sequence".               
        public int IrrigCoverEffects { get;set; }                       // Cover effects options for EROSION calculations - choose which components affect erosion including "Canopy and Stubble", "Stubble only" or "None".        
        public double IrrigRunoffProportion1 { get;set; }               // Runoff percentage (of inflow) for first irrigation.               
        public double IrrigRunoffProportion2 { get;set; }               // Runoff percentage (of inflow) for later irrigations.               
        public Sequence IrrigRunoffSequence { get;set; }                // Predefined runoff dates and amounts.               
        public bool UseRingTank { get;set; }                            // Switch to simulate ring-tank component during irrigation to limit supply.                
        public bool ResetRingTank { get;set;}                           // Switch to allow ring-tank capacity to be reset a predefined date. NOTE that this introduces a volumebalance error into calculations, but is used for modelling "partial" years conditions.               
        public int AdditionalInflowFormat { get;set;}                   // Allows an additional inflow other than catchment runoff (for example, from Coal Seam Gas). Options include "Constant Inflow" and "Predefined Sequence"               
        public DayMonthData ResetRingTankDate { get;set; }              // date to reset ringtank capacity.                
        public double RingTankSeepageRate {get;set; }                   // Ring-tank seepage rate (mm/day) losses.               
        public double RingTankDepth { get;set;}                         // Depth of ring-tank (m)              
        public double RingTankArea {get;set; }                          // Ring-tank surface area (ha).       
        public double CatchmentArea {get;set;}                          // Runoff catchment area (ha) which supplies water for ring-tank.             
        public double IrrigatedArea { get;set;}                         // Irrigated (paddock) area (ha) used to calculate how much water drained from ring-tank during an irrigation.               
        public double AdditionalInflow { get;set; }                     // Constant additional inflow (ML/day) into ringtank other than catchment runoff.                
        public double RunoffCaptureRate { get;set; }                    // Pumping capacity (ML/day) for capturing catchment runoff as input into the ring-tank;       
        public double RingTankEvapCoefficient { get;set; }              // Surface water evaporation coeficient for ring-tank.               
        public double IrrigDeliveryEfficiency {  get;set; }             // Efficiency for getting water from the ring-tank to the paddock.               
        public double CapactityAtReset {  get;set; }                    // Ring-tank capacity (forced) at reset date.        
        public Sequence AdditionalInflowSequence {  get;set; }          // Predefined sequence (values and dates) of additional ring-tank inflow.                       
        public bool UsePonding {  get;set; }                            // Switch to simulate ponding conditions which sets evaporation to potential evaporation.                

        public IrrigationEvaporationOptions EvaporationOptions{get;set;}
        public double EvaporationProportion{get;set;}
    }
}
