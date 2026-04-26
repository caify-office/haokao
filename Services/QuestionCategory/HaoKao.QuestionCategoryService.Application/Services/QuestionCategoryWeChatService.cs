using HaoKao.Common;
using HaoKao.QuestionCategoryService.Application.Interfaces;
using HaoKao.QuestionCategoryService.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.QuestionCategoryService.Application.Services;

/// <summary>
/// 题库类别App 接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class QuestionCategoryWeChatService(IQuestionCategoryWebService service) : IQuestionCategoryWeChatService
{
    private readonly IQuestionCategoryWebService _service = service ?? throw new ArgumentNullException(nameof(service));

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
        return _service.GetCategoryDict();
    }

    /// <summary>
    /// 获取类别列表
    /// </summary>
    /// <returns></returns>
    [HttpGet, AllowAnonymous]
    public Task<List<BrowseQuestionCategoryViewModel>> GetCategoryList()
    {
        return _service.GetCategoryList();
    }
}