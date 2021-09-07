using HowLeaky_IO;
using HowLeaky_IO.Outputs;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HowLeakyConsole
{
    class Program
    {
        public static int Progress { get; set; }
        static void Main(string[] args)
        {
            try
            {
                
                if (args!=null&&args.Length>0 && args[0].Contains(".hlk"))
                {
                    var consoleoutput = new ConsoleOutputLogger(true);
                    var argstrings = string.Join(",", args);
                    var inputspath = args[0];
                    var deleteExisting=argstrings.Contains("-x");
                    var outputdaily = argstrings.Contains("-d");
                    var outputmonthly = argstrings.Contains("-m");
                    var outputyear = argstrings.Contains("-y");
                    var type= outputyear ? HowLeakyOutputType.YearlyCsv:(outputmonthly?HowLeakyOutputType.MonthlyCsv: HowLeakyOutputType.DailyCsv);
                    var outputscsv = GetOutputsCSV(args);
                    var cores = GetCores(args);
                    var controller = new SimulationController();
                    var project = new ProjectHLK();
                    project.Open(consoleoutput, inputspath);
                    try
                    {
                        project.AddConsoleOutput("Preparing to rerun all simulations...");
                        project.AddConsoleOutput($"Outputs will be saved in {project.OutputsDirectory}...");
                        if(deleteExisting)
                        { 
                            if (Directory.Exists(project.OutputsDirectory))
                            {
                                var files = Directory.GetFiles(project.OutputsDirectory, "*.hlkbinout", SearchOption.AllDirectories).ToList();
                                if (files.Count > 0)
                                {
                                    project.AddConsoleOutput($"Please wait... deleting {files.Count} existing binary output files");
                                    foreach (var file in files)
                                    {
                                        File.Delete(file);
                                    }
                                }
                                var files2 = Directory.GetFiles(project.OutputsDirectory, "*.csv", SearchOption.AllDirectories).ToList();
                                if (files2.Count > 0)
                                {
                                    project.AddConsoleOutput($"Please wait... deleting {files2.Count} existing csv output files");
                                    foreach (var file in files2)
                                    {
                                        File.Delete(file);
                                    }
                                }
                            }
                        }
                        
                        SimulationEngineExecute(project, outputscsv, cores, type, null);
                    }
                    catch (Exception ex)
                    {
                        project.AddErrorOutput(ex.ToString());
                    }
                    
                }
                else
                {
                    Console.WriteLine("Input parameters not configured correctly. Use:");
                    Console.WriteLine("   howleakyconsole \"<projectname.hlk>\" -<optional parameters>");
                    Console.WriteLine("Optional parameters include:");
                    Console.WriteLine("   -x : delete existing outputs");
                    Console.WriteLine("   -d : include daily outputs");
                    Console.WriteLine("   -m : include monthly outputs");
                    Console.WriteLine("   -y : include yearly outputs");
                    Console.WriteLine("   -c : number of processing cores");
                    Console.WriteLine("   -o : outputs list in csv format");
                    Console.WriteLine("Examples:");
                    Console.WriteLine("    howleakyconsole \"D:\\HLProjects\\project.hlk\" -m -c1 -o\"Rain,SoilEvap\"");
                    Console.WriteLine("    howleakyconsole \"..\\myvalidation.hlk\" -m -o\"Rain,Runoff,DeepDrainage\"");
                    Console.WriteLine(" ");
                    Console.WriteLine("Outputs include:");
                    Console.WriteLine("     Climate'");
                    Console.WriteLine("         Rainfall mm - use 'Rain'");
                    Console.WriteLine("         Maximum temperature oC - use 'MaxTemp'");
                    Console.WriteLine("         Minimum temperatures oC - use 'MinTemp'");
                    Console.WriteLine("         Pan evaporation mm - use 'PanEvap'");
                    Console.WriteLine("         Solar radiation MJ/m2/day - use 'SolarRadiation'");
                    Console.WriteLine("     Water-balance'");

                    Console.WriteLine("         Runoff mm - use 'Runoff'");
                    Console.WriteLine("         Soil evaporation mm - use 'SoilEvap'");
                    Console.WriteLine("         Transpiration mm - use 'Transpiration'");
                    Console.WriteLine("         Evapotranspiration mm - use 'EvapoTransp'");
                    Console.WriteLine("         Deep drainage mm - use 'DeepDrainage'");
                    Console.WriteLine("         Overflow mm - use 'Overflow'");
                    Console.WriteLine("         Potential soil evaporation mm - use 'PotSoilEvap'");
                    Console.WriteLine("         Irrigation mm - use 'Irrigation'");
                    Console.WriteLine("         Runoff from Irrigation mm - use 'RunoffFromIrrigation'");
                    Console.WriteLine("         Runoff from rainfall mm - use 'RunoffFromRainfall'");
                    Console.WriteLine("         Lateral flow mm - use 'LateralFlow'");
                    Console.WriteLine("         Volume balance error % - use 'VBE'");
                    Console.WriteLine("         Runoff Curve No - use 'RunoffCurveNo'");
                    Console.WriteLine("         Runoff Rention No - use 'RunoffRetentionNumber'");
                    Console.WriteLine("         Layer 1 Sat Index - use 'Layer1SatIndex'");

                    Console.WriteLine("     Soil'");
                    Console.WriteLine("         Hillslope erosion t/ha - use 'HillSlopeErosion'");
                    Console.WriteLine("         Off-site sediment delivery t/ha - use 'OffSiteSedDelivery'");
                    Console.WriteLine("         Total available soil water mm - use 'TotalSoilWater'");
                    Console.WriteLine("         Soil water deficit mm - use 'SoilWaterDeficit'");
                    Console.WriteLine("         Total crop residue kg/ha - use 'TotalCropResidue'");
                    Console.WriteLine("         Total Residue Cover % - use 'TotalResidueCover'");
                    Console.WriteLine("         Total Cover % - use 'TotalCoverAllCrops'");
                    Console.WriteLine("         Layer Outputs: Available soil water mm - use 'SoilWater'");
                    Console.WriteLine("         Layer Outputs: Drainage mm - use 'Drainage'");

                    Console.WriteLine("     Crops'");

                    Console.WriteLine("         Days since planting days - use 'DaysSincePlanting'");
                    Console.WriteLine("         Leaf Area Index (if applicable)  - use 'LAI'");
                    Console.WriteLine("         Crop cover % - use 'GreenCover'");
                    Console.WriteLine("         Residue cover % - use 'ResidueCover'");
                    Console.WriteLine("         Total cover % - use 'TotalCover'");
                    Console.WriteLine("         Crop residue kg/ha - use 'ResidueAmount'");
                    Console.WriteLine("         Dry matter kg/ha - use 'DryMatter'");
                    Console.WriteLine("         Root depth mm - use 'RootDepth'");
                    Console.WriteLine("         Yield t/ha - use 'Yield'");
                    Console.WriteLine("         Potential transpiration mm - use 'PotTranspiration'");
                    Console.WriteLine("         Growth regulator - use 'GrowthRegulator'");
                    Console.WriteLine("         Water Stress Index - use 'WaterStressIndex'");
                    Console.WriteLine("         Temperature Stress Index - use 'TempStressIndex'");
                    Console.WriteLine("         In-crop rainfall mm - use 'CropRainfall'");
                    Console.WriteLine("         In-crop irrigation mm - use 'CropIrrigation'");
                    Console.WriteLine("         In-crop runoff mm - use 'CropRunoff'");
                    Console.WriteLine("         In-crop soil evaporation mm - use 'SoilEvaporation'");
                    Console.WriteLine("         In-crop Transpiration mm - use 'CropTranspiration'");
                    Console.WriteLine("         In-crop evapotranspiration mm - use 'CropEvapoTranspiration'");
                    Console.WriteLine("         In-crop Drainage - use 'Crop - use 'mm - use 'CropDrainage'");
                    Console.WriteLine("         In-crop LateralFlow - use 'Crop - use 'mm - use 'CropLateralFlow'");
                    Console.WriteLine("         In-crop Overflow - use 'Crop - use 'mm - use 'CropOverflow'");
                    Console.WriteLine("         In-crop SoilErosion - use 'Crop - use 'mm - use 'CropSoilErosion'");
                    Console.WriteLine("         In-crop SedimentDelivery - use 'Crop - use 'mm - use 'CropSedimentDelivery'");
                    Console.WriteLine("         PlantingCount - use 'Crop - use 'mm - use 'PlantingCount'");
                    Console.WriteLine("         HarvestCount - use 'Crop - use 'mm - use 'HarvestCount'");
                    Console.WriteLine("         CropDeathCount - use 'Crop - use 'mm - use 'CropDeathCount'");
                    Console.WriteLine("         FallowCount - use 'Crop - use 'mm - use 'FallowCount'");

                    Console.WriteLine("     Irrigation'");

                    Console.WriteLine("         IrrigationRunoff mm - use 'IrrigationRunoff'");
                    Console.WriteLine("         IrrigationApplied mm - use 'IrrigationApplied'");
                    Console.WriteLine("         IrrigationInfiltration mm - use 'IrrigationInfiltration'");
                    Console.WriteLine("         Evaporation losses ML - use 'RingTankEvaporationLosses'");
                    Console.WriteLine("         Seepage losses ML - use 'RingTankSeepageLosses'");
                    Console.WriteLine("         Overtopping losses ML - use 'RingTankOvertoppingLosses'");
                    Console.WriteLine("         Irrigation losses ML - use 'RingTankIrrigationLosses'");
                    Console.WriteLine("         Total losses ML - use 'RingTankTotalLosses'");
                    Console.WriteLine("         Captured runoff inflow ML - use 'RingTankRunoffCaptureInflow'");
                    Console.WriteLine("         Rainfall inflow ML - use 'RingTankRainfalInflow'");
                    Console.WriteLine("         Effective additional inflow ML - use 'RingTankEffectiveAdditionalInflow'");
                    Console.WriteLine("         Total additional inflow ML - use 'RingTankTotalAdditionalInflow'");
                    Console.WriteLine("         Total inflow ML - use 'RingTankTotalInflow'");
                    Console.WriteLine("         Ineffective additional inflow ML - use 'RingTankIneffectiveAdditionalInflow'");
                    Console.WriteLine("         Storage volume ML - use 'RingTankStorageVolume'");
                    Console.WriteLine("         Irrigation storage level % - use 'RingTankStorageLevel'");

                    Console.WriteLine("     Phosphorus'");
                    Console.WriteLine("         Particulate concentration mg/L - use 'ParticulateConc'");
                    Console.WriteLine("         Dissolved concentration mg/L - use 'DissolvedConc'");
                    Console.WriteLine("         Bioavailable particulate P concentration mg/L - use 'BioAvailParticPConc'");
                    Console.WriteLine("         Bioavailable P concentration mg/L - use 'BioAvailPConc'");
                    Console.WriteLine("         Total P concentration mg/L - use 'TotalPConc'");
                    Console.WriteLine("         Particulate P export kg/ha - use 'ParticPExport'");
                    Console.WriteLine("         Dissolved export kg/ha - use 'PhosExportDissolve'");
                    Console.WriteLine("         Bioavailable particulate P export kg/ha - use 'BioAvailParticPExport'");
                    Console.WriteLine("         Total bioavailable export kg/ha - use 'TotalBioAvailExport'");
                    Console.WriteLine("         Total phosphorus export kg/ha - use 'TotalP'");
                    Console.WriteLine("         PPHLC kg/ha - use 'PPHLC'");

                    Console.WriteLine("     Pesticides'");
                    Console.WriteLine("         Applied pest on veg g/ha - use 'AppliedPestOnVeg'");
                    Console.WriteLine("         Applied pest on stubble g/ha - use 'AppliedPestOnStubble'");
                    Console.WriteLine("         Applied pest on soil g/ha - use 'AppliedPestOnSoil'");
                    Console.WriteLine("         Pest on veg g/ha - use 'PestOnVeg'");
                    Console.WriteLine("         Pest on stubble g/ha - use 'PestOnStubble'");
                    Console.WriteLine("         Pest in soil g/ha - use 'PestInSoil'");
                    Console.WriteLine("         Pest soil conc. mg/kg - use 'PestSoilConc'");
                    Console.WriteLine("         Pest sediment phase conc. mg/kg - use 'PestSedPhaseConc'");
                    Console.WriteLine("         Pest water phase conc. ug/L - use 'PestWaterPhaseConc'");
                    Console.WriteLine("         Pest runoff conc. ug/L - use 'PestRunoffConc'");
                    Console.WriteLine("         Pest lost in runoff water g/ha - use 'PestLostInRunoffWater'");
                    Console.WriteLine("         Pest lost in runoff sediment g/ha - use 'PestLostInRunoffSediment'");
                    Console.WriteLine("         Total pest lost in runoff g/ha - use 'TotalPestLostInRunoff'");
                    Console.WriteLine("         Pest lost in leaching g/ha - use 'PestLostInLeaching'");
                    Console.WriteLine("         Pest losses as percent of last input % - use 'PestLossesPercentOfInput'");

                    Console.WriteLine("     Nitrates'");
                    Console.WriteLine("         Dissolved NO3 N in Runoff mg/L - use 'NO3NDissolvedInRunoff'");
                    Console.WriteLine("         NO3 Runoff Load kg/ha - use 'NO3NRunoffLoad'");
                    Console.WriteLine("         Dissolved NO3 N in Leaching mg/L - use 'NO3NDissolvedLeaching'");
                    Console.WriteLine("         NO3 N Leaching Load kg/ha - use 'NO3NLeachingLoad'");
                    Console.WriteLine("         Particulate N in Runoff kg/ha - use 'ParticNInRunoff'");
                    Console.WriteLine("         PNHLC kg/ha - use 'PNHLCa'");
                    Console.WriteLine("         NO3 N Store (top layer) kg/ha - use 'NO3NStoreTopLayer'");
                    Console.WriteLine("         NO3 N Store (bot layer) kg/ha - use 'NO3NStoreBotLayer'");
                    Console.WriteLine("         Total N Store (top layer) kg/ha - use 'TotalNStoreTopLayer'");
                    Console.WriteLine("         Drainage in NO3 period mm - use 'DrainageInNO3Period'");
                    Console.WriteLine("         Runoff in NO3 period mm - use 'RunoffInNO3Period'");

                    Console.WriteLine("     Solutes'");
                    Console.WriteLine("         Total Soil Solute kg/ha - use 'TotalSoilSolute'");
                    Console.WriteLine("         Total Soil Water Solute (Concentration) mg/L_soil-water - use '??'");
                    Console.WriteLine("         Layer Solute (Load) kg/ha - use 'LayerSoluteLoad'");
                    Console.WriteLine("         Layer Solute (Concentration) mg/L_soil-water - use 'LayerSoluteConcmgPerL'");
                    Console.WriteLine("         Layer Solute (Concentration) mg/kg soil - use 'LayerSoluteConcmgPerkg'");
                    Console.WriteLine("         Leachate Solute Concentration mg/L soil-water - use 'LeachateSoluteConcmgPerL'");
                    Console.WriteLine("         Leachate Solute Load kg/ha - use 'LeachateSoluteLoadkgPerha'");
                }
            }
            catch (Exception ex)
            {
                Console.Write($"{ex}");

            }
        }

        private static string GetOutputsCSV(string[] args)
        {
            foreach (var arg in args)
            {
                if (arg.Contains("-o"))
                {
                    return arg.Replace("-o","").Trim();
                }
            }
            return "Rain,Runoff";
        }

        private static int GetCores(string[] args)
        {
            foreach (var arg in args)
            {
                if (arg.Contains("-c"))
                {
                    var coresstring = arg.Replace("-c", "").Trim();
                    int cores;
                    if (Int32.TryParse(coresstring, out cores))
                    {
                        if (cores < Environment.ProcessorCount - 1)
                            return cores;
                    }

                }
            }
            return Environment.ProcessorCount - 1;
        }

        private static void SimulationEngineExecute(ProjectHLK project, string outputs, int cores, HowLeakyOutputType outputType,int? targetindex, Action onSuccess = null)
        {
            var controller = new SimulationController();
            var myprogress = new GlobalProgress(project.Simulations.Count);
            myprogress.StartExecution();
            var tokenSource = new CancellationTokenSource();
            CancellationToken ct = tokenSource.Token;
            project.OutputsCSV = outputs;
            project.AddConsoleOutput("Running simulations... please wait...");
            //var runSimulationsTask = Task.Run(() => Controller.Execute(CurrentProject, MyProgress));
            var taskList=new System.Collections.Generic.List<Task>();
            taskList.Add( Task.Factory.StartNew(delegate
            {               
                controller.Execute(project, tokenSource, cores, targetindex, outputType, myprogress);
            }, tokenSource.Token)); // Pass same token to StartNew.
            Task.WaitAll(taskList.ToArray());
        }




    }
}
