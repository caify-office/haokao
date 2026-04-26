using HaoKao.AgreementService.Application.Services.Management;
using HaoKao.AgreementService.Application.ViewModels.StudentAgreement;
using HaoKao.AgreementService.Domain.Entities;
using HaoKao.AgreementService.Domain.Queries;
using HaoKao.AgreementService.Domain.Repositories;
using Newtonsoft.Json;

namespace HaoKao.AgreementService.Application.Services.Web;

/// <summary>
/// 学员协议接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class StudentAgreementWebService(
    IStudentAgreementService service,
    IStudentAgreementRepository repository,
    ICourseAgreementService courseAgreementService,
    ICourseAgreementRepository courseAgreementRepository,
    IStaticCacheManager cacheManager
) : IStudentAgreementWebService
{
    #region 初始参数

    private readonly IStudentAgreementService _service = service ?? throw new ArgumentNullException(nameof(service));
    private readonly IStudentAgreementRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICourseAgreementService _courseAgreementService = courseAgreementService ?? throw new ArgumentNullException(nameof(courseAgreementService));
    private readonly ICourseAgreementRepository _courseAgreementRepository = courseAgreementRepository ?? throw new ArgumentNullException(nameof(courseAgreementRepository));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    public async Task<SignedStudentAgreementViewModel> Get(Guid id)
    {
        var studentAgreement = await _service.GetEntity(id);
        var result = studentAgreement.MapToDto<SignedStudentAgreementViewModel>();
        var agreement = await _courseAgreementService.Get(studentAgreement.AgreementId);
        result.AgreementContent = agreement.Content;
        return result;
    }

    /// <summary>
    /// 获取用户已签署的记录(我的协议)
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<MyAgreementListViewModel>> GetMyAgreement([FromBody] List<QueryMyAgreementViewModel> queryViewModel)
    {
        var courseAgreementList = await GetCourseAgreementList();
        var studentAgreementList = await GetStudentAgreementList();
        var list = new List<MyAgreementListViewModel>(courseAgreementList.Capacity);
        foreach (var vm in queryViewModel)
        {
            var ca = courseAgreementList.FirstOrDefault(x => x.Id == vm.AgreementId);
            if (ca == null) continue;
            var sa = studentAgreementList.FirstOrDefault(x => x.ProductId == vm.ProductId);
            list.Add(new()
            {
                Id = sa?.Id,
                ProductId = vm.ProductId,
                AgreementId = ca.Id,
                AgreementName = ca.Name,
                CreateTime = sa?.CreateTime,
            });
        }
        return list;

        Task<List<CourseAgreement>> GetCourseAgreementList()
        {
            var query = new CourseAgreementQuery { Ids = queryViewModel.Select(x => x.AgreementId).ToList() };
            var key = JsonConvert.SerializeObject(query).ToMd5();
            return _cacheManager.GetAsync(
                GirvsEntityCacheDefaults<CourseAgreement>.QueryCacheKey.Create(key),
                () => _courseAgreementRepository.GetWhereAsync(query.GetQueryWhere())
            );
        }

        Task<List<StudentAgreement>> GetStudentAgreementList()
        {
            var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
            var query = new StudentAgreementQuery { CreatorId = userId };
            return _cacheManager.GetAsync(
                GirvsEntityCacheDefaults<StudentAgreement>.QueryCacheKey.Create(query.GetCacheKey()),
                () => _repository.GetWhereAsync(query.GetQueryWhere())
            );
        }
    }

    /// <summary>
    /// 产品是否签署协议
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    [HttpGet("{productId:guid}")]
    public async Task<bool> HasBeenSigned(Guid productId)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var query = new StudentAgreementQuery { ProductId = productId, CreatorId = userId };
        var entity = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<StudentAgreement>.QueryCacheKey.Create(query.GetCacheKey()),
            () => _repository.GetAsync(query.GetQueryWhere())
        );
        return entity != null;
    }

    /// <summary>
    /// 创建学员协议
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public async Task<Guid> Create([FromBody] CreateStudentAgreementViewModel model)
    {
        await _service.Create(model);
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var query = new StudentAgreementQuery { AgreementId = model.AgreementId, ProductId = model.ProductId, CreatorId = userId };
        var entity = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<StudentAgreement>.QueryCacheKey.Create(query.GetCacheKey()),
            () => _repository.GetAsync(query.GetQueryWhere())
        );
        return entity.Id;
    }

    #endregion
}