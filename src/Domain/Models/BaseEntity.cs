using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public abstract class BaseEntity : EventEntity
{
    public long Id { get; set; }
    public Guid EntityId { get; set; }
}

public abstract class EventEntity 
{
    private readonly List<BaseEvent> _events = new();
    
    [NotMapped]
    public IReadOnlyCollection<BaseEvent> Events => _events.AsReadOnly();
    
    public void AddEvent(BaseEvent @event)
    {
        _events.Add(@event);
    }
    
    public void RemoveEvent(BaseEvent @event)
    {
        _events.Remove(@event);
    }
    
    public void ClearEvents()
    {
        _events.Clear();
    }
}