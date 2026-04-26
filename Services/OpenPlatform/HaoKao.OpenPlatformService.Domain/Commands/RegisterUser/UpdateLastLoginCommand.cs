namespace HaoKao.OpenPlatformService.Domain.Commands.RegisterUser;

public record UpdateLastLoginCommand(Guid Id) : Command("更新最后登录时间");