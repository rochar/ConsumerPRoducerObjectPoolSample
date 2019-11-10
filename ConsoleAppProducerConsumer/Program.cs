using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;

namespace ConsoleAppProducerConsumer
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
           BenchmarkRunner.Run<Benchmark>();

           Console.WriteLine("Completed");
           Console.ReadKey();
        }
    }
}