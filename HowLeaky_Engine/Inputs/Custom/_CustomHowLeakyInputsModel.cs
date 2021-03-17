using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace HowLeaky_SimulationEngine.Inputs
{
    public class _CustomHowLeakyInputsModel
    {
        public _CustomHowLeakyInputsModel(Guid? id, string name)
        {
            Id=id;
            Name=name;
        }
        public Guid? Id{get;set;}
        public string Name{get;set;}

     }
}
