using HaoKao.LearnProgressService.Domain.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using HaoKao.LearnProgressService.Domain.Queries.EntityQuery;

namespace HaoKao.LearnProgressService.Domain.Repositories;

public interface ILearnProgressRepository : IRepository<LearnProgress>
{
    /// <summary>
    /// 根据主键获取指定,这里应该是传的videoId查询当前视频的播放进度
    /// </summary>
    /// <param name="videoId">视频id</param>
    /// <param name="creatorId">创建用户id</param>
    /// <returns></returns>
    Task<LearnProgress> GetLearnProgress(Guid videoId, Guid creatorId);

    /// <summary>
    /// 根据Identifier查询学习进度
    /// </summary>
    /// <param name="identifier">视频Identifier</param>
    /// <returns></returns>
    Task<LearnProgress> GetLearnProgressByIdentifier(string identifier);
    /// <summary>
    /// 查询学员学习进度
    /// </summary>
    /// <param name="studentId">学员权限表StudentId字段</param>
    /// <param name="productId">产品id</param>
    /// <param name="subjectId">科目id</param>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">页容积</param>
    /// <returns></returns>
    Task<Tuple<int, List<Dictionary<string, object>>>> QueryCourseLearningProgress(Guid studentId, Guid productId, Guid subjectId,int pageIndex,int pageSize);


    /// <summary>
    /// 统计学员学习进度
    /// </summary>
    /// <param name="studentId">学员权限表StudentId字段</param>
    /// <param name="productId">产品id</param>
    /// <param name="subjectId">科目id</param>
    /// <returns></returns>
    Task<Tuple<int, int, List<Dictionary<string, object>>>> CourseLearningProgressStatistics(Guid studentId, Guid productId, Guid subjectId);

    /// <summary>
    /// 更新用户学习视频实际进度
    /// </summary>
    /// <param name="Id">主键id</param>
    /// <param name="maxProgress">最新进度</param>
    /// <param name="duration">时长</param>
    /// <returns></returns>
    Task<int> UpdateFactProgress(Guid Id, int maxProgress, int duration);

    /// <summary>
    /// 右侧折线图拉取
    /// </summary>
    /// <param name="videoIds">视频ids</param>
    /// <param name="creatorId">创建者用户id</param>
    /// <param name="startTime">查询开始时间</param>
    /// <param name="endTime">查询结束时间</param>
    /// <returns></returns>
    Task<List<UserProgressByDateModel>> GetUserProgressByDateList(string videoIds, Guid creatorId, string startTime, string endTime);

    /// <summary>
    /// 听课记录按日期排序
    /// </summary>
    /// <param name="videoIds">视频ids</param>
    /// <param name="creatorId">创建者用户id</param>
    /// <returns></returns>
    Task<List<UserProgressRecordByDateModel>> GetUserProgressRecordByDateList(string videoIds, Guid creatorId);

    /// <summary>
    /// 合并学习进度
    /// </summary>
    /// <returns></returns>
    Task MergeLearnProgressDo();

    /// <summary>
    /// 按产品id，科目id，用户id，获取学习总时长（单位秒）
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<float> GetLearnDurations(Guid productId, Guid subjectId, Guid userId);
}