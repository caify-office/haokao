using Girvs.BusinessBasis.Entities;
using HaoKao.StudentService.Application.Interfaces;
using HaoKao.StudentService.Application.ViewModels;
using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Domain.Queries;
using HaoKao.StudentService.Domain.Repositories;
using HaoKao.StudentService.Domain.Works;

namespace HaoKao.StudentService.Application.Services;

/// <summary>
/// 学员接口服务--管理端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "学员",
    "33044d4a-78ca-4015-8ce1-250baa038f4f",
    "512",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class StudentService(
    IStaticCacheManager cacheManager,
    IStudentRepository repository
) : IStudentService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IStudentRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="model">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<QueryStudentViewModel> Get([FromQuery] QueryStudentViewModel model)
    {
        var query = model.MapToQuery<StudentQuery>();
        // var cacheKey = GirvsEntityCacheDefaults<Student>.QueryCacheKey.Create(query.GetCacheKey());
        // var tempQuery = await _cacheManager.GetAsync(cacheKey, async () =>
        // {
        await _repository.GetByStudentQueryAsync(query);
        //     return query;
        // });

        // if (!query.Equals(tempQuery))
        // {
        //     query.RecordCount = tempQuery.RecordCount;
        //     query.Result = tempQuery.Result;
        // }
        return query.MapToQueryDto<QueryStudentViewModel, Student>();
    }

    #endregion

    #region 测试方法

    [HttpGet, AllowAnonymous]
    public Task AutoMigrationAsync()
    {
        return _repository.AutoMigrationAsync();
    }

    [HttpGet, AllowAnonymous]
    public Task SyncStudentFollowAsync([FromServices] ISyncStudentFollowWork work)
    {
        return work.ExecuteAsync();
    }

    [HttpGet, AllowAnonymous]
    public Task UpdateStudentFollowAsync([FromServices] IUpdateStudentFollowWork work)
    {
        return work.ExecuteAsync();
    }

    [HttpGet, AllowAnonymous]
    public Task AutoAllocationAsync([FromServices] IAutoAllocationWork work)
    {
        return work.ExecuteAsync();
    }

    [HttpGet, AllowAnonymous]
    public Task UnionStudentAsync([FromServices] IUnionStudentWork work)
    {
        return work.ExecuteAsync();
    }

    [HttpGet, AllowAnonymous]
    public Task<RegisterUserPageDto> GetRegisterUserList(
        DateTime start, DateTime end,
        bool? isFollowed, bool? isBandingWeiXin,
        int pageIndex, int pageSize
    )
    {
        return _repository.GetRegisterUsers(start, end, isFollowed, isBandingWeiXin, pageIndex, pageSize);
    }

    #endregion
}