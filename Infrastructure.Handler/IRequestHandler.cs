using DefaultEcs;

namespace Infrastructure.Handler;

public interface IRequestHandler<TMessage> where TMessage : struct
{
    Task OnHandleAsync(Entity entity, TMessage message, CancellationToken ct);
}