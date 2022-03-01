using HowLeaky_SimulationEngine.Errors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace HowLeaky_SimulationEngine.Tools
{

    public enum SequenceType
    {
        Unknown,
        JDayValue,
        DateValue,
        Date

    }
    public class Sequence
    {
       
        private String _Value;

        public List<BrowserDate> Dates { get; set; }
        public List<int> JDays { get; set; }

        public List<double> Values { get; set; }

        public Dictionary<int, double> Dict { get; set; }

        public string Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                parseStringValue();
            }
        }

        public SequenceType Type { get; set; }
        

        public Sequence()
        {
            Dates = new List<BrowserDate>();
            Values = new List<double>();
            JDays = new List<int>();
            Value = "";
        }

        public Sequence(string stringvalue, bool canInterpolate) : this()
        {
            try
            {
                Type = ExtractType(stringvalue);
                switch (Type)
                {
                    case SequenceType.JDayValue: ExtractJDayValues(stringvalue, canInterpolate); break;
                    case SequenceType.DateValue: ExtractDateValues(stringvalue); break;
                    case SequenceType.Date: ExtractDates(stringvalue); break;
                }
            }
            catch (Exception ex)
            {

            }
        }


        private SequenceType ExtractType(string stringvalue)
        {
            if (!String.IsNullOrEmpty(stringvalue.Trim()))
            {
                var items = stringvalue.Replace("\"", "").Split(',').ToList();
                if (items.Count > 0)
                {
                    float testvalue;
                    var firsttest = items[0];
                    if (float.TryParse(firsttest, out testvalue))
                    {
                        if (testvalue > 0 && testvalue < 1000)
                        {
                            return SequenceType.JDayValue;
                        }
                    }
                    var isdual = TestIsDual(items);
                    if (isdual)
                    {
                        return SequenceType.DateValue;
                    }
                    return SequenceType.Date;
                }

            }
            return SequenceType.Unknown;
        }





        private void ExtractJDayValues(string stringvalue, bool canInterpolate)
        {
           
            if (!String.IsNullOrEmpty(stringvalue))
            {

                var items = stringvalue.Replace("\"", "").Replace("[", "").Replace("]", "").Split(',').ToList();
                for (var i = 0; i < items.Count; i = i + 2)
                {
                    if (i + 1 < items.Count)
                    {
                        var first = items[i].Trim();
                        var second = items[i + 1].Trim();
                        int value1;
                        double value2;

                        if (int.TryParse(first, out value1))
                        {
                            
                            if (double.TryParse(second, out value2))
                            {
                                JDays.Add(value1);
                                Values.Add(value2);
                            }
                            
                        }

                    }
                }
                Dict = new Dictionary<int, double>();

                if (canInterpolate)
                {
                    int count = JDays.Count;
                    if (count > 1)
                    {
                        int index1 = count - 1;
                        int index2 = 0;
                        for (int i = 1; i <= 366; ++i)
                        {
                            var day1 = JDays[index1];
                            var day2 = JDays[index2];
                            var value1 = Values[index1];
                            var value2 = Values[index2];
                            if (i == day2)
                            {
                                Dict.Add(day2, value2);
                                ++index1;
                                ++index2;
                                if (index1 >= count)
                                {
                                    index1 = index2 - 1;
                                }
                                if (index2 >= count)
                                {
                                    index2 = index1 - 1;
                                }
                            }
                            //else if (i == day1)
                            //{
                            //    Dict.Add(day1, value1);
                            //}
                            else
                            {
                                if (i >= day1 && i <= day2)
                                {

                                    Dict.Add(i, Iterpolate(i, day1, value1, day2, value2));
                                }
                                else if (day1 > i)
                                {
                                    Dict.Add(i, Iterpolate(i, day1 - 366, value1, day2, value2));
                                }
                                else if (day2 < i)
                                {
                                    Dict.Add(i, Iterpolate(i, day1, value1, day2 + 366, value2));
                                }



                            }

                        }
                    }
                    else if (count == 1)
                    {
                        var value = Values[0];
                        for (int i = 1; i <= 366; ++i)
                        {
                            Dict.Add(i, value);
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= 366; ++i)
                        {
                            Dict.Add(i, 0);
                        }
                    }
                }
                else
                {

                    int index = 0;
                    for (int i = 1; i <= 366; ++i)
                    {
                        var day = index < JDays.Count ? JDays[index] : -1;

                        if (i == day)
                        {
                            var value = Values[index];
                            Dict.Add(day, value);
                            ++index;
                        }
                        else
                        {
                            Dict.Add(i, 0);
                        }
                    }
                }
            }
        }

        public double Iterpolate(int day, int day1, double value1, int day2, double value2)
        {
            if (day == day1)
            {

                return value1;

            }
            else if (day == day2)
            {
                return value2;
            }
            else //if (day < day2)
            {
                double m, c, denom;
                denom = (day2 - day1);
                if (denom != 0)
                {
                    m = (value2 - value1) / denom;
                }
                else
                {
                    return 0;
                }
                c = value2 - m * day2;
                return (m * day + c);
            }
            return 0;
        }

        private void ExtractDateValues(string stringvalue)
        {
            try
            {
                var items = stringvalue.Replace("\"", "").Split(',').ToList();

                for (var i = 0; i < items.Count; i = i + 2)
                {
                    if (i + 1 < items.Count)
                    {
                        var first = items[i].Trim();
                        var second = items[i + 1].Trim();
                        DateTime date;
                        if (DateTime.TryParseExact(first, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        {
                            var value = double.Parse(second);
                            Dates.Add(new BrowserDate(date));
                            Values.Add(value);
                        }
                        else if (DateTime.TryParseExact(first, "d/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        {
                            var value = double.Parse(second);
                            Dates.Add(new BrowserDate(date));
                            Values.Add(value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        private void ExtractDates(string stringvalue)
        {
            try
            {
                var items = stringvalue.Replace("\"", "").Split(',').ToList();
                for (var i = 0; i < items.Count; ++i)
                {
                    var first = items[i].Trim();

                    DateTime date;
                    if (DateTime.TryParseExact(first, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    {

                        Dates.Add(new BrowserDate(date));

                    }
                    else if (DateTime.TryParseExact(first, "d/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    {

                        Dates.Add(new BrowserDate(date));

                    }
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }




        private bool TestIsDual(List<string> items)
        {
            if (items.Count > 1)
            {
                double value;
                if (double.TryParse(items[1], out value))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool ContainsDate(BrowserDate date)
        {
            if (Type == SequenceType.Date && Type == SequenceType.DateValue)
            {
                if (Dates.Any(x => x.DateInt == date.DateInt))
                {
                    return true;
                }
            }
            else if (Type == SequenceType.JDayValue)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public double ValueAtDate(BrowserDate date)
        {
            if (Type == SequenceType.Date && Type == SequenceType.DateValue)
            {
                var dateobject = Dates.FirstOrDefault(x => x.DateInt == date.DateInt);
                if (dateobject != null)
                {
                    int dateIndex = Dates.IndexOf(dateobject);

                    if (dateIndex >= 0)
                    {
                        return Values[dateIndex];
                    }
                }
            }
            else if (Type == SequenceType.JDayValue)
            {
                var jday = date.GetJDay();
                if (jday > 365)
                {
                    jday = 365;
                }
                return Dict[jday];
            }
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        private void parseStringValue()
        {
            List<String> parts = new List<string>(_Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));

            //foreach (String s in parts)
            for (int i = 0; i < parts.Count; i += 2)
            {
                string s = parts[i];

                List<String> seqParts = new List<string>(s.Split(new string[] { "\"", ",", " " }, StringSplitOptions.RemoveEmptyEntries));
                BrowserDate thisDate = parseDate(seqParts[0]);

                if (thisDate != null)
                {
                    Dates.Add(thisDate);

                    string[] valParts = parts[i + 1].Split(new string[] { "\"", ",", " " }, StringSplitOptions.RemoveEmptyEntries);

                    double value = -1;
                    double.TryParse(valParts[0], out value);
                    Values.Add(value);
                }

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            List<String> parts = new List<string>();

            for (int i = 0; i < Dates.Count; i++)
            {
                parts.Add("\"" + Dates[i].ToString("dd/mm/yyyy") + "," + Values[i].ToString() + "\"");
            }

            return String.Join(",", parts.ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        BrowserDate parseDate(String dateString)
        {
            int year, month, day;
            List<string> dateParts = new List<string>(dateString.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries));

            if (int.TryParse(dateParts[0], out day) && int.TryParse(dateParts[1], out month) && int.TryParse(dateParts[2], out year))
            {
                return new BrowserDate(year, month, day);
            }

            return null;
        }


    }
}
