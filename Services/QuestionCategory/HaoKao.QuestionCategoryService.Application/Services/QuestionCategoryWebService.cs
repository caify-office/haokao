using HaoKao.Common;
using HaoKao.QuestionCategoryService.Application.Interfaces;
using HaoKao.QuestionCategoryService.Application.ViewModels;
using HaoKao.QuestionCategoryService.Domain.Entities;
using HaoKao.QuestionCategoryService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace HaoKao.QuestionCategoryService.Application.Services;

[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class QuestionCategoryWebService(
    IQuestionCategoryService service,
    IQuestionCategoryRepository repository,
    IMapper mapper,
    IStaticCacheManager cacheManager
) : IQuestionCategoryWebService
{
    private readonly IQuestionCategoryService _service = service ?? throw new ArgumentNullException(nameof(service));
    private readonly IQuestionCategoryRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}"), AllowAnonymous]
    public Task<BrowseQuestionCategoryViewModel> Get(Guid id)
    {
        return _service.Get(id);
    }

    /// <summary>
    /// 获取类别字典
    /// </summary>
    /// <returns></returns>
    [HttpGet, AllowAnonymous]
    public Task<Dictionary<Guid, string>> GetCategoryDict()
    {
        return _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<QuestionCategory>.QueryCacheKey.Create(nameof(GetCategoryDict)),
            () => _repository.Query.OrderBy(x => x.CreateTime).ToDictionaryAsync(x => x.Id, x => x.Name)
        );
    }

    /// <summary>
    /// 获取类别列表
    /// </summary>
    /// <returns></returns>
    [HttpGet, AllowAnonymous]
    public async Task<List<BrowseQuestionCategoryViewModel>> GetCategoryList()
    {
        var list = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<QuestionCategory>.QueryCacheKey.Create(nameof(GetCategoryList)),
            () => _repository.Query.OrderBy(x => x.CreateTime).ToListAsync()
        );
        return _mapper.Map<List<BrowseQuestionCategoryViewModel>>(list);
    }
}