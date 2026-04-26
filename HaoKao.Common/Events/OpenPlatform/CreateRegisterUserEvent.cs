namespace HaoKao.Common.Events.OpenPlatform;

public record CreateRegisterUserEvent(Guid CreatorId, string Phone, string NickName) : IntegrationEvent;