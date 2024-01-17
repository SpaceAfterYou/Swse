using Infrastructure.Networking.Session;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Channels;

namespace Infrastructure.Networking.DependencyInjection;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddNetworking(this IServiceCollection @this) => @this
        .AddSingleton<SyncServer>()
        .AddTransient<SyncSession>()
        .AddKeyedSingleton("connection", (s, k) => Channel.CreateUnbounded<BridgeSession>())
        .AddKeyedSingleton("disconnection", (s, k) => Channel.CreateUnbounded<BridgeSession>());
}
