using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSFunctionGenerator
{
    public class Command
    {
        /// <summary>
        /// Specifies the action for the server to take.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Specifies the type of function for the server to generate.
        /// </summary>
        public string FunctionType { get; set; }
    }
}
