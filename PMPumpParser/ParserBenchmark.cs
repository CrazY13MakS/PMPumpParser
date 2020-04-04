using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PMPumpParser
{
    //[Config(typeof(Config))]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser] // we need to enable it in explicit way
    //[RyuJitX64Job] // let's run the benchmarks for 32 & 64 bit
    public class ParserBenchmark
    {
        private static HttpClient httpClient = new HttpClient();
        private CustomTextParser _parser;

        [GlobalSetup]
        public async Task GlobalSetup()
        {
            var responseMessage = await httpClient.GetAsync("https://bit.ly/2R7OofY").ConfigureAwait(false);
            var responseBody = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            _parser = new CustomTextParser(responseBody);
        }

        [Benchmark]
        public void Dictionary()
        {
            _parser.ParseWithDictionary();
        }

        [Benchmark]
        public void Hashset()
        {
            _parser.ParseWithHashset();
        }

        [Benchmark]
        public void SpanAndHashset()
        {
            _parser.ParseWithSpanAndHashset();
        }

        [Benchmark]
        public void SpanAndDictionary()
        {
            _parser.ParseUsingDictionaryAndSpan();
        }


    }
}
