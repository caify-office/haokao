using HaoKao.PaperService.Application.Interfaces;
using HaoKao.PaperService.Application.ViewModels;
using HaoKao.PaperService.Domain.Entities;

namespace HaoKao.PaperService.Application.Services;

/// <summary>
/// 试卷 -wechat
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class PaperWeChatService(IPaperWebService service) : IPaperWeChatService
{
    /// <summary>
    /// 根据Id获取试卷
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public Task<BrowsePaperWebViewModel> Get(Guid id)
    {
        return service.Get(id);
    }

    /// <summary>
    /// 根据查询获取列表，不带分页
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<IReadOnlyList<Paper>> Get(Guid subjectId, Guid categoryId)
    {
        return service.Get(subjectId, categoryId);
    }

    /// <summary>
    /// App获取试卷列表
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    [HttpGet("Query"), AllowAnonymous]
    public Task<PaperQueryViewModel> Get([FromQuery] PaperQueryViewModel queryViewModel)
    {
        return service.Get(queryViewModel);
    }

    /// <summary>
    /// 根据主键获取基本信息以及题型介绍
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    public Task<BrowsePaperViewModel> GetPaperInfo(Guid id)
    {
        return service.GetPaperInfo(id);
    }

    /// <summary>
    /// 根据主键获取基本信息以及题型介绍
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public Task<BrowsePaperViewModel> GetPaperDetailInfo(Guid id)
    {
        return service.GetPaperDetailInfo(id);
    }

    /// <summary>
    /// 根据主键获取试卷试题结构信息
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    public Task<BrowsePaperStructViewModel> GetPaperStructInfo(Guid id)
    {
        return service.GetPaperStructInfo(id);
    }

    /// <summary>
    /// 根据科目获取所有试卷总题数
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}"), AllowAnonymous]
    public Task<int> GetQuestionTotalBySubject(Guid subjectId)
    {
        return service.GetQuestionTotalBySubject(subjectId);
    }

    /// <summary>
    /// 根据科目Id查询所有试卷题目总数
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet, AllowAnonymous]
    public Task<BrowsePaperQuestionCountViewModel> GetSubjectPaperQuestionCount(Guid subjectId)
    {
        return service.GetSubjectPaperQuestionCount(subjectId);
    }
}