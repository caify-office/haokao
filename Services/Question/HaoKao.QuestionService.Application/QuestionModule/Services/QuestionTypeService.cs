using HaoKao.QuestionService.Application.QuestionHandlers;
using HaoKao.QuestionService.Application.QuestionModule.Interfaces;

namespace HaoKao.QuestionService.Application.QuestionModule.Services;

/// <summary>
/// 题型服务接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
public class QuestionTypeService : IQuestionTypeService
{
    /// <summary>
    /// 获取题型列表
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public dynamic GetQuestionTypeList()
    {
        var questionTypes = EngineContext.Current.Resolve<IEnumerable<IQuestion>>();
        return questionTypes.Select(x => new
        {
            x.QuestionTypeName,
            x.QuestionTypeId,
            x.Code,
        }).OrderBy(x => x.Code).ToList();
    }

    /// <summary>
    /// 获取题型配置
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public dynamic GetQuestionTypeConfig(Guid id)
    {
        return QuestionManager.GetByQuestionId(id);
    }

    /// <summary>
    /// 获取题型信息列表
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public dynamic GetQuestionTypeInfoList()
    {
        return EngineContext.Current.Resolve<IEnumerable<IQuestion>>().Select(x => new
        {
            BasicInfo = new
            {
                TypeId = x.QuestionTypeId,
                TypeName = x.QuestionTypeName,
                TypeDescription = "",
                QuestionCount = 10,
                QuestionScore = 2,
                TotalScore = 20,
            },
            x.ScoringRules,
            x.Code,
        }).OrderBy(x => x.Code).ToList();
    }
}