using BenchmarkDotNet.Running;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PMPumpParser
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
            Console.WriteLine("Summary lenth: " + summary.Length);
        }
    }
}
