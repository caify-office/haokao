using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.OrderService.Infrastructure.Migrations
{
    public partial class addSpQueryAllExpiredOrder : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_QueryAllExpiredOrder`;
CREATE DEFINER=`root`@`%` PROCEDURE `Sp_QueryAllExpiredOrder`(IN `SchemaName` CHAR(60))
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
          AND LEFT(t.TABLE_NAME, 6) = 'Order_';
    /*定义 设置循环结束标识done值怎么改变 的逻辑*/
    DECLARE CONTINUE
        HANDLER FOR NOT FOUND
        SET done = 1;

    SET @SqlWhere = '(`OrderState` = 0 OR `OrderState` = 1) AND `CreateTime` <= DATE_SUB(NOW(), INTERVAL 2 HOUR)';
    SET @SQL = CONCAT('SELECT `Id`, `TenantId` FROM `Order` WHERE ', @SqlWhere);


    OPEN rs; /*打开游标*/
    REPEAT/* 循环开始 */
    FETCH rs INTO table_name;
        IF NOT done THEN
            SET @table_name_suffix = RIGHT(table_name, 32);
            SET @SQL = CONCAT(@SQL, ' UNION ALL SELECT `Id`, `TenantId` FROM `Order_', @table_name_suffix, '` WHERE ', @SqlWhere);
        END IF;
    UNTIL done END REPEAT;/*关闭游标*/
    CLOSE rs;
    SET @queryString = CONCAT('SELECT * FROM (', @SQL, ') t');
    PREPARE stmt FROM @queryString; EXECUTE stmt; DEALLOCATE PREPARE stmt;
END;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}