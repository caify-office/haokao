using HaoKao.FeedBackService.Domain.Entities;
using HaoKao.FeedBackService.Domain.Repositories;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace HaoKao.FeedBackService.Infrastructure.Repositories;

public class FeedBackRepository : Repository<FeedBack>, IFeedBackRepository
{
    public override Task<FeedBack> GetByIdAsync(Guid id)
    {
        return Queryable.AsNoTracking()
                        .Include(x => x.ChildQuestion)
                        .Include(x => x.FeedBackReplies)
                        .FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// 查询用户提交次数
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<int> GetUserCount(Guid userId)
    {
        return await Queryable.Where(p => p.CreatorId == userId).CountAsync();
    }
}