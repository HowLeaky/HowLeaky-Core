using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace HowLeaky_SimulationEngine.Errors
{

    public class SourceInfo
    {
        public SourceInfo (string className, string methodName, string lineNumber)
        {
            ClassName=className;
            MethodName=methodName;
            LineNumber=lineNumber;
        }
        public string ClassName{get;set;}
        public string MethodName{get;set;}
        public string LineNumber{get;set;}

        internal string ExtractMessage()
        {
            return $"Exception rethrown from {MethodName} in {ClassName} on line {LineNumber}";
        }
    }
     public class ErrorLogger
    {
        static readonly object newlock = new object();

        public static Exception CreateException(Exception ex, string parameters=null)
        {           
             var sourceInfo=ExtractParams(ex);
            var message=sourceInfo!=null?sourceInfo.ExtractMessage():"Undefined <source info not found>";
            var newexception=new Exception(message, ex);
            if(ex.Data.Contains("sourceClassName"))
            {
                newexception.Data["sourceClassName"]=ex.Data["sourceClassName"];   
                newexception.Data["sourceMethodName"]=ex.Data["sourceMethodName"];   
                newexception.Data["sourceLineNumber"]=ex.Data["sourceLineNumber"];   
            }
            else            
            {   
                if(sourceInfo!=null)
                {
                    newexception.Data["sourceClassName"]=sourceInfo.ClassName;
                    newexception.Data["sourceMethodName"]=sourceInfo.MethodName; 
                    newexception.Data["sourceLineNumber"]=sourceInfo.LineNumber;
                }                
            }
            return newexception;            
        }

        
         public static SourceInfo ExtractParams(Exception e)
        {
            try
            {
                var dict=new Dictionary<string,string>();
                MethodBase site = e.TargetSite;//Get the methodname from the exception.
                string methodName = site == null ? "" : site.Name;//avoid null ref if it's null.
                methodName = ExtractBracketed(methodName);

                StackTrace stkTrace = new System.Diagnostics.StackTrace(e, true);
                for (int i = 0; i < 3; i++)
                {
                    //In most cases GetFrame(0) will contain valid information, but not always. That's why a small loop is needed. 
                    var frame = stkTrace.GetFrame(i);
                    int lineNum = frame.GetFileLineNumber();//get the line and column numbers
                    int colNum = frame.GetFileColumnNumber();
                    string className = ExtractBracketed(frame.GetMethod().ReflectedType.FullName);
                    
                   
                    if (lineNum + colNum > 0)
                    {
                        return new SourceInfo(className,methodName,lineNum.ToString());                        
                    }
                        
                }
                return null;

            }
            catch (Exception ee)
            {
                //Avoid any situation that the Trace is what crashes you application. While trace can log to a file. Console normally not output to the same place.
                Console.WriteLine("Tracing exception in d(Exception e)" + ee.Message);
            }
            return null;
        }

       
        private static string ExtractBracketed(string str)
        {
            string s;
            if (str.IndexOf('<') > -1) //using the Regex when the string does not contain <brackets> returns an empty string.
                s = Regex.Match(str, @"\<([^>]*)\>").Groups[1].Value;
            else
                s = str; 
            if (s == "")
                return  "'Emtpy'"; //for log visibility we want to know if something it's empty.
            else
                return s;

        }
    }
}
