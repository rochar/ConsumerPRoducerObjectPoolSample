using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ConsoleAppProducerConsumer
{
    public partial class Benchmark
    {
        internal class Consumer
        {
            private readonly ChannelReader<DomainDto> _reader;
            private readonly Action<DomainDto> _processResult;

            public Consumer(ChannelReader<DomainDto> reader, Action<DomainDto> processResult)
            {
                _reader = reader;
                _processResult = processResult;
            }

            public async Task ConsumeData()
            {
                while (await _reader.WaitToReadAsync())
                {
                    if (_reader.TryRead(out var data))
                    {
                        _processResult(data);
                    }
                }
            }
        }
    }
}