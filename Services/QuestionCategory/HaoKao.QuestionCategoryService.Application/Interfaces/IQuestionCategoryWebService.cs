using HaoKao.QuestionCategoryService.Application.ViewModels;

namespace HaoKao.QuestionCategoryService.Application.Interfaces;

public interface IQuestionCategoryWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseQuestionCategoryViewModel> Get(Guid id);
    /// <summary>
    /// 获取类别字典
    /// </summary>
    /// <returns></returns>
    Task<Dictionary<Guid, string>> GetCategoryDict();

    /// <summary>
    /// 获取类别列表
    /// </summary>
    /// <returns></returns>
    Task<List<BrowseQuestionCategoryViewModel>> GetCategoryList();
}