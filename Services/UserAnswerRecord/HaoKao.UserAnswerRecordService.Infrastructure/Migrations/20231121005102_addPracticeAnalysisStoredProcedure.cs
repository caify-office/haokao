using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Migrations
{
    public partial class addPracticeAnalysisStoredProcedure : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_BuildRecordSql`;
CREATE DEFINER=`root`@`%`  PROCEDURE `Sp_BuildRecordSql`(IN subjectId CHAR(36), IN tenantId CHAR(36), OUT dynamic_sql VARCHAR(5000))
BEGIN
    -- 答题记录表名
    SET @record_suffix = CONCAT('AnswerRecord', '_', REPLACE(tenantId, '-', ''));
    SET @record_table_sql = CONCAT('SELECT t.TABLE_NAME INTO @record_table
FROM information_schema.TABLES t
WHERE t.TABLE_SCHEMA LIKE ''%UserAnswerRecord%''
  AND t.TABLE_NAME = ''', @record_suffix, ''' ORDER BY t.TABLE_ROWS DESC LIMIT 1');

    PREPARE stmt FROM @record_table_sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;

    -- 答题记录试题表名
    SET @record_question_suffix = CONCAT('AnswerQuestion', '_', REPLACE(tenantId, '-', ''));
    SET @record_question_table_sql = CONCAT('SELECT t.TABLE_NAME INTO @record_question_table
FROM information_schema.TABLES t
WHERE t.TABLE_SCHEMA LIKE ''%UserAnswerRecord%''
  AND t.TABLE_NAME = ''', @record_question_suffix, ''' ORDER BY t.TABLE_ROWS DESC LIMIT 1');

    PREPARE stmt FROM @record_question_table_sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;

    -- 试题库名
    SET @question_suffix = CONCAT('Question', '_', REPLACE(tenantId, '-', ''));
    SET @question_table_sql = CONCAT('SELECT t.TABLE_SCHEMA, t.TABLE_NAME INTO @question_schema, @question_table
FROM information_schema.TABLES t
WHERE t.TABLE_SCHEMA LIKE ''%Question%''
  AND t.TABLE_NAME = ''', @question_suffix, ''' ORDER BY t.TABLE_ROWS DESC LIMIT 1');

    PREPARE stmt FROM @question_table_sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;

    -- 查询的时间范围, 近30天内的数据
    -- SET @startDate = DATE_SUB('2023-01-01', INTERVAL 30 DAY);
    SET @startDate = DATE_SUB(CURDATE(), INTERVAL 30 DAY);
    SET @endDate = DATE_ADD(CURDATE(), INTERVAL 1 DAY);

    SET dynamic_sql = CONCAT('WITH Questions AS (SELECT Id, AbilityIds
                   FROM ', @question_schema, '.', @question_table, '
                   WHERE SubjectId = ''', subjectId, '''
                     AND QuestionTypeId != ''68eae47e-3df7-4c78-9e73-0294bcbdd7ac''),
     AllRecords AS (SELECT uaq.QuestionId, uaq.QuestionTypeId, uaq.ParentId,
                           uaq.JudgeResult, uar.CreatorId, uar.CreateTime,
                           q.AbilityIds, uar.ElapsedTime, uar.AnswerCount
                    FROM ', @record_question_table, ' uaq
                    JOIN ', @record_table, ' uar on uar.Id = uaq.AnswerRecordId
                    JOIN Questions q on q.Id = uaq.QuestionId
                    WHERE SubjectId = ''', subjectId, '''
                      AND uar.CreateTime >= ''', @startDate, '''
                      AND uar.CreateTime < ''', @endDate, '''
                    ),
     LatestRecords AS (SELECT r.*
                       FROM AllRecords r
                       JOIN (SELECT CreatorId, QuestionId, MAX(CreateTime) AS MaxCreateTime
                             FROM AllRecords
                             WHERE JudgeResult != 2
                             GROUP BY CreatorId, QuestionId) grouped_r
                            ON r.CreatorId = grouped_r.CreatorId
                                AND r.QuestionId = grouped_r.QuestionId
                                AND r.CreateTime = grouped_r.MaxCreateTime)
');
END");

            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_PracticeSituation`;
CREATE DEFINER=`root`@`%` PROCEDURE `Sp_PracticeSituation`(IN subjectId CHAR(36), IN tenantId CHAR(36), IN userId CHAR(36))
BEGIN
    -- out结果, 存放动态SQL
    DECLARE dynamic_sql VARCHAR(5000);

    -- 调用存储过程, 生成动态SQL
    CALL Sp_BuildRecordSql(subjectId, tenantId, dynamic_sql);

    SET @userWhere = CONCAT('CreatorId = ', '\'', userId, '\'');

    -- 统计最新的答题情况
    SET @stmt_sql = CONCAT(dynamic_sql, '
    SELECT COUNT(0),
           SUM(IF(JudgeResult = 0, 1, 0)),
           SUM(IF(JudgeResult = 1, 1, 0)),
           SUM(IF(JudgeResult = 3, 1, 0))
    INTO @AnswerCount, @CorrectCount, @WrongCount, @MissedCount
    FROM LatestRecords
    WHERE ', @userWhere);
    PREPARE stmt FROM @stmt_sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;

    -- 统计总答题数和正确数
    SET @stmt_sql = CONCAT(dynamic_sql, '
    SELECT COUNT(DISTINCT QuestionId)
    INTO @TotalAnswerCount
    FROM AllRecords
    WHERE JudgeResult != 2 AND ', @userWhere);
    PREPARE stmt FROM @stmt_sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;

    SET @stmt_sql = CONCAT(dynamic_sql, '
    SELECT COUNT(DISTINCT QuestionId)
    INTO @TotalCorrectCount
    FROM AllRecords
    WHERE JudgeResult = 0 AND ', @userWhere);
    PREPARE stmt FROM @stmt_sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;

    -- 统计平均做题数量
    SET @stmt_sql = CONCAT(dynamic_sql, '
    SELECT SUM(Count) / COUNT(0) INTO @AverageAnswerCount
    FROM (SELECT COUNT(DISTINCT QuestionId) Count
          FROM AllRecords
          WHERE JudgeResult != 2
          GROUP BY CreatorId) t');
    PREPARE stmt FROM @stmt_sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;

    -- 用户拥有权的试题总数
    SET @stmt_sql = CONCAT(dynamic_sql, 'SELECT COUNT(*) INTO @QuestionCount FROM Questions');
    PREPARE stmt FROM @stmt_sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;

    -- 返回查询结果
    SELECT COALESCE(@AnswerCount, 0),
           COALESCE(@CorrectCount, 0),
           COALESCE(@WrongCount, 0),
           COALESCE(@MissedCount, 0),
           COALESCE(@TotalAnswerCount, 0),
           COALESCE(@TotalCorrectCount, 0),
           COALESCE(@QuestionCount, 0),
           COALESCE(@AverageAnswerCount, 0);
END");

            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_AccuracyRank`;
CREATE DEFINER=`root`@`%` PROCEDURE `Sp_AccuracyRank`(IN subjectId CHAR(36), IN tenantId CHAR(36))
BEGIN
    -- out结果, 存放动态SQL
    DECLARE dynamic_sql VARCHAR(5000);

    -- 调用存储过程, 生成动态SQL
    CALL Sp_BuildRecordSql(subjectId, tenantId, dynamic_sql);

    -- 执行并返回结果
    SET @stmt_sql = CONCAT(dynamic_sql, '
    SELECT COALESCE(SUM(IF(JudgeResult = 0, 1, 0)), 0) AS CorrectCount,
           COALESCE(SUM(IF(JudgeResult != 2, 1, 0)), 0) AS AnswerCount
    FROM LatestRecords
    GROUP BY CreatorId');
    PREPARE stmt FROM @stmt_sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
END");

            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_QuestionTypeAccuracy`;
CREATE DEFINER=`root`@`%` PROCEDURE `Sp_QuestionTypeAccuracy`(IN subjectId CHAR(36), IN tenantId CHAR(36), IN userId CHAR(36))
BEGIN
    -- out结果, 存放动态SQL
    DECLARE dynamic_sql VARCHAR(5000);

    -- 调用存储过程, 生成动态SQL
    CALL Sp_BuildRecordSql(subjectId, tenantId, dynamic_sql);

    -- 执行并返回结果
    SET @stmt_sql = CONCAT(dynamic_sql, '
    SELECT COALESCE(SUM(IF(JudgeResult = 0, 1, 0)), 0) AS CorrectCount,
           COALESCE(SUM(IF(JudgeResult != 2, 1, 0)), 0) AS AnswerCount,
           QuestionTypeId,
           0 IsCaseAnalysis,
           CreatorId
    FROM LatestRecords
    WHERE ParentId IS NULL
    GROUP BY CreatorId, QuestionTypeId
    UNION ALL
    SELECT COALESCE(SUM(IF(JudgeResult = 0, 1, 0)), 0) AS CorrectCount,
           COALESCE(SUM(IF(JudgeResult != 2, 1, 0)), 0) AS AnswerCount,
           QuestionTypeId,
           1 IsCaseAnalysis,
           CreatorId
    FROM LatestRecords
    WHERE ParentId IS NOT NULL
    GROUP BY CreatorId, QuestionTypeId;');
    PREPARE stmt FROM @stmt_sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
END");

            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_PracticeAbility`;
CREATE DEFINER=`root`@`%` PROCEDURE `Sp_PracticeAbility`(IN subjectId CHAR(36), IN tenantId CHAR(36))
BEGIN
    -- out结果, 存放动态SQL
    DECLARE dynamic_sql VARCHAR(5000);

    -- 调用存储过程, 生成动态SQL
    CALL Sp_BuildRecordSql(subjectId, tenantId, dynamic_sql);

    -- 执行并返回结果
    SET @stmt_sql = CONCAT(dynamic_sql, '
    SELECT COALESCE(SUM(IF(FIND_IN_SET(\'d67222e1-539c-405a-bba7-442e0b517f5a\', AbilityIds), 1, 0)), 0) Numeracy,
           COALESCE(SUM(IF(FIND_IN_SET(\'d67222e1-539c-405a-bba7-442e0b517f5a\', AbilityIds) AND JudgeResult = 0, 1, 0)), 0) CorrectNumeracy,
           COALESCE(SUM(IF(FIND_IN_SET(\'d67222e1-539c-405a-bba7-442e0b517f5d\', AbilityIds), 1, 0)), 0) Judgment,
           COALESCE(SUM(IF(FIND_IN_SET(\'d67222e1-539c-405a-bba7-442e0b517f5d\', AbilityIds) AND JudgeResult = 0, 1, 0)), 0) CorrectJudgment,
           COALESCE(SUM(IF(FIND_IN_SET(\'d67222e1-539c-405a-bba7-442e0b517f5f\', AbilityIds), 1, 0)), 0) Analytical,
           COALESCE(SUM(IF(FIND_IN_SET(\'d67222e1-539c-405a-bba7-442e0b517f5f\', AbilityIds) AND JudgeResult = 0, 1, 0)), 0) CorrectAnalytical,
           COALESCE(SUM(IF(FIND_IN_SET(\'d67222e1-539c-405a-bba7-442e0b517f5c\', AbilityIds), 1, 0)), 0) Understanding,
           COALESCE(SUM(IF(FIND_IN_SET(\'d67222e1-539c-405a-bba7-442e0b517f5c\', AbilityIds) AND JudgeResult = 0, 1, 0)), 0) CorrectUnderstanding,
           COALESCE(SUM(IF(FIND_IN_SET(\'d67222e1-539c-405a-bba7-442e0b517f5b\', AbilityIds), 1, 0)), 0) Memory,
           COALESCE(SUM(IF(FIND_IN_SET(\'d67222e1-539c-405a-bba7-442e0b517f5b\', AbilityIds) AND JudgeResult = 0, 1, 0)), 0) CorrectMemory,
           CreatorId
    FROM LatestRecords
    GROUP BY CreatorId');
    PREPARE stmt FROM @stmt_sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
END");

            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_PracticeSpeed`;
CREATE DEFINER=`root`@`%` PROCEDURE `Sp_PracticeSpeed`(IN subjectId CHAR(36), IN tenantId CHAR(36))
BEGIN
    -- out结果, 存放动态SQL
    DECLARE dynamic_sql VARCHAR(5000);

    -- 调用存储过程, 生成动态SQL
    CALL Sp_BuildRecordSql(subjectId, tenantId, dynamic_sql);

    SET @elapsed_name = CONCAT('ElapsedTimeRecord', '_', REPLACE(tenantId, '-', ''));

    -- 执行并返回结果
    SET @stmt_sql = CONCAT(dynamic_sql, '
    SELECT (SELECT COALESCE(SUM(ElapsedSeconds), 0)
            FROM `', @elapsed_name, '`
            WHERE SubjectId = ''', subjectId, '''
            AND CreatorId = t.CreatorId
            AND ProductId != ''00000000-0000-0000-0000-000000000000'') AS ElapsedTime,
           COALESCE(SUM(AnswerCount), 0) AS AnswerCount,
           CreatorId
    FROM AllRecords t
    WHERE AnswerCount > 0
    GROUP BY CreatorId');
    PREPARE stmt FROM @stmt_sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_BuildRecordSql`;");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_PracticeSituation`;");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_AccuracyRank`;");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_QuestionTypeAccuracy`;");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_PracticeAbility`;");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_PracticeSpeed`;");
        }
    }
}