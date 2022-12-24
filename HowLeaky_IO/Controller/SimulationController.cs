using Figgle;
using HowLeaky_SimulationEngine.Errors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace HowLeaky_IO
{
    public class SimulationController
    {
        public GlobalProgress Progress{get;set;}
        public string InputsPath{get;set; }
        public string OutputsPath{get;set; }
  
        public SimulationController()
        {
           
     
        }

        public void Execute( ProjectHLK project, CancellationTokenSource tokenSource,  int cores, int?targetindex, HowLeakyOutputType outputType,GlobalProgress progress=null)
        {
            try
            {
                if(progress==null)
                {
                    Progress=new GlobalProgress(project.Simulations.Count);
                }
                else
                {
                    Progress=progress;
                }
            
           
                CancellationToken ct = tokenSource.Token;
                var simdict = project.GroupedSimulations;
            
                ct.ThrowIfCancellationRequested();
                Progress.Max=targetindex==null?project.Simulations.Count:1;
                project.PrepareReferenceCounts();
                if(project.GroupedSimulations.Count()>1)//cores)
                {

                    Parallel.ForEach(simdict.ToList(), new ParallelOptions() { MaxDegreeOfParallelism = cores }, simkeypair =>
                   {
                        var worker=new SimulationThread(project, simkeypair, outputType);
                       if (ct.IsCancellationRequested)
                       {
                            ct.ThrowIfCancellationRequested();
                       }                   
                        worker.Execute(Progress, ct);

                       Debug.WriteLine("");
                    });
               
                }
                else
                {
                    var climatedatasets=project.DataFiles.ToList();
                    foreach(var datafile in climatedatasets)
                    {
                        if(!datafile.OpenFull())
                        {
                            throw new Exception($"Climate file {datafile} could not be openned");
                        }
                    }
                
                    var simulations=targetindex==null?project.Simulations:new List<Simulation>(){project.Simulations.FirstOrDefault(x=>x.Index==(int)targetindex)};

                    Parallel.ForEach(simulations,  new ParallelOptions() { MaxDegreeOfParallelism = cores }, sim =>
                    {
                        var worker=new SimulationThread(project, new List<Simulation>(){sim }, outputType);             
                        if (ct.IsCancellationRequested)
                        {                     
                            ct.ThrowIfCancellationRequested();
                        }                
                         worker.Execute(Progress, ct);   
                    
                    });
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

        }

    }
}
