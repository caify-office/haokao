using HaoKao.PaperService.Application.Interfaces;
using HaoKao.PaperService.Application.ViewModels;
using HaoKao.PaperService.Domain.Entities;
using HaoKao.PaperService.Domain.Repositories;

namespace HaoKao.PaperService.Application.Services;

[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class PaperWebService(
    IStaticCacheManager cacheManager,
    IPaperRepository repository,
    IPaperService service
) : IPaperWebService
{
    [HttpGet("{id:guid}")]
    public async Task<BrowsePaperWebViewModel> Get(Guid id)
    {
        var paper = await GetPaperById(id);
        var result = paper.MapToDto<BrowsePaperWebViewModel>();
        if (string.IsNullOrEmpty(paper?.StructJson)) return result;

        var viewModel = JsonSerializer.Deserialize<PaperStructViewModel>(paper.StructJson);
        if (viewModel is not { TempleteStructDatas.Count: > 0 }) return result;

        var i = 0;
        foreach (var structData in viewModel.TempleteStructDatas)
        {
            var basicInfo = structData.BasicInfo;

            if (structData.Questions is { Count: > 0, })
            {
                result.QuestionIds.AddRange(basicInfo.TypeName == "案例分析题"
                                         ? structData.Questions.SelectMany(x => x.Questions.Select(q => q.Id)).ToList()
                                         : structData.Questions.Select(x => x.Id).ToList());
            }

            result.QuestionTypes.Add(new BrowsePaperQuestionViewModel
            {
                Sort = i++,
                QuestionCount = basicInfo.QuestionCount,
                TypeId = basicInfo.TypeId,
                Typename = basicInfo.TypeName,
                Score = basicInfo.QuestionScore,
                Questions = structData.Questions,
                ScoringRules = structData.ScoringRules, //题型评分规则
            });
        }

        return result;
    }

    /// <summary>
    /// 根据查询获取列表，不带分页
    /// </summary>
    [HttpGet]
    public async Task<IReadOnlyList<Paper>> Get(Guid subjectId, Guid categoryId)
    {
        return await cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Paper>.QueryCacheKey.Create($"subjectId={subjectId}&categoryId={categoryId}"),
            () => repository.GetPaperList(subjectId, categoryId)
        );
    }

    /// <summary>
    /// 根据科目和分类获取试卷列表(Id和名称)
    /// </summary>
    [HttpGet]
    public Task<IReadOnlyList<Paper>> GetPaperList(Guid subjectId, Guid categoryId)
    {
        return Get(subjectId, categoryId);
    }

    /// <summary>
    /// App获取试卷列表
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    [HttpGet("Query")]
    public Task<PaperQueryViewModel> Get([FromQuery] PaperQueryViewModel queryViewModel)
    {
        return service.Get(queryViewModel);
    }

    /// <summary>
    /// 根据主键获取指定
    /// 返回基本信息以及题型介绍
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    public async Task<BrowsePaperViewModel> GetPaperInfo(Guid id)
    {
        var paper = await GetPaperById(id);
        var result = paper.MapToDto<BrowsePaperViewModel>();
        if (string.IsNullOrEmpty(paper?.StructJson)) return result;

        var viewModel = JsonSerializer.Deserialize<PaperStructViewModel>(paper.StructJson);
        result.QuestionTypes = viewModel?.TempleteStructDatas.Select(structData => new BrowsePaperQuestionInfoViewModel
        {
            QuestionCount = structData.BasicInfo.QuestionCount,
            TypeId = structData.BasicInfo.TypeId,
            Typename = structData.BasicInfo.TypeName,
            Score = structData.BasicInfo.QuestionScore,
            ScoringRules = structData.ScoringRules, //题型评分规则
        }).ToList();

        return result;
    }

    [HttpGet("{id:guid}")]
    public  Task<BrowsePaperViewModel> GetPaperDetailInfo(Guid id)
    {
      return  service.GetPaperDetailInfo(id);
    }

    /// <summary>
    /// 根据主键获取指定
    /// 返回试卷试题结构信息
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}"), Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
    public async Task<BrowsePaperStructViewModel> GetPaperStructInfo(Guid id)
    {
        var paper = await GetPaperById(id);
        return paper.MapToDto<BrowsePaperStructViewModel>();
    }

    /// <summary>
    /// 根据科目获取所有试卷总题数
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public async Task<int> GetQuestionTotalBySubject(Guid subjectId)
    {
        var result = await GetSubjectPaperQuestionCount(subjectId);
        return result.PaperQuestionCount;
    }

    /// <summary>
    /// 根据科目Id查询所有试卷题目总数
    /// </summary>
    [HttpGet]
    public async Task<BrowsePaperQuestionCountViewModel> GetSubjectPaperQuestionCount(Guid subjectId)
    {
        var result = await cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Paper>.QueryCacheKey.Create($"subjectId={subjectId}PaperCountAndQuestionCount"),
            () => repository.GetPaperCountAndPaperQuestionCount(subjectId)
        );
        return JsonSerializer.Deserialize<BrowsePaperQuestionCountViewModel>(result);
    }

    private Task<Paper> GetPaperById(Guid id)
    {
        return cacheManager.GetAsync(
     GirvsEntityCacheDefaults<Paper>.ByIdCacheKey.Create(id.ToString()),
     () => repository.GetByIdAsync(id)
 ) ?? throw new GirvsException("对应的试卷不存在", StatusCodes.Status404NotFound);
    }
}