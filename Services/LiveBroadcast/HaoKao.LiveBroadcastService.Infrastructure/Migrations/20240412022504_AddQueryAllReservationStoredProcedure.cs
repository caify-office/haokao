using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LiveBroadcastService.Infrastructure.Migrations
{
    public partial class AddQueryAllReservationStoredProcedure : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_QueryAllReservation`;
CREATE DEFINER=`root`@`%` PROCEDURE `Sp_QueryAllReservation`(IN `SchemaName` CHAR(60))
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
          AND LEFT(t.TABLE_NAME, LENGTH('LiveReservation_')) = 'LiveReservation_';
    /*定义 设置循环结束标识done值怎么改变 的逻辑*/
    DECLARE CONTINUE
        HANDLER FOR NOT FOUND
        SET done = 1;

    SET @SqlWhere = 't1.LiveStatus = 0 AND t1.StartTime > NOW() AND TIMESTAMPDIFF(MINUTE, NOW(), t1.StartTime) <= 5 AND t0.Notified = false';

    SET @SQL = CONCAT('SELECT t0.Id, t1.`Name`, t1.StartTime, t1.EndTime, t0.Phone, t0.ReservationSource, t0.TenantId, t0.OpenId
    FROM LiveReservation t0
    JOIN LiveVideo t1 ON t1.Id = t0.LiveVideoId
    WHERE ', @SqlWhere);

    OPEN rs; /*打开游标*/
    REPEAT/* 循环开始 */
    FETCH rs INTO table_name;
        IF NOT done THEN
            SET @table_name_suffix = RIGHT(table_name, 32);
            SET @SQL = CONCAT(@SQL, ' UNION ALL (SELECT t0.Id, t1.`Name`, t1.StartTime, t1.EndTime, t0.Phone, t0.ReservationSource, t0.TenantId, t0.OpenId
    FROM LiveReservation_', @table_name_suffix, ' t0
    JOIN LiveVideo_', @table_name_suffix, ' t1 ON t1.Id = t0.LiveVideoId
    WHERE ', @SqlWhere , ')');
        END IF;
    UNTIL done END REPEAT;/*关闭游标*/
    CLOSE rs;
    SET @queryString = CONCAT('SELECT * FROM (', @SQL, ') t');
    PREPARE stmt FROM @queryString; EXECUTE stmt; DEALLOCATE PREPARE stmt;
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
