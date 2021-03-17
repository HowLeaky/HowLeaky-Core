using HowLeaky_ValidationEngine.Models.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HowLeaky_ValidationEngine.Models
{
    public class PostModel
    {
        public PostModel()
        {
            Projects = new List<Validation_HLKProject>();
            CumulativePlots = new List<Validation_CumulativeChart>();
            ScatterPlots = new List<Validation_ScatterChart>();
            Report=new Validation_Report()
            {
                Id=Guid.NewGuid(),
                Date=DateTime.UtcNow
            };       
        }

        public Validation_Report Report { get; set; }
        public List<Validation_HLKProject> Projects { get; set; }
        public List<Validation_CumulativeChart> CumulativePlots { get; set; }
        public List<Validation_ScatterChart> ScatterPlots { get; set; }

        public Validation_HLKProject CreateProjectModel(string projectname, HashSet<string> errorOutputList)
        {
            var project=new Validation_HLKProject()
            {
                Id=Guid.NewGuid(),
                ReportID=Report.Id,
                ProjectFileName=Path.GetFileName(projectname)

            };
            project.SimulationErrors=String.Join("\n",errorOutputList.ToList());
            Projects.Add(project);
            ++Report.ProjectsCount;
            return project;
        }

        public Validation_ScatterChart CreateScatterPlotModel(Validation_HLKProject projectvm, string item, ScatterType type)
        {
            var scatterplot=new Validation_ScatterChart()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectvm.Id,
                Title = item,
                Type=type
            };
            ScatterPlots.Add(scatterplot);
            return scatterplot;


        }

        public Validation_CumulativeChart CreateCumulativePlotModel(Validation_HLKProject projectvm, string item)
        {
            var cumulativeplot = new Validation_CumulativeChart()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectvm.Id,
                Title = item
                
            };
            CumulativePlots.Add(cumulativeplot);
            return cumulativeplot;


        }
    }
}
