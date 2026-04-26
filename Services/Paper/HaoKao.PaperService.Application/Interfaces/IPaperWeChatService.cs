using HaoKao.PaperService.Application.ViewModels;
using HaoKao.PaperService.Domain.Entities;

namespace HaoKao.PaperService.Application.Interfaces;

public interface IPaperWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据Id获取试卷
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<BrowsePaperWebViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，不带分页
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    Task<IReadOnlyList<Paper>> Get(Guid subjectId, Guid categoryId);

    /// <summary>
    /// App获取试卷列表
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    Task<PaperQueryViewModel> Get(PaperQueryViewModel queryViewModel);

    /// <summary>
    /// 根据主键获取基本信息以及题型介绍
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowsePaperViewModel> GetPaperInfo(Guid id);

    /// <summary>
    /// 根据主键获取基本信息以及题型介绍
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<BrowsePaperViewModel> GetPaperDetailInfo(Guid id);

    /// <summary>
    /// 根据主键获取试卷试题结构信息
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowsePaperStructViewModel> GetPaperStructInfo(Guid id);

    /// <summary>
    /// 根据科目获取所有试卷总题数
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    Task<int> GetQuestionTotalBySubject(Guid subjectId);

    /// <summary>
    ///  根据科目Id查询所有试卷题目总数
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    Task<BrowsePaperQuestionCountViewModel> GetSubjectPaperQuestionCount(Guid subjectId);
}