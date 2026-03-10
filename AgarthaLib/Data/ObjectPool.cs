using System;
using System.Collections.Generic;

namespace AgarthaLib.Data
{
    public class ObjectPool<T>
    {
        public int Count { get; protected set; }

        protected readonly Queue<T> _available = new();
        protected readonly Func<T> _createObjectFunc;

        public ObjectPool(int numPreallocated = 0, Func<T> createObjectFunc = null)
        {
            _createObjectFunc = createObjectFunc;

            if (numPreallocated > 0)
                Give(MakeObject());
        }

        public T Take()
        {
            if (_available.Count > 0)
            {
                Count--;
                return _available.Dequeue();
            }
            else return MakeObject();
        }

        public void Give(T t)
        {
            Count++;
            _available.Enqueue(t);
        }

        private T MakeObject()
        {
            if (_createObjectFunc != null)
                return _createObjectFunc();
            else throw new InvalidOperationException("No factory method provided.");
        }
    }
}
