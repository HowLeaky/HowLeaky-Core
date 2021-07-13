using HowLeaky_SimulationEngine.Errors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace HowLeaky_SimulationEngine.Tools
{
    public class Sequence
    {
        private String _Value;
        [XmlIgnore]
        public List<BrowserDate> Dates { get; set; }
        [XmlIgnore]
        public List<double> Values { get; set; }
        [XmlText]
        public string Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                parseStringValue();
            }
        }
        ///// <summary>
        /// 
        /// </summary>
        /// </summary>
        public Sequence()
        {
            Dates = new List<BrowserDate>();
            Values = new List<double>();

            Value = "";
        }

        public Sequence(string stringvalue) : this()
        {
            try
            {
                if (!String.IsNullOrEmpty(stringvalue.Trim()))
                {
                    var items = stringvalue.Replace("\"", "").Split(',').ToList();
                    var isdual = TestIsDual(items);
                    if (isdual)
                    {
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
                    else
                    {


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
            if (Dates.Any(x => x.DateInt == date.DateInt))
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
            var dateobject = Dates.FirstOrDefault(x => x.DateInt == date.DateInt);
            if (dateobject != null)
            {
                int dateIndex = Dates.IndexOf(dateobject);

                if (dateIndex >= 0)
                {
                    return Values[dateIndex];
                }
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

        public double GetValueForDayIndex( BrowserDate engineTodaysDate)
        {
            throw new NotImplementedException();
        }
    }
}
