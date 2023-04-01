using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace HowLeaky_IO
{

    public enum SILOFormat
    {
        StandardP51,
        Detailed
    }

    public class P51DataFile
    {
        public P51DataFile(string datafile)
        {
            FileName = datafile;
            Name=Path.GetFileNameWithoutExtension(datafile);
        }

       public string FileName{get;set;}
        public int ReferenceCount{get;set;}



         
        public string Name{get;set;}

        public SILOFormat Format { get; set; }
        public BrowserDate StartDate { get; set; }
        public BrowserDate EndDate { get; set; }
     
       public bool HasLoaded{get;set;}
       
         static readonly object newlock = new object();
        public void DecrementReferenceCount()
        {
            lock(newlock)
            {
                --ReferenceCount;
                if(ReferenceCount==0)
                {
                    TimeSeries=null;
                    HasLoaded=false;
                }
            }
        }


        public int Count()
        {
            if(StartDate!=null&&EndDate!=null)
            {
                return EndDate.DateInt-StartDate.DateInt+1;
            }
            return 0;
        }

        public List<List<double?>>TimeSeries{get;set;}
            
        

        public bool CheckForRemoteUpdates()
        {
            if (AllowDownloads)
            {
                bool result = false;
                if (File.Exists(FileName))
                    result = OpenFull();
                //if (NeedToUpdate())
                //    result = DownloadSynchronousData();
                return result;
            }
            else
                return OpenFull();
        }

        public DateTime? DataEndDate { get; set; }


       

        public bool OpenFull(string _filename=null)
        {
            try
            {
                string filename=String.IsNullOrEmpty(_filename)?FileName:_filename;
                String partfilename = String.Format("{0}.part", filename);
                if (!HasLoaded)
                {
                    if (!File.Exists(filename))
                    {
                        if (File.Exists(partfilename))
                        {
                            File.Copy(partfilename, filename);
                            File.Delete(partfilename);
                        }
                    }
                    if (File.Exists(filename))
                    {
                        int index = -1;
                        //load the main data file.
                        if (Format == SILOFormat.StandardP51)
                        {
                            index = LoadStandardFromFileAtIndex(0, filename);
                        }
                        else
                        {
                            index = LoadExtendedFromFileAtIndex(0, filename);
                        }
                        //now try to load any attached datafiles and cleanup
                        if (index != -1)
                        {
                            TestFile();//latest date is less than today
                            HasLoaded = true;
                            return true;
                        }
                        else
                        {
                            //cliamte data could not be accessed. 
                            ClearAllData();
                            HasLoaded = false;
                            return false;
                        }
                    }
                    return false;
                }
                return true;
            }
            catch (Exception )
            {

            }
            return false;
        }

        public bool Open()//appends newly loaded data.
        {
            
            String partfilename = String.Format("{0}.part", FileName);
            if (File.Exists(partfilename))
            {
                int index = (HasLoaded ? Count()  : -1);
                bool originalhasproblems = (index == -1);
                if (File.Exists(partfilename))
                {
                    int newindex  = Format == SILOFormat.StandardP51 ? LoadStandardFromFileAtIndex(index, partfilename) : LoadExtendedFromFileAtIndex(index, partfilename);
                    //now need to clean up
                    //(1) Delete "part" file

                    if (newindex != -1)
                    {
                        if (originalhasproblems == false)
                        {
                            //(2) Append new data to original datafile.
                            AppendToDataFile(index);
                            CleanUpFiles1(partfilename);
                        }
                        else
                        {
                            CleanUpFiles2(partfilename);
                        }
                        HasLoaded = true;
                        TestFile();//latest date is less than today
                        HasLoaded = true;
                        return true;
                    }
                    else
                    {
                        //Found problem with datafile.
                        if (HasLoaded)
                            return true; //just use what we have
                        else
                            return false;//start complaining
                    }
                }
                else
                {
                    TestFile();//latest date is less than today
                    HasLoaded = true;
                    return true;
                }
            }     
            
            return false;
        }

        private void CleanUpFiles1(String partfilename)
        {
            if (File.Exists(partfilename))
            {
                if (!File.Exists(FileName))
                    File.Copy(partfilename, FileName);
                File.Delete(partfilename);
            }
        }
        private void CleanUpFiles2(String partfilename)
        {
            if (File.Exists(partfilename))
            {
                File.Copy(partfilename, FileName,true);
                File.Delete(partfilename);
            }
        }

        protected void Reset()
        {
          
            ClearAllData();
            HasLoaded = false;
          
          
        }


        public  void ClearAllData()
        {
            try
            {
                foreach (var timeseries in TimeSeries)
                {
                    TimeSeries.Clear();
                }
                
            }
            catch (Exception )
            {

            }
        }

       
        private int LoadStandardFromFileAtIndex(int startindex,string filename)
        {
            try 
            {
                var datetext="";
                float tmax;
                float tmin;
                float rain;
                float pan;
                float solarrad;
                int index=startindex;
                string line;
                bool foundstart=false;

                if (startindex <= 0) CreateTimeSeries(5);
                var MaxTempValues = TimeSeries[0];
                var MinTempValues =  TimeSeries[1];
                var RainfallValues = TimeSeries[2]; 
                var PanEvapValues =  TimeSeries[3];
                var SolarRadValues = TimeSeries[4];
               
                //DateTime date;
                using (StreamReader infile = new StreamReader(filename))
                {
                    while ((line = infile.ReadLine()) != null)
                    {
                        if (foundstart)
                        {
                            if (index >= startindex)
                            {
                                String[] dellist = line.Split(new[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
                                if (dellist.Length == 8)
                                {
                                    datetext = dellist[0];
                                    if(StartDate==null)
                                    {
                                        StartDate=new BrowserDate(datetext,"yyyyMMdd");
                                    }
                                    tmax = Convert.ToSingle(dellist[2]);
                                    tmin = Convert.ToSingle(dellist[3]);
                                    rain = Convert.ToSingle(dellist[4]);
                                    pan = Convert.ToSingle(dellist[5]);
                                    solarrad = Convert.ToSingle(dellist[6]);

                                   
                                    MaxTempValues.Add(tmax);
                                    MinTempValues.Add(tmin);
                                    RainfallValues.Add(rain);
                                    PanEvapValues.Add(pan);
                                    SolarRadValues.Add(solarrad);
                                    ++index;

                                }
                                
                            }
                        }
                        else
                            if (line.Contains("date") && line.Contains("jday"))
                                foundstart = true;
                    }
                    //infile.Close(); shouldn't need to close when using a "using"!
                }
                if(foundstart)
                {
                   
                    EndDate=new BrowserDate(datetext,"yyyyMMdd");
                   
                    --index;
                    return index;
                }

            }
            catch (Exception ) 
            {
        
            }

            return -1;
        }
      
     
        private int LoadExtendedFromFileAtIndex(int startindex, string filename)
        {
            try
            {
                var datetext="";
                float tmax;
                float tmin;
                float rain;
                float pan;
                float solarrad;
                float vp;
                float rhmax;
                float rhmin;
                float fao56;
                int index = startindex;
                string line;
                bool foundstart = false;

                if (startindex <= 0) CreateTimeSeries(9);
                
                var MaxTempValues = TimeSeries[0];
                var MinTempValues = TimeSeries[1];                
                var RainfallValues = TimeSeries[2];
                var PanEvapValues = TimeSeries[3];
                var SolarRadValues = TimeSeries[4];
                var VPValues = TimeSeries[5];
                var RHMaxValues = TimeSeries[6];
                var RHMinValues = TimeSeries[7];
                var FAO56Values = TimeSeries[8];
                
              
                //DateTime date;
                using (StreamReader infile = new StreamReader(filename))
                {
                    while ((line = infile.ReadLine()) != null)
                    {
                        if (foundstart)
                        {
                            if (index >= startindex)
                            {
                                String[] dellist = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                if (dellist.Length == 18)
                                {
                                    //" * Patched Point data for station: 41023 DALBY POST OFFICE                        Lat: -27.18 Long: 151.26"

                                    //Date       Day Date2      T.Max Smx T.Min Smn Rain   Srn  Evap Sev Radn   Ssl VP    Svp RHmaxT RHminT  FAO56    
                                    //(yyyymmdd)  () (ddmmyyyy)  (oC)  ()  (oC)  ()   (mm)  ()  (mm)  () (MJ/m2) () (hPa)  ()   (%)    (%)    (mm) 
                                    //    (0)     (1)    (2)    (*3*)  (4) (*5*) (6)  (*7*) (8) (*9*) (10)(*11*) (12)(*13*)(14)(*15*) (*16*)  (*17*)
                                    datetext = dellist[0];
                                    if(StartDate==null)
                                    {
                                        StartDate=new BrowserDate(datetext,"yyyyMMdd");
                                    }
                                    tmax = Convert.ToSingle(dellist[3]);
                                    tmin = Convert.ToSingle(dellist[5]);
                                    rain = Convert.ToSingle(dellist[7]);
                                    pan = Convert.ToSingle(dellist[9]);
                                    solarrad = Convert.ToSingle(dellist[11]);
                                    vp = Convert.ToSingle(dellist[13]);
                                    rhmax = Convert.ToSingle(dellist[15]);
                                    rhmin = Convert.ToSingle(dellist[16]);
                                    fao56 = Convert.ToSingle(dellist[17]);
     
                                    MaxTempValues.Add(tmax);
                                    MinTempValues.Add(tmin);                                          
                                    RainfallValues.Add(rain);
                                    PanEvapValues.Add(pan);
                                    SolarRadValues.Add(solarrad);
                                    VPValues.Add(vp);
                                    RHMaxValues.Add(rhmax);
                                    RHMinValues.Add(rhmin);
                                    FAO56Values.Add(fao56);
                                    ++index;
                                        

                                    
                                }
                            }
                        }
                        else
                            if (line.Contains("yyyymmdd") && line.Contains("ddmmyyyy"))
                            foundstart = true;
                    }
                    //infile.Close(); shouldn't need to close when using a "using"!
                }
                if (foundstart)
                {
                    EndDate=new BrowserDate(datetext,"yyyyMMdd");                    
                    --index;
                    return index;
                }

            }
            catch (Exception)
            {

            }

            return -1;
        }

        private void CreateTimeSeries(int count)
        {
            TimeSeries=new List<List<double?>>();
            for(var i=0;i<count;++i)
            {
                TimeSeries.Add(new List<double?>());
            }
        }

        private void AppendToDataFile(int startindex)
        {
            try
            {
                if (Format == SILOFormat.StandardP51)
                {
                    if (startindex != -1)
                    {
                        //var startdate=new BrowserDate()
                        var MaxTempValues = TimeSeries[0];
                        var MinTempValues = TimeSeries[1];
                        var RainfallValues = TimeSeries[2];
                        var PanEvapValues = TimeSeries[3];
                        var SolarRadValues = TimeSeries[3];
                        const String formatstring =
                            "{0:yyyyMMdd}\t{1}\t{2:0.##}\t{3:0.##}\t{4:0.##}\t{5:0.##}\t{6:0.##}\t{7:0.##}";
                        using (StreamWriter w = File.AppendText(FileName))
                        {
                            int count = MaxTempValues.Count;
                            for (int i = startindex; i < count; ++i)
                            {
                                var date=new DateTime();
                                var dayofyear=1;
                                w.WriteLine(String.Format(formatstring, date, dayofyear, MaxTempValues[i],
                                    MinTempValues[i], RainfallValues[i], PanEvapValues[i], SolarRadValues[i], "-1"));
                            }
                        }
                    }

                }
                else
                {
                    if (startindex != -1)
                    {
                        
                        var MaxTempValues = TimeSeries[0];
                        var MinTempValues = TimeSeries[1];
                        var RainfallValues = TimeSeries[2];
                        var PanEvapValues = TimeSeries[3];
                        var SolarRadValues = TimeSeries[4];
                        var VPValues = TimeSeries[5];
                        var RHMaxValues = TimeSeries[6];
                        var RHMinValues = TimeSeries[7];
                        var FAO56Values = TimeSeries[8];
                        //" * Patched Point data for station: 41023 DALBY POST OFFICE                        Lat: -27.18 Long: 151.26"

                        //Date       Day Date2      T.Max Smx T.Min Smn Rain   Srn  Evap Sev Radn   Ssl VP    Svp RHmaxT RHminT  FAO56    
                        //(yyyymmdd)  () (ddmmyyyy)  (oC)  ()  (oC)  ()   (mm)  ()  (mm)  () (MJ/m2) () (hPa)  ()   (%)    (%)    (mm) 
                        //    (0)     (1)    (2)    (*3*)  (4) (*5*) (6)  (*7*) (8) (*9*) (10)(*11*) (12)(*13*)(14)(*15*) (*16*)  (*17*)
                        const String formatstring =
                            "{0:yyyyMMdd}\t{1}\t{2:ddmmyyyy}\t{3:N2}\t{4:N2}\t{5:N2}\t{6:N2}\t{7:N2}\t{8:N2}\t{9:N2}\t{10:N2}\t{11:N2}\t{12:N2}\t{13:N2}\t{14:N2}\t{15:N2}\t{16:N2}\t{17:N2}";
                        using (StreamWriter w = File.AppendText(FileName))
                        {
                            var dummy = 0.0f;
                            int count = MaxTempValues.Count;
                            for (int i = startindex; i < count; ++i)
                            {
                                var date=new DateTime();
                                var dayofyear=1;
                                w.WriteLine(String.Format(formatstring,
                                   date, /*0*/
                                    dayofyear, /*1*/
                                    date, /*2*/
                                    MaxTempValues[i]/*MaxTempValues[i]*/, /*3*/
                                    dummy, /*4*/
                                    MinTempValues[i]/*MinTempValues[i]*/, /*5*/
                                    dummy, /*6*/
                                    RainfallValues[i], /*7*/
                                    dummy, /*8*/
                                    PanEvapValues[i]/*PanEvapValues[i]*/, /*9*/
                                    dummy, /*10*/
                                    SolarRadValues[i]/*SolarRadValues[i]*/, /*11*/
                                    dummy, /*12*/
                                    VPValues[i]/*VPValues[i]*/, /*13*/
                                    dummy, /*14*/
                                    RHMaxValues[i]/*RHMaxValues[i]*/, /*15*/
                                    RHMinValues[i]/*RHMinValues[i]*/, /*16*/
                                    FAO56Values[i] /*17*/
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ) 
            {
                
            } 
        }
              

       // bool HasRetested;
        private void TestFile()
        {
            try
            {
                //if (!HasRetested)
                //{
                //    if (DateArrays.Count > 0 && DateArrays[0].EndDate!=null)
                //    {
                //        HasRetested = true;
                //        DateTime end = (DateTime) DateArrays[0].EndDate;
                //        DateTime today = DateTime.Today;
                //        if (end > today) //Houston we have a problem
                //        {
                //            //if we get this far, then we should redownload the datafile...
                //            RedownloadDataFile();
                //        }
                //    }
                //}
            }
            catch (Exception ) 
            {
                
            }
        }

 //       bool fulldownloadrequired;
        private void RedownloadDataFile()
        {
            //try
            //{
            //    Reset();
            //    var startyear = GetStartYear(YearsRange);
            //    DateTime startdate = new DateTime(startyear, 1, 1);
            //    UpdateRemoteURINameFromDate(startdate);
            //    DownloadSynchronousData();
            //}
            //catch (Exception ) 
            //{
                
            //}
        }


        public void UpdateRemoteURIName()
        {
            //if (!HasLoaded || DataEndDate == null)
            //{
            //    var startyear = StartYear;
            //    DateTime startdate = new DateTime(startyear, 1, 1);
            //    UpdateRemoteURINameFromDate(startdate);
            //}
            //else
            //{
            //    DateTime date=(DateTime)DataEndDate;
            //    UpdateRemoteURINameFromDate(date.AddDays(1));
            //}
        }

        public void UpdateRemoteURINameFromDate(DateTime startdate)
        {
            try
            {
                //String stationNo=Location.StationCode;
                //String format = (Format==SILOFormat.StandardP51?@"p51": @"FAO56");
                //String start=startdate.ToString("yyyyMMdd");
                //String finish=DateTime.Now.ToString("yyyyMMdd");
                //const String username = @"CLIMATEANALYSIS";
                //const String password = @"MCUSMARTPHONE";
           
                ////EndDate=Record.EndDate;
    
                //if(String.Equals(start,finish)==false)
                //{
                //    RemoteURI=String.Format("https://legacy.longpaddock.qld.gov.au/cgi-bin/silo/PatchedPointDataset.php?station={0}&format={1}&start={2}&finish={3}&username={4}&password={5}"
                //            , stationNo,format,start,finish,username,password);
    
    
                //    System.Diagnostics.Debug.WriteLine(@"link: "+RemoteURI);
                //}
                //else
                //    RemoteURI= @"";
            }
            catch (Exception ) 
            {
                
            }
        }

      

        public bool NeedToUpdate()
        {
            
                //try
                //{
                //    if (File.Exists(FullFileName)&&DataEndDate!=null)
                //    {
                //        DateTime yesterday = DateTime.Now.AddDays(-1);
                //        bool result= ! DateUtilities.IsTheSameDay(yesterday, (DateTime)DataEndDate);
                //        return result;
                //    }
                //}
                //catch (Exception ) 
                //{
                
                //}
                //return true;
                return false;
            
        }

        public bool AllowDownloads { get; set; }
    }
}
