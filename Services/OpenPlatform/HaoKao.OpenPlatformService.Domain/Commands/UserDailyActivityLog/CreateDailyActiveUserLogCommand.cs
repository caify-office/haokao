namespace HaoKao.OpenPlatformService.Domain.Commands.UserDailyActivityLog;

public record CreateDailyActiveUserLogCommand(Guid UserId, string ClientId) : Command("新增用户每日活跃记录");