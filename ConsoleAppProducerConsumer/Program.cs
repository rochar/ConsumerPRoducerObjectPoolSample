using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;

namespace ConsoleAppProducerConsumer
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {

            //Benchmark b = new Benchmark();
            //b.NrOfElements = 5000000;
            //await b.ProducerConsumerFromPool();
           BenchmarkRunner.Run<Benchmark>();

            Console.WriteLine("Completed");
            Console.ReadKey();
        }
    }
}