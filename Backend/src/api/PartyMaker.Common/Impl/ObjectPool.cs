using System;
using System.Collections.Generic;
using System.Threading;

namespace PartyMaker.Common.Impl
{
    public class ObjectPool<T>
    {
        private readonly AutoResetEvent _poolModifiedEvent;
        private readonly Queue<T> _pool;
        private readonly object _accessSync;
        private readonly int _maxSize;
        private readonly List<T> _poolGeneratedItems;

        private int _generatedItemsCount;
        private Func<T> _generator;
        private Action<T> _releaser;

        public ObjectPool(Func<T> generator, Action<T> releaser, int maxSize)
        {
            _generator = generator;
            _releaser = releaser;
            _maxSize = maxSize;

            _pool = new Queue<T>();
            _poolGeneratedItems = new List<T>();
            _poolModifiedEvent = new AutoResetEvent(false);
            _accessSync = new object();
        }

        public ObjectPool(Func<T> generator, int maxSize)
        {
            _generator = generator;
            _maxSize = maxSize;

            _pool = new Queue<T>();
            _poolGeneratedItems = new List<T>();
            _poolModifiedEvent = new AutoResetEvent(false);
            _accessSync = new object();
        }

        public ObjectPool(Func<T> generator, Action<T> releaser, int initialSize, int maxSize)
        {
            _generator = generator;
            _releaser = releaser;
            _maxSize = maxSize;
            _generatedItemsCount = initialSize;

            _pool = new Queue<T>();
            _poolGeneratedItems = new List<T>();
            _accessSync = new object();
            _poolModifiedEvent = new AutoResetEvent(false);

            Prepopulate(initialSize);
        }

        public ObjectPool(Func<T> generator, int initialSize, int maxSize)
        {
            _generator = generator;
            _maxSize = maxSize;
            _generatedItemsCount = initialSize;

            _pool = new Queue<T>();
            _poolGeneratedItems = new List<T>();
            _accessSync = new object();
            _poolModifiedEvent = new AutoResetEvent(false);

            Prepopulate(initialSize);
        }

        protected ObjectPool(int maxSize)
        {
            _maxSize = maxSize;

            _pool = new Queue<T>();
            _poolGeneratedItems = new List<T>();
            _poolModifiedEvent = new AutoResetEvent(false);
            _accessSync = new object();
        }

        public virtual T Get()
        {
            lock (_accessSync)
            {
                if (_maxSize > _generatedItemsCount)
                {
                    if (_pool.Count > 0)
                    {
                        return _pool.Dequeue();
                    }

                    _generatedItemsCount++;
                    var item = _generator();

                    _poolGeneratedItems.Add(item);

                    return item;
                }
            }

            while (true)
            {
                _poolModifiedEvent.WaitOne();

                lock (_accessSync)
                {
                    if (_pool.Count > 0)
                    {
                        return _pool.Dequeue();
                    }
                }
            }
        }

        public virtual void Release(T item)
        {
            lock (_accessSync)
            {
                if (!_poolGeneratedItems.Contains(item))
                {
                    throw new InvalidOperationException("Object does not belong to pool");
                }

                _releaser?.Invoke(item);

                _pool.Enqueue(item);
            }

            _poolModifiedEvent.Set();
        }

        protected void SetGenerator(Func<T> generator)
        {
            _generator = generator;
        }

        protected void SetReleaser(Action<T> releaser)
        {
            _releaser = releaser;
        }

        protected void Prepopulate(int initialSize)
        {
            for (int i = 0; i < initialSize; i++)
            {
                var item = _generator();

                _pool.Enqueue(item);
                _poolGeneratedItems.Add(item);
            }
        }
    }
}
