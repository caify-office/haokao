using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.DataStatisticsService.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class addView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            // 添加视图
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS ProgressStatistics;
CREATE VIEW ProgressStatistics AS
WITH
    `UserPermission` AS (
        SELECT DISTINCT
            sp.`TenantId`,
            sp.`ProductId`,
            sp.`StudentId` AS `RegisterUserId`,
            sp.`ExpiryTime`,
            sp.`ProductType`,
            pp.`PermissionId`,
            pp.`SubjectId`
        FROM `StudentPermission` sp
        JOIN `ProductPermission` pp
          ON sp.`ProductId` = pp.`ProductId`
         AND sp.`TenantId` = pp.`TenantId`
        WHERE sp.`Enable` = 1
        ORDER BY pp.`PermissionId`
    ),
    `Course` AS (
        SELECT DISTINCT
            courseChapter.`CourseId`,
            courseVideo.Id AS `CourseVideoId`,
            FLOOR(courseVideo.Duration/60) as `Duration` ,
            courseChapter.`TenantId`
        FROM `MergeChapter` AS courseChapter
        LEFT JOIN `MergeVideo` AS courseVideo
               ON courseChapter.`Id` = courseVideo.`CourseChapterId`
              AND courseChapter.`TenantId` = courseVideo.`TenantId`
        WHERE courseVideo.`Duration` IS NOT NULL
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
            p.`ExpiryTime`,
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
            SUM(`MaxProgress`) `MaxProgress`,
            IFNULL(ROUND(SUM(`MaxProgress`) / SUM(`Duration`), 2), 0) AS `CourseRatio`,
            Count(1) as `CourseCount`,
            Count( CASE WHEN `IsEnd` =1 THEN 1 END) AS `IsEndCourseCount`,
         -- Count(DISTINCT CourseVideoId) as `CourseCount`,
         -- Count(DISTINCT  CASE WHEN `IsEnd` =1 THEN CourseVideoId END) AS `IsEndCourseCount`,
            MAX(`LastLearnCourseTime`) `LastLearnCourseTime`,
            1 `PermissionExpiryType1`
        FROM `UserLearnProgress`
        WHERE `ExpiryTime` > NOW()
        GROUP BY `TenantId`, `RegisterUserId`
        UNION ALL
        SELECT
            `TenantId`,
            `RegisterUserId`,
            SUM(`Duration`) `Duration`,
            SUM(`MaxProgress`) `MaxProgress`,
            IFNULL(ROUND(SUM(`MaxProgress`) / SUM(`Duration`), 2), 0) AS `CourseRatio`,
            Count(1) as `CourseCount`,
            Count( CASE WHEN `IsEnd` =1 THEN 1 END) AS `IsEndCourseCount`,
         -- Count(DISTINCT CourseVideoId) as `CourseCount`,
         -- Count(DISTINCT  CASE WHEN `IsEnd` =1 THEN CourseVideoId END) AS `IsEndCourseCount`,
            MAX(`LastLearnCourseTime`) `LastLearnCourseTime`,
            2 `PermissionExpiryType1`
        FROM `UserLearnProgress`
        WHERE `ExpiryTime` < NOW()
        GROUP BY `TenantId`, `RegisterUserId`
        UNION ALL
        SELECT
            `TenantId`,
            `RegisterUserId`,
            SUM(`Duration`) `Duration`,
            SUM(`MaxProgress`) `MaxProgress`,
            IFNULL(ROUND(SUM(`MaxProgress`) / SUM(`Duration`), 2), 0) AS `CourseRatio`,
            Count(1) as `CourseCount`,
            Count( CASE WHEN `IsEnd` =1 THEN 1 END) AS `IsEndCourseCount`,
         -- Count(DISTINCT CourseVideoId) as `CourseCount`,
         -- Count(DISTINCT  CASE WHEN `IsEnd` =1 THEN CourseVideoId END) AS `IsEndCourseCount`,
            MAX(`LastLearnCourseTime`) `LastLearnCourseTime`,
            3 `PermissionExpiryType1`
        FROM `UserLearnProgress`
        GROUP BY `TenantId`, `RegisterUserId`
    ),
    `QuestionStatistics` AS (
        SELECT
            `TenantId`,
            `SubjectId`,
            `QuestionCategoryId`,
            COUNT(DISTINCT q.`Id`) AS `QuestionCount`
        FROM `UnionQuestion` q
        WHERE `QuestionTypeId` != '68eae47e-3df7-4c78-9e73-0294bcbdd7ac'
        GROUP BY `TenantId`, `SubjectId`, `QuestionCategoryId`
    ),
    `UserAnswerStatistics` AS (
        SELECT
            `u`.`TenantId`,
            `u`.`CreatorId`,
            `u`.`SubjectId`,
            `u`.`QuestionCategoryId`,
            COUNT(DISTINCT `t`.`QuestionId`) `AnsweredCount`,
            MAX(`u`.`CreateTime`) `LastAnswerTime`
        FROM `UnionAnswerRecord` AS `u`
        INNER JOIN (
            SELECT `u0`.`QuestionId`, `u0`.`UnionAnswerRecordId`
            FROM `UnionAnswerQuestion` AS `u0`
            WHERE `u0`.`QuestionTypeId` != '68eae47e-3df7-4c78-9e73-0294bcbdd7ac'
              AND `u0`.`JudgeResult` != 2
        ) AS `t` ON `u`.`Id` = `t`.`UnionAnswerRecordId`
        GROUP BY  `u`.`TenantId`, `u`.`CreatorId`, `u`.`SubjectId`, `u`.`QuestionCategoryId`
    ),
    `UserQuestionStatistics` AS (
        SELECT
            up.`TenantId`,
            up.`RegisterUserId`,
            SUM(IFNULL(qs.`QuestionCount`, 0)) `QuestionCount`,
            SUM(IFNULL(uas.`AnsweredCount`, 0)) `AnsweredCount`,
            IFNULL(ROUND(SUM(IFNULL(uas.`AnsweredCount`, 0)) / SUM(IFNULL(qs.`QuestionCount`, 0)), 2), 0) AS `AnswerRadio`,
            MAX(uas.`LastAnswerTime`) `LastAnswerTime`,
            1 AS `PermissionExpiryType2`
        FROM `UserPermission` up
        LEFT JOIN `QuestionStatistics` qs
               ON up.`TenantId` = qs.`TenantId`
              AND up.`SubjectId` = qs.`SubjectId`
              AND up.`PermissionId` = qs.`QuestionCategoryId`
        LEFT JOIN `UserAnswerStatistics` uas
               ON up.`TenantId` = uas.`TenantId`
              AND up.`RegisterUserId` = uas.`CreatorId`
              AND up.`SubjectId` = uas.`SubjectId`
              AND up.`PermissionId` = uas.`QuestionCategoryId`
        WHERE up.`ProductType` = 0 AND up.`ExpiryTime` > NOW()
        GROUP BY up.`TenantId`, up.`RegisterUserId`
        UNION ALL
        SELECT
            up.`TenantId`,
            up.`RegisterUserId`,
            SUM(IFNULL(qs.`QuestionCount`, 0)) `QuestionCount`,
            SUM(IFNULL(uas.`AnsweredCount`, 0)) `AnsweredCount`,
            IFNULL(ROUND(SUM(IFNULL(uas.`AnsweredCount`, 0)) / SUM(IFNULL(qs.`QuestionCount`, 0)), 2), 0) AS `AnswerRadio`,
            MAX(uas.`LastAnswerTime`) `LastAnswerTime`,
            2 AS `PermissionExpiryType2`
        FROM `UserPermission` up
        LEFT JOIN `QuestionStatistics` qs
               ON up.`TenantId` = qs.`TenantId`
              AND up.`SubjectId` = qs.`SubjectId`
              AND up.`PermissionId` = qs.`QuestionCategoryId`
        LEFT JOIN `UserAnswerStatistics` uas
               ON up.`TenantId` = uas.`TenantId`
              AND up.`RegisterUserId` = uas.`CreatorId`
              AND up.`SubjectId` = uas.`SubjectId`
              AND up.`PermissionId` = uas.`QuestionCategoryId`
        WHERE up.`ProductType` = 0 AND up.`ExpiryTime` < NOW()
        GROUP BY up.`TenantId`, up.`RegisterUserId`
        UNION ALL
        SELECT
            up.`TenantId`,
            up.`RegisterUserId`,
            SUM(IFNULL(qs.`QuestionCount`, 0)) `QuestionCount`,
            SUM(IFNULL(uas.`AnsweredCount`, 0)) `AnsweredCount`,
            IFNULL(ROUND(SUM(IFNULL(uas.`AnsweredCount`, 0)) / SUM(IFNULL(qs.`QuestionCount`, 0)), 2), 0) AS `AnswerRadio`,
            MAX(uas.`LastAnswerTime`) `LastAnswerTime`,
            3 AS `PermissionExpiryType2`
        FROM `UserPermission` up
        LEFT JOIN `QuestionStatistics` qs
               ON up.`TenantId` = qs.`TenantId`
              AND up.`SubjectId` = qs.`SubjectId`
              AND up.`PermissionId` = qs.`QuestionCategoryId`
        LEFT JOIN `UserAnswerStatistics` uas
               ON up.`TenantId` = uas.`TenantId`
              AND up.`RegisterUserId` = uas.`CreatorId`
              AND up.`SubjectId` = uas.`SubjectId`
              AND up.`PermissionId` = uas.`QuestionCategoryId`
        WHERE up.`ProductType` = 0
        GROUP BY up.`TenantId`, up.`RegisterUserId`
    ),
    `Result` AS (
        SELECT
            u.`Id` AS `RegisterUserId`,
            u.`NickName`, u.`Phone`,
            s.`TenantId`, s.`IsPaidStudent`,
            IFNULL(l.`MaxProgress`, 0) `MaxProgress`,
            IFNULL(l.`Duration`, 0) `CourseDuration`,
            IFNULL(l.`CourseRatio`, 0) `CourseRatio`,
            IFNULL(l.`CourseCount`, 0) `CourseCount`,
            IFNULL(l.`IsEndCourseCount`, 0) `IsEndCourseCount`,
            IFNULL(l.`PermissionExpiryType1`, 3) `PermissionExpiryType1`,
            l.`LastLearnCourseTime`,
            IFNULL(q.`QuestionCount`, 0) `QuestionCount`,
            IFNULL(q.`AnsweredCount`, 0) `AnsweredCount`,
            IFNULL(q.`AnswerRadio`, 0) `AnswerRadio`,
            IFNULL(q.`PermissionExpiryType2`, 3) `PermissionExpiryType2`,
            q.`LastAnswerTime`,
            CASE
                WHEN  l.`LastLearnCourseTime` IS NULL THEN q.`LastAnswerTime`
                WHEN q.`LastAnswerTime` IS NULL THEN l.`LastLearnCourseTime`
                WHEN l.`LastLearnCourseTime` > q.`LastAnswerTime` THEN l.`LastLearnCourseTime`
                ELSE q.`LastAnswerTime`
            END AS `LastLearnTime`
        FROM `UnionStudent` s 
        JOIN `RegisterUser` u ON u.Id = s.`RegisterUserId`
        LEFT JOIN `UserLearnStatistics` l ON l.`TenantId`= s.`TenantId` AND l.`RegisterUserId` = s.`RegisterUserId`
        LEFT JOIN `UserQuestionStatistics` q ON q.`TenantId` = s.`TenantId` AND q.`RegisterUserId` = s.`RegisterUserId`
        ORDER BY s.`TenantId`, s.`RegisterUserId`
    )
    SELECT DISTINCT * FROM `Result` WHERE CourseCount>0 OR QuestionCount>0;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 删除视图
            migrationBuilder.Sql("DROP VIEW IF EXISTS ProgressStatistics;");
        }
    }
}
