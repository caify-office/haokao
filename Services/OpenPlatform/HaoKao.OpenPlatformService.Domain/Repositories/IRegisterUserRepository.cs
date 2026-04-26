using System.Data;

namespace HaoKao.OpenPlatformService.Domain.Repositories;

public interface IRegisterUserRepository : IRepository<RegisterUser>
{
    Task<bool> ExistByExternalIdentity(string scheme, string uniqueIdentifier);

    Task<ExternalIdentity> GetByExternalIdentity(string scheme, string uniqueIdentifier);

    Task<RegisterUser> GetByInclude(Expression<Func<RegisterUser, bool>> predicate);

    /// <summary>
    /// 获取总注册用户数, 今日注册用户数, 今日活跃用户数
    /// </summary>
    Task<(int Total, int Today, int Active)> QueryRegisteredCountAndActiveCount();

    /// <summary>
    /// 每日注册用户走势
    /// </summary>
    Task<Dictionary<string, int>> QueryDailyRegisteredUserTrend(DateTime? start, DateTime? end, DateTime? prev, DateTime? next);

    /// <summary>
    /// 查询用户注册客户端分组数据
    /// </summary>
    /// <returns></returns>
    Task<DataTable> QueryRegisteredUserClientGrouping(DateTime? start, DateTime? end);

     Task<List<RegisterUser>> GetAllWithWeiXin();
}