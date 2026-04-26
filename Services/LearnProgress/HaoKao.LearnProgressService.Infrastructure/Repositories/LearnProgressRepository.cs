using HaoKao.LearnProgressService.Domain.Repositories;
using System.Threading.Tasks;
using System;
using System.Linq;
using Girvs.BusinessBasis.Queries;
using System.Collections.Generic;
using Girvs.Extensions.Collections;
using Girvs.Infrastructure;
using System.Data;
using HaoKao.LearnProgressService.Domain.Queries.EntityQuery;
using HaoKao.Common.Extensions;
using HaoKao.LearnProgressService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace HaoKao.LearnProgressService.Infrastructure.Repositories;

public class LearnProgressRepository : Repository<LearnProgress>, ILearnProgressRepository
{
    /// <summary>
    /// 读取当前用户的最新播放记录
    /// </summary>
    /// <param name="videoId">视频id</param>
    /// <param name="creatorId">用户id</param>
    /// <returns></returns>
    public async Task<LearnProgress> GetLearnProgress(Guid videoId, Guid creatorId)
    {
        var result =await  Queryable.Where(predicate => predicate.CreatorId == creatorId && predicate.VideoId == videoId).ToListAsync();
        return result.OrderByDescending(x => x.CreateTime).FirstOrDefault();
    }
    /// <summary>
    /// 根据Identifier查询学习进度
    /// </summary>
    /// <param name="identifier">标识符</param>
    /// <returns></returns>

    public async Task<LearnProgress> GetLearnProgressByIdentifier(string identifier)
    {
        var result =await Queryable.Where(predicate => predicate.Identifier == identifier).ToListAsync();
        return result.FirstOrDefault();
    }
    /// <summary>
    /// 更新用户学习视频实际进度
    /// </summary>
    /// <param name="Id">主键id</param>
    /// <param name="maxProgress">愿进度</param>
    /// <param name="Duration">时长</param>
    /// <returns></returns>

