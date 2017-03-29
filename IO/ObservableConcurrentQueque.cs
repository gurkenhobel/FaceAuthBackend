using System;
using System.Collections.Concurrent;
namespace FaceAuthService.IO
{
    class ObservableConcurrentQueque<T>
    {
        public event EventHandler CollectionChanged;

        private ConcurrentQueue<T> _queque;

        public int Count
        {
            get
            {
                return _queque.Count;
            }
        }

        public ObservableConcurrentQueque()
        {
            _queque = new ConcurrentQueue<T>();
        }

        public void Enqueque(T item)
        {
            _queque.Enqueue(item);
            CollectionChanged?.Invoke(this, null);
        }

        public T Dequeque()
        {
            T res = default(T);
            if(_queque.TryDequeue(out res))
                CollectionChanged?.Invoke(this, null);
            return res;
        }
    }
}