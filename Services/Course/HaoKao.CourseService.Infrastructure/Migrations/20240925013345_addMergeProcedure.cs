using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CourseVideoModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class addMergeProcedure : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.AlterColumn<decimal>(
                    name: "Duration",
                    table: GetShardingTableName<CourseVideo>(),
                    type: "decimal(18,2)",
                    precision: 18,
                    scale: 2,
                    nullable: false,
                    comment: "时长",
                    oldClrType: typeof(decimal),
                    oldType: "decimal(18,2)",
                    oldPrecision: 18,
                    oldScale: 2,
                    oldComment: "及格分数");
            }

            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_MergeChapter`;
            CREATE DEFINER=`root`@`%` PROCEDURE `Sp_MergeChapter`()
BEGIN
    /*用于判断是否结束循环*/
    DECLARE done INT DEFAULT 0;
    /*用于存储结果集记录*/
    DECLARE table_name VARCHAR(150);
    /*定义游标*/
    DECLARE rs CURSOR FOR
        SELECT t.TABLE_NAME
        FROM information_schema.`TABLES` t
        WHERE t.TABLE_SCHEMA  = DATABASE()
		  AND t.Table_Name  REGEXP '^CourseChapter_[0-9a-f]{32}$';
    /*定义 设置循环结束标识done值怎么改变 的逻辑*/
    DECLARE CONTINUE
        HANDLER FOR NOT FOUND
        SET done = 1;

    -- 1. 创建新表（如果不存在）
    CREATE TABLE IF NOT EXISTS MergeChapter LIKE CourseChapter;
    
    -- 2. 清空新表数据（如果需要从头开始合并）
    TRUNCATE TABLE MergeChapter;

    OPEN rs; /*打开游标*/
    REPEAT/* 循环开始 */
    FETCH rs INTO table_name;
        IF NOT done THEN
            -- 动态合并每个表的数据到新表
					SET @sql = CONCAT('INSERT INTO MergeChapter  SELECT * FROM ', table_name,' t  ');
					PREPARE stmt FROM @sql;
					EXECUTE stmt;
					DEALLOCATE PREPARE stmt;
        END IF;
    UNTIL done END REPEAT;/*关闭游标*/
    CLOSE rs;
END");

            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_MergeVideo`;
CREATE DEFINER=`root`@`%` PROCEDURE `Sp_MergeVideo`()
BEGIN
    /*用于判断是否结束循环*/
    DECLARE done INT DEFAULT 0;
    /*用于存储结果集记录*/
    DECLARE table_name VARCHAR(150);
    /*定义游标*/
    DECLARE rs CURSOR FOR
        SELECT t.TABLE_NAME
        FROM information_schema.`TABLES` t
        WHERE t.TABLE_SCHEMA  = DATABASE()
          AND t.Table_Name  REGEXP '^CourseVideo_[0-9a-f]{32}$';
    /*定义 设置循环结束标识done值怎么改变 的逻辑*/
    DECLARE CONTINUE
        HANDLER FOR NOT FOUND
        SET done = 1;

    -- 1. 创建新表（如果不存在）
    CREATE TABLE IF NOT EXISTS MergeVideo LIKE CourseVideo;
    
    -- 2. 清空新表数据（如果需要从头开始合并）
     TRUNCATE TABLE MergeVideo;

    OPEN rs; /*打开游标*/
    REPEAT/* 循环开始 */
    FETCH rs INTO table_name;
        IF NOT done THEN
            -- 动态合并每个表的数据到新表
					SET @sql = CONCAT('INSERT INTO MergeVideo  SELECT * FROM ', table_name,' t  ');
					PREPARE stmt FROM @sql;
					EXECUTE stmt;
					DEALLOCATE PREPARE stmt;
        END IF;
    UNTIL done END REPEAT;/*关闭游标*/
    CLOSE rs;
END");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.AlterColumn<decimal>(
                    name: "Duration",
                    table: GetShardingTableName<CourseVideo>(),
                    type: "decimal(18,2)",
                    precision: 18,
                    scale: 2,
                    nullable: false,
                    comment: "及格分数",
                    oldClrType: typeof(decimal),
                    oldType: "decimal(18,2)",
                    oldPrecision: 18,
                    oldScale: 2,
                    oldComment: "时长");
            }

        }
    }
}
