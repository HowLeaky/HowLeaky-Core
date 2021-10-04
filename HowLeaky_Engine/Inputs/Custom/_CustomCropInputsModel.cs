using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_SimulationEngine.Inputs
{
    public class _CustomCropInputsModel:_CustomHowLeakyInputsModel
    {
        public _CustomCropInputsModel():base()
        {
        }

        public _CustomCropInputsModel(Guid? id, string name):base(id,name)
        {

        }
        public bool IsLAI()
        {
            return this is HowLeakyInputs_LAIVeg;
        }
    }
}
