using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace HowLeaky_SimulationEngine.Tools
{
    //public static class DateUtilities
    //{
    //    public static DateTime NULLDATE = new DateTime(1500, 1, 1);
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="date"></param>
    //    /// <returns></returns>
    //    public static int MonthsSinceYear0(DateTime date)
    //    {
    //        return date.Year * 12 + (date.Month - 1); //Jan 00 is 0
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="months"></param>
    //    /// <returns></returns>
    //    public static DateTime DateFromMonthsSinceYear0(int months)
    //    {
    //        int year = (int)Math.Floor((double)months / 12.0);
    //        int month = months - year * 12 + 1;
    //        return new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59, 999);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="date"></param>
    //    /// <returns></returns>
    //    public static int DaysSinceYear0(DateTime date)
    //    {
    //        TimeSpan span = date - new DateTime(0);
    //        return span.Days;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="days"></param>
    //    /// <returns></returns>
    //    public static DateTime DateFromDaysSinceYear0(int days)
    //    {
    //        DateTime refdate = new DateTime(0);
    //        return refdate.AddDays(days);
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="date1"></param>
    //    /// <param name="date2"></param>
    //    /// <returns></returns>
    //    public static int MonthsBetween(DateTime date1, DateTime date2)
    //    {
    //        if (date1 > date2)
    //            return MonthsSinceYear0(date1) - MonthsSinceYear0(date2);
    //        return MonthsSinceYear0(date2) - MonthsSinceYear0(date1);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="month"></param>
    //    /// <returns></returns>
    //    public static String ShortMonthText(int month)
    //    {
    //        switch (month)
    //        {
    //            case 1: return "Jan";
    //            case 2: return "Feb";
    //            case 3: return "Mar";
    //            case 4: return "Apr";
    //            case 5: return "May";
    //            case 6: return "Jun";
    //            case 7: return "Jul";
    //            case 8: return "Aug";
    //            case 9: return "Sep";
    //            case 10: return "Oct";
    //            case 11: return "Nov";
    //            case 12: return "Dec";

    //        }
    //        return "";
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="value"></param>
    //    /// <returns></returns>
    //    public static int TextToMonth(string value)
    //    {
    //        if (String.Equals(value, "Jan")) return 1;
    //        else if (String.Equals(value, "Feb")) return 2;
    //        else if (String.Equals(value, "Mar")) return 3;
    //        else if (String.Equals(value, "Apr")) return 4;
    //        else if (String.Equals(value, "May")) return 5;
    //        else if (String.Equals(value, "Jun")) return 6;
    //        else if (String.Equals(value, "Jul")) return 7;
    //        else if (String.Equals(value, "Aug")) return 8;
    //        else if (String.Equals(value, "Sep")) return 9;
    //        else if (String.Equals(value, "Oct")) return 10;
    //        else if (String.Equals(value, "Nov")) return 11;
    //        else if (String.Equals(value, "Dec")) return 12;

    //        return -1;
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="dateTime"></param>
    //    /// <returns></returns>
    //    public static DateTime StartOfTheDay(DateTime dateTime)
    //    {
    //        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="dateTime"></param>
    //    /// <returns></returns>
    //    public static DateTime StartOfTheMonth(DateTime dateTime)
    //    {
    //        return new DateTime(dateTime.Year, dateTime.Month, 1);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="dateTime"></param>
    //    /// <returns></returns>
    //    public static DateTime StartOfTheYear(DateTime dateTime)
    //    {
    //        return new DateTime(dateTime.Year, 1, 1);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="dateTime"></param>
    //    /// <returns></returns>
    //    public static DateTime EndOfTheDay(DateTime dateTime)
    //    {
    //        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="dateTime"></param>
    //    /// <returns></returns>
    //    public static DateTime EndOfTheMonth(DateTime dateTime)
    //    {
    //        return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month), 23, 59, 59, 999);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="dateTime"></param>
    //    /// <returns></returns>
    //    public static DateTime EndOfTheYear(DateTime dateTime)
    //    {
    //        return new DateTime(dateTime.Year, 12, 31, 23, 59, 59, 999);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="index"></param>
    //    /// <returns></returns>
    //    public static String GetMonthTextFromIndex(int index)
    //    {
    //        try
    //        {
    //            switch (index)
    //            {
    //                case 1: return "January";
    //                case 2: return "February";
    //                case 3: return "March";
    //                case 4: return "April";
    //                case 5: return "May";
    //                case 6: return "June";
    //                case 7: return "July";
    //                case 8: return "August";
    //                case 9: return "September";
    //                case 10: return "October";
    //                case 11: return "November";
    //                case 12: return "December";
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            ErrorLogger.HandleError(ex, "", true);
    //        }
    //        return "";

    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="start"></param>
    //    /// <param name="end"></param>
    //    /// <param name="shortformat"></param>
    //    /// <param name="longformat"></param>
    //    /// <param name="joiner"></param>
    //    /// <returns></returns>
    //    public static String GetFormattedPeriodTextForStartDate(DateTime start, DateTime end,
    //        String shortformat, String longformat, String joiner)
    //    {
    //        try
    //        {
    //            int startyear = start.Year;
    //            int endyear = end.Year;
    //            if (startyear == endyear)
    //            {
    //                return $"{start.ToString(shortformat)}{joiner}{end.ToString(longformat)}";
    //            }
    //            else
    //            {
    //                return $"{start.ToString(longformat)}{joiner}{end.ToString(longformat)}";
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            ErrorLogger.HandleError(ex, "", true);
    //        }
    //        return "";

    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="index"></param>
    //    /// <returns></returns>
    //    public static String GetShortMonthTextFromIndex(int index)
    //    {
    //        try
    //        {
    //            if (index > 12) index -= 12;
    //            switch (index)
    //            {
    //                case 1: return "Jan";
    //                case 2: return "Feb";
    //                case 3: return "Mar";
    //                case 4: return "Apr";
    //                case 5: return "May";
    //                case 6: return "Jun";
    //                case 7: return "Jul";
    //                case 8: return "Aug";
    //                case 9: return "Sep";
    //                case 10: return "Oct";
    //                case 11: return "Nov";
    //                case 12: return "Dec";
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            ErrorLogger.HandleError(ex, "", true);
    //        }
    //        return "";
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="startday"></param>
    //    /// <param name="startmonth"></param>
    //    /// <param name="endday"></param>
    //    /// <param name="endmonth"></param>
    //    /// <param name="year"></param>
    //    /// <returns></returns>
    //    public static int CalculateNumberOfDaysBetweenMonthsAndDays(int startday, int startmonth, int endday, int endmonth, int year)
    //    {
    //        try
    //        {
    //            int endyear;

    //            if (startmonth < endmonth)
    //                endyear = year;
    //            else if (startmonth == endmonth)
    //            {
    //                if (startday < endday)
    //                    endyear = year;
    //                else
    //                    endyear = year + 1;
    //            }
    //            else
    //                endyear = year + 1;

    //            DateTime startdate = new DateTime(year, startmonth, startday);
    //            int maxday = DateTime.DaysInMonth(endyear, endmonth);
    //            if (endday > maxday) endday = maxday;
    //            DateTime enddate = new DateTime(endyear, endmonth, endday);
    //            return (enddate - startdate).Days + 1;
    //        }
    //        catch (Exception ex)
    //        {
    //            ErrorLogger.HandleError(ex, "", true);
    //        }
    //        return 0;
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="startmonth"></param>
    //    /// <param name="endmonth"></param>
    //    /// <param name="year"></param>
    //    /// <returns></returns>
    //    public static int CalculateNumberOfDaysBetweenMonths(int startmonth, int endmonth, int year)
    //    {
    //        try
    //        {
    //            int endyear;
    //            int startday = 1; //dont really need to have this as I will only be letting users select monthly periods
    //            if (startmonth <= endmonth)
    //                endyear = year;
    //            else
    //                endyear = year + 1;

    //            while (endmonth > 12)
    //            {
    //                endmonth = endmonth - 12;
    //                endyear = endyear + 1;
    //            }
    //            DateTime startdate = new DateTime(year, startmonth, startday);
    //            DateTime enddate = new DateTime(endyear, endmonth, DateTime.DaysInMonth(endyear, endmonth));
    //            return (enddate - startdate).Days + 1;
    //        }
    //        catch (Exception ex)
    //        {
    //            ErrorLogger.HandleError(ex, "", true);
    //        }
    //        return 0;
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="date1"></param>
    //    /// <param name="date2"></param>
    //    /// <returns></returns>
    //    public static bool IsTheSameDay(DateTime date1, DateTime date2)
    //    {
    //        return (date1.Year == date2.Year && date1.DayOfYear == date2.DayOfYear);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="year"></param>
    //    /// <param name="jday"></param>
    //    /// <returns></returns>
    //    public static DateTime GetDateFromYearAndJDay(int year, int jday)
    //    {
    //        return new DateTime(year, 1, 1).AddDays(jday - 1);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="date"></param>
    //    /// <param name="startWindow"></param>
    //    /// <param name="endWindow"></param>
    //    /// <returns></returns>
    //    public static bool isDateInWindow(DateTime date, DateTime startWindow, DateTime endWindow)
    //    {
    //        if (date >= startWindow && date <= endWindow)
    //        {
    //            return true;
    //        }
    //        return false;
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="date"></param>
    //    /// <param name="sequence"></param>
    //    /// <returns></returns>
    //    public static int IsDateInSequenceList(DateTime date, Sequence sequence)
    //    {
    //        return sequence.Dates.IndexOf(date);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="date"></param>
    //    /// <param name="startWindow"></param>
    //    /// <param name="endWindow"></param>
    //    /// <returns></returns>
    //    public static bool IsDateInWindow(DateTime date, DayMonthData startWindow, DayMonthData endWindow)
    //    {
    //        int currYear = date.Year;

    //        DateTime _startWindow = new DateTime(currYear, startWindow.Month, startWindow.Day);
    //        DateTime _endWindow = new DateTime(currYear, endWindow.Month, endWindow.Day);


    //        if (_startWindow <= _endWindow)
    //        {
    //            //Easiest case 
    //            return (date >= _startWindow && date <= _endWindow);
    //        }
    //        else
    //        {
    //            //Start and end window are in different years
    //            if (date > _endWindow)
    //            {
    //                return false;
    //            }
    //            else if (date <= _endWindow && date >= new DateTime(currYear, 1, 1))
    //            {
    //                return true;
    //            }
    //            else if (date < _startWindow)
    //            {
    //                return false;
    //            }
    //            else if (date >= _startWindow && date <= new DateTime(currYear, 12, 31))
    //            {
    //                return true;
    //            }
    //        }
    //        return false;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="text"></param>
    //    /// <returns></returns>
    //    public static DateTime? TryParseDate(string text, string format = "yyyyMMdd")
    //    {
    //        if (text.Length == format.Length)
    //        {
    //            DateTime dateTime;
    //            if (DateTime.TryParseExact(text, format, CultureInfo.InvariantCulture, DateTimeStyles.None,
    //                out dateTime))
    //            {
    //                return dateTime;
    //            }
    //        }

    //        return null;
    //    }
    //}
}
