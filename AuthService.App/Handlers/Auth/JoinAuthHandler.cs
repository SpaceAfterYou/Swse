using DefaultEcs;
using Infrastructure.Handler;

namespace AuthService.App.Handlers.Auth;

internal sealed class JoinAuthHandler : IRequestHandler<int>
{
    public Task OnHandleAsync(Entity entity, int message, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
