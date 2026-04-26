using HaoKao.AnsweringQuestionService.Application.ViewModels;
using HaoKao.AnsweringQuestionService.Application.ViewModels.AnsweringQuestionReply;

namespace HaoKao.AnsweringQuestionService.Application.AppService.ManageMent;

public interface IAnsweringQuestionReplyService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseAnsweringQuestionReplyViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<AnsweringQuestionReplyQueryViewModel> Get(AnsweringQuestionReplyQueryViewModel queryViewModel);

    /// <summary>
    /// 创建答疑回复
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateAnsweringQuestionReplyViewModel model);

    /// <summary>
    /// 根据主键删除指定答疑回复
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定答疑回复
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    Task Update(Guid id, UpdateAnsweringQuestionReplyViewModel model);
}