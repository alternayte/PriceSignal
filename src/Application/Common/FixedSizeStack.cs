using System.Collections.Concurrent;

namespace Application.Common;

public class FixedSizeStack<T>
{
    private readonly ConcurrentStack<T> _stack = new();
    private readonly int _maxSize;
    
    public FixedSizeStack(int maxSize)
    {
        _maxSize = maxSize;
    }
    
    public void Push(T item)
    {
        _stack.Push(item);
        while (_stack.Count > _maxSize)
        {
            _stack.TryPop(out _);
        }
    }
    
    public T[] ToArray()
    {
        return _stack.ToArray();
    }
    
    public T Peek()
    {
        return _stack.TryPeek(out var item) ? item : default;
    }
    
    public bool TryPop(out T item)
    {
        return _stack.TryPop(out item);
    }
    
    public bool TryPeek(out T item)
    {
        return _stack.TryPeek(out item);
    }
    
    public void Clear()
    {
        while (_stack.TryPop(out _)) { }
    }
    
    public int Count => _stack.Count;
    
    public bool IsEmpty => _stack.IsEmpty;
    
    public bool IsFull => _stack.Count == _maxSize;
    
    public bool Contains(T item)
    {
        return _stack.Contains(item);
    }
    
}