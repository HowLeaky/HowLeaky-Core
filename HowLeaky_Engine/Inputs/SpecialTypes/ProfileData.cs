using HowLeaky_SimulationEngine.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HowLeaky_SimulationEngine.Tools
{
     public class ProfileData
    {
       public List<int> jdays;
       public Dictionary<string, List<double>> values;

        int dayIndex = 0;
        //public ProfileData(List<string> columns)
        //{
        //    jdays = new List<int>();
        //    values = new Dictionary<string, List<double>>
        //    {
        //        { columns[0], new List<double>() },
        //        { columns[1], new List<double>() },
        //        { columns[2], new List<double>() }
        //    };

        //    dayIndex = 0;
        //}

        public ProfileData(string stringvalue)
        {
            var headers=new List<string> (new string[] { "Green Cover", "Residue Cover","Root Depth" });
            jdays = new List<int>();
            values = new Dictionary<string, List<double>>
            {
                { headers[0], new List<double>() },
                { headers[1], new List<double>() },
                { headers[2], new List<double>() }
            };

            dayIndex = 0;

            
                 var list=stringvalue.Split('|').ToList();                    
                foreach(var item in list)
                {
                    var values=item.Split(',').ToList();
                    var dateint=int.Parse(values[0]);
                    var value1=double.Parse(values[1]);
                    var value2=double.Parse(values[2]);
                    var value3=double.Parse(values[3]);
                    AddDate(dateint);
                    AddValue("Green Cover",value1);
                    AddValue("Residue Cover", value2);
                    AddValue("Root Depth", value3);    
                }
            
        }

        public void AddDate(int jday)
        {
            jdays.Add(jday);
        }

        public void AddValue(string name, double value)
        {
            List<double> data = values[name];

            data.Add(value);
        }


        public double GetValueForDayIndex(string datakey, BrowserDate today)
        {

            
            
           // UpdateDayIndex(today);
            List<double> data = values[datakey];
            int count = data.Count;
            for (int i = 0; i < count; ++i)
            {
                if (dayIndex < jdays[0])
                {
                    //double m, c, denom;
                    //denom = (double)(jdays[0]);
                    //if (denom != 0)
                    //{
                    //    //TODO: fix this
                    //    //m = data[0] / denom; //This is not correct
                    //}
                    //else
                    //{
                    //    return 0;
                    //}
                    //c = data[0] - m * jdays[0];
                    //return (m * dayindex + c);

                    //return 0;

                    //After discussion with Dan Rattray...
                    return data[0];

                }
                else if (dayIndex == jdays[i])
                {
                    return data[i];
                }
                else if (dayIndex < jdays[i])
                {
                    double m, c, denom;
                    denom = (jdays[i] - jdays[i - 1]);
                    if (denom != 0)
                    {
                        m = (data[i] - data[i - 1]) / denom;
                    }
                    else
                    {
                        return 0;
                    }
                    c = data[i] - m * jdays[i];
                    return (m * dayIndex + c);
                }
            }
            return data[count - 1];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dayindex"></param>
        /// <param name="today"></param>
        //public void UpdateDayIndex(int dayindex, DateTime today)
        public void UpdateDayIndex(BrowserDate today)
        {
            try
            {
                int last = jdays.Count;
                if (last >= 0)
                {
                    int dayno = today.GetJDay();    //CHECK
                    if (jdays[last - 1] <= 366)
                    {
                        dayIndex = dayno;
                    }
                    else
                    {
                        int nolaps = (int)(jdays[last - 1] / 366) + 1;
                        int resetvalue = nolaps * 365;
                        if (dayIndex >= resetvalue && dayno == 1)
                        {
                            dayIndex = 1;
                        }
                        else
                        {
                            dayIndex++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }
    }
}
