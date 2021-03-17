using System.Collections.Generic;

namespace HowLeaky_IO
{
    public class ParameterDataSetPtr
    {
        public ParameterDataSetPtr(ParameterDataSet dataset)
        {
            Overrides=new List<ParameterOverride>();
            DataSet=dataset;
        }

        public ParameterDataSet DataSet{get;set;}
        public List<ParameterOverride>Overrides{get;set;}
    }
}
