using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.StudentService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.StudentService.Infrastructure.Migrations
{
    public partial class addStudentParameterConfig : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<StudentParameterConfig>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<StudentParameterConfig>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        NickName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "昵称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        PropertyName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "设置值字段名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        PropertyType = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "设置值类型")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        PropertyValue = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "设置值")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Desc = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "描述")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TenantName = table.Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_StudentParameterConfig", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<StudentParameterConfig>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_StudentParameterConfig_UserId_PropertyName",
                    table: GetShardingTableName<StudentParameterConfig>(),
                    columns: new[] { "UserId", "PropertyName" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<StudentParameterConfig>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<StudentParameterConfig>());
            }

        }
    }
}
