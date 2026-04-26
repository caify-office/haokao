using HaoKao.CourseService.Domain.CourseChapterModule;
using HaoKao.CourseService.Domain.CourseModule;
using HaoKao.CourseService.Domain.CourseVideoModule;

namespace HaoKao.CourseService.Infrastructure.Repositories;

public class CourseChapterRepository : Repository<CourseChapter>, ICourseChapterRepository
{
    public async Task<int> ClearCourseChapter(Guid id)
    {
        var tableName = EngineContext.Current.GetEntityShardingTableParameter<CourseChapter>().GetCurrentShardingTableName();
        var strSql = $"DELETE FROM {tableName} WHERE CourseId='{id}'";
        var context = EngineContext.Current.Resolve<CourseDbContext>();
        return await context.Database.ExecuteSqlRawAsync(strSql);
    }

    /// <summary>
    /// 章节视频树形列表,需要将视频也兼容进去
    /// 只保留一级章
    /// </summary>
    /// <param name="courseId"></param>
    /// <returns></returns>
    public async Task<List<dynamic>> GetChapterVideoTreeByQueryAsync(Guid? courseId)
    {
        var courseTable = EngineContext.Current.GetEntityShardingTableParameter<Course>().GetCurrentShardingTableName();
        var courseChapterTable = EngineContext.Current.GetEntityShardingTableParameter<CourseChapter>().GetCurrentShardingTableName();
        var courseVideoTable = EngineContext.Current.GetEntityShardingTableParameter<CourseVideo>().GetCurrentShardingTableName();

        var sql = @$"
SELECT c.SubjectId, v.Id, v.VideoId, v.VideoName, v.DisplayName, v.QzName,
       v.CourseChapterId AS ChapterId ,a.CourseId, a.ParentId,
       a.Name AS ChapterName, v.IsTry
FROM {courseChapterTable} a
INNER JOIN {courseVideoTable} v ON a.Id = v.courseChapterId
LEFT JOIN {courseTable} c ON c.Id = a.CourseId
WHERE a.ParentId = '{Guid.Empty}'
  AND a.CourseId = '{courseId}'
ORDER BY a.Sort, v.Sort"; //ps暂时只取章一级数据,已和产品确认

        var context = EngineContext.Current.Resolve<CourseDbContext>();
        var connection = context.Database.GetDbConnection();
        await context.Database.OpenConnectionAsync();
        var command = connection.CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;

        var result = new List<dynamic>();
        var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new
            {
                qzname = reader["QzName"].ToString(),
                id = reader["Id"].ToString(),
                parentid = reader["ParentId"].ToString(),
                videoid = reader["VideoId"].ToString(),
                videoname = reader["VideoName"].ToString(),
                displayName = reader["DisplayName"].ToString(),
                chaptername = reader["ChapterName"].ToString(),
                chapterid = reader["ChapterId"].ToString(),
                istry = Convert.ToBoolean(reader["IsTry"]),
            });
        }
        return result;
    }

    /// <summary>
    /// 课程章节树形
    /// </summary>
    /// <param name="id"></param>
    /// <param name="courseId"></param>
    /// <returns></returns>
    public async Task<List<dynamic>> GetChapterNodeTreeByQueryAsync(Guid? id, Guid? courseId)
    {
        var tableName = EngineContext.Current.GetEntityShardingTableParameter<CourseChapter>().GetCurrentShardingTableName();
        var sql = @$"
SELECT t.*,
       CASE WHEN (SELECT COUNT(1) FROM {tableName} c WHERE c.ParentId = t.Id) > 0 
       THEN FALSE ELSE TRUE END AS IsLeafOld
FROM {tableName} t
{SqlWhere()}
ORDER BY t.Sort
";

        var context = EngineContext.Current.Resolve<CourseDbContext>();
        var connection = context.Database.GetDbConnection();
        await context.Database.OpenConnectionAsync();
        var command = connection.CreateCommand();
        command.CommandText = sql;

        command.CommandType = CommandType.Text;

        var result = new List<dynamic>();
        var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new
            {
                Id = reader["Id"].ToString(),
                CourseId = reader["CourseId"].ToString(),
                Name = reader["Name"].ToString(),
                Sort = int.Parse(reader["Sort"].ToString()),
                ParentId = reader["ParentId"].ToString(),
                IsLeaf = Convert.ToBoolean(reader["IsLeafOld"]),
            });
        }
        return result;

        StringBuilder SqlWhere()
        {
            var tenantId = EngineContext.Current.ClaimManager.GetTenantId();
            var builder = new StringBuilder($"WHERE t.TenantId = '{tenantId}' AND t.CourseId = '{courseId}'");
            if (id.HasValue)
            {
                builder.Append($"\n  AND t.ParentId = '{id.Value}'");
            }
            else
            {
                builder.Append($"\n  AND t.ParentId = '{Guid.Empty.ToString()}'");
            }
            return builder;
        }
    }

    /// <summary>
    /// 判断这些课程下是否有试听视频
    /// </summary>
    /// <param name="courseIds"></param>
    /// <returns></returns>
    public async Task<bool> IsExistTry(Guid[] courseIds)
    {
        var context = EngineContext.Current.Resolve<CourseDbContext>();
        var courseChapter = context.CourseChapters;
        var courseVideo = context.CourseVideos;
        var courseVideoResult =
            from cc in courseChapter.Where(x => courseIds.Contains(x.CourseId))
            join cv in courseVideo on cc.Id equals cv.CourseChapterId
            select cv;

        return await courseVideoResult.AnyAsync(x => x.IsTry);
    }
}