using HowLeaky_SimulationEngine.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Attributes
{
     public class Output : Attribute
    {
        public String Description { get; set; }
        public String Unit { get; set;}
        public double Scale { get; set; } = 1;

        public AggregationTypeEnum AggregationType { get; set; } = AggregationTypeEnum.Mean;
        public AggregationSequenceEnum AggregationSequence { get; set; } = AggregationSequenceEnum.Always;

        public Output() { }

        public Output(string Description ="", string Unit = "", double Scale = 1, AggregationTypeEnum AggregationType = AggregationTypeEnum.Mean, AggregationSequenceEnum AggregationSequence = AggregationSequenceEnum.Always)
        {
            this.Description = Description;
            this.Unit = Unit;
            this.Scale = Scale;

            this.AggregationType = AggregationType;
            this.AggregationSequence = AggregationSequence;
        }
    }
}
