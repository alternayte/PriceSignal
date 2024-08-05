namespace Application.Common.Interfaces;

public interface IPubSub
{
    Task PublishAsync<T>(string subject, T message);
    void Subscribe<T>(string stream, Func<T,Task> handler, string? subject = null);   
}