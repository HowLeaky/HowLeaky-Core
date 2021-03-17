// Developed by DHM Environmental Software Engineering Pty. Ltd. 2020
// The Copyright of this file belongs Dairy Australia Pty. Ltd. and/or DHM Environmental Software Engineering Pty. Ltd. (hereafter known as DHM) and selected clients of DHM.
// No content of this file may be reproduced, modified or used in software development without the express written permission from the copyright holders.
// Where permission has been granted to use or modify this file, the full copyright information must remain unchanged at the top of the file.
// Where permission has been granted to modify this file, changes must be clearly identified through adding comments and annotations to the source-code,
// and a description of the changes (including who has made the changes), must be included after this copyright information.

using System;
using System.Collections.Generic;



namespace HowLeaky_ValidationEngine.Maths.Regression
{
    public enum DatesAgreement
    {
        Undefined,
        DatesAgree,
        FirstSmaller,
        FirstLarger
    }
    public enum ScatterFilter
    {
        All,
        XValues,
        YValues
    }


    public class RegressionObject
    {
        double ticks_conversion;
        public RegressionObject(double ticks_conv)
        {
            ticks_conversion = ticks_conv;
        }
        public double? Slope { get; set; }
        public double? SlopeYr { get; set; }
        public double? Intercept { get; set; }
        public double RSquared { get; set; }
        public double R { get; set; }
        public double PValue { get; set; }
        public double RMSE { get; set; }
        public double SSEMeanObs { get; set; }
        public double SSE { get; set; }

        public double GetYValue(DateTime dateTime)
        {
            try
            {
                return (double)Slope * dateTime.Ticks / ticks_conversion + (double)Intercept;
            }
            catch (Exception ex)
            {

            }
            return 0;
        }
    }
    public enum ResultsStatus
    {
        rsSuccess,
        rsFailed
    }
    public class CalculationStatus
    {
        internal void BeginUpdate()
        {
            throw new NotImplementedException();
        }
        internal void EndUpdate()
        {
            throw new NotImplementedException();
        }

        public ResultsStatus Result { get; set; }

        public bool WasSuccessful { get; set; }

        public bool CanContinue { get; set; }
    }
    public class RegressionEngine
    {
        #region Fields
        RegressionObject results;
        double r;
        double rsquared;
        double sumx;
        double sumy;
        double sumxsqr;
        double sumysqr;
        double sumxy;
        double sumyminusymean_sqr;
        double sumyminusypredict_sqr;
        double sumxminusxmean_sqr;
        double sumymeanminusypredict_sqr;
        List<DateTime> dates;
        List<double> datavalues1;
        List<double> datavalues2;
        public bool OneIsToOne { get; set; }
        double ticks_conversion;
        public CalculationStatus CalcStatus { get; set; }

        #endregion

        #region Constructor
        public RegressionEngine()
        {
            dates = null;
            datavalues1 = null;
            datavalues2 = null;
            results = null;
        }

        public RegressionEngine(List<double> _datavalues1, List<double> _datavalues2)
        {
            dates = null;
            datavalues1 = _datavalues1;
            datavalues2 = _datavalues2;
            results = null;
        }

        //public RegressionEngine(BrowserDataArray values, BrowserDateArray dates)
        //{
        //    ConnectInputs(values, dates);
        //}

        //public RegressionEngine(BrowserDataArray values1, BrowserDataArray values2)
        //{
        //    ConnectInputs(values1, values2);
        //}

        //public RegressionEngine(BrowserScatterDataPair pair)
        //{
        //    ConnectInputs(pair.Values, pair.Values2);
        //}

        #endregion


