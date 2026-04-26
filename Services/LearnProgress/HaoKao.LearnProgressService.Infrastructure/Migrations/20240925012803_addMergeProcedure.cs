using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LearnProgressService.Infrastructure.Migrations
{
    public partial class addMergeProcedure : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_MergeLearnProgress`;
             CREATE DEFINER=`root`@`%` PROCEDURE `Sp_MergeLearnProgress`()
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
          AND t.Table_Name LIKE '%LearnProgress_%'; 
    /*定义 设置循环结束标识done值怎么改变 的逻辑*/
    DECLARE CONTINUE
        HANDLER FOR NOT FOUND
        SET done = 1;

    -- 1. 创建新表（如果不存在）
    CREATE TABLE IF NOT EXISTS MergeProgress LIKE LearnProgress;
    
    -- 2. 清空新表数据（如果需要从头开始合并）
    -- TRUNCATE TABLE MergeProgress;

    OPEN rs; /*打开游标*/
    REPEAT/* 循环开始 */
    FETCH rs INTO table_name;
        IF NOT done THEN
            -- 动态合并每个表的数据到新表
					SET @sql = CONCAT('INSERT INTO MergeProgress  SELECT * FROM ', table_name,' t WHERE Not Exists (SELECT 1 from MergeProgress WHERE t.Id=MergeProgress.Id) ');
					-- SELECT @sql;
					-- SET querySql =CONCAT(querySql,@sql);
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

        }
    }
}
