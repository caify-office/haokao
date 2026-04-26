using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.StudentService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.StudentService.Infrastructure.Migrations
{
    public partial class initdatabase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


           
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<Student>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        NickName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "昵称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Phone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "手机号码")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "邮箱")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        IsFirst = table.Column<int>(type: "int", nullable: false),
                        StudyBase = table.Column<int>(type: "int", nullable: false),
                        SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        SubjectName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "科目名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Student", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
          
            
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_QueryAllStudent`;
CREATE PROCEDURE `Sp_QueryAllStudent`(IN userid CHAR (36),IN SchemaName CHAR (60))
BEGIN/*用于判断是否结束循环*/
DECLARE done INT DEFAULT 0;/*用于存储结果集记录*/
DECLARE table_name VARCHAR (150);/*定义游标*/
DECLARE rs CURSOR FOR 
SELECT t.TABLE_NAME FROM information_schema.`TABLES` t WHERE t.TABLE_SCHEMA=SchemaName AND LEFT (t.TABLE_NAME,8)='Student_';/*定义 设置循环结束标识done值怎么改变 的逻辑*/
DECLARE CONTINUE 
HANDLER FOR NOT FOUND 
SET done=1; 
SET @SQL=CONCAT('
		SELECT
		*
		FROM
		Student e 
		WHERE
	e.UserId =''',userid,''''); 
SET @table_name_suffix='';/*打开游标*/
SET @field_exist_sql = '';
SET @FieldCount = 0;
OPEN rs;/* 循环开始 */
REPEAT FETCH rs INTO table_name; 
IF NOT done THEN 	
  SET @field_exist_sql = CONCAT('select count(0) INTO @FieldCount from information_schema.columns where table_name = ''',table_name,''' and column_name = ''CurrentNodeName''');
	PREPARE exist_result FROM @field_exist_sql;
	EXECUTE exist_result;
	DEALLOCATE PREPARE exist_result;
	
  IF @FieldCount = 0 THEN
		SET @SQL=CONCAT(@SQL,' UNION ALL SELECT
		*
						FROM
						',table_name,' e where e.UserId=''',userid,''''); 
	ELSE
		SET @SQL=CONCAT(@SQL,' UNION ALL SELECT	SELECT
		*
						FROM
						',table_name,' e where e.UserId=''',userid,''''); 
	END IF;
END IF; 
UNTIL done END REPEAT;/*关闭游标*/
CLOSE rs; PREPARE stmt FROM @SQL; 
EXECUTE stmt; 
DEALLOCATE PREPARE stmt; END");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
                migrationBuilder.DropTable(
                    name: GetShardingTableName<Student>());
            

        }
    }
}
