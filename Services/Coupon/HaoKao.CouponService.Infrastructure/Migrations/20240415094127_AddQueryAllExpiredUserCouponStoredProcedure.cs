using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CouponService.Infrastructure.Migrations
{
    public partial class AddQueryAllExpiredUserCouponStoredProcedure : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_QueryAllExpiredUserCoupon`;
CREATE DEFINER=`root`@`%` PROCEDURE `Sp_QueryAllExpiredUserCoupon`(IN `SchemaName` CHAR(60))
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
          AND LEFT(t.TABLE_NAME, LENGTH('UserCoupon_')) = 'UserCoupon_';
    /*定义 设置循环结束标识done值怎么改变 的逻辑*/
    DECLARE CONTINUE
        HANDLER FOR NOT FOUND
        SET done = 1;

    SET @SQL = 'SELECT t.* FROM (
  SELECT uc.Id, uc.TenantId, uc.OpenId, uc.ChannelType,
         CASE WHEN c.TimeType = 1 THEN c.EndDate ELSE DATE_ADD(uc.CreateTime, INTERVAL c.`Hour` HOUR) END EndDate,
         c.CouponName
  FROM UserCoupon uc
  JOIN Coupon c ON uc.CouponId = c.Id
  WHERE uc.IsUse = FALSE AND uc.IsLock = FALSE AND uc.Notified = FALSE
) t
WHERE t.EndDate > NOW() AND TIMESTAMPDIFF(MINUTE, NOW(), t.EndDate) <= 120';

    OPEN rs; /*打开游标*/
    REPEAT/* 循环开始 */
    FETCH rs INTO table_name;
        IF NOT done THEN
            SET @table_name_suffix = RIGHT(table_name, 32);
            SET @SQL = CONCAT(@SQL, '
UNION ALL SELECT t.* FROM (
  SELECT uc.Id, uc.TenantId, uc.OpenId, uc.ChannelType,
         CASE WHEN c.TimeType = 1 THEN c.EndDate ELSE DATE_ADD(uc.CreateTime, INTERVAL c.`Hour` HOUR) END EndDate,
         c.CouponName
  FROM UserCoupon_', @table_name_suffix, ' uc
  JOIN Coupon_', @table_name_suffix, ' c ON uc.CouponId = c.Id
  WHERE uc.IsUse = FALSE AND uc.IsLock = FALSE AND uc.Notified = FALSE
) t
WHERE t.EndDate > NOW() AND TIMESTAMPDIFF(MINUTE, NOW(), t.EndDate) <= 120');
        END IF;
    UNTIL done END REPEAT;/*关闭游标*/
    CLOSE rs;
    SET @queryString = CONCAT('SELECT * FROM (', @SQL, ') t');
    PREPARE stmt FROM @queryString; EXECUTE stmt; DEALLOCATE PREPARE stmt;
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
