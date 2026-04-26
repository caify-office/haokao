namespace HaoKao.OpenPlatformService.Domain.Commands.RegisterUser;

public record BindExternalUserCommand(string Phone, ExternalUserCommand ExternalUserCommand) : Command("绑定外部用户到注册用户");