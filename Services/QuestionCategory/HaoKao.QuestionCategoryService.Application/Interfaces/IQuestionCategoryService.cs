using HaoKao.QuestionCategoryService.Application.ViewModels;

namespace HaoKao.QuestionCategoryService.Application.Interfaces;

public interface IQuestionCategoryService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseQuestionCategoryViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<QuestionCategoryQueryViewModel> Get(QuestionCategoryQueryViewModel queryViewModel);

    /// <summary>
    /// 获取类别列表
    /// </summary>
    /// <returns></returns>
    Task<List<BrowseQuestionCategoryViewModel>> GetCategoryList();

    /// <summary>
    /// 创建题库类别
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateQuestionCategoryViewModel model);

    /// <summary>
    /// 根据主键删除指定题库类别
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定题库类别
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Update(UpdateQuestionCategoryViewModel model);
}