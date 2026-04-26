using System;
using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.NotificationMessageService.Infrastructure.Migrations
{
    public partial class initdatatable : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<InSiteMessageSetting>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<InSiteMessageSetting>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        AppId = table.Column<string>(type: "varchar(100)", nullable: true, comment: "短信平台 AppId")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        AppSecret = table.Column<string>(type: "varchar(200)", nullable: true, comment: "短信平台 AppSecret")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Templates = table.Column<string>(type: "text", nullable: true, comment: "模板配置")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_InSiteMessageSetting", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<MobileMessageSetting>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<MobileMessageSetting>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        MobileMessagePlatform = table.Column<int>(type: "int", nullable: false),
                        SmsSdkAppId = table.Column<string>(type: "varchar(100)", nullable: true, comment: "短信SDk 应用ID")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        SignList = table.Column<string>(type: "varchar(4000)", nullable: true, comment: "签名列表")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        DefaultSign = table.Column<string>(type: "varchar(40)", nullable: true, comment: "默认签名")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        AppId = table.Column<string>(type: "varchar(100)", nullable: true, comment: "短信平台 AppId")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        AppSecret = table.Column<string>(type: "varchar(200)", nullable: true, comment: "短信平台 AppSecret")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Templates = table.Column<string>(type: "text", nullable: true, comment: "模板配置")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_MobileMessageSetting", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<NotificationMessage>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<NotificationMessage>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Title = table.Column<string>(type: "varchar(100)", nullable: true, comment: "消息标题")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ParameterContent = table.Column<string>(type: "varchar(500)", nullable: true, comment: "消息内容")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorName = table.Column<string>(type: "varchar(50)", nullable: true, comment: "创建者")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        ReceivingChannel = table.Column<long>(type: "bigint", nullable: false),
                        SendState = table.Column<int>(type: "int", nullable: false),
                        NotificationMessageType = table.Column<int>(type: "int", nullable: false),
                        MessageTemplateId = table.Column<string>(type: "varchar(100)", nullable: true, comment: "消息模板Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Failure = table.Column<string>(type: "varchar(255)", nullable: true, comment: "发送失败内容")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Receiver = table.Column<string>(type: "varchar(50)", nullable: true, comment: "接收者")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        IsRead = table.Column<bool>(type: "tinyint(1)", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_NotificationMessage", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<TenantSignSetting>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<TenantSignSetting>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Sign = table.Column<string>(type: "varchar(100)", nullable: true, comment: "租户签名名称")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_TenantSignSetting", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<WechatMessageSetting>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<WechatMessageSetting>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        AppId = table.Column<string>(type: "varchar(100)", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        AppSecret = table.Column<string>(type: "varchar(200)", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Templates = table.Column<string>(type: "text", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_WechatMessageSetting", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<InSiteMessageSetting>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<InSiteMessageSetting>(),
                    columns: new[] { "Id", "AppId", "AppSecret", "Templates" },
                    values: new object[] { new Guid("4f579602-853c-482b-893f-48f755e4cc40"), null, null, "[{\"NotificationMessageType\":4,\"TemplateId\":\"您报考的{0}考试已完成{1}，请继续报考并在报名时间内完成报考。\",\"Desc\":\"您报考的{0}考试已完成{1}，请继续报考并在报名时间内完成报考。\"}]" });
            }


            if (IsCreateShardingTable<MobileMessageSetting>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<MobileMessageSetting>(),
                    columns: new[] { "Id", "AppId", "AppSecret", "DefaultSign", "MobileMessagePlatform", "SignList", "SmsSdkAppId", "Templates" },
                    values: new object[] { new Guid("d26043f7-c92f-4265-b859-44fcce01212c"), "AKIDKVVGbCYdS2oMQFbADE9qHSdbKBc2ayR6", "rU1JXgeU9dttds1uYzISRAOBj36tGQVW", "好慧考", 1, "好慧考,经济师云课堂", "1400559681", "[{\"NotificationMessageType\":0,\"TemplateId\":\"1073230\",\"Desc\":\"您正在申请手机注册，验证码为：{1}，{2}分钟内有效！\"},{\"NotificationMessageType\":1,\"TemplateId\":\"1074912\",\"Desc\":\"{1}为您的登录验证码，请于{2}分钟内填写。\"},{\"NotificationMessageType\":2,\"TemplateId\":\"1076233\",\"Desc\":\"{1}为您的找回密码验证码，请于{2}分钟内填写。\"},{\"NotificationMessageType\":3,\"TemplateId\":\"1253909\",\"Desc\":\"验证码：{1}，您正在进行变更手机号操作，验证码{2}分钟内有效。请勿将验证码告知他人。\"},{\"NotificationMessageType\":4,\"TemplateId\":\"1403274\",\"Desc\":\"您好，您本次报名的{1}，完成{2}环节执行，状态为：{3}，如非本人操作，请忽略本短信。\"},{\"NotificationMessageType\":5,\"TemplateId\":\"1220437\",\"Desc\":\"您好，恭喜您{1} 报名成功，请留意考试时间。\"}]" });
            }


            if (IsCreateShardingTable<WechatMessageSetting>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<WechatMessageSetting>(),
                    columns: new[] { "Id", "AppId", "AppSecret", "Templates" },
                    values: new object[] { new Guid("c3032e7b-477f-4922-8054-e61a45610a49"), "webxinXXXXXXX", "webxinAppSecret", "[{\"NotificationMessageType\":4,\"TemplateId\":\"微信模板Id\",\"Desc\":\"报名成功通知 {{first.DATA}} 考试名称:{{keyword1.DATA}} 环节名称:{{keyword2.DATA}} 报名人:{{keyword3.DATA}} 申请时间:{{keyword4.DATA}} 备注:{{remark.DATA}}\"}]" });
            }


            if (IsCreateShardingTable<NotificationMessage>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_NotificationMessage_CreateTime_Receiver_ParameterContent_Tit~",
                    table: GetShardingTableName<NotificationMessage>(),
                    columns: new[] { "CreateTime", "Receiver", "ParameterContent", "Title", "NotificationMessageType", "ReceivingChannel", "SendState", "IsRead" });
            }

            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS Sp_QueryAllSiteMessage;