    public async Task<int> UpdateFactProgress(Guid Id, int maxProgress, int Duration)
    {

        var tableName = EngineContext.Current.GetEntityShardingTableParameter<LearnProgress>().GetCurrentShardingTableName();
        string strsql = $"update  {tableName} set  maxprogress={maxProgress},totalprogress={Duration} where id='{Id}' ";
        return await ExecuteNonQueryAsync(strsql);
    }
    public async Task<int> ExecuteNonQueryAsync(string strsql)
    {
        var context = EngineContext.Current.Resolve<LearnProgressDbContext>();
        var connection = context.Database.GetDbConnection();
        await context.Database.OpenConnectionAsync();
        var command = connection.CreateCommand();
        command.CommandText = strsql;
        command.CommandType = CommandType.Text;
        var rows = await command.ExecuteNonQueryAsync();
        await context.Database.CloseConnectionAsync();
        return rows;
    }
    /// <summary>
    /// 右侧折线图拉取
    /// </summary>
    /// <param name="videoIds"></param>
    /// <param name="creatorId"></param>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <returns></returns>
    public async Task<List<UserProgressByDateModel>> GetUserProgressByDateList(string videoIds,Guid creatorId,string startTime, string endTime)
    {
        var LearnProgresstableName = EngineContext.Current.GetEntityShardingTableParameter<LearnProgress>().GetCurrentShardingTableName();

        string sqlv = $@"SELECT a.curr_date AS DATE, IFNULL(b.VALUE,0) AS progress
        FROM (
            SELECT DATE_SUB('{endTime}', INTERVAL 6 DAY) AS curr_date
            UNION ALL
            SELECT DATE_SUB('{endTime}', INTERVAL 5 DAY) AS curr_date
            UNION ALL
            SELECT DATE_SUB('{endTime}', INTERVAL 4 DAY) AS curr_date
            UNION ALL
            SELECT DATE_SUB('{endTime}', INTERVAL 3 DAY) AS curr_date
            UNION ALL
            SELECT DATE_SUB('{endTime}', INTERVAL 2 DAY) AS curr_date
            UNION ALL
            SELECT DATE_SUB('{endTime}', INTERVAL 1 DAY) AS curr_date
            UNION ALL
            SELECT '{endTime}' AS curr_date
        )a LEFT JOIN (
                 
      SELECT  a.date , COALESCE(SUM( a.progress), 0)   AS VALUE  FROM (     SELECT   
         DATE_FORMAT(createtime,'%Y-%m-%d') AS DATE,
  Progress FROM  {LearnProgresstableName}
    WHERE `CreatorId`='{creatorId}'
    AND createtime>='{startTime}'  AND  createtime<='{endTime}' AND videoid  in
('{videoIds.Replace(",", "','")}')
      GROUP BY createtime,Progress ORDER   BY createtime DESC )AS a   WHERE a.date>='{startTime}'  AND  a.date<='{endTime}' GROUP  BY  a.date 
        ) b ON a.curr_date = DATE";
      
        var context = EngineContext.Current.Resolve<LearnProgressDbContext>();
        var connection = context.Database.GetDbConnection();
        await context.Database.OpenConnectionAsync();
        var command = connection.CreateCommand();
        command.CommandText = sqlv;
        command.CommandType = CommandType.Text;

        var result = new List<UserProgressByDateModel>();
        var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new UserProgressByDateModel
            {
                Date = reader["Date"].ToString(),
                Progress = int.Parse(reader["Progress"].ToString()),
            });
        }
        context.Database.CloseConnection();
        return result;
    }
    /// <summary>
    /// 读取用户作答记录
    /// </summary>
    /// <param name="videoIds">视频ids</param>
    /// <param name="creatorId">用户id</param>
    /// <returns></returns>
    public async Task<List<UserProgressRecordByDateModel>> GetUserProgressRecordByDateList(string videoIds, Guid creatorId)
    {
        List<UserProgressRecordByDateModel> Objs = [];
        var Records = await GetUserProgressRecordsList(videoIds, creatorId);
        List<string> VideoParams = [];
        Records.ForEach(item =>
        {
            //存在播放进度会超出总进度的异常,目前不确定产生的原因,存在将播放进度置为和总进度一致,
            if (item.Progress > item.TotalProgress)
                item.Progress = item.TotalProgress;
            item.PlayDate = item.CreateTime.ToString("HH:mm").ToString();
            item.SortDate = item.CreateTime.ToString("yyyy-MM-dd");
            //同一天 多个时间段
            var userrecords = Records.Where(x => x.DATE == item.DATE && x.VideoId == item.VideoId);
            if (userrecords.Count() > 1)
            {
                if (!VideoParams.Contains(string.Format("{0}{1}", item.DATE, item.VideoId)))
                    //取最新 时间段的
                    Objs.Add(userrecords.OrderByDescending(x => x.CreateTime).FirstOrDefault());
                VideoParams.Add(string.Format("{0}{1}", item.DATE, item.VideoId));
            }
            else
                Objs.Add(item);
        });

        return Objs.OrderByDescending(x => x.DATE).ToList();
    }

    /// <summary>
    ///读取用户作答记录按videoid排序
    /// </summary>
    /// <param name="Videoids"></param>
    /// <param name="CreatorId"></param>
    /// <returns></returns>
    public async Task<List<UserProgressRecordByDateModel>> GetUserProgressRecordsList(string Videoids, Guid CreatorId)
    {
        var LearnProgresstableName = EngineContext.Current.GetEntityShardingTableParameter<LearnProgress>().GetCurrentShardingTableName();

        string sqlv = $@"SELECT videoid, createtime,maxprogress as progress, totalprogress, courseid,isend,id,ChapterId, DATE_FORMAT(createtime, '%Y-%m-%d') AS DATE  FROM  {LearnProgresstableName} WHERE videoid  in ('{Videoids.Replace(",", "','")}')
 and   `CreatorId`='{CreatorId}' 
    GROUP BY  videoid,createtime,progress,TotalProgress,courseid ,isend,id,ChapterId,DATE ";
        var context = EngineContext.Current.Resolve<LearnProgressDbContext>();
        var connection = context.Database.GetDbConnection();
        await context.Database.OpenConnectionAsync();
        var command = connection.CreateCommand();
        command.CommandText = sqlv;
        command.CommandType = CommandType.Text;

        var result = new List<UserProgressRecordByDateModel>();
        var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new UserProgressRecordByDateModel
            {
                CreateTime = DateTime.Parse(reader["createtime"].ToString()),
                Progress = int.Parse(reader["progress"].ToString()),
                TotalProgress = int.Parse(reader["totalprogress"].ToString()),
                DATE = reader["DATE"].ToString(),
                VideoId = Guid.Parse(reader["videoid"].ToString()),
                CourseId = Guid.Parse(reader["courseid"].ToString()),
                IsEnd = bool.Parse(reader["isend"].ToString()),
                Id = Guid.Parse(reader["id"].ToString()),
                ChapterId = Guid.Parse(reader["ChapterId"].ToString()),
            });
        }
        context.Database.CloseConnection();
        return result;
    }

    public override async Task<List<LearnProgress>> GetByQueryAsync(QueryBase<LearnProgress> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).CountAsync();

        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result =
                await Queryable.Where(query.GetQueryWhere())
                               .SelectProperties(query.QueryFields)
                               .OrderBy(x => x.CreateTime)
                               .Skip(query.PageStart)
                               .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }
    /// <summary>
    /// 查询学员学习进度
    /// </summary>
    /// <param name="studentId">学员权限表StudentId字段</param>
    /// <param name="productId">产品id</param>
    /// <param name="subjectId">科目id</param>
    /// <returns></returns>
    public async Task<Tuple<int,List<Dictionary<string,object>>>> QueryCourseLearningProgress(Guid studentId,Guid productId, Guid subjectId, int pageIndex, int pageSize)
    {
        var context = EngineContext.Current.Resolve<LearnProgressDbContext>();
        var database = context.Database;
        await database.OpenConnectionAsync();
        var connection = database.GetDbConnection();
        var command = connection.CreateCommand();

        var tenantId = EngineContext.Current.ClaimManager.GetTenantId();

       command.AddParameter("studentId", studentId);
       command.AddParameter("productId", productId);
       command.AddParameter("subjectId", subjectId);
       command.AddParameter("tenantId", tenantId);
       command.AddParameter("pageIndex", pageIndex);
       command.AddParameter("pageSize", pageSize);
       var totalRowsParam = command.AddOutParameter("totalRows");
       command.CommandType = CommandType.StoredProcedure;
       command.CommandText = "Sp_QueryCourseLearningProgress";

        var result = command.GetResult();

        // 获取总行数
        int totalRows = Convert.ToInt32(totalRowsParam.Value);

        await context.Database.CloseConnectionAsync();

        return new Tuple<int, List<Dictionary<string, object>>>(totalRows, result);
      
    }

    /// <summary>
    /// 统计学员学习进度
    /// </summary>
    /// <param name="studentId">学员权限表StudentId字段</param>
    /// <param name="productId">产品id</param>
    /// <param name="subjectId">科目id</param>
    /// <returns></returns>
    public async Task<Tuple<int, int, List<Dictionary<string, object>>>> CourseLearningProgressStatistics(Guid studentId, Guid productId, Guid subjectId)
    {
        var context = EngineContext.Current.Resolve<LearnProgressDbContext>();
        var database = context.Database;
        await database.OpenConnectionAsync();
        var connection = database.GetDbConnection();
        var command = connection.CreateCommand();

        var tenantId = EngineContext.Current.ClaimManager.GetTenantId();

        command.AddParameter("studentId", studentId);
        command.AddParameter("productId", productId);
        command.AddParameter("subjectId", subjectId);
        command.AddParameter("tenantId", tenantId);
        var couseCount = command.AddOutParameter("couseCount");
        var couseIsEndCount = command.AddOutParameter("couseIsEndCount");
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "Sp_CourseLearningProgressStatistics";
        //获取查询结果
        var result = command.GetResult();

        // 获取课程数
        int couseCountValue = Convert.ToInt32(couseCount.Value);
        //已学完课程数
        int couseIsEndCountValue = Convert.ToInt32(couseIsEndCount.Value);

        await context.Database.CloseConnectionAsync();

        return new Tuple<int,int, List<Dictionary<string, object>>>(couseCountValue,couseIsEndCountValue, result);

    }

     public async Task MergeLearnProgressDo()
    {
        var _serviceProvider = EngineContext.Current.Resolve<IServiceProvider>();
        //先获取合成所有的进度表并拿到结果
        await using var tenantScope = _serviceProvider.CreateAsyncScope();
        await using var tenantDbContextQuery = tenantScope.ServiceProvider.GetRequiredService<LearnProgressDbContext>();
        var tables = await tenantDbContextQuery.GetTableNameList(nameof(LearnProgress));

        foreach (var table in tables)
        {
            table.SetTenantId();
            await using var tenantScope1 = _serviceProvider.CreateAsyncScope();
            await using var tenantDbContext = tenantScope1.ServiceProvider.GetRequiredService<LearnProgressDbContext>();
            tenantDbContext.ShardingAutoMigration();
        }
        await using var tenantScope2 = _serviceProvider.CreateAsyncScope();
        await using var tenantDbContextSp = tenantScope2.ServiceProvider.GetRequiredService<LearnProgressDbContext>();

        var database = tenantDbContextSp.Database;
        await database.ExecuteSqlRawAsync("Call Sp_MergeLearnProgress() ");
    }
    /// <summary>
    /// 按产品id，科目id，用户id，获取学习总时长（单位秒）
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<float> GetLearnDurations(Guid productId, Guid subjectId, Guid userId)
    {
      var learnDurations= await Queryable.Where(x=>x.ProductId==productId&&x.SubjectId==subjectId&&x.CreatorId==userId).GroupBy(x=>new {x.ProductId,x.SubjectId,x.CreatorId,x.VideoId }).Select(x=>x.Max(a=>a.Progress)).SumAsync();
        return learnDurations;
    }
}

