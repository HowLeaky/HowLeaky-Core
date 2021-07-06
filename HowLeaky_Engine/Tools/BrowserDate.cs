using HowLeaky_SimulationEngine.Errors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace HowLeaky_SimulationEngine.Tools
{
    public class BrowserDate
    {
        public BrowserDate()
        {

            LoadDateTime(DateTime.UtcNow);

        }

        public BrowserDate(string datestring, string format)
        {
            LoadDateTime(DateTime.ParseExact(datestring, format, CultureInfo.InvariantCulture));
        }

        public BrowserDate(int dateint)
        {
            DateInt = dateint;
            var date = ReferenceDate().AddDays(DateInt);
            Year = date.Year;
            Month = date.Month;
            Day = date.Day;
        }

        public BrowserDate(float datefloat)
        {
            DateInt = (int)datefloat;
            var date = ReferenceDate().AddDays(DateInt);
            Year = date.Year;
            Month = date.Month;
            Day = date.Day;


        }

        public BrowserDate(int year, int jday)
        {
            var date = new DateTime(year, 1, 1).AddDays(jday - 1);
            LoadDateTime(date);
        }



        public BrowserDate(int year, int month, int day)
        {
            try
            {

            
                var date = new DateTime(year, month, day);
               LoadDateTime(date);
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        public BrowserDate(DateTime date)
        {
            LoadDateTime(date);
        }

        public BrowserDate(BrowserDate source)
        {
            try
            {
                
                DateInt = source.DateInt;
                Day = source.Day;
                Month = source.Month;
                Year = source.Year;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);

            }
        }
        public BrowserDate(int year, int month, bool usestart)
        {

            Day = usestart ? 1 : LastDayOfMonth(year, month);
            Month = month;
            Year = year;
            var date = new DateTime(Year, Month, Day);
            DateInt = Convert(date);
        }

        public int DateInt { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public float GetUnixTime()
        {
            var basedate = new DateTime(1970, 1, 1);
            TimeSpan span = GetDateTime() - basedate;
            return (float)span.TotalMilliseconds;
            
        }

        public void LoadDateTime(DateTime date)
        {
            try
            {
                DateInt = Convert(date);
                Year = date.Year;
                Month = date.Month;
                Day = date.Day;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }

        public int Convert(DateTime date)
        {
            return (date - ReferenceDate()).Days;//NOT TESTED  
        }

        public static int DaysInYear(int year)
        {
            return DateTime.IsLeapYear(year) ? 366 : 365;
        }

        public BrowserDate IncrementDay()
        {
            var date = new BrowserDate(DateInt + 1);
            DateInt = date.DateInt;
            Day = date.Day;
            Month = date.Month;
            Year = date.Year;
            return date;
        }

        public BrowserDate DecrementDay()
        {
            var date = new BrowserDate(DateInt - 1);
            DateInt = date.DateInt;
            Day = date.Day;
            Month = date.Month;
            Year = date.Year;
            return date;
        }

        public BrowserDate AddDays(float days)
        {
            var date = new BrowserDate(DateInt + days);
            //DateInt=date.DateInt;
            //Day=date.Day;
            //Month=date.Month;
            //Year=date.Year;
            return date;
        }

        public BrowserDate GetStartOfMonth()
        {
            var date=new BrowserDate(Year,Month,1);
            return date;
        }

        public BrowserDate GetStartOfYear()
        {
            var date=new BrowserDate(Year,1,1);
            return date;
        }


        public bool IsBefore(BrowserDate date)
        {
            return (DateInt < date.DateInt);
        }

        public BrowserDate IncrementYear()
        {
            var date = new BrowserDate(Year + 1, Month, Day);
            DateInt = date.DateInt;
            Day = date.Day;
            Month = date.Month;
            Year = date.Year;
            return date;
        }

        public int DaysFrom(BrowserDate date)
        {
            return Math.Abs(DateInt - date.DateInt);
        }

        //public int MonthsFrom(BrowserDate date)
        //{
        //    return (int)(Math.Abs(DateInt - date.DateInt) / (365.2422 / 12.0) + 0.5);
        //}

        public int MonthsFrom(BrowserDate date)
        {
            return ((Year - date.Year) * 12) + Month - date.Month;
        }

        public BrowserDate AddMonths(int months)
        {
            var day = Day;
            var month = Month;
            var year = Year;

            month = month + months;
            while (month < 1)
            {
                month = month + 12;
                year = year - 1;
            }
            while (month > 12)
            {
                month = month - 12;
                year = year + 1;
            }
            var lastday = LastDayOfMonth(year, month);
            if (day > lastday)
            {
                day = lastday;
            }
            return new BrowserDate(year, month, day);

        }

        public BrowserDate AddYears(int years)
        {
            var day = Day;
            var month = Month;
            var newyear = Year + years;
            var lastday = LastDayOfMonth(newyear, month);
            if (day > lastday)
            {
                day = lastday;
            }
            var date = new BrowserDate(newyear, month, day);
            DateInt = date.DateInt;
            Day = date.Day;
            Month = date.Month;
            Year = date.Year;
            return date;
        }

        public static int LastDayOfMonth(int year, int month)
        {
            var ndays = new int[] { -1, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            var days = ndays[month];
            if (month == 2 && IsLeapYear(year))
            {
                ++days;
            }
            return days;
        }

        public static bool IsLeapYear(int year)
        {
            return DateTime.IsLeapYear(year);
            //if (year % 4 == 0)
            //{
            //    if (year % 100 == 0)
            //    {
            //        if (year % 400 == 0)
            //            return true;
            //        else
            //            return false;
            //    }
            //    else
            //        return true;
            //}
            //else
            //    return false;

        }

        public string DateIntString()
        {
            return $"{DateInt}";//[NSString stringWithFormat:@"%ld",(long)DateInt];
        }

        public bool IsBeforeYesterday()
        {
            return IsBefore(BrowserDate.Yesterday());
        }
        public bool IsBeforeLastMonth()
        {
            BrowserDate today = BrowserDate.Today();
            var year = today.Year;
            var month = today.Month - 1;
            if (month < 0)
            {
                month = month + 12;
                --year;
            }
            var testdate = new BrowserDate(year, month, 1);
            return IsBefore(testdate);
        }

        public bool IsBetween(BrowserDate date1, BrowserDate date2)
        {
            return DateInt >= date1.DateInt && DateInt <= date2.DateInt;
        }

        public bool IsSameDate(BrowserDate date)
        {
            return DateInt == date.DateInt;
        }

        public string ToString(string format)
        {
            var result = format;
            if (result.Contains("ddd"))
            {
                var suffix = "";
                switch (Day)
                {
                    case 1:
                    case 21:
                    case 31:
                        suffix = "st"; break;
                    case 2:
                    case 22:
                        suffix = "nd"; break;
                    case 3:
                    case 23:
                        suffix = "rd"; break;
                    default:
                        suffix = "th"; break;
                }
                result = result.Replace("ddd", $"{Day}{suffix}");

            }
            else if (result.Contains("dd"))
            {
                if (Day < 10)
                {
                    result = result.Replace("dd", $"0{Day}");
                }
                else
                {
                    result = result.Replace("dd", $"{Day}");
                }
            }
            else if (result.Contains("d"))
            {
                result = result.Replace("d", $"{Day}");
            }

            if (result.Contains("MMMM"))
            {
                var monthtext = "";
                switch (Month)
                {
                    case 1: monthtext = "January"; break;
                    case 2: monthtext = "February"; break;
                    case 3: monthtext = "March"; break;
                    case 4: monthtext = "April"; break;
                    case 5: monthtext = "May"; break;
                    case 6: monthtext = "June"; break;
                    case 7: monthtext = "July"; break;
                    case 8: monthtext = "August"; break;
                    case 9: monthtext = "September"; break;
                    case 10: monthtext = "October"; break;
                    case 11: monthtext = "November"; break;
                    case 12: monthtext = "December"; break;
                }
                result = result.Replace("MMMM", monthtext);

            }
            else if (result.Contains("MMM"))
            {
                var monthtext = "";
                switch (Month)
                {
                    case 1: monthtext = "Jan"; break;
                    case 2: monthtext = "Feb"; break;
                    case 3: monthtext = "Mar"; break;
                    case 4: monthtext = "Apr"; break;
                    case 5: monthtext = "May"; break;
                    case 6: monthtext = "Jun"; break;
                    case 7: monthtext = "Jul"; break;
                    case 8: monthtext = "Aug"; break;
                    case 9: monthtext = "Sep"; break;
                    case 10: monthtext = "Oct"; break;
                    case 11: monthtext = "Nov"; break;
                    case 12: monthtext = "Dec"; break;
                }
                result = result.Replace("MMM", monthtext);
            }
            else if (result.Contains("MM"))
            {
                if (Month < 10)
                {
                    result = result.Replace("MM", $"0{Month}");
                }
                else
                {
                    result = result.Replace("MM", $"{Month}");
                }
            }
            else if (result.Contains("mm"))
            {
                if (Month < 10)
                {
                    result = result.Replace("mm", $"0{Month}");
                }
                else
                {
                    result = result.Replace("mm", $"{Month}");
                }
            }
            else if (result.Contains("M"))
            {
                result = result.Replace("MM", $"{Month}");
            }
            if (result.Contains("yyyy"))
            {
                result = result.Replace("yyyy", $"{Year}");
            }
            else if (result.Contains("yy"))
            {
                var text = $"{Year}";
                text = text.Substring(2);
                result = result.Replace("yy", text);
            }
            return result;
        }

        public bool IsLastDayOfMonth()
        {
            var lastday=LastDayOfMonth(Year,Month);
            return Day==lastday;
        }

        public bool IsLastDayOfYear()
        {
            return Month==12 && Day==31;            
        }

        public static BrowserDate Today()
        {
            return new BrowserDate();
        }

        public static BrowserDate Yesterday()
        {
            return (new BrowserDate()).DecrementDay();
        }
        public static BrowserDate DateWithYearMonthAndDay(int year, int month, int day)
        {
            return new BrowserDate(year, month, day);

        }


        public static BrowserDate DateWithYearAndJDay(int year, int jday)
        {
            return new BrowserDate(year, jday);
        }
        public static BrowserDate DateWithDateInt(int dateint)
        {
            return new BrowserDate(dateint);
        }

        public static BrowserDate DateWithDateFloat(float datefloat)
        {
            return new BrowserDate(datefloat);
        }
        public static BrowserDate DateWithDateTime(DateTime date)
        {
            return new BrowserDate(date);
        }

        public BrowserDate EndOfTheMonth()
        {
           return new BrowserDate(Year,Month,LastDayOfMonth(Year,Month));
        }

        public DateTime GetDateTime()
        {
            return ReferenceDate().AddDays(DateInt);
        }

        public DateTime ReferenceDate()
        {
            return new DateTime(1900, 1, 1);
        }

        public int GetJDay()
        {
            var basedate = new BrowserDate(Year, 1, 1);
            return DateInt - basedate.DateInt + 1;
        }
    }
}