CREATE DEFINER=`root`@`%` PROCEDURE `Sp_QueryAllSiteMessage`(IN IdCard CHAR (18),IN SchemaName CHAR (60),IN  startt INT , in endd INT,out count INT)
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

SET @table_name_suffix='';/*打开游标*/
OPEN rs;/* 循环开始 */
REPEAT FETCH rs INTO table_name; IF NOT done THEN 
SET @table_name_suffix=RIGHT (table_name,33); 
SET @SQL=CONCAT(@SQL,' UNION ALL 	SELECT
		r.Id Id,
		r.Title Title,
		r.ParameterContent ParameterContent,
		r.CreateTime CreateTime,
		r.IsRead IsRead,
		r.TenantId TenantId
    FROM
	  NotificationMessage',@table_name_suffix,' r
	  where r.IdCard=''',IdCard,'''AND',CateGoryStr,'AND r.SendState=1'); END IF; UNTIL done END REPEAT;/*关闭游标*/		
CLOSE rs; 
SET @CountSQL=CONCAT('SELECT Count(1) into @Count FROM (',@SQL,') a');
		SET @LimitSQL=CONCAT('SELECT * FROM (',@SQL,') a  ORDER BY a.CreateTime DESC LIMIT ', startt,',',endd);
PREPARE stmt FROM @LimitSQL; EXECUTE stmt; DEALLOCATE PREPARE stmt;
PREPARE stmt FROM @CountSQL; EXECUTE stmt; DEALLOCATE PREPARE stmt;
set count=@Count;
 END");


            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS Sp_QueryAllSiteUnReadCount;
CREATE DEFINER=`root`@`%` PROCEDURE `Sp_QueryAllSiteUnReadCount`(IN IdCard CHAR (18),IN SchemaName CHAR (60),OUT count INT)
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
SET @table_name_suffix='';/*打开游标*/
OPEN rs;/* 循环开始 */
REPEAT FETCH rs INTO table_name; IF NOT done THEN 
SET @table_name_suffix=RIGHT (table_name,33); 
SET @SQL=CONCAT(@SQL,' UNION ALL 	SELECT
		r.Id Id,
		r.IsRead IsRead
		FROM
	  NotificationMessage',@table_name_suffix,' r
	  where r.IdCard=''',IdCard,'''AND',CateGoryStr,'AND r.SendState=1'); END IF; UNTIL done END REPEAT;/*关闭游标*/		
CLOSE rs; 
SET @CountSQL=CONCAT('SELECT Count(1) into @Count FROM (',@SQL,') a WHERE  a.IsRead= 0');
PREPARE stmt FROM @CountSQL; EXECUTE stmt; DEALLOCATE PREPARE stmt;
set count=@Count;
 END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<InSiteMessageSetting>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<InSiteMessageSetting>());
            }


            if (IsCreateShardingTable<MobileMessageSetting>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<MobileMessageSetting>());
            }


            if (IsCreateShardingTable<NotificationMessage>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<NotificationMessage>());
            }


            if (IsCreateShardingTable<TenantSignSetting>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<TenantSignSetting>());
            }


            if (IsCreateShardingTable<WechatMessageSetting>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<WechatMessageSetting>());
            }

        }
    }
}
