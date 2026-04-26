using HaoKao.NotificationMessageService.Domain.MessageSenders;

namespace HaoKao.NotificationMessageService.Infrastructure.MessageSender;

public class InSiteMessageSender : IInSiteMessageSender
{
    public InSiteMessageSetting Setting { get; set; }

    public string[] Parameter { get; set; }

    public RegisteredUser RegisteredUser { get; set; }

    public Task<(NotificationMessageSendState, string)> SendAsync(MessageTemplate messageTemplate)
    {
        var templateContent = messageTemplate.Desc;
        if (Parameter is { Length: > 0 })
        {
            var message = string.Format(templateContent, Parameter);
            return Task.FromResult((NotificationMessageSendState.SendSuccess, message));
        }

        return Task.FromResult((NotificationMessageSendState.SendSuccess, templateContent));
    }
}