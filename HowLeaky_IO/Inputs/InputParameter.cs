
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HowLeaky_IO
{
    public class InputParameter
    {
        public InputParameter()
        {

        }

        public Guid Id { get; set; }
   
        public string NameInFile { get; set; }
        public string NameInCode {get;set;}
        public string Value { get; set; }
     
    }
}
