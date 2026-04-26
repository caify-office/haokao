using Girvs.Infrastructure;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Repositories;
using System.Text;

namespace HaoKao.ProductService.Infrastructure.Repositories;

public class SupervisorStudentRepository : Repository<SupervisorStudent>, ISupervisorStudentRepository
{
    public bool ExistSupervisorClass(Guid registerUserId, Guid productId)
    {
        return Queryable.Any(x => x.SupervisorClass.ProductId == productId && x.RegisterUserId == registerUserId);
    }

    public bool ExistSupervisorClass(Guid registerUserId, Guid productId, Guid supervisorClassId)
    {
        return Queryable.Any(x => x.SupervisorClass.ProductId == productId && x.RegisterUserId == registerUserId && x.SupervisorClassId == supervisorClassId);
    }

    public async Task<List<SupervisorStudent>> GetStatisticsData(List<SupervisorStudent> supervisorStudent, Guid productId)
    {
        //获取需要统计得学员Id
        var registerIds = supervisorStudent.Select(x => x.RegisterUserId).ToList();
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId();
        //构建统计sql并执行拿去统计结果
        var statisticsSql = CreateStatisticsSql();
        var dbContext = EngineContext.Current.Resolve<ProductDbContext>();
        var result = await dbContext.Database.SqlQueryRaw<StatisticsDataModel>(statisticsSql).ToListAsync();
        //给学员统计数据赋值
        foreach (var item in supervisorStudent)
        {
            var target = result.Find(x => x.RegisterUserId == item.RegisterUserId);
            if (target == null) continue;
            item.CourseDuration = target.Duration;
            item.MaxProgress = target.MaxProgress;
            item.CourseRatio = target.CourseRatio;
            item.CourseCount = target.CourseCount;
            item.IsEndCourseCount = target.IsEndCourseCount;
            item.LastLearnTime = target.LastLearnCourseTime;
        }
        return supervisorStudent;

        string CreateStatisticsSql()
        {
            var sb = new StringBuilder();
            sb.Append(@"WITH
    `UserPermission` AS (
        SELECT DISTINCT
            sp.`TenantId`,
            sp.`ProductId`,
            sp.`StudentId` AS `RegisterUserId`,
            sp.`ProductType`,
            pp.`PermissionId`
        FROM `StudentPermission` sp
        JOIN `ProductPermission` pp
          ON sp.`ProductId` = pp.`ProductId`
         AND sp.`TenantId` = pp.`TenantId`
         AND sp.`Enable` = 1 
         AND 
               (");
            var studentIdQuery = new StringBuilder();
            foreach (var studentId in registerIds)
            {
                studentIdQuery.Append($" sp.StudentId='{studentId}' or");
            }
            var lastIndex = studentIdQuery.ToString().LastIndexOf("or", StringComparison.OrdinalIgnoreCase);
            if (lastIndex != -1)
            {
                studentIdQuery.Remove(lastIndex, 2);
            }
            sb.Append(studentIdQuery.ToString());
            sb.Append($@")
               AND sp.ProductId='{productId}' 
                   AND sp.TenantId='{tenantId}'
        ORDER BY pp.`PermissionId`
    ),
    `Course` AS (
        SELECT DISTINCT
            courseChapter.`CourseId`,
            courseVideo.Id AS `CourseVideoId`,
            FLOOR(courseVideo.Duration/60) as `Duration` ,
            courseChapter.`TenantId`
        FROM `MergeChapter` AS courseChapter
        JOIN `MergeVideo` AS courseVideo
               ON courseChapter.`Id` = courseVideo.`CourseChapterId`
              AND courseChapter.`TenantId` = courseVideo.`TenantId`
                            AND  courseVideo.`Duration` IS NOT NULL 
        ORDER BY courseChapter.`CourseId`
    ),
    `LearnProgress` AS (
        SELECT
            `TenantId`, `CreatorId`, `VideoId`,
            FLOOR( MAX(`Progress`) /60)  AS `Progress` ,
            FLOOR( MAX(`TotalProgress`) /60) AS `TotalProgress`,
            FLOOR( MAX(`MaxProgress`) /60)  AS `MaxProgress`,
            MAX(`CreateTime`) AS `LastLearnCourseTime`,
                        MAX(`IsEnd`)` IsEnd`
        FROM `MergeProgress` 
        GROUP BY `TenantId`, `CreatorId`, `VideoId`
    ),
    `UserLearnProgress` AS (
        SELECT
            p.`TenantId`,
            p.`RegisterUserId`,
            c.`CourseVideoId`,
            c.`Duration`,
            COALESCE(`MaxProgress`, 0) `MaxProgress`,
            lp.`LastLearnCourseTime`,
            COALESCE(lp.`IsEnd`,0)  `IsEnd`
        FROM `UserPermission` p
        JOIN `Course` c
          ON p.`TenantId` = c.`TenantId`
         AND p.`PermissionId` = c.`CourseId`
         AND p.`ProductType` = 1
        LEFT JOIN `LearnProgress` lp
               ON p.`TenantId` = lp.`TenantId`
              AND c.`CourseVideoId` = lp.`VideoId`
              AND p.`RegisterUserId` = lp.`CreatorId`
    ),
     `UserLearnStatistics` AS(
        SELECT
            `TenantId`,
            `RegisterUserId`,
            SUM(`Duration`) `Duration`,
            IFNULL(SUM(`MaxProgress`),0)   `MaxProgress`,
            IFNULL(ROUND(SUM(`MaxProgress`) / SUM(`Duration`), 2), 0) AS `CourseRatio`,
            IFNULL(Count(1),0)  as `CourseCount`,
           IFNULL(Count( CASE WHEN `IsEnd` =1 THEN 1 END),0)  AS `IsEndCourseCount`,
            MAX(`LastLearnCourseTime`) `LastLearnCourseTime`
        FROM `UserLearnProgress`
        GROUP BY `TenantId`, `RegisterUserId`
    )

        SELECT DISTINCT * FROM `UserLearnStatistics` ;");

            return sb.ToString();
        }
    }
}

internal class StatisticsDataModel
{
    public Guid TenantId { get; set; }

    public Guid RegisterUserId { get; set; }

    public float Duration { get; set; }

    public float MaxProgress { get; set; }

    public float CourseRatio { get; set; }

    public int CourseCount { get; set; }

    public int IsEndCourseCount { get; set; }

    public DateTime? LastLearnCourseTime { get; set; }
}