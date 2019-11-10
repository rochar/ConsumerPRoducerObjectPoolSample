using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ConsoleAppProducerConsumer
{
    public partial class Benchmark
    {
        internal class ConsumerWithPool
        {
            private readonly ChannelReader<DomainDto> _reader;
            private readonly Action<DomainDto> _processResult;
            private readonly ObjectPool<DomainDto> _objectPool;

            public ConsumerWithPool(ChannelReader<DomainDto> reader, Action<DomainDto> processResult, ObjectPool<DomainDto> objectPool)
            {
                _reader = reader;
                _processResult = processResult;
                _objectPool = objectPool;
            }

            public async Task ConsumeData()
            {
                int i = 0;
                while (await _reader.WaitToReadAsync().ConfigureAwait(false))
                {
                    if (_reader.TryRead(out var data))
                    {
                        i++;
#if DEBUG
                        if (i % 1000 == 0)
                        {
                            Console.WriteLine("Consumer " + i);
                        }
#endif
                        _processResult(data);
                        _objectPool.Return(data);
                    }
                }
            }
        }
    }
}