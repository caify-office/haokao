using Girvs.AuthorizePermission;
using Girvs.AuthorizePermission.Enumerations;
using Girvs.Extensions;
using HaoKao.Common;
using HaoKao.OpenPlatformService.Application.ViewModels.RegisterUser;
using HaoKao.OpenPlatformService.Domain.Entities;
using HaoKao.OpenPlatformService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HaoKao.OpenPlatformService.Application.Services.Management;

/// <summary>
/// 产品指标接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "产品指标",
    "9052b346-f868-48ed-b2cf-43471642c5bb",
    "32",
    SystemModule.SystemModule,
    3
)]
public class ProductMetricsService(
    IRegisterUserRepository repository,
    IDailyActiveUserLogRepository dailyActivityRepository
) : IProductMetricsService
{
    #region 初始参数

    private readonly IRegisterUserRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IDailyActiveUserLogRepository _dailyActiveUserRepository = dailyActivityRepository ?? throw new ArgumentNullException(nameof(dailyActivityRepository));

    #endregion

    #region 注册用户

    /// <summary>
    /// 获取总注册用户数, 今日注册用户数, 今日活跃用户数
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser)]
    public async Task<dynamic> QueryRegisteredCountAndActiveCount()
    {
        var result = await _repository.QueryRegisteredCountAndActiveCount();
        return new { result.Total, result.Today, result.Active };
    }

    /// <summary>
    /// 查询每日注册用户走势
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser)]
    public async Task<List<DailyTrendListViewModel>> GetDailyRegisteredUserTrend([FromQuery] DailyTrendQueryViewModel queryViewModel)
    {
        var result = await _repository.QueryDailyRegisteredUserTrend(queryViewModel.Start,
                                                                     queryViewModel.End,
                                                                     queryViewModel.PrevDate,
                                                                     queryViewModel.NextDate);

        return result.Select(x => new DailyTrendListViewModel { Date = x.Key, Count = x.Value }).ToList();
    }

    /// <summary>
    /// 查询用户注册客户端分组数据
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser)]
    public async Task<List<ClientGroupingListViewModel>> QueryRegisteredUserClientGrouping([FromQuery] ProductMetricsQueryViewModel queryViewModel)
    {
        var data = await _repository.QueryRegisteredUserClientGrouping(queryViewModel.Start, queryViewModel.End);
        var list = new List<ClientGroupingListViewModel>(data.Rows.Count);
        for (var i = 0; i < data.Rows.Count; i++)
        {
            var row = data.Rows[i];
            var items = row.ItemArray;
            list.Add(new()
            {
                ClientId = items[0].ToString(),
                Count = Convert.ToInt32(items[1]),
            });
        }
        return list;
    }

    #endregion

    #region 活跃用户

    /// <summary>
    /// 查询每日活跃用户走势
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser)]
    public async Task<List<DailyTrendListViewModel>> GetDailyActiveUserTrend([FromQuery] DailyTrendQueryViewModel queryViewModel)
    {
        var result = await _dailyActiveUserRepository.QueryDailyActiveUserTrend(queryViewModel.Start,
                                                                                queryViewModel.End,
                                                                                queryViewModel.PrevDate,
                                                                                queryViewModel.NextDate);

        return result.Select(x => new DailyTrendListViewModel { Date = x.Key, Count = x.Value }).ToList();
    }

    /// <summary>
    /// 查询每日活跃用户客户端分组数据
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser)]
    public async Task<List<ClientGroupingListViewModel>> QueryDailyActiveUserClientGrouping([FromQuery] ProductMetricsQueryViewModel queryViewModel)
    {
        Expression<Func<DailyActiveUserLog, bool>> predicate = x => true;
        if (queryViewModel.Start.HasValue)
        {
            predicate = predicate.And(x => x.CreateTime >= queryViewModel.Start.Value);
        }
        if (queryViewModel.End.HasValue)
        {
            var end = queryViewModel.End.Value.AddDays(1);
            predicate = predicate.And(x => x.CreateTime <= end);
        }

        var list = await _dailyActiveUserRepository.Query
                                                   .Where(predicate)
                                                   .GroupBy(x => x.ClientId)
                                                   .Select(x => new ClientGroupingListViewModel
                                                   {
                                                       ClientId = x.Key,
                                                       Count = x.Count()
                                                   }).ToListAsync();
        return list;
    }

    #endregion
}