using HowLeaky_SimulationEngine.Engine;
using HowLeaky_IO.Outputs;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HowLeaky_IO
{
    public enum HowLeakyOutputType
    {
        DailyBin,
        DailyCsv,
        MonthlyCsv,
        YearlyCsv,
        DailyAndMonthlyCsv

    }
    public class SimulationThread
    {
        public HowLeakyOutputType OutputType { get; set; }
        public ProjectHLK Project { get; set; }
     
        public HowLeakyEngine HLEngine { get; set; }
        public List<Simulation> Simulations { get; set; }


        public SimulationThread(ProjectHLK project, KeyValuePair<string, List<Simulation>> pair, HowLeakyOutputType outputType = HowLeakyOutputType.DailyBin)
        {
            var remapdict = BuildOutputsRemapDict(project.OutputsCSV.Split(',').ToList());
            OutputType = outputType;
      
            HLEngine = new HowLeakyEngine(project.OutputsCSV, remapdict);
            Simulations = pair.Value;
            Project = project;
        }

        public SimulationThread(ProjectHLK project, List<Simulation> simulations, HowLeakyOutputType outputType = HowLeakyOutputType.DailyBin)
        {
            var remapdict = BuildOutputsRemapDict(project.OutputsCSV.Split(',').ToList());
            OutputType = outputType;
        
            HLEngine = new HowLeakyEngine(project.OutputsCSV, remapdict);
            Simulations = simulations;
            Project = project;
        }

        private Dictionary<string, HowLeaky_SimulationEngine.Outputs.Definitions.OutputAttributes> BuildOutputsRemapDict(List<string> outputs)
        {

            var orderindex = 0;
            var dict = new Dictionary<string, HowLeaky_SimulationEngine.Outputs.Definitions.OutputAttributes>();
            foreach (var output in outputs)
            {
                var temp = output.ToLower();
                if (temp.Contains("temp") || temp.Contains("conc"))
                {
                    dict.Add(output, new HowLeaky_SimulationEngine.Outputs.Definitions.OutputAttributes(output, null, output, "#000000", 1, orderindex++, false));
                }
                else
                {
                    dict.Add(output, new HowLeaky_SimulationEngine.Outputs.Definitions.OutputAttributes(output, null, output, "#000000", 1, orderindex++, true)); ;
                }

            }
            return dict;

        }



        public void Execute(GlobalProgress progress, CancellationToken ct)
        {
            try
            {

                var path = Project.OutputsDirectory;
        
                foreach (var sim in Simulations)
                {
                    if (ct.IsCancellationRequested)
                    {
                        ct.ThrowIfCancellationRequested();
                    }
                    if (!sim.OutputExists())
                    {
                        var inputs = sim.GenerateInputs();
                        if (inputs != null)
                        {
                            HLEngine.Execute(inputs, (outputs) =>
                            {

                                //SQLite.UpdateDataTable(sim,outputs);
                                if (outputs == null && outputs.TimeSeries.Count == 0)
                                {
                                    Project.AddErrorOutput("No outputs generated in simulation");
                                }
                                else
                                {
                                    Project.CheckZeroArrays(sim.GenerateOutputName(), outputs);
                                    switch (OutputType)
                                    {
                                        case HowLeakyOutputType.DailyBin: HowLeakyBinOutput.WriteOutputs(path, sim, outputs); break;
                                        case HowLeakyOutputType.DailyCsv: HowLeakyCsvOutput.WriteDailyOutputs(path, sim, outputs); break;
                                        case HowLeakyOutputType.MonthlyCsv: HowLeakyCsvOutput.WriteMonthlyOutputs(path, sim, outputs); break;
                                        case HowLeakyOutputType.YearlyCsv: HowLeakyCsvOutput.WriteYearlyOutputs(path, sim, outputs); break;
                                        case HowLeakyOutputType.DailyAndMonthlyCsv: HowLeakyCsvOutput.WriteDailyOutputs(path, sim, outputs); HowLeakyCsvOutput.WriteMonthlyOutputs(path, sim, outputs); break;
                                    }
                                }
                                sim.Reset();
                            }, (exception) =>
                            {
                                Project.AddErrorOutput(exception.ToString());
                            });
                        }
                        else
                        {
                            foreach (var error in sim.Errors)
                            {
                                Project.AddErrorOutput(error);
                            }
                        }
                    }

                    progress.Increment();
                }
                ////SQLite.WriteToTable();
            }
            catch (Exception ex)
            {
                Project.AddErrorOutput(ex.ToString());
            }
            finally
            {
                //if(SQLite!=null)
                //{
                //    SQLite.CloseConnection();
                //}
            }
        }

        //private string GenerateOutputCSV()
        //{
        //    var list= List<string>();
        //    foreach(var sim in Simulations)
        //    {
        //        foreach(var ouutput in )
        //    }



        //}


    }
}