using HaoKao.LearnProgressService.Application.ViewModels.LearnProgress;
using HaoKao.LearnProgressService.Domain.Queries.EntityQuery;

namespace HaoKao.LearnProgressService.Application.AppService.Web;

public interface ILearnProgressWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 分产品分科目获取学习时长(单位小时)
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="subjectId"></param>
    Task<float> GetLearnDurations(Guid productId, Guid subjectId);
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseLearnProgressViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<LearnProgressQueryViewModel> GetList(LearnProgressQueryViewModel queryViewModel);

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateLearnProgressViewModel model);
    /// <summary>
    /// 听课记录按日期排序
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<List<UserProgressRecordByDateModel>> GetListByDate(QueryByVideoIds model);
    /// <summary>
    /// 读取用户的听课数据统计(左侧)
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<LearnRcordStaticViewModel> GetLearnRcordStaticAsync(QueryByVideoIds model);
    /// <summary>
    /// 读取用户的听课数据统计(右侧)
    /// </summary>
    /// <returns></returns>
    Task<List<UserProgressByDateModel>> GetLearnRcordByWeekStaticAsync(QueryLearnRcordByWeekStatic model);
    /// <summary>
    /// 视频是否学完(更新)
    /// </summary>
    /// <param name="Input"></param>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    Task UpdateIsEnd(UpdateIsEndModel Input);
}