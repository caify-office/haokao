using HaoKao.FeedBackService.Domain.Entities;

namespace HaoKao.FeedBackService.Domain.Repositories;

public interface IFeedBackRepository : IRepository<FeedBack>
{
    Task<int> GetUserCount(Guid userId);
}