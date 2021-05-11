using Figgle;
using HowLeaky_IO;
using HowLeaky_IO.Outputs;
using HowLeaky_ValidationEngine.Maths.Regression;
using HowLeaky_ValidationEngine.Models;
using HowLeaky_ValidationEngine.Models.Validation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HowLeaky_ValidationEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            //var version = "Version 6.01";
            var argstrings = string.Join(",", args);
            var branch= (args.Count()>0?args[0]:"Visual Studio Build").Replace("/","_");
            var buildnumber= args.Count() > 1 ? args[1] : "Unknown Build#";
            var username= args.Count()>2?args[2]:"Developer";
            var prepareProjects = argstrings.Contains("-p");

            var version=$"{branch}_{buildnumber}";// ({username})";

           // var generateBaseData = argstrings.Contains("-b");

            var consoleoutput = new ConsoleOutputLogger(true);
           // var controller = new SimulationController();

            var report = new PostModel();
            report.Report.BaseName = "Initial";
            report.Report.BranchName = version;

            var directories = ExtractDirectories();
            foreach (var directory in directories)
            {
                consoleoutput.ErrorOutputList.Clear();
                var name = Path.GetFileName(directory);
                consoleoutput.AddConsoleOutput(FiggleFonts.Standard.Render($"Preparing {name}"), false);
                //   consoleoutput.AddConsoleOutput($"Preparing {name}", false);
                PrepareSimulatedOutputsList(consoleoutput, directory, prepareProjects);
                PrepareMeasuredDataList(consoleoutput, directory, prepareProjects);

                //if (generateBaseData)
                // {
                RunSimulations(consoleoutput, directory, version);
                // }
                //  else
                // {
                var projectvm = report.CreateProjectModel(directory, consoleoutput.ErrorOutputList);
                ExecuteProjectValidations(consoleoutput, report,projectvm, directory);
                ExecuteProjectValidations2(consoleoutput, report,projectvm, directory);
                //}
                consoleoutput.AddConsoleOutput("");
                consoleoutput.AddConsoleOutput("");
            }
            PostResults(report);
        }

        private static string PostResults(PostModel report)
        {
            string url = "http://howleaky.com/api/JenkinsAPI/PostJenkinsReport";
            //string url = "https://localhost:44331/api/JenkinsAPI/PostJenkinsReport";
            var webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.Method = "POST";
            webrequest.ContentType = "application/json";
            using (var stream = new StreamWriter(webrequest.GetRequestStream()))
            {
                var serialized = JsonConvert.SerializeObject(report);
                stream.Write(serialized);
            }

            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            var enc = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            string result = string.Empty;
            result = responseStream.ReadToEnd();
            webresponse.Close();
            return result;


        }

        private static void RunSimulations(ConsoleOutputLogger consoleoutput, string directory, string version)
        {
            var outputscsv = GetOutputsCSV(directory);
            var cores = 1;//Environment.ProcessorCount - 1;
            var controller = new SimulationController();
            var project = new ProjectHLK();
            var projectname = Path.GetFileName(directory);
            var filename = Path.Combine(directory, $"{projectname}.hlk");
            if (File.Exists(filename))
            {
                if (project.Open(consoleoutput, filename) != null)
                {
                    try
                    {
                        if (!Directory.Exists(project.OutputsDirectory))
                        {
                            Directory.CreateDirectory(project.OutputsDirectory);
                        }
                        project.OutputsDirectory = Path.Combine(project.OutputsDirectory, version);
                        if (!Directory.Exists(project.OutputsDirectory))
                        {
                            Directory.CreateDirectory(project.OutputsDirectory);
                        }

                        project.AddConsoleOutput("Preparing to rerun all simulations...");
                        project.AddConsoleOutput($"Outputs will be saved in {project.OutputsDirectory}...");

                        if (Directory.Exists(project.OutputsDirectory))
                        {

                            var files = Directory.GetFiles(project.OutputsDirectory, "*.csv", SearchOption.AllDirectories).ToList();
                            if (files.Count > 0)
                            {
                                project.AddConsoleOutput($"Please wait... deleting {files.Count} existing csv output files");
                                foreach (var file in files)
                                {
                                    File.Delete(file);
                                }
                            }
                        }
                        SimulationEngineExecute(project, outputscsv, cores, HowLeakyOutputType.DailyAndMonthlyCsv, null);
                    }
                    catch (Exception ex)
                    {
                        project.AddErrorOutput(ex.ToString());
                    }
                }
            }
        }

        private static string GetOutputsCSV(string directory)
        {
            var filename = Path.Combine(directory, "SimulatedOutputs.txt");
            var list = new List<string>();
            var lines = File.ReadAllLines(filename);
            foreach (var line in lines)
            {
                var items = line.Split(",").ToList();
                if (items.Count == 3)
                {
                    var name = items[1].Trim();
                    var state = items[2].ToLower().Trim();
                    if (state == "true")
                    {
                        list.Add(name);
                    }
                }
            }
            return String.Join(",", list);
        }

        private static void PrepareSimulatedOutputsList(ConsoleOutputLogger consoleoutput, string projectname, bool rebuild)
        {
            var filename = Path.Combine(projectname, "SimulatedOutputs.txt");
            if (rebuild || !File.Exists(filename))
            {
                var list = new List<string>();
                list.Add("Rainfall mm,Rain,true");
                list.Add("Maximum temperature oC,MaxTemp,true");
                list.Add("Minimum temperatures oC,MinTemp,true");
                list.Add("Pan evaporation mm,PanEvap,true");
                list.Add("Solar radiation MJ/m2/day,SolarRadiation,false");
                list.Add("Runoff mm,Runoff,true");
                list.Add("Soil evaporation mm,SoilEvap,true");
                list.Add("Transpiration mm,Transpiration,true");
                list.Add("Evapotranspiration mm,EvapoTransp,true");
                list.Add("Deep drainage mm,DeepDrainage,true");
                list.Add("Overflow mm,Overflow,false");
                list.Add("Potential soil evaporation mm,PotSoilEvap,false");
                list.Add("Irrigation mm,Irrigation,true");
                list.Add("Runoff from Irrigation mm,RunoffFromIrrigation,false");
                list.Add("Runoff from rainfall mm,RunoffFromRainfall,false");
                list.Add("Lateral flow mm,LateralFlow,false");
                list.Add("Volume balance error %,VBE,false");
                list.Add("Runoff Curve No,RunoffCurveNo,false");
                list.Add("Runoff Rention No,RunoffRetentionNumber,false");
                list.Add("Layer 1 Sat Index,Layer1SatIndex,false");
                list.Add("Hillslope erosion t/ha,HillSlopeErosion,true");
                list.Add("Off-site sediment delivery t/ha,OffSiteSedDelivery,true");
                list.Add("Total available soil water mm,TotalSoilWater,false");
                list.Add("Soil water deficit mm,SoilWaterDeficit,false");
                list.Add("Total crop residue kg/ha,TotalCropResidue,false");
                list.Add("Total Residue Cover %,TotalResidueCover,false");
                list.Add("Total Cover %,TotalCoverAllCrops,false");
                list.Add("Layer Outputs: Available soil water mm,SoilWater,false");
                list.Add("Layer Outputs: Drainage mm,Drainage,false");
                list.Add("Days since planting days,DaysSincePlanting,false");
                list.Add("Leaf Area Index (if applicable) ,LAI,false");
                list.Add("Crop cover %,GreenCover,false");
                list.Add("Residue cover %,ResidueCover,false");
                list.Add("Total cover %,TotalCover,false");
                list.Add("Crop residue kg/ha,ResidueAmount,false");
                list.Add("Dry matter kg/ha,DryMatter,false");
                list.Add("Root depth mm,RootDepth,false");
                list.Add("Yield t/ha,Yield,false");
                list.Add("Potential transpiration mm,PotTranspiration,false");
                list.Add("Growth regulator,GrowthRegulator,false");
                list.Add("Water Stress Index,WaterStressIndex,false");
                list.Add("Temperature Stress Index,TempStressIndex,false");
                list.Add("In-crop rainfall mm,CropRainfall,false");
                list.Add("In-crop irrigation mm,CropIrrigation,false");
                list.Add("In-crop runoff mm,CropRunoff,false");
                list.Add("In-crop soil evaporation mm,SoilEvaporation,false");
                list.Add("In-crop Transpiration mm,CropTranspiration,false");
                list.Add("In-crop evapotranspiration mm,CropEvapoTranspiration,false");
                list.Add("In-crop Drainage,Crop,mm,CropDrainage,false");
                list.Add("In-crop LateralFlow,Crop,mm,CropLateralFlow,false");
                list.Add("In-crop Overflow,Crop,mm,CropOverflow,false");
                list.Add("In-crop SoilErosion,Crop,mm,CropSoilErosion,false");
                list.Add("In-crop SedimentDelivery,Crop,mm,CropSedimentDelivery,false");
                list.Add("PlantingCount,Crop,mm,PlantingCount,false");
                list.Add("HarvestCount,Crop,mm,HarvestCount,false");
                list.Add("CropDeathCount,Crop,mm,CropDeathCount,false");
                list.Add("FallowCount,Crop,mm,FallowCount,false");
                list.Add("IrrigationRunoff mm,IrrigationRunoff,false");
                list.Add("IrrigationApplied mm,IrrigationApplied,false");
                list.Add("IrrigationInfiltration mm,IrrigationInfiltration,false");
                list.Add("Evaporation losses ML,RingTankEvaporationLosses,false");
                list.Add("Seepage losses ML,RingTankSeepageLosses,false");
                list.Add("Overtopping losses ML,RingTankOvertoppingLosses,false");
                list.Add("Irrigation losses ML,RingTankIrrigationLosses,false");
                list.Add("Total losses ML,RingTankTotalLosses,false");
                list.Add("Captured runoff inflow ML,RingTankRunoffCaptureInflow,false");
                list.Add("Rainfall inflow ML,RingTankRainfalInflow,false");
                list.Add("Effective additional inflow ML,RingTankEffectiveAdditionalInflow,false");
                list.Add("Total additional inflow ML,RingTankTotalAdditionalInflow,false");
                list.Add("Total inflow ML,RingTankTotalInflow,false");
                list.Add("Ineffective additional inflow ML,RingTankIneffectiveAdditionalInflow,false");
                list.Add("Storage volume ML,RingTankStorageVolume,false");
                list.Add("Irrigation storage level %,RingTankStorageLevel,false");
                list.Add("Particulate concentration mg/L,ParticulateConc,false");
                list.Add("Dissolved concentration mg/L,DissolvedConc,false");
                list.Add("Bioavailable particulate P concentration mg/L,BioAvailParticPConc,false");
                list.Add("Bioavailable P concentration mg/L,BioAvailPConc,false");
                list.Add("Total P concentration mg/L,TotalPConc,false");
                list.Add("Particulate P export kg/ha,ParticPExport,false");
                list.Add("Dissolved export kg/ha,PhosExportDissolve,false");
                list.Add("Bioavailable particulate P export kg/ha,BioAvailParticPExport,false");
                list.Add("Total bioavailable export kg/ha,TotalBioAvailExport,false");
                list.Add("Total phosphorus export kg/ha,TotalP,false");
                list.Add("PPHLC kg/ha,PPHLC,false");
                list.Add("Applied pest on veg g/ha,AppliedPestOnVeg,false");
                list.Add("Applied pest on stubble g/ha,AppliedPestOnStubble,false");
                list.Add("Applied pest on soil g/ha,AppliedPestOnSoil,false");
                list.Add("Pest on veg g/ha,PestOnVeg,false");
                list.Add("Pest on stubble g/ha,PestOnStubble,false");
                list.Add("Pest in soil g/ha,PestInSoil,false");
                list.Add("Pest soil conc. mg/kg,PestSoilConc,false");
                list.Add("Pest sediment phase conc. mg/kg,PestSedPhaseConc,false");
                list.Add("Pest water phase conc. ug/L,PestWaterPhaseConc,false");
                list.Add("Pest runoff conc. ug/L,PestRunoffConc,false");
                list.Add("Pest lost in runoff water g/ha,PestLostInRunoffWater,false");
                list.Add("Pest lost in runoff sediment g/ha,PestLostInRunoffSediment,false");
                list.Add("Total pest lost in runoff g/ha,TotalPestLostInRunoff,false");
                list.Add("Pest lost in leaching g/ha,PestLostInLeaching,false");
                list.Add("Pest losses as percent of last input %,PestLossesPercentOfInput,false");
                list.Add("Dissolved NO3 N in Runoff mg/L,NO3NDissolvedInRunoff,false");
                list.Add("NO3 Runoff Load kg/ha,NO3NRunoffLoad,false");
                list.Add("Dissolved NO3 N in Leaching mg/L,NO3NDissolvedLeaching,false");
                list.Add("NO3 N Leaching Load kg/ha,NO3NLeachingLoad,false");
                list.Add("Particulate N in Runoff kg/ha,ParticNInRunoff,false");
                list.Add("PNHLC kg/ha,PNHLCa,false");
                list.Add("NO3 N Store (top layer) kg/ha,NO3NStoreTopLayer,false");
                list.Add("NO3 N Store (bot layer) kg/ha,NO3NStoreBotLayer,false");
                list.Add("Total N Store (top layer) kg/ha,TotalNStoreTopLayer,false");
                list.Add("Drainage in NO3 period mm,DrainageInNO3Period,false");
                list.Add("Runoff in NO3 period mm,RunoffInNO3Period,false");
                list.Add("Total Soil Solute kg/ha,TotalSoilSolute,false");
                list.Add("Total Soil Water Solute (Concentration) mg/L_soil-water,??,false");
                list.Add("Layer Solute (Load) kg/ha,LayerSoluteLoad,false");
                list.Add("Layer Solute (Concentration) mg/L_soil-water,LayerSoluteConcmgPerL,false");
                list.Add("Layer Solute (Concentration) mg/kg soil,LayerSoluteConcmgPerkg,false");
                list.Add("Leachate Solute Concentration mg/L soil-water,LeachateSoluteConcmgPerL,false");
                list.Add("Leachate Solute Load kg/ha,LeachateSoluteLoadkgPerha,false");

                System.IO.File.WriteAllLines(filename, list);

            }

        }

        private static List<string> ExtractDirectories()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var keyword = "HowLeaky_JenkinsReport";
            var basePath = path.Substring(0, path.LastIndexOf(keyword) + keyword.Length);
            var datapath = Path.Combine(basePath, "Data");
            var paths = Directory.GetDirectories(datapath, "*").ToList();
            return paths;
        }


        private static void PrepareMeasuredDataList(ConsoleOutputLogger consoleoutput, string projectname, bool rebuild)
        {
            var filename = Path.Combine(projectname, "MeasuredSimulatedPairs.txt");
            if (rebuild || !File.Exists(filename))
            {
                var measuredpath = Path.Combine(projectname, "Imported");

                var files = Directory.GetFiles(measuredpath);
                var list = new List<string>();
                foreach (var file in files)
                {
                    var name = Path.GetFileName(file);
                    var data = File.ReadAllLines(file);
                    var headers = ExtractMeasuredDataHeaders(data);
                    foreach (var header in headers)
                    {
                        list.Add($"{name},{header.Trim()},null,null,false");
                    }
                }
                if (list.Count > 0)
                {
                    if (File.Exists(filename))
                    {
                        System.IO.File.Move(filename, $"{filename.Replace("txt", "")}.old{DateTime.Now:ddMMyyyhhmm}.txt");
                    }
                    System.IO.File.WriteAllLines(filename, list);
                }
            }

        }


        public static List<string> ExtractMeasuredDataHeaders(string[] lines)
        {
            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                if (trimmed.Count() > 4 && trimmed.Substring(0, 4).ToLower() == "date")
                {
                    var dellist = trimmed.Split(",").ToList();
                    if (dellist.Count == 1)
                    {
                        dellist = trimmed.Split("\t").ToList();
                        if (dellist.Count > 1)
                        {
                            return dellist;
                        }
                    }
                    else if (dellist.Count > 1)
                    {
                        return dellist;
                    }
                }
            }
            return new List<string>();
        }
        public static void ExecuteProjectValidations(ConsoleOutputLogger consoleoutput, PostModel postmodel, Validation_HLKProject projectvm,string projectname)
        {
            try
            {
              
                var outputsPath = Path.Combine(projectname, "Outputs");
                var basePath = Path.Combine(outputsPath, postmodel.Report.BaseName);
                var versionPath = Path.Combine(outputsPath, postmodel.Report.BranchName);
                var baseFiles = Directory.GetFiles(basePath, "*.csv");
                var versionFiles = Directory.GetFiles(versionPath, "*.csv");
                if (versionFiles.Count() > 0)
                {
                    var headers = ExtractHeaders(versionFiles[0]);
                    var index = 0;
                    foreach (var header in headers)
                    {
                        ++index;
                        var biglist = new List<List<List<double>>>();
                        var simnames = new List<string>();
                        var colors = new List<string>();

                        var xdata = new List<double>();
                        var ydata = new List<double>();
                        var scatterplotmodel = postmodel.CreateScatterPlotModel(projectvm, header, ScatterType.CompareVersions);
                        var colorindex = 0;
                        foreach (var filename1 in versionFiles)
                        {
                            var outputname = Path.GetFileName(filename1);
                            var simname = ExtractSimName(outputname);
                            var color = GetUniqueColor(colorindex++);
                            var filename2 = baseFiles.FirstOrDefault(x => Path.GetFileName(x) == outputname);
                            if (filename2 != null)
                            {
                                var extracted = ExtractScatterData(postmodel, projectvm, filename1, filename2, index);
                                if (extracted.Count == 2)
                                {
                                    simnames.Add(simname);
                                    colors.Add(color);
                                    var x = extracted[0];
                                    var y = extracted[1];
                                    if (x.Count > 0 && y.Count > 0)
                                    {
                                        xdata.AddRange(x);
                                        ydata.AddRange(y);
                                    }
                                    biglist.Add(extracted);
                                }
                            }
                        }
                        var x_zeros = xdata.Count(x => Math.Abs(x) < 0.000001);
                        var y_zeros = ydata.Count(x => Math.Abs(x) < 0.000001);
                        var all_x_zeros = x_zeros == xdata.Count();
                        var all_y_zeros = y_zeros == ydata.Count();
                        if (!all_x_zeros && !all_y_zeros)
                        {
                            var scatterplot = new RegressionEngine(xdata, ydata);
                            var results = scatterplot.Calculate();
                            if (results != null)
                            {
                                scatterplotmodel.R2 = results.RSquared;
                                scatterplotmodel.RMSE = results.RMSE;
                                if (results.Slope != null && results.Intercept != null)
                                {
                                    scatterplotmodel.Slope = (double)results.Slope;
                                    scatterplotmodel.Intercept = (double)results.Intercept;
                                }
                            }
                        }
                        else
                        {
                            scatterplotmodel.R2 = 0;
                            scatterplotmodel.RMSE = 0;
                            scatterplotmodel.Slope = 0;
                            scatterplotmodel.Intercept = 0;
                        }
                        scatterplotmodel.NamesCsv = string.Join("|", simnames);
                        scatterplotmodel.ColorsCsv = string.Join("|", colors);
                        scatterplotmodel.DataCsv = ConvertDataToCsv(biglist);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void ExecuteProjectValidations2(ConsoleOutputLogger consoleoutput, PostModel postmodel, Validation_HLKProject projectvm, string projectname)
        {
            try
            {
               
                var outputsPath = Path.Combine(projectname, "Outputs");
                var versionPath = Path.Combine(outputsPath, postmodel.Report.BranchName);
                //   var measuredPath = Path.Combine(outputsPath, "Imported");               
                var versionFiles = Directory.GetFiles(versionPath, "*Daily.csv").ToList();

                var measSimLinks = ExtractMeasuredSimulatedLinkages(projectname, versionFiles);
                if (measSimLinks.Count > 0)
                {
                    foreach (var item in measSimLinks)
                    {
                        var cumpolotmodel = postmodel.CreateCumulativePlotModel(projectvm, item.Title());
                        cumpolotmodel.Colors = $"#949494|#1e90ff";
                        cumpolotmodel.Names = $"Measured|Predicted";
                        cumpolotmodel.StartDateInt=item.Dates.FirstOrDefault().DateInt;
                        cumpolotmodel.EndDateInt=item.Dates.LastOrDefault().DateInt;
                        var s2 = String.Join(",", item.MeasCumValues.Select(x => $"{x:F4}"));
                        var s3 = String.Join(",", item.PredCumValues.Select(x => $"{x:F4}"));
                        cumpolotmodel.Data = $"{s2}|{s3}";
                    }

                    var groupedMeasSimLinks = GroupMeasuredSimLinks(measSimLinks);
                    var index = 0;
                    foreach (var keypair in groupedMeasSimLinks)
                    {
                        ++index;
                        var biglist = new List<List<List<double>>>();
                        var simnames = new List<string>();
                        var colors = new List<string>();

                        var xdata = new List<double>();
                        var ydata = new List<double>();
                        var scatterplotmodel = postmodel.CreateScatterPlotModel(projectvm, keypair.Key, ScatterType.MeasuredPredicted);
                        var colorindex = 0;
                        foreach (var meassimlink in keypair.Value)
                        {

                            var simname = meassimlink.Title();
                            var color = GetUniqueColor(colorindex++);
                            
                            simnames.Add(simname);
                            colors.Add(color);
                            var x = meassimlink.ScatterValuesX;
                            var y = meassimlink.ScatterValuesY;
                            if (x.Count > 0 && y.Count > 0)
                            {
                                xdata.AddRange(x);
                                ydata.AddRange(y);
                            }
                            var list = new List<List<double>>();
                            list.Add(x);
                            list.Add(y);
                            biglist.Add(list);



                        }
                        var x_zeros = xdata.Count(x => Math.Abs(x) < 0.000001);
                        var y_zeros = ydata.Count(x => Math.Abs(x) < 0.000001);
                        var all_x_zeros = x_zeros == xdata.Count();
                        var all_y_zeros = y_zeros == ydata.Count();
                        if (!all_x_zeros && !all_y_zeros)
                        {
                            var scatterplot = new RegressionEngine(xdata, ydata);
                            var results = scatterplot.Calculate();
                            if (results != null)
                            {
                                scatterplotmodel.R2 = results.RSquared;
                                scatterplotmodel.RMSE = results.RMSE;
                                if (results.Slope != null && results.Intercept != null)
                                {
                                    scatterplotmodel.Slope = (double)results.Slope;
                                    scatterplotmodel.Intercept = (double)results.Intercept;
                                }
                            }
                        }
                        else
                        {
                            scatterplotmodel.R2 = 0;
                            scatterplotmodel.RMSE = 0;
                            scatterplotmodel.Slope = 0;
                            scatterplotmodel.Intercept = 0;
                        }
                        scatterplotmodel.NamesCsv = string.Join("|", simnames);
                        scatterplotmodel.ColorsCsv = string.Join("|", colors);
                        scatterplotmodel.DataCsv = ConvertDataToCsv(biglist);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private static Dictionary<string, List<MeasSimLinkage>> GroupMeasuredSimLinks(List<MeasSimLinkage> measSimLinks)
        {
            var dictionary = new Dictionary<string, List<MeasSimLinkage>>();
            foreach (var item in measSimLinks)
            {
                if (dictionary.ContainsKey(item.PredColName) == false)
                {
                    dictionary.Add(item.PredColName, new List<MeasSimLinkage>());
                }
                dictionary[item.PredColName].Add(item);
            }
            return dictionary;
        }

        private static List<MeasSimLinkage> ExtractMeasuredSimulatedLinkages(string basePath, List<string> outputfiles)
        {
            var list = new List<MeasSimLinkage>();
            var filename = Path.Combine(basePath, "MeasuredSimulatedPairs.txt");
            if (File.Exists(filename))
            {
                var lines = File.ReadAllLines(filename);
                foreach (var line in lines)
                {
                    var linkage = ExtractLinkage(basePath, outputfiles, line);
                    if (linkage != null)
                    {
                        list.Add(linkage);
                    }
                }
            }
            return list;
        }

        private static MeasSimLinkage ExtractLinkage(string basePath, List<string> outputfiles, string line)
        {
            if (!String.IsNullOrEmpty(line))
            {
                var items = line.Split(",").ToList();
                if (items.Count == 4)
                {
                    var measfilenamestem = items[0].Trim();
                    var measoutputname = items[1].Trim();
                    var simid = items[2].Trim();
                    var simoutputname = items[3].Trim();
                    if (simid != "null" && simoutputname != "null")
                    {
                        var measuredpath = Path.Combine(basePath, "Imported");
                        var measfilename = Path.Combine(measuredpath, measfilenamestem);
                        if (File.Exists(measfilename))
                        {
                            var outputfilename = outputfiles.FirstOrDefault(x => x.Contains($"_Sim{simid}_Daily"));
                            if (outputfilename != null)
                            {
                                var linkage = new MeasSimLinkage(measfilename, measoutputname, simid, outputfilename, simoutputname);
                                if (linkage.ExtractData())
                                {
                                    return linkage;
                                }

                            }
                        }
                    }
                }
            }
            return null;
        }

        private static string ConvertDataToCsv(List<List<List<double>>> biglist)
        {
            var list = new List<string>();
            foreach (var sim in biglist)
            {
                var list2 = new List<string>();
                var count = sim[0].Count;
                for (var i = 0; i < count; ++i)
                {
                    var x = sim[0][i];
                    var y = sim[1][i];
                    list2.Add($"{x:f5},{y:f5}");
                }
                list.Add(string.Join("%", list2));
            }
            return string.Join("|", list);
        }

        private static string GetUniqueColor(int index)
        {
            if (index == 0) return "#0000FF";
            else if (index == 1) return "#FF0000";
            else if (index == 2) return "#008080";
            else if (index == 3) return "#FF00FF";
            else if (index == 4) return "#008000";
            else if (index == 5) return "#4080FF";
            else if (index == 6) return "#0000B0";
            else if (index == 7) return "#808000";
            else if (index == 8) return "#808040";
            else if (index == 9) return "#FF0080";
            else if (index == 10) return "#C08080";
            else if (index == 11) return "#800080";
            else if (index == 12) return "#004080";
            else if (index == 13) return "#A00000";
            else if (index == 14) return "#400040";
            else if (index == 15) return "#00D700";
            else if (index == 16) return "#404080";
            else if (index == 17) return "#B8DE1D";
            else if (index == 18) return "#408080";
            else if (index == 19) return "#353D18";
            else if (index == 20) return "#040257";
            else if (index == 21) return "#000080";
            else if (index == 22) return "#323838";
            else if (index == 23) return "#1B8E9E";
            else if (index == 24) return "#7E5945";
            else if (index == 25) return "#093311";
            else if (index == 26) return "#FF5828";
            else if (index == 27) return "#B923C5";
            else if (index == 28) return "#6868F9";
            else if (index == 29) return "#808080";
            else if (index == 30) return "#00FF00";
            else if (index == 31) return "#3CB371";
            else if (index == 31) return "#7B68EE";
            else if (index == 33) return "#8FBC8B";
            return "Black";
        }

        private static string ExtractSimName(string outputname)
        {
            var items = outputname.Split("_").ToList();
            return items[1];
        }

        private static List<List<double>> ExtractScatterData(PostModel postmodel, Validation_HLKProject projectvm, string filename1, string filename2, int index)
        {
            var list = new List<List<double>>();
            if (File.Exists(filename1) && File.Exists(filename2))
            {
                var lines1 = File.ReadAllLines(filename1).ToList();
                var lines2 = File.ReadAllLines(filename2).ToList();
                if (lines1.Count > 0 && lines1[0] == lines2[0])
                {

                    lines1.RemoveAt(0);
                    lines2.RemoveAt(0);


                    var data1 = ExtractData(lines1, index);
                    var data2 = ExtractData(lines2, index);
                    list.Add(data1);
                    list.Add(data2);

                }
            }
            return list;

        }

        private static List<string> ExtractHeaders(string filename)
        {
            if (File.Exists(filename))
            {
                var lines1 = File.ReadAllLines(filename).ToList();
                if (lines1.Count > 0)
                {
                    var headeritems = lines1[0].Split(",").ToList();
                    headeritems.RemoveAt(0);
                    return headeritems;
                }
            }
            return new List<string>();
        }
        private static List<double> ExtractData(List<string> lines, int index)
        {
            var list = new List<double>();
            foreach (var line in lines)
            {
                var items = line.Split(",").ToList();
                if (index < items.Count)
                {
                    var valuetext = items[index];
                    if (valuetext != "null")
                    {
                        double value;
                        if (double.TryParse(valuetext, out value))
                        {
                            list.Add(value);
                        }
                    }
                }
            }
            return list;
        }

        private static bool SimulationEngineExecute(ProjectHLK project, string outputs, int cores, HowLeakyOutputType outputType, int? targetindex, Action onSuccess = null)
        {
            var controller = new SimulationController();
            var myprogress = new GlobalProgress(project.Simulations.Count);
            myprogress.StartSimulations();
            var tokenSource = new CancellationTokenSource();
            CancellationToken ct = tokenSource.Token;
            project.OutputsCSV = outputs;
            project.AddConsoleOutput("Running simulations... please wait...");
            //var runSimulationsTask = Task.Run(() => Controller.Execute(CurrentProject, MyProgress));
            var taskList = new System.Collections.Generic.List<Task>();
            taskList.Add(Task.Factory.StartNew(delegate
            {
                controller.Execute(project, tokenSource, cores, targetindex, outputType, myprogress);
            }, tokenSource.Token)); // Pass same token to StartNew.
            Task.WaitAll(taskList.ToArray());
            return true;
        }
    }
}
