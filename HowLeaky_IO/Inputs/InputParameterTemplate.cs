using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HowLeaky_IO
{
    public enum ParameterType
    {
        None = 0,
        Label = 25,
        InputEditText = 50,
        InputEditValue = 100,
        InputSelectValue = 200,
        InputSelectEnum = 250,
        InputSelectDbRecord = 300,
        InputEditVector = 400,
        InputSelectVector = 500,
        InputCalculated = 600,
        InputTimeSeries=700,
        Output = 800
    }
    public class DataSetParameterTemplate
    {
        public Guid Id { get; set; }
        public Guid? DataSetTemplateId { get;  set; }
        public DateTime?CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime?ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Comments { get; set; }

        public string SourceFileName { get; set; }       
        public string LabelTitle { get; set; }
        public string CodeName { get; set; }        
        public string ClassName { get; set; }       
        public string Description { get;  set; }
        public string SuggestedValues{get;set;}                                       
        public int OrderIndex { get; set; }      
        public ParameterType InputType { get; set; }
        public string DefaultValue { get; set; }
        public string Units { get; set; }
        public string MinValue { get; set; }
        public string MaxValue { get; set; }
        public string Step{get;set;}
        public string EnumCsv { get; set; }
        public int DecimalPlaces { get; set; }
        public string DbTableName{get;set;}
      //  public HowLeakyDataType Type { get; set; }

        internal bool ContainsSearchTerm(string search)
        {
            if(!String.IsNullOrEmpty(CodeName)&&CodeName.ToLower().Contains(search))return true;
            if(!String.IsNullOrEmpty(LabelTitle)&&LabelTitle.ToLower().Contains(search))return true;
            if(!String.IsNullOrEmpty(SourceFileName)&&SourceFileName.ToLower().Contains(search))return true;
            if(!String.IsNullOrEmpty(Description)&&Description.ToLower().Contains(search))return true;
            return false;
        }
    }
}
