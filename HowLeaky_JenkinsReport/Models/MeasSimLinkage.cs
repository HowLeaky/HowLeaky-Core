using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace HowLeaky_ValidationEngine.Models
{
    class MeasSimLinkage
    {


        public MeasSimLinkage(string measfilename, string measoutputname, string simid,string outputfilename, string simoutputname)
        {
            SimId=simid;
            MeasFileName = measfilename;
            MeasColName = measoutputname;
            PredFileName = outputfilename;
            PredColName = simoutputname;
        }



        public string Title()
        {
            var name1=Path.GetFileNameWithoutExtension(MeasFileName);
            return$"{name1} {MeasColName} vs Sim {SimId} {PredColName}"; 
        }

        public string SimId { get;set;}
        public string MeasFileName { get; set; }
        public string MeasColName { get; set; }

        public string PredFileName { get; set; }
        public string PredColName { get; set; }



        public List<double> MeasCumValues { get; set; }
        public List<BrowserDate> Dates { get; set; }


        public List<double> PredCumValues { get; set; }

        public BrowserDate StartDate { get; set; }
        public BrowserDate EndDate { get; set; }

        public List<double> ScatterValuesX { get; set; }
        public List<double> ScatterValuesY { get; set; }



        internal bool ExtractData()
        {
            try
            {
                var measdata = ExtractTimeSeries(MeasFileName, MeasColName);
                var preddata = ExtractTimeSeries(PredFileName, PredColName);
                if (measdata != null & preddata != null && measdata.Dates.Count > 0 && preddata.Dates.Count > 0)
                {
                    return ExecuteExtractData(measdata, preddata);                    
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }


        public bool ExecuteExtractData(TimeSeries meas, TimeSeries pred)
        {
            try
            {
                var measDateInts = meas.Dates.OrderBy(x => x.DateInt).Select(x => x.DateInt).ToList();
                var predDateInts = pred.Dates.Select(x => x.DateInt).ToList();
                var measdaily = meas.Values;
                var preddaily = pred.Values;
                Dates = new List<BrowserDate>();

                MeasCumValues = new List<double>();
                PredCumValues = new List<double>();
                ScatterValuesX = new List<double>();
                ScatterValuesY = new List<double>();

                StartDate = new BrowserDate(measDateInts.First());
                EndDate = new BrowserDate(measDateInts.Last());
                double sumMeas = 0;
                double sumPred = 0;
                var countmeas=0;
                var countpred=0;
                for (var i = StartDate.DateInt; i <= EndDate.DateInt; ++i)
                {
                    double? measValue;
                    double? predValue;
                    var date = new BrowserDate(i);

                    var measindex = measDateInts.IndexOf(date.DateInt);
                    if (measindex >= 0 && measindex < preddaily.Count)
                    {
                        measValue = measdaily[measindex];
                        sumMeas += (double)measValue;
                        ++countmeas;
                    }
                    else
                    {
                        measValue = (double?)null;
                    }
                    var predindex = predDateInts.IndexOf(date.DateInt);
                    if (predindex >= 0&&predindex<preddaily.Count)
                    {
                        predValue = preddaily[predindex];
                        sumPred += (double)predValue;
                        ++countpred;
                        
                    }
                    else
                    {
                        predValue = (double?)null;
                        if (measValue != null)
                        {
                            var lastpreddate=pred.Dates.Last();
                            Debug.WriteLine(lastpreddate.ToString("dd/MM/yyyy"));
                        }
                    }

                    Dates.Add(date);
                    MeasCumValues.Add(sumMeas);
                    PredCumValues.Add(sumPred);
                    if (measValue != null && predValue != null)
                    {
                        ScatterValuesX.Add((double)measValue);
                        ScatterValuesY.Add((double)predValue);
                        //ScatterValues.Add(new List<double>(new double[2] { (double)measValue, (double)predValue }));
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

            }

            return false;



        }


        public TimeSeries ExtractTimeSeries(string filename, string colname)
        {
            var lines = File.ReadAllLines(filename).ToList();
            int? firstline = null;
            string datepattern = "";
            var lineno = -1;
            int colIndex = -1;
            TimeSeries timeseries = null;

            foreach (var line in lines)
            {
                ++lineno;
                var dellist = line.Split(new[] { '\t', ',' }, StringSplitOptions.None).Select(x => x.Trim()).ToList();
                if (firstline != null && dellist.Count > 0)
                {
                    var datetext = dellist[0].Trim();
                    if (!String.IsNullOrEmpty(datetext))
                    {
                        BrowserDate date = null;
                        try
                        {
                            date = new BrowserDate(datetext, datepattern);
                        }
                        catch
                        {
                            foreach (var format in formats)
                            {
                                DateTime dateValue;
                                if (DateTime.TryParseExact(datetext, format,
                                                   CultureInfo.InvariantCulture,
                                                   DateTimeStyles.None,
                                                   out dateValue))
                                {
                                    date = new BrowserDate(dateValue);
                                }

                            }
                        }

                        double value;
                        if (colIndex < dellist.Count)
                        {
                            if (Double.TryParse(dellist[colIndex], out value))
                            {
                                timeseries.Values.Add(value);
                                timeseries.Dates.Add(date);
                            }
                            else
                            {
                                timeseries.Values.Add(null);
                                timeseries.Dates.Add(date);
                            }
                        }
                        else
                        {
                            timeseries.Values.Add(null);
                            timeseries.Dates.Add(date);
                        }

                    }

                }
                else if (dellist[0].ToLower() == "date" || dellist[0].Trim().ToLower().Replace("/", "").Trim() == "date")
                {
                    firstline = lineno + 1;
                    var count = dellist.Count;
                    datepattern = DetectDatePattern(lines, (int)firstline);
                    for (var i = 1; i < count; ++i)
                    {
                        if (dellist[i].ToLower().Trim() == colname.ToLower())
                        {
                            colIndex = i;
                            timeseries = new TimeSeries();
                            break;
                        }
                    }


                }


            }
            return timeseries;

        }
        public string[] formats = {"M/d/yyyy", "MM/dd/yyyy",
                            "d/M/yyyy", "dd/MM/yyyy",
                            "yyyy/M/d", "yyyy/MM/dd",
                            "yyyyMMdd",
                            "M/d/yy", "MM/dd/yy",
                            "d/M/yy", "dd/MM/yy",
                            "yy/M/d", "yy/MM/dd",
                            "yyMMdd",
                            "M-d-yyyy", "MM-dd-yyyy",
                            "d-M-yyyy", "dd-MM-yyyy",
                            "yyyy-M-d", "yyyy-MM-dd",
                            "M-d-yy", "MM-dd-yy",
                            "d-M-yy", "dd-MM-yy",
                            "yy-M-d", "yy-MM-dd",
                            "M.d.yyyy", "MM.dd.yyyy",
                            "d.M.yyyy", "dd.MM.yyyy",
                            "yyyy.M.d", "yyyy.MM.dd",
                            "M d yyyy", "MM dd yyyy",
                            "d M yyyy", "dd MM yyyy",
                            "yyyy M d", "yyyy MM dd"
                           };

        public string DetectDatePattern(List<string> lines, int first)
        {


            var dict = new Dictionary<string, int>();
            foreach (var format in formats)
            {
                dict.Add(format, 0);
            }
            DateTime dateValue;
            for (var i = first; i < 100; ++i)
            {
                if (i < lines.Count)
                {
                    var line = lines[i];
                    var dellist = line.Split(new[] { '\t', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
                    if (dellist.Count > 0)
                    {
                        for (var j = 0; j < formats.Count(); ++j)
                        {
                            var dateStringFormat = formats[j];
                            if (DateTime.TryParseExact(dellist[0], dateStringFormat,
                                                       CultureInfo.InvariantCulture,
                                                       DateTimeStyles.None,
                                                       out dateValue))
                            {
                                dict[dateStringFormat]++;
                            }
                        }
                    }
                }
            }
            var selected = dict.OrderBy(x => x.Value).Select(x => x.Key).LastOrDefault();
            return selected;
        }

       
    }
}



