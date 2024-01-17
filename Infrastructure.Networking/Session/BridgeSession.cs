using Infrastructure.Networking.Abstractions;
using NetCoreServer;

namespace Infrastructure.Networking.Session;

public sealed partial class BridgeSession(TcpSession session)
{
    public bool SendAsync<T>(T message) where T : IBinaryOutcomingMessage
    {
        var bytes = message.ToBinary();
        return session.SendAsync(bytes);
    }
}