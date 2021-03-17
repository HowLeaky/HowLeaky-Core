using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Tools
{
    class MathTools
    {
        public static int MISSING_DATA_VALUE = -32768;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool DoublesAreEqual(double a, double b)
        {
            if ((Math.Abs(a - b) < 0.0000001))
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static double CheckConstraints(double value, double max, double min)
        {
            if (value >= min && value <= max)
                return value;
            else if (value < min)
                return min;
            return max;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        /// <returns></returns>
        public static double Divide(double numerator, double denominator)
        {
            if (!DoublesAreEqual(denominator , 0) && !DoublesAreEqual(denominator,  MISSING_DATA_VALUE))
            {
                return numerator / denominator;
            }
            //Probably should throw something here
            return 0;
        }

        public static void LogDivideByZeroError(string fn, string zerovar, string calcvar)
        {
            //	if(ZerosList->Count==0)
            //	{
            //
            //		if(CurrentSimulationObject)
            //		ZerosList->Add("WARNING!!! for "+CurrentSimulationObject->Name);
            //
            //		ZerosList->Add("******* DETAILS *********");
            //		ZerosList->Add("There were divide by zero errors trapped during simulations in the function \""+fn+"\"");
            //		ZerosList->Add("The variable(s) "+zerovar+" was found to be zero while trying to calculate "+calcvar);
            //	}
            //	else
            //	{
            //		String text1="There were divide by zero errors trapped during simulations in the function \""+fn+"\"";
            //		String text2="The variable(s) "+zerovar+" was found to be zero while trying to calculate "+calcvar;
            //		bool foundtext=false;
            //		for(int i=0;i<ZerosList->Count;++i)
            //		{
            //			if(ZerosList->Strings[i]==text2)
            //			{
            //				foundtext=true;
            //				i=ZerosList->Count;
            //			}
            //		}
            //		if(!foundtext)
            //		{
            //			ZerosList->Add(text1);
            //			ZerosList->Add(text2);
            //		}
            //
            //	}
        }
    }
}
