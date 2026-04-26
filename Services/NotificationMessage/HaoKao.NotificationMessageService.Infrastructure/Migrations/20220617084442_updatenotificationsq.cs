using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.NotificationMessageService.Infrastructure.Migrations
{
    public partial class updatenotificationsq : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS Sp_QueryAllSiteMessage;
CREATE PROCEDURE `Sp_QueryAllSiteMessage`(IN IdCard CHAR (18),IN TenantAccessId CHAR(36), IN SchemaName CHAR (60),IN  startt INT , in endd INT,out count INT)
BEGIN/*用于判断是否结束循环*/
DECLARE done INT DEFAULT 0;/*用于存储结果集记录*/
DECLARE table_name VARCHAR (150);/*定义游标*/
DECLARE CateGoryStr VARCHAR (150);/*定义查询条件*/
DECLARE rs CURSOR FOR 
SELECT t.TABLE_NAME FROM information_schema.`TABLES` t WHERE t.TABLE_SCHEMA=SchemaName AND LEFT (t.TABLE_NAME,20)='NotificationMessage_';/*定义 设置循环结束标识done值怎么改变 的逻辑*/
DECLARE CONTINUE 
HANDLER FOR NOT FOUND 
SET done=1; 
SET CateGoryStr='(r.ReceivingChannel=2 OR r.ReceivingChannel=3  OR r.ReceivingChannel=6 OR r.ReceivingChannel=7)';

SET @SQL=CONCAT('
		SELECT
		r.Id Id,
		r.Title Title,
		r.ParameterContent ParameterContent,
		r.CreateTime CreateTime,
		r.IsRead IsRead,
		r.TenantId TenantId
		FROM
		NotificationMessage r 
		WHERE
	  r.IdCard =''',IdCard,''' AND',CateGoryStr,'AND r.SendState=1'); 

/*根据TenantAccessId 获取所有相关的考试Ids*/
SET @ExamIdQuerySql = CONCAT('select GROUP_CONCAT(REPLACE(e.Id,''-'','''')) into @ExamIds from Exam e where e.TenantAccessId=''',TenantAccessId,'''');
PREPARE stmt FROM @ExamIdQuerySql; EXECUTE stmt; DEALLOCATE PREPARE stmt;

/*打开游标*/
OPEN rs;
/* 循环开始 */
REPEAT FETCH rs INTO table_name; IF NOT done THEN 
SET @table_name_suffix=RIGHT (table_name,32); 
IF TenantAccessId='00000000-0000-0000-0000-000000000000' OR  LOCATE(@table_name_suffix,@ExamIds) > 0 THEN
SET @SQL=CONCAT(@SQL,' UNION ALL 	SELECT
		r.Id Id,
		r.Title Title,
		r.ParameterContent ParameterContent,
		r.CreateTime CreateTime,
		r.IsRead IsRead,
		r.TenantId TenantId
    FROM
	  NotificationMessage_',@table_name_suffix,' r
	  where r.IdCard=''',IdCard,'''AND',CateGoryStr,'AND r.SendState=1');
END IF; END IF; UNTIL done END REPEAT;/*关闭游标*/		
CLOSE rs; 


SET @CountSQL=CONCAT('SELECT Count(1) into @Count FROM (',@SQL,') a');
SET @LimitSQL=CONCAT('SELECT * FROM (',@SQL,') a  ORDER BY a.CreateTime DESC LIMIT ', startt,',',endd);
PREPARE stmt FROM @LimitSQL; EXECUTE stmt; DEALLOCATE PREPARE stmt;
PREPARE stmt FROM @CountSQL; EXECUTE stmt; DEALLOCATE PREPARE stmt;
set count=@Count;
END");


            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS Sp_QueryAllSiteUnReadCount;
CREATE PROCEDURE `Sp_QueryAllSiteUnReadCount`(IN IdCard CHAR (18),IN TenantAccessId CHAR(36),IN SchemaName CHAR (60),OUT count INT)
BEGIN/*用于判断是否结束循环*/
DECLARE done INT DEFAULT 0;/*用于存储结果集记录*/
DECLARE table_name VARCHAR (150);/*定义游标*/
DECLARE CateGoryStr VARCHAR (150);/*定义查询条件*/
DECLARE rs CURSOR FOR 
SELECT t.TABLE_NAME FROM information_schema.`TABLES` t WHERE t.TABLE_SCHEMA=SchemaName AND LEFT (t.TABLE_NAME,20)='NotificationMessage_';/*定义 设置循环结束标识done值怎么改变 的逻辑*/
DECLARE CONTINUE 
HANDLER FOR NOT FOUND 
SET done=1; 
SET CateGoryStr='(r.ReceivingChannel=2 OR r.ReceivingChannel=3  OR r.ReceivingChannel=6 OR r.ReceivingChannel=7)';

SET @SQL=CONCAT('
		SELECT
		r.Id Id,
		r.IsRead IsRead
		FROM
		NotificationMessage r 
		WHERE
	  r.IdCard =''',IdCard,''' AND',CateGoryStr,'AND r.SendState=1'); 
		
/*根据TenantAccessId 获取所有相关的考试Ids*/
SET @ExamIdQuerySql = CONCAT('select GROUP_CONCAT(REPLACE(e.Id,''-'','''')) into @ExamIds from Exam e where e.TenantAccessId=''',TenantAccessId,'''');
PREPARE stmt FROM @ExamIdQuerySql; EXECUTE stmt; DEALLOCATE PREPARE stmt;

/*打开游标*/
OPEN rs;
/* 循环开始 */
REPEAT FETCH rs INTO table_name; IF NOT done THEN 
SET @table_name_suffix=RIGHT (table_name,32); 
IF TenantAccessId='00000000-0000-0000-0000-000000000000' OR  LOCATE(@table_name_suffix,@ExamIds) > 0 THEN
SET @SQL=CONCAT(@SQL,' UNION ALL 	SELECT
		r.Id Id,
		r.IsRead IsRead
		FROM
	  NotificationMessage_',@table_name_suffix,' r
	  where r.IdCard=''',IdCard,'''AND',CateGoryStr,'AND r.SendState=1'); 
END IF;
END IF; UNTIL done END REPEAT;/*关闭游标*/		
CLOSE rs; 

SET @CountSQL=CONCAT('SELECT Count(1) into @Count FROM (',@SQL,') a WHERE  a.IsRead= 0');
PREPARE stmt FROM @CountSQL; EXECUTE stmt; DEALLOCATE PREPARE stmt;
set count=@Count;
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
