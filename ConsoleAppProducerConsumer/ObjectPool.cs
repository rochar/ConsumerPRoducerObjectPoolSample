using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppProducerConsumer
{
    public sealed class ObjectPool<T>
    {
        private readonly Stack<T> _objects;
        private readonly Func<T> _objectGenerator;
        private readonly int _maxElements;
        private readonly object _lockMaxElements = new object();

        public ObjectPool(Func<T> objectGenerator, int maxElements)
        {
            _objects = new Stack<T>(maxElements);
            _objectGenerator = objectGenerator;
            _maxElements = maxElements;
        }

        public int Count => _objects.Count;
        public T Rent()
        {
            lock (_lockMaxElements)
            {
                if (_objects.Count > 0)
                {
                    return _objects.Pop();
                }
            }
            return _objectGenerator();
        }

        public void Return(T item)
        {
            lock (_lockMaxElements)
            {
                if (_objects.Count < _maxElements)
                {
                    _objects.Push(item);
                }
            }
        }
    }
}
