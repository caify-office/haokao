using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LiveBroadcastService.Infrastructure.Migrations
{
    public partial class AddTrackOnlineUserTrendStoredProcedure : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_TrackOnlineUserTrend`;
CREATE DEFINER=`root`@`%` PROCEDURE `Sp_TrackOnlineUserTrend`(IN `SchemaName` CHAR(60), IN `Interval` INT)
BEGIN
    /*用于判断是否结束循环*/
    DECLARE done INT DEFAULT 0;
    /*用于存储结果集记录*/
    DECLARE table_name VARCHAR(150);
    /*定义游标*/
    DECLARE rs CURSOR FOR
        SELECT t.TABLE_NAME
        FROM information_schema.`TABLES` t
        WHERE t.TABLE_SCHEMA = SchemaName
          AND LEFT(t.TABLE_NAME, LENGTH('LiveOnlineUserTrend_')) = 'LiveOnlineUserTrend_';
    /*定义 设置循环结束标识done值怎么改变 的逻辑*/
    DECLARE CONTINUE
        HANDLER FOR NOT FOUND
        SET done = 1;

    OPEN rs; /*打开游标*/
    REPEAT/* 循环开始 */
    FETCH rs INTO table_name;
        IF NOT done THEN
            SET @table_name_suffix = RIGHT(table_name, 32);
            SET @SQL = CONCAT('INSERT INTO `LiveOnlineUserTrend_', @table_name_suffix, '`(`Id`, `LiveId`, `Interval`, `TotalCount`, `OnlineCount`, `CreateTime`, `TenantId`)
SELECT UUID(), `t0`.`Id`, ', `Interval`, ', `t1`.`TotalCount`, `t1`.`OnlineCount`, NOW(), `t0`.`TenantId`
FROM `LiveVideo_', @table_name_suffix, '` `t0`
JOIN (
  SELECT `LiveId`, COUNT(1) `TotalCount`, SUM(CASE WHEN `IsOnline` THEN 1 ELSE 0 END) `OnlineCount`
  FROM `LiveOnlineUser_', @table_name_suffix, '`
  GROUP BY `LiveId`
) `t1` ON `t1`.`LiveId` = `t0`.`Id`
WHERE `t0`.`LiveStatus` = 1');
            PREPARE stmt FROM @SQL; EXECUTE stmt; DEALLOCATE PREPARE stmt;
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
