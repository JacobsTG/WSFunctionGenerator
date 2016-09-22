using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSFunctionGenerator
{
    class Program
    {

        static void Main(string[] args)
        {
            var server = new Server("ws://0.0.0.0:50000");
            server.Start();

            var input = Console.ReadLine();
        }
    }
}
