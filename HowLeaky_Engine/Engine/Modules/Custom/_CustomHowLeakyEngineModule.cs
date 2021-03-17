using System;

namespace HowLeaky_SimulationEngine.Engine
{
    public class _CustomHowLeakyEngineModule
    {

        public _CustomHowLeakyEngineModule() { }

        public _CustomHowLeakyEngineModule(HowLeakyEngine sim)
        {
            this.Engine = sim;
        }
      
        public HowLeakyEngine Engine { get; set; }

        public string Name{get;set;}
      
        public virtual void Initialise() { }

        public virtual void Simulate() { }

        public virtual void SetStartOfDayParameters() {}

        public virtual void ResetAnyParametersIfRequired() {}
        
        public void LogDivideByZeroError(string s, string s2, string s3)
        {
            
               throw new Exception("Divide by 0 error in {s} when calculate {s3}: {s2} was 0");
            
        }
       
    }
}
