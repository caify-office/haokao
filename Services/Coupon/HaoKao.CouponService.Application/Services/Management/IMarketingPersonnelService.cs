using HaoKao.CouponService.Application.ViewModels.MarketingPersonnel;

namespace HaoKao.CouponService.Application.Services.Management;

public interface IMarketingPersonnelService : IAppWebApiService, IManager
{
     /// <summary>
     /// 根据主键获取指定
     /// </summary>
     /// <param name="id">主键</param>
     Task<BrowseMarketingPersonnelViewModel> Get(Guid id);

     /// <summary>
     /// 根据查询获取列表，用于分页
     /// </summary>
     /// <param name="queryViewModel">查询对象</param>
     Task<MarketingPersonnelQueryViewModel> Get(MarketingPersonnelQueryViewModel queryViewModel);

     /// <summary>
     /// 创建
     /// </summary>
     /// <param name="model">新增模型</param>
     Task Create(CreateMarketingPersonnelViewModel model);

     /// <summary>
     /// 根据主键删除指定
     /// </summary>
     /// <param name="id">主键</param>
     Task Delete(Guid id);

     /// <summary>
     /// 根据主键更新指定
     /// </summary>
     /// <param name="id">主键</param>
     /// <param name="model">新增模型</param>
     Task Update(Guid id, UpdateMarketingPersonnelViewModel model);
}