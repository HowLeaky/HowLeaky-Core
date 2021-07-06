using HowLeaky_SimulationEngine.Outputs;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_IO.Tools
{

    public class PairedTimeSeries
    {
        public PairedTimeSeries()
        {
            XData = new List<double>();
            YData = new List<double>();

        }

        public List<double> XData { get; set; }
        public List<double> YData { get; set; }
        public BrowserDate StartDate { get; set; }
        public BrowserDate EndDate { get; set; }

        public int Symbol { get;set;}
        public float Size { get;set;}
        public string Color1 { get;set;}
        public string Color2 { get;set;}
        public float Slope { get;set;}
        public float Intercept { get;set;}
        public float R2 { get;set;}
        public float RMSE { get;set;}
        public string Title { get;set;}

        public string XAxisTitle { get;set;}
        public string YAxisTitle { get; set; }

        public List<float[]> GetDataArray()
        {
            var list=new List<float[] >();
            for(var i=0;i<XData.Count;++i)
            {
                list.Add(new float[2]{ (float)XData[i], (float)YData[i] });
            }

            return list;
        }
    }
    public class PairedTimeSeriesEngine
    {
        public HowLeakyOutputTimeSeries TimeSeries1 { get; set; }
        public HowLeakyOutputTimeSeries TimeSeries2 { get; set; }

        public PairedTimeSeriesEngine(HowLeakyOutputTimeSeries timeseries1, HowLeakyOutputTimeSeries timeseries2)
        {
            TimeSeries1 = timeseries1;
            TimeSeries2 = timeseries2;
        }
        public PairedTimeSeries Generate(int dataformat, int? startyear, int? endyear, int? jstart, int? jend, bool infill)
        {
            try
            {
                var pair = new PairedTimeSeries();

                if (dataformat == 0)
                {
                    var start1 = TimeSeries1.StartDate;
                    var end1 = TimeSeries1.EndDate;
                    var values1 = TimeSeries1.DailyValues;

                    var start2 = TimeSeries2.StartDate;
                    var end2 = TimeSeries2.EndDate;
                    var values2 = TimeSeries2.DailyValues;

                    var mindate = start1.DateInt < start2.DateInt ? start1 : start2;
                    var maxdate = end1.DateInt > start2.DateInt ? end1 : end2;

                    bool foundfirst = false;
                    
                    for (var index = mindate.DateInt; index <= maxdate.DateInt; ++index)
                    {
                        var date = new BrowserDate(index);
                        var index1 = index - start1.DateInt;
                        var index2 = index - start2.DateInt;
                        if (!foundfirst)
                        {
                            var check1 = index >= start1.DateInt;
                            var check2 = index >= start2.DateInt;
                            var check3 = startyear == null || date.Year >= ((int)startyear);
                            foundfirst = check1 && check2 && check3;
                            pair.StartDate = date;
                        }
                        if (foundfirst)
                        {
                            var check1 = index <= end1.DateInt;
                            var check2 = index <= end2.DateInt;
                            var check3 = endyear == null || date.Year <= ((int)endyear);
                            var cancontinue = check1 && check2 && check3;

                            if (cancontinue)
                            {
                                var assigned = false;
                                var value1 = index1 < values1.Count ? values1[index1] : null;
                                var value2 = index2 < values2.Count ? values2[index2] : null;
                                if (jstart != null && jend != null)
                                {
                                    var jday = date.GetJDay();
                                    if ((int)jstart < (int)jend)
                                    {
                                        if (jday >= jstart && jday <= jend)
                                        {
                                            if (value1 != null && value2 != null)
                                            {
                                                pair.XData.Add((double)value1);
                                                pair.YData.Add((double)value2);
                                                assigned = true;
                                            }

                                        }

                                    }
                                    else
                                    {
                                        if (jday >= jend && jday <= jstart)
                                        {
                                            if (value1 != null && value2 != null)
                                            {
                                                pair.XData.Add((double)value1);
                                                pair.YData.Add((double)value2);
                                                assigned = true;
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    if (value1 != null && value2 != null)
                                    {
                                        pair.XData.Add((double)value1);
                                        pair.YData.Add((double)value2);
                                        assigned = true;
                                    }
                                }
                                if (!assigned && infill)
                                {
                                    if (value1 != null)
                                        pair.XData.Add((double)value1);
                                    else
                                        pair.XData.Add(0);
                                    if (value2 != null)
                                        pair.YData.Add((double)value2);
                                    else
                                        pair.YData.Add(0);
                                }
                            }
                            else
                            {
                                pair.EndDate=date;
                                index = maxdate.DateInt+1;
                            }
                        }
                    }

                }
                return pair;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}
