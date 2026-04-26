using HaoKao.AnsweringQuestionService.Application.ViewModels;
using HaoKao.AnsweringQuestionService.Application.ViewModels.AnsweringQuestion;

namespace HaoKao.AnsweringQuestionService.Application.AppService.ManageMent;

public interface IAnsweringQuestionService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseAnsweringQuestionViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<AnsweringQuestionQueryViewModel> Get(AnsweringQuestionQueryViewModel queryViewModel);
}