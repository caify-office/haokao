using HaoKao.Common.Events.AuditLogs;

namespace HaoKao.Common.CommandOperate;

public class SendCommandOperateEvent(IEventBus eventBus,ILogger<SendCommandOperateEvent> logger) : ICommandOperateHandler
{
    private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

    public async Task Handle(Command command, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation($"请求头信息:{JsonConvert.SerializeObject(EngineContext.Current.HttpContext.Request.Headers)}");
        }
        catch (Exception e)
        {
            logger.LogInformation($"反序列化错误：{e.ToString()}");
        }
        
        if (!string.IsNullOrEmpty(command.CommandDesc))
        {
            var sourceType = EngineContext.Current.ClaimManager.GetIdentityType() == IdentityType.ManagerUser
                ? SourceType.Server
                : SourceType.Register;

            await _eventBus.PublishAsync(new CreateAuditLogEvent(
                                             string.Empty,
                                             command.CommandDesc,
                                             sourceType,
                                             GetIP(),
                                             ConvertToGuid(command.MessageSource.TenantId),
                                             command.MessageSource.TenantName,
                                             JsonConvert.SerializeObject(command),
                                             ConvertToGuid(command.MessageSource.SourceNameId),
                                             command.MessageSource.SourceName));
        }
    }

    private static string GetIP()
    {
        try
        {
           
            return EngineContext.Current.HttpContext.Request.Headers["X-Forwarded-For"].ToString();
        }
        catch
        {
            return "localhost";
        }
    }

    private static Guid ConvertToGuid(string guid)
    {
        return Guid.TryParse(guid, out var newGuid) ? newGuid : Guid.Empty;
    }
}