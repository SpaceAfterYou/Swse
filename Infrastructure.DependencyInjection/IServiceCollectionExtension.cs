using DefaultEcs;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection @this) => @this
        .AddSingleton<World>();
}
