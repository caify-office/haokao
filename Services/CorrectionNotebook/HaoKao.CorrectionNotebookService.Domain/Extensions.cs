using Girvs.Driven.Notifications;

namespace HaoKao.CorrectionNotebookService.Domain;

public static class IMediatorHandlerExtensions
{
    public static Task RaiseBadRequestEvent(this IMediatorHandler mediator, string key, string message, CancellationToken cancellationToken = default)
    {
        return mediator.RaiseEvent(new DomainNotification(key, message, StatusCodes.Status400BadRequest), cancellationToken);
    }
}