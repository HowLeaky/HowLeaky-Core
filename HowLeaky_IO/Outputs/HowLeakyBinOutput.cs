using HowLeaky_SimulationEngine.Errors;
using HowLeaky_SimulationEngine.Outputs;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace HowLeaky_IO.Outputs
{
    public class HowLeakyBinOutput
    {
        public HowLeakyBinOutput()
        {
            
        }

        static public bool WriteOutputs(string path, Simulation sim, HowLeakyOutputs outputs)
        {
            try
            {
                if(Directory.Exists(path))
                {
                    var filename=sim.GenerateBinaryPathName();
                    
                    using (FileStream fileStream = new FileStream(filename, FileMode.Create)) // destiny file directory.
                    {
                        using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                        {
                            binaryWriter.Write(sim.Index);
                            binaryWriter.Write(outputs.TimeSeries.Count);
                            binaryWriter.Write(outputs.StartDate.DateInt);
                            binaryWriter.Write(outputs.EndDate.DateInt);
                            
                            foreach(var output in outputs.TimeSeries)
                            {
                                 binaryWriter.Write(output.OutputDefn.Name);
                                foreach(var data in output.DailyValues)
                                {
                                    binaryWriter.Write(data.Value);
                                }
                            }                   
                        }
                    }
                    return true;
                }
            }
            catch(Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

            return false;
        }

        

        static public List<TimeSeriesViewModel> ReadOutputs(string path)
        {
            try
            {
                if(File.Exists(path))
                {
                    var list=new List<TimeSeriesViewModel>();
                    using (FileStream fileStream = new FileStream(path, FileMode.Open)) // destiny file directory.
                    {
                        
                        using (BinaryReader binaryReader = new BinaryReader(fileStream))
                        {
                            binaryReader.BaseStream.Position = 0;
                            var simindex=binaryReader.ReadInt32();
                            var timeseriescount=binaryReader.ReadInt32();
                            var start=new BrowserDate(binaryReader.ReadInt32());
                            var  end=new BrowserDate(binaryReader.ReadInt32());
                            var datacount=end.DateInt-start.DateInt+1;
                            var outputs=new HowLeakyOutputs(start,end);
                            var timeserieslist=outputs.TimeSeries;
                            for(var index1=0;index1<timeseriescount;++index1)
                            {
                                var name=binaryReader.ReadString();
                                var vm=new TimeSeriesViewModel(simindex,name,start,end);
                                for(var i=0;i<datacount;++i)
                                {
                                    vm.Values[i]=binaryReader.ReadDouble();
                                }
                                list.Add(vm);
                            }
                           
                   
                        }
                    }
                    return list;
                }
            }
            catch(Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

            return new List<TimeSeriesViewModel>();
        }

        static public List<HowLeakyOutputTimeSeries> ReadOutputs2(string path)
        {
            try
            {
                if(File.Exists(path))
                {
                    var list=new List<HowLeakyOutputTimeSeries>();
                    using (FileStream fileStream = new FileStream(path, FileMode.Open)) // destiny file directory.
                    {
                        
                        using (BinaryReader binaryReader = new BinaryReader(fileStream))
                        {
                            binaryReader.BaseStream.Position = 0;
                            var simindex=binaryReader.ReadInt32();
                            var timeseriescount=binaryReader.ReadInt32();
                            var start=new BrowserDate(binaryReader.ReadInt32());
                            var  end=new BrowserDate(binaryReader.ReadInt32());
                            var datacount=end.DateInt-start.DateInt+1;
                         
                            for(var index1=0;index1<timeseriescount;++index1)
                            {
                                var name=binaryReader.ReadString();
                                var vm=new HowLeakyOutputTimeSeries(simindex,name,start,end,datacount,true);
                                for(var i=0;i<datacount;++i)
                                {
                                    vm.DailyValues[i]=binaryReader.ReadDouble();
                                }
                                list.Add(vm);
                            }
                           
                   
                        }
                    }
                    return list;
                }
            }
            catch(Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }

            return new List<HowLeakyOutputTimeSeries>();
        }


        //static public List<DygraphViewModel> ReadDygraphOutputs(string path, int dataformat, bool multichart)
        //{
        //    try
        //    {                
        //        if(File.Exists(path))
        //        {
        //            var ts=new List<DygraphViewModel>();
                    
        //            using (FileStream fileStream = new FileStream(path, FileMode.Open)) // destiny file directory.
        //            {
                        
        //                using (BinaryReader binaryReader = new BinaryReader(fileStream))
        //                {
        //                    binaryReader.BaseStream.Position = 0;
        //                    var simindex=binaryReader.ReadInt32();
        //                    var timeseriescount=binaryReader.ReadInt32();
        //                    var start=new BrowserDate(binaryReader.ReadInt32());
        //                    var  end=new BrowserDate(binaryReader.ReadInt32());
        //                    var datacount=end.DateInt-start.DateInt+1;
        //                    double?min=null;
        //                    double?max=null;
        //                    var chartcount=multichart?timeseriescount:1;
        //                    var timeseriescountperchart=multichart?1:timeseriescount;
        //                    var first=start.GetDateTime();
        //                    for(var i=0;i<chartcount;++i)
        //                    {
        //                        if(dataformat==0)
        //                        {
                                    
        //                            ts.Add(new DygraphViewModel(timeseriescountperchart,datacount));
        //                        }
        //                        else if(dataformat==1)
        //                        {
        //                            var startmonth=start.Month;
        //                            var endmonth=end.Month;
        //                            var startyear=start.Year;
        //                            var endyear=end.Year;
        //                            var datacount2=(12-startmonth+1)+endmonth+12*(endyear-startyear-1);
        //                            ts.Add(new DygraphViewModel(timeseriescountperchart,datacount2));
        //                        }
        //                        else if(dataformat==2)
        //                        {                                    
        //                            var startyear=start.Year;
        //                            var endyear=end.Year;
        //                            var datacount2=endyear-startyear+1;
        //                            ts.Add(new DygraphViewModel(timeseriescountperchart,datacount2));
        //                        }
        //                        ts[i].Labels[0]="date";                                                                                                                                                                                                     
        //                    }
        //                    if(dataformat==0)//daily
        //                    {
        //                        for(var index1=0;index1<timeseriescount;++index1)
        //                        {     
        //                            var timeseriesindex=multichart?0:index1;
        //                            var chartindex=multichart?index1:0;

        //                            ts[chartindex].Labels[timeseriesindex+1]=binaryReader.ReadString();
        //                            ts[chartindex].Colors[timeseriesindex]="#0000FF";
        //                            for(var i=0;i<datacount;++i)
        //                            {
        //                                var value=binaryReader.ReadDouble();
        //                                if(min==null||value<min)
        //                                {
        //                                    min=value;
        //                                }
        //                                if(max==null||value>max)
        //                                {
        //                                    max=value;
        //                                }
        //                                ts[chartindex].Data[i,timeseriesindex+1]=value;
        //                                if(timeseriesindex==0)
        //                                {
        //                                    ts[chartindex].Data[i,0]= Convert(first.AddDays(i));
        //                                }
        //                            }
                                
        //                        }
        //                    }                            
        //                    else if(dataformat==1)//monthly
        //                    {                                                               
        //                        for(var index1=0;index1<timeseriescount;++index1)
        //                        {     
        //                            var timeseriesindex=multichart?0:index1;
        //                            var chartindex=multichart?index1:0;

        //                            ts[chartindex].Labels[timeseriesindex+1]=binaryReader.ReadString();
        //                            ts[chartindex].Colors[timeseriesindex]="#0000FF";
        //                            var lastmonth=start.Month;
        //                            var sum=0.0;
        //                            var dateindex=0;
        //                            for(var i=0;i<datacount;++i)
        //                            {
        //                                var date=start.AddDays(i);
        //                                if(date.Month==lastmonth&&i<datacount-1)
        //                                {
        //                                    sum += binaryReader.ReadDouble();
        //                                }
        //                                else
        //                                {        
        //                                    lastmonth=date.Month;
        //                                    sum = binaryReader.ReadDouble();
        //                                    if(min==null||sum<min)
        //                                    {
        //                                        min=sum;
        //                                    }
        //                                    if(max==null||sum>max)
        //                                    {
        //                                        max=sum;
        //                                    }
        //                                    ts[chartindex].Data[dateindex,timeseriesindex+1]=sum;
        //                                    if(timeseriesindex==0)
        //                                    {
        //                                        ts[chartindex].Data[dateindex,0]= Convert(first.AddDays(i));
        //                                    }
        //                                    ++dateindex;
        //                                }

        //                            }
                                
        //                        }
        //                    }
        //                    else if(dataformat==2)//yearly
        //                    {
        //                        for(var index1=0;index1<timeseriescount;++index1)
        //                        {     
        //                            var timeseriesindex=multichart?0:index1;
        //                            var chartindex=multichart?index1:0;
                                    
        //                            ts[chartindex].Labels[timeseriesindex+1]=binaryReader.ReadString();
        //                            ts[chartindex].Colors[timeseriesindex]="#0000FF";
        //                            var lastyear=start.Year;
        //                            var sum=0.0;
        //                            var dateindex=0;
        //                            for(var i=0;i<datacount;++i)
        //                            {
        //                                var date=start.AddDays(i);
        //                                if(date.Year==lastyear&&i<datacount-1)
        //                                {
        //                                    sum += binaryReader.ReadDouble();
        //                                }
        //                                else
        //                                {         
        //                                    lastyear=date.Year;
        //                                    sum = binaryReader.ReadDouble();
        //                                    if(min==null||sum<min)
        //                                    {
        //                                        min=sum;
        //                                    }
        //                                    if(max==null||sum>max)
        //                                    {
        //                                        max=sum;
        //                                    }
        //                                    ts[chartindex].Data[dateindex,timeseriesindex+1]=sum;
        //                                    if(timeseriesindex==0)
        //                                    {
        //                                        ts[chartindex].Data[dateindex,0]= Convert(first.AddDays(i));
        //                                    }
        //                                     ++dateindex;
        //                                }

        //                            }
                                
        //                        }
        //                    }


        //                    for(var i=0;i<chartcount;++i)
        //                    {
                                
        //                     ts[i].Ymin=(double)min;
        //                     ts[i].Ymax=(double)max;
        //                     ts[i].Startms=Convert(start.GetDateTime());
        //                     ts[i].Endms=Convert(end.GetDateTime());
        //                    }
        //                    return  ts;
        //                }
        //            }
                 
        //        }
        //    }
        //    catch(Exception ex)
        //    {

        //    }

        //    return null;
        //}

        //static public long Convert(DateTime date)
        //{
        //   // return date.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;

        //     return (long)((date.ToUniversalTime().Ticks - DatetimeMinTimeTicks) / 10000);
        //}

        //  private static readonly long DatetimeMinTimeTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;

      

        public List<string>GetOutputsText(string filename)
        {
             var list=new List<string>();
            var timeseries=ReadOutputs(filename);
					 
            var timeseriescount=timeseries.Count;
            var first=timeseries.FirstOrDefault();
            if(first!=null)
            {
                var datacount=first.Values.Count;
                var start=first.StartDate;
               
                list.Add($"Date,{string.Join(", ",timeseries.Select(x=>x.Name).ToList())}");
                for(var i=0;i<datacount;++i)
                {
                    var date=start.AddDays(i);
                    var datetext=$"{date.ToString("dd/MM/yyyy")}";
                    var valuetext=$"{string.Join(", ",timeseries.Select(x=>FormatValue(x.Values[i])).ToList())}";
                    list.Add($"{datetext}, {valuetext}");
                }		
            }
            return list;
        }


        public string FormatValue(double value)
        {
            return $"{value:F6}";
            //if(value<0.00000001)
            //{
            //    return $"0";
            //}
           
            //if(value<0.0000001)
            //{
            //    return $"{value:F10}";
            //}
            //if(value<0.000001)
            //{
            //    return $"{value:F9}";
            //}
            //if(value<0.00001)
            //{
            //    return $"{value:F8}";
            //}
            //if(value<0.0001)
            //{
            //    return $"{value:F7}";
            //}
            //if(value<0.001)
            //{
            //    return $"{value:F6}";
            //}
            //if(value<0.01)
            //{
            //    return $"{value:F5}";
            //}
            //if(value<0.1)
            //{
            //    return $"{value:F4}";
            //}
            //return $"{value:F1}";
        }




    }


    //public class DygraphViewModel
    //{
    //    public DygraphViewModel(int timeseriescount, int datacount)
    //    {
    //        Data=new object[datacount,timeseriescount+1];
    //        Labels=new string[timeseriescount+1];
    //        Colors=new string[timeseriescount];

    //    }

    //    public object[,] Data {get;set;}
    //    public string[] Labels {get;set;}
    //    public string[] Colors{get;set;}
    //    public double Startms{get;set;}
    //    public double Endms{get;set;}

    //    public double Ymin{get;set;}
    //    public double Ymax{get;set;}
    //}

   




    public class TimeSeriesViewModel
    {
        public TimeSeriesViewModel(string name, BrowserDate start, BrowserDate end)
        {
            SimIndex=-1;
            Name=name;
            StartDate=new BrowserDate(start);
            EndDate=new BrowserDate(end);
            var count=end.DateInt-start.DateInt+1;
             Values=new List<double>(new double[count]);
        }
        public TimeSeriesViewModel(int simindex,string name, BrowserDate start, BrowserDate end)
        {
            SimIndex=simindex;
            Name=name;
            StartDate=start;
            EndDate=end;
            var count=end.DateInt-start.DateInt+1;
            Values=new List<double>(new double[count]);
        }
        public int SimIndex{get;set;}
        public string Name{get;set;}
        public List<double>Values{get;set;}
        public BrowserDate StartDate{get;set;}
        public BrowserDate EndDate{get;set;}
    }
}
