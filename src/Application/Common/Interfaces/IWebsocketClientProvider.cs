namespace Application.Common.Interfaces;

public interface IWebsocketClientProvider
{
    public string GetUri();
    public void SetUri(string url);
    public void Start(Func<string, Task> onMessageReceived);
    public void Stop();
    public void Subscribe(IEnumerable<string> streams);
    public void Unsubscribe(IEnumerable<string> streams);
}