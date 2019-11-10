using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ConsoleAppProducerConsumer
{
    public partial class Benchmark
    {
        internal class Producer
        {
            private readonly ChannelWriter<DomainDto> _writer;
            private readonly int _objectsToProduce;
            private readonly Func<int, ExternalDto> _simulateProducerWork;

            public Producer(ChannelWriter<DomainDto> writer, int objectsToProduce, Func<int, ExternalDto> simulateProducerWork)
            {
                _writer = writer;
                _objectsToProduce = objectsToProduce;
                _simulateProducerWork = simulateProducerWork;
            }

            public async Task BeginProducing()
            {
                for (var i = 0; i < _objectsToProduce; i++)
                {
                    //assume that this were retrieve from another component / system                     
                    var externalDto = _simulateProducerWork(i);

                    //map to domain dto
                    var domainDto = new DomainDto()
                    {
                        Id = externalDto.Id,
                        Status = externalDto.Status ? DomainDto.StatusValue.Ok : DomainDto.StatusValue.NotOk
                    };
                    await _writer.WriteAsync(domainDto);
                }
            }
        }
    }
}