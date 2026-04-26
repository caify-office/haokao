namespace HaoKao.SalespersonService.Application;

internal static class IMediatorHandlerExtensions
{
    public static async Task<T> TrySendCommand<T>(
        this IMediatorHandler bus, IRequest<T> request,
        DomainNotificationHandler notifications,
        CancellationToken cancellationToken = default
    )
    {
        var result = await bus.SendCommand(request, cancellationToken);

        if (notifications.HasNotifications())
        {
            var errorMessage = notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return (T)result;
    }
}