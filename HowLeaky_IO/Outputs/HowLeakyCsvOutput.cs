using HowLeaky_SimulationEngine.Errors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HowLeaky_IO.Outputs
{
    public class HowLeakyCsvOutput
    {


        static public bool WriteDailyOutputs(string path, Simulation sim, HowLeaky_SimulationEngine.Outputs.HowLeakyOutputs outputs)
        {
            if (System.IO.Directory.Exists(path))
            {
                var headers=outputs.TimeSeries.Select(x=>x.OutputDefn.Name).ToList();
                headers.Insert(0, "Date");
                var filename = System.IO.Path.Combine(path, $"{sim.GenerateOutputName()}_Daily.csv");
                int count = 0;
                var list = new List<List<double?>>();
                foreach (var timeseries in outputs.TimeSeries)
                {
                    var values = timeseries.DailyValues;
                    if (count == 0)
                    {
                        count = values.Count; ;
                    }
                    list.Add(values);
                }
                if(count==0)
                {
                    throw new Exception("No data in outputs");
                }
                return WriteOutputs(filename,  outputs.StartDate, outputs.EndDate, 1, headers,count, list);
            }
            return false;
        }
        static public bool WriteMonthlyOutputs(string path, Simulation sim, HowLeaky_SimulationEngine.Outputs.HowLeakyOutputs outputs)
        {
            if (System.IO.Directory.Exists(path))
            {
                var headers = outputs.TimeSeries.Select(x => x.OutputDefn.Name).ToList();
                headers.Insert(0, "Date");
                var filename = System.IO.Path.Combine(path, $"{sim.GenerateOutputName()}_Monthly.csv");
                int count = 0;
                var list = new List<List<double?>>();
                foreach (var timeseries in outputs.TimeSeries)
                {
                    var values = timeseries.MonthlyValues();
                    if (count == 0)
                    {
                        count = values.Count; ;
                    }
                    list.Add(values);
                }
                return WriteOutputs(filename, outputs.StartDate, outputs.EndDate, 2, headers,count, list);
            }
            return false;
        }
        static public bool WriteYearlyOutputs(string path, Simulation sim, HowLeaky_SimulationEngine.Outputs.HowLeakyOutputs outputs)
        {
            if (System.IO.Directory.Exists(path))
            {
                var headers = outputs.TimeSeries.Select(x => x.OutputDefn.Name).ToList();
                headers.Insert(0, "Date");
                var filename = System.IO.Path.Combine(path,$"{sim.GenerateOutputName()}_Yearly.csv");
                int count = 0;
                var list = new List<List<double?>>();
                foreach (var timeseries in outputs.TimeSeries)
                {
                    var values = timeseries.YearlyValues();
                    if (count == 0)
                    {
                        count = values.Count; ;
                    }
                    list.Add(values);
                }
                return WriteOutputs(filename,  outputs.StartDate, outputs.EndDate,3, headers,count, list);
            }
            return false;
        }

        static public bool WriteOutputs(string filename, HowLeaky_SimulationEngine.Tools.BrowserDate startDate, HowLeaky_SimulationEngine.Tools.BrowserDate endDate,int format, List<string>headers,int count, List<List<double?>> values)
        {
            try
            {

                using (var w = new System.IO.StreamWriter(filename))
                {
                    w.WriteLine(string.Join(",", headers));
                    w.Flush();
                    for (var row = 0; row < count; ++row)
                    {
                        var list = new List<string>();
                        if (format==1)
                        {
                            var date = startDate.AddDays(row);
                            list.Add(date.ToString("dd/MM/yyyy"));
                        }
                        else if (format==2)
                        {
                            var date = new HowLeaky_SimulationEngine.Tools.BrowserDate(startDate.DateInt).AddMonths(row);                            
                            var lastday = HowLeaky_SimulationEngine.Tools.BrowserDate.LastDayOfMonth(date.Year, date.Month);
                            date = new HowLeaky_SimulationEngine.Tools.BrowserDate(date.Year, date.Month, lastday);
                            list.Add(date.ToString("MM/yyyy"));
                        }
                        else if(format==3)
                        {
                            var date = new HowLeaky_SimulationEngine.Tools.BrowserDate(startDate.DateInt).AddYears(row);
                            list.Add(date.ToString("yyyy"));
                        }
                        
                        foreach (var array in values)
                        {
                            list.Add($"{array[row]:F5}");
                        }
                        w.WriteLine(string.Join(",", list));
                        w.Flush();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

            //return false;
        }
    }
}
