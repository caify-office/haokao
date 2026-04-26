using HaoKao.NotificationMessageService.Domain.Models;

namespace HaoKao.NotificationMessageService.Domain.Repositories;

public interface IRegisteredUserRepository : IManager
{
    Task<RegisteredUser> GetRegisteredUser(string cardId, string contactNumber);
}