namespace ConsoleAppProducerConsumer
{
    internal sealed class DomainDto
    {
        public int Id { get; set; }
        public StatusValue Status { get; set; }

        internal enum StatusValue
        {
            Ok,
            NotOk
        }
    }
}