using System.Collections.Concurrent;

namespace Application.Common;

public class FixedSizeQueue<T>
{
    private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
    private readonly int _maxSize;

    public FixedSizeQueue(int maxSize)
    {
        _maxSize = maxSize;
    }

    public void Enqueue(T item)
    {
        _queue.Enqueue(item);
        while (_queue.Count > _maxSize)
        {
            _queue.TryDequeue(out _);
        }
    }

    public T[] ToArray()
    {
        return _queue.ToArray();
    }
}