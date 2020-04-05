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
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetAsync("https://bit.ly/2R7OofY").ConfigureAwait(false);
            var responseBody = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            var _parser = new CustomTextParser(responseBody);
            Console.WriteLine(_parser.ParseUsingDictionaryAndSpan());
            // Uncomment to run benchmark
            // var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
            // Console.WriteLine("Summary lenth: " + summary.Length);
        }
    }
}
