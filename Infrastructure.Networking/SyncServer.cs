using Infrastructure.Networking.Session;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreServer;
using System.Net;
using System.Threading.Channels;

namespace Infrastructure.Networking;

public sealed class SyncServer(IServiceProvider provider, IConfiguration configuration) : TcpServer(GetEndPoint(configuration))
{
    private readonly Channel<SyncSession> _connectionChannel = provider.GetRequiredKeyedService<Channel<SyncSession>>("connection");
    private readonly Channel<SyncSession> _disconnectionChannel = provider.GetRequiredKeyedService<Channel<SyncSession>>("disconnection");

    protected override TcpSession CreateSession() => provider.GetRequiredService<SyncSession>();

    protected override void OnConnected(TcpSession value)
    {
        if (value is SyncSession session)
        {
            if (_connectionChannel.Writer.TryWrite(session) is false)
            {
                if (session.Disconnect() is false)
                {
                    throw new Exception("Client connection issue");
                }
            }
        }
    }

    protected override void OnDisconnected(TcpSession value)
    {
        if (value is SyncSession session)
        {
            if (_disconnectionChannel.Writer.TryWrite(session) is false)
            {
                throw new Exception("Client disconnection issue");
            }
        }
    }

    private static IPEndPoint GetEndPoint(IConfiguration configuration)
    {
        var value = configuration.GetRequiredSection("EndPoint").Value;
        ArgumentNullException.ThrowIfNull(value, "EndPoint");

        var values = value.Split(':');
        if (values.Length != 2) throw new ArgumentException("EndPoint must be like 127.0.0.1:10000");

        return new(IPAddress.Parse(values[0]), ushort.Parse(values[1]));
    }
}
