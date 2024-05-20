using System.Threading.Channels;

namespace Infrastructure.Channels;

public static class WebsocketChannel
{
    public static readonly Channel<string> SocketChannel = Channel.CreateUnbounded<string>();
}