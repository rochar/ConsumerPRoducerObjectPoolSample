using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ConsoleAppProducerConsumer
{
    public partial class Benchmark
    {
        internal class ProducerWithPool
        {
            private readonly ChannelWriter<DomainDto> _writer;
            private readonly int _objectsToProduce;
            private readonly ObjectPool<DomainDto> _objectPool;
            private readonly Func<int, ExternalDto> _simulateProducerWork;

            public ProducerWithPool(ChannelWriter<DomainDto> writer, int objectsToProduce, Func<int, ExternalDto> simulateProducerWork, ObjectPool<DomainDto> objectPool)
            {
                _writer = writer;
                _objectsToProduce = objectsToProduce;
                _objectPool = objectPool;
                _simulateProducerWork = simulateProducerWork;

            }

            public async Task BeginProducing()
            {
                for (var i = 0; i < _objectsToProduce; i++)
                {

#if DEBUG
                    if (i % 1000 == 0)
                    {
                        Console.WriteLine("Producer " + i);
                    }
#endif

                    //assume that this were retrieve from another component / system                     
                    var externalDto = _simulateProducerWork(i);


                    var domainDto = _objectPool.Rent();

                    //map to consumer dto
                    domainDto.Id = externalDto.Id;
                    domainDto.Status = externalDto.Status ? DomainDto.StatusValue.Ok : DomainDto.StatusValue.NotOk;

                    await _writer.WriteAsync(domainDto).ConfigureAwait(false);
                }
            }
        }
    }
}