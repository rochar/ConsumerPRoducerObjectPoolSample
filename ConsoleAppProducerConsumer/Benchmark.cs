using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Mathematics;

namespace ConsoleAppProducerConsumer
{
    [MemoryDiagnoser]
    public partial class Benchmark
    {
        [Params(100000,1000000, 5000000)]
        public int NrOfElements { get; set; }

        private void SimulateConsumerWork(DomainDto domainDto)
        {
            //simulate some work - Faster 
            if (domainDto.Id % 15000 == 0)
            { 
                Thread.Sleep(1); 
            }
        }
        private ExternalDto SimulateProducerWork(int i)
        {
            //simulate some work - slower 
            if (i % 1500 == 0)
            {
                Thread.Sleep(1);
            }
            return new ExternalDto()
            {
                Id = i,
                Status = i % 2 == 0
            };
        }

        [Benchmark]
        public async Task ProducerConsumer()
        {
            var channel = Channel.CreateUnbounded<DomainDto>();

            var producer1 = new Producer(channel.Writer, NrOfElements, SimulateProducerWork);
            var consumer1 = new Consumer(channel.Reader, SimulateConsumerWork);

            Task consumerTask1 = consumer1.ConsumeData(); // begin consuming
            Task producerTask1 = producer1.BeginProducing(); // begin producing

            await producerTask1.ContinueWith(_ => channel.Writer.Complete());

            await consumerTask1;
        }

        [Benchmark]
        public async Task ProducerConsumerFromPool()
        {
            var channel = Channel.CreateUnbounded<DomainDto>();

            var domainDtoPool = new ObjectPool<DomainDto>(()=> new DomainDto(), 2000);

            var producer1 = new ProducerWithPool(channel.Writer, NrOfElements, SimulateProducerWork,domainDtoPool);
            var consumer1 = new ConsumerWithPool(channel.Reader, SimulateConsumerWork, domainDtoPool);

            Task consumerTask1 = consumer1.ConsumeData(); // begin consuming
            Task producerTask1 = producer1.BeginProducing(); // begin producing

            await producerTask1.ContinueWith(_ => channel.Writer.Complete());

            await consumerTask1;
        }
       
    }
}
