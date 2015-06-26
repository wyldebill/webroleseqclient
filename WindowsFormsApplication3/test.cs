using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace WindowsFormsApplication3
{
    class test
    {
        public int Value { get; set; }
        public test()
        {
            Value = 10;

            Log.Information("" +
                "Creating test object with {value}", this.Value);

        }
    }
}
