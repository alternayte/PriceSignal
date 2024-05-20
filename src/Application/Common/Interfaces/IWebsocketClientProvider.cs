namespace Application.Common.Interfaces;

public interface IWebsocketClientProvider
{
    public void Start(Func<string, Task> onMessageReceived);
    public void Stop();
}