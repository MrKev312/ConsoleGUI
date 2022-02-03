using System;
using System.Collections.Concurrent;

namespace RW.EventfulConcurrentQueue
{
    public sealed class EventfulConcurrentQueue<T>
    {
        private readonly ConcurrentQueue<T> _queue;

        public EventfulConcurrentQueue()
        {
            _queue = new ConcurrentQueue<T>();
        }

        public void Enqueue(T item)
        {
            _queue.Enqueue(item);
            OnItemEnqueued();
        }

        public bool TryDequeue(out T result)
        {
            var success = _queue.TryDequeue(out result);

            if (success)
            {
                OnItemDequeued(result);
            }
            return success;
        }

        public event EventHandler? ItemEnqueued;
        public event EventHandler<ItemDequeuedEventArgs<T>>? ItemDequeued;

        void OnItemEnqueued()
        {
            ItemEnqueued?.Invoke(this, EventArgs.Empty);
        }

        void OnItemDequeued(T item)
        {
            ItemDequeued?.Invoke(this, new ItemDequeuedEventArgs<T>(item));
        }
    }
    public sealed class ItemDequeuedEventArgs<T> : EventArgs
    {
        public ItemDequeuedEventArgs(T item)
        {
            Item = item;
        }

        public T Item { get; set; }
    }
}