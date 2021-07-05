using System;
using System.Collections.Generic;
using System.Text;

namespace HowLeaky_Engine.UnitTests
{
    public class _CustomUnitTestClass
    {
        public bool ValuesAreEqual(double val1, double val2)
        {
            var engine = new HowLeakyEngine();
            return Math.Abs(val1 - val2) < 0.000001;
        }
    }
}
