namespace HaoKao.Common.Events.Authorize;

public record AuthorizeEvent(
    List<AuthorizeDataRuleModel> AuthorizeDataRules,
    List<AuthorizePermissionModel> AuthorizePermissions) : IntegrationEvent;