        #region PUBLIC METHODS
        /// <summary>
        /// /// ConnectInputs is the main input into the RegressionEngine and MUST be
        /// called prior to calling Calculate. 
        /// </summary>
        /// <param name="timeseries1">first time series</param>
        /// <param name="timeseries2">second time series</param>
        /// <param name="definition">Definition for which to generate scatter plot/regression</param>
        //public void ConnectInputs(BrowserDataArray values, BrowserDateArray datearray)
        //{
        //    try
        //    {
        //        dates = datearray.Dates;
        //        datavalues2 = values.Values;
        //        ticks_conversion = 1000000.0 * 1000.0 * 1000.0 * 1000.0;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //public void ConnectInputs(BrowserDataArray values1, BrowserDataArray values2)
        //{
        //    try
        //    {
        //        datavalues1 = values1.Values;
        //        datavalues2 = values2.Values;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        /// <summary>
        /// Calculate is the key function that the user will call to generate a scatterplot, runn a regression
        /// and generate a BrowserScatterObject objects. If there is  an error in the analysis, then this 
        /// function will return null. Please ensure that ConnectInputs is called prior to calling this function.
        /// </summary>
        /// <returns>BrowserScatterObject object</returns>
        public RegressionObject Calculate()
        {
            try
            {
                if (Initialise())
                {
                    if (RunLinearRegression())
                        return results;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        #endregion

        bool Initialise()
        {
            try
            {
                if ((datavalues1 != null && datavalues2 != null) || (dates != null && datavalues2 != null))
                {
                    sumx = 0;
                    sumy = 0;
                    sumxsqr = 0;
                    sumysqr = 0;
                    sumxy = 0;
                    sumyminusymean_sqr = 0;
                    sumxminusxmean_sqr = 0;
                    sumyminusypredict_sqr = 0;
                    sumymeanminusypredict_sqr = 0;
                    results = new RegressionObject(ticks_conversion);
                    if (dates != null)
                        return dates.Count == datavalues2.Count;
                    return datavalues1.Count == datavalues2.Count;
                }
                return false;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        bool RunLinearRegression()
        {
            try
            {
                double n = datavalues2.Count;
                if (n > 0)
                {
                    double x, y, ymean, xmean, m, y1;
                    double p = 0;

                    for (int i = 0; i < n; ++i)
                    {
                        y = datavalues2[i];
                        if (datavalues1 != null)
                        {
                            x = datavalues1[i];
                        }
                        else
                        {
                            x = dates[i].Ticks / ticks_conversion;
                        }
                        sumx += x;
                        sumy += y;
                        sumxsqr += Math.Pow(x, 2);
                        sumysqr += Math.Pow(y, 2);
                        sumxy += (x * y);
                    }
                    ymean = sumy / n;
                    xmean = sumx / n;
                    if (!OneIsToOne)
                    {
                        m = (n * sumxy - sumx * sumy) / (n * sumxsqr - Math.Pow(sumx, 2));
                        y1 = 1 / n * (sumy - m * sumx);
                    }
                    else
                    {
                        m = 1;
                        y1 = 0;
                    }
                    for (int i = 0; i < n; ++i)
                    {
                        y = datavalues2[i];
                        if (datavalues1 != null)
                        {
                            x = datavalues1[i];
                        }
                        else
                        {
                            x = dates[i].Ticks / ticks_conversion;
                        }

                        double yminusymean = y - ymean;
                        double xminusmean = x - xmean;
                        sumyminusymean_sqr += Math.Pow(yminusymean, 2);
                        sumxminusxmean_sqr += Math.Pow(xminusmean, 2);

                        double ypredict = (m * x) + y1;
                        double yminusypredict = y - ypredict;
                        sumyminusypredict_sqr += Math.Pow(yminusypredict, 2);

                        sumymeanminusypredict_sqr += Math.Pow(ymean - ypredict, 2);
                    }
                    //SE = sb1 = sqrt [ Σ(yi - ŷi)2 / (n - 2) ] / sqrt [ Σ(xi - x)2 ]
                    //

                    if (sumyminusymean_sqr != 0)
                    {
                        double part1 = n * sumxsqr - Math.Pow(sumx, 2);
                        double part2 = n * sumysqr - Math.Pow(sumy, 2);
                        if (part1 >= 0 && part2 >= 0)
                        {
                            r = (n * sumxy - sumx * sumy) / (Math.Pow(part1, 0.5) * Math.Pow(part2, 0.5));
                            rsquared = 1 - sumyminusypredict_sqr / sumyminusymean_sqr;
                            if (rsquared < 0)
                            {
                                rsquared = 0;
                            }
                        }
                        else
                        {
                            r = 0;
                            rsquared = 0;
                            m = 0;
                        }
                    }
                    else
                    {
                        r = 0;
                        rsquared = 0;
                    }

                    if (n > 2)
                    {
                        if (rsquared < 1 && rsquared >= 0)
                        {
                            var standardError = Math.Sqrt(sumyminusypredict_sqr / (n - 2)) / Math.Sqrt(sumxminusxmean_sqr);
                            var t = m / standardError;
                            p = Student(t, n - 2);
                            //	t=fabs(r)*pow(n-2,0.5)/pow(1-rsquared,0.5);
                            //p = 0;//1-tDistriIntegral(t,n-2);    //dont have this library any more...
                        }
                        else
                        {
                            rsquared = 1;
                            p = 0;
                        }
                    }
                    results.Slope = m;
                    if (datavalues1 == null)
                    {
                        var totalspan = dates[dates.Count - 1] - dates[0];
                        var totalincrements = (dates[dates.Count - 1].Ticks - dates[0].Ticks) / ticks_conversion;
                        results.SlopeYr = m * totalincrements / (totalspan.TotalDays / 365.0f);
                    }

                    results.Intercept = y1;
                    results.RSquared = rsquared;

                    if (!OneIsToOne)
                    {
                        results.R = r;
                        results.PValue = p;
                    }
                    else
                    {
                        results.R = 0;
                        results.PValue = 0;
                    }
                    results.RMSE = Math.Pow(sumyminusypredict_sqr / n, 0.5);
                    results.SSEMeanObs = sumymeanminusypredict_sqr;
                    results.SSE = sumyminusypredict_sqr;
                    return true;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }




        //https://msdn.microsoft.com/en-us/magazine/mt620016.aspx
        public static double Student(double t, double df)
        {
            // for large integer df or double df
            // adapted from ACM algorithm 395
            // returns 2-tail p-value
            double n = df; // to sync with ACM parameter name
            double a, b, y;
            t = t * t;
            y = t / n;
            b = y + 1.0;
            if (y > 1.0E-6) y = Math.Log(b);
            a = n - 0.5;
            b = 48.0 * a * a;
            y = a * y;
            y = (((((-0.4 * y - 3.3) * y - 24.0) * y - 85.5) /
              (0.8 * y * y + 100.0 + b) + y + 3.0) / b + 1.0) *
              Math.Sqrt(y);
            return 2.0 * Gauss(-y); // ACM algorithm 209
        }

        public static double Gauss(double z)
        {
            // input = z-value (-inf to +inf)
            // output = p under Standard Normal curve from -inf to z
            // e.g., if z = 0.0, function returns 0.5000
            // ACM Algorithm #209
            double y; // 209 scratch variable
            double p; // result. called 'z' in 209
            double w; // 209 scratch variable
            if (z == 0.0)
                p = 0.0;
            else
            {
                y = Math.Abs(z) / 2;
                if (y >= 3.0)
                {
                    p = 1.0;
                }
                else if (y < 1.0)
                {
                    w = y * y;
                    p = ((((((((0.000124818987 * w
                      - 0.001075204047) * w + 0.005198775019) * w
                      - 0.019198292004) * w + 0.059054035642) * w
                      - 0.151968751364) * w + 0.319152932694) * w
                      - 0.531923007300) * w + 0.797884560593) * y * 2.0;
                }
                else
                {
                    y = y - 2.0;
                    p = (((((((((((((-0.000045255659 * y
                      + 0.000152529290) * y - 0.000019538132) * y
                      - 0.000676904986) * y + 0.001390604284) * y
                      - 0.000794620820) * y - 0.002034254874) * y
                      + 0.006549791214) * y - 0.010557625006) * y
                      + 0.011630447319) * y - 0.009279453341) * y
                      + 0.005353579108) * y - 0.002141268741) * y
                      + 0.000535310849) * y + 0.999936657524;
                }
            }
            if (z > 0.0)
                return (p + 1.0) / 2;
            else
                return (1.0 - p) / 2;
        }
    }
}
