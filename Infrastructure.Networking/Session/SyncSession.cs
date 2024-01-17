using DefaultEcs;
using NetCoreServer;

namespace Infrastructure.Networking.Session;

public sealed class SyncSession(World world, TcpServer server) : TcpSession(server)
{
    public Entity Entity { get; } = world.CreateEntity();

    protected override void OnConnected()
    {
        Entity.Set(new BridgeSession(this));
    }

    protected override void Dispose(bool disposingManagedResources)
    {
        if (disposingManagedResources)
        {
            Entity.Disable();
            Entity.Dispose();
        }

        base.Dispose(disposingManagedResources);
    }
}
