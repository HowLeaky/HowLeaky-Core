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
        public int Count { get; set; }
        public int Missing { get; set; }
        public int Symbol { get; set; }
        public float Size1 { get; set; }
        public float Size2 { get;set;}
        public string Color1 { get; set; }
        public string Color2 { get; set; }
        public float Slope { get; set; }
        public float Intercept { get; set; }
        public float R2 { get; set; }
        public float RMSE { get; set; }
        public string Title { get; set; }

        public float? MaxY { get;set;}
      


        public string XAxisTitle { get; set; }
        public string YAxisTitle { get; set; }
        public string TimeSeriesName1 { get; set; }
        public string TimeSeriesName2 { get; set; }

        public List<float[]> GetDataArray()
        {
            var list = new List<float[]>();
            for (var i = 0; i < XData.Count; ++i)
            {
                list.Add(new float[2] { (float)XData[i], (float)YData[i] });
            }

            return list;
        }

        public List<float[]> GetSeriesArray1()
        {
            var index = 0;
            var list = new List<float[]>();
            for (var i = 0; i < XData.Count; ++i)
            {
                list.Add(new float[2] { StartDate.AddDays(i).DateInt, (float)XData[i] });
                ++index;
            }

            return list;
        }

        public List<float[]> GetSeriesArray2()
        {
            var index=0;
            var list = new List<float[]>();
            for (var i = 0; i < YData.Count; ++i)
            {
                list.Add(new float[2] { StartDate.AddDays(i).DateInt, (float)YData[i] });
                ++index;
            }

            return list;
        }

        public void MakeCumulative()
        {
            var xdata = new List<double>();
            var ydata = new List<double>();
            var sum1 = 0.0;
            foreach (var data in XData)
            {
                sum1 += data;
                xdata.Add(sum1);
            }
            var sum2 = 0.0;
            foreach (var data in YData)
            {
                sum2 += data;
                ydata.Add(sum2);
            }
            XData=xdata;
            YData=ydata;
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
        public PairedTimeSeries Generate(int dataformat, BrowserDate datastart, BrowserDate dataend,int? jstart, int? jend, bool infill)
        {
            try
            {
                var pair = new PairedTimeSeries();
                var start1 = TimeSeries1.StartDate;
                var end1 = TimeSeries1.EndDate;
                var values1 = TimeSeries1.DailyValues;

                var start2 = TimeSeries2.StartDate;
                var end2 = TimeSeries2.EndDate;
                var values2 = TimeSeries2.DailyValues;

                var mindate = start1.DateInt < start2.DateInt ? start1 : start2;
                var maxdate = end1.DateInt > end2.DateInt ? end1 : end2;
                if (datastart != null)
                {
                    mindate = datastart.DateInt > mindate.DateInt ? datastart : mindate;
                }
                if (dataend != null)
                {
                    maxdate = datastart.DateInt < maxdate.DateInt ? dataend : maxdate;
                }

                pair.StartDate = mindate;
                pair.EndDate = maxdate;
                                    
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
                           
                            foundfirst = check1 && check2 ;
                            pair.StartDate = date;
                        }
                        if (foundfirst)
                        {
                            var check1 = index <= end1.DateInt;
                            var check2 = index <= end2.DateInt;
                            
                            var cancontinue = check1 && check2 ;

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
                                pair.EndDate = date;
                                index = maxdate.DateInt + 1;
                            }
                        }
                    }

                 if (dataformat==1)
                {
                    var xvalues=new List<double>();
                    var yvalues=new List<double>();
                    var startdate=pair.StartDate;
                    var currentmonth=startdate.Month;
                    var sum1 = 0.0;
                    var sum2 = 0.0;
                    var count=0;
                    for(var i=0;i<pair.XData.Count;++i)
                    {
                        var date=startdate.AddDays(i);
                        var month=date.Month;
                        var x=pair.XData[i];
                        var y=pair.YData[i];
                        if(month==currentmonth)
                        {
                            if(Math.Abs(x - 32768)>0.00001)
                            { 
                                sum1+=x;
                            }
                            if (Math.Abs(y - 32768) > 0.00001)
                            {
                                sum2 +=y;
                            }
                            ++count;
                        }
                        else
                        {
                            xvalues.Add(sum1);
                            yvalues.Add(sum2);
                            currentmonth=month;
                            sum1=0;
                            sum2=0;
                            count = 1;
                            if (Math.Abs(x - 32768) > 0.00001)
                            {
                                sum1 = x;
                            }
                            if (Math.Abs(y - 32768) > 0.00001)
                            {
                                sum2 = y;
                            }
                        }                        
                    }
                    pair.XData=xvalues;
                    pair.YData=yvalues;
                    pair.Count=pair.XData.Count;
                }
                else if (dataformat == 2)
                {
                    var xvalues = new List<double>();
                    var yvalues = new List<double>();
                    var startdate = pair.StartDate;
                    var currentyear = startdate.Year;
                    var sum1 = 0.0;
                    var sum2 = 0.0;
                    var count = 0;
                    for (var i = 0; i < pair.XData.Count; ++i)
                    {
                        var date = startdate.AddDays(i);
                        var year = date.Year;
                        var x = pair.XData[i];
                        var y = pair.YData[i];
                        if (year == currentyear)
                        {
                            if (Math.Abs(x - 32768) > 0.00001)
                            {
                                sum1 += x;
                            }
                            if (Math.Abs(y - 32768) > 0.00001)
                            {
                                sum2 += y;
                            }
                            ++count;
                        }
                        else
                        {
                            xvalues.Add(sum1);
                            yvalues.Add(sum2);
                            currentyear = year;
                            sum1 = 0;
                            sum2 = 0;
                            count = 1;
                            if (Math.Abs(x - 32768) > 0.00001)
                            {
                                sum1 = x;
                            }
                            if (Math.Abs(y - 32768) > 0.00001)
                            {
                                sum2 = y;
                            }
                        }
                    }
                    pair.XData = xvalues;
                    pair.YData = yvalues;
                    pair.Count = pair.XData.Count;
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
