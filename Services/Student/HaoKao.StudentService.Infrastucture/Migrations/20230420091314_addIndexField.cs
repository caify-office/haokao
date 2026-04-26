using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.StudentService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.StudentService.Infrastructure.Migrations
{
    public partial class addIndexField : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
                migrationBuilder.AddColumn<string>(
                    name: "TenantName",
                    table: GetShardingTableName<Student>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "租户名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            


            
                migrationBuilder.CreateIndex(
                    name: "IX_Student_UserId_Phone_NickName_TenantId",
                    table: GetShardingTableName<Student>(),
                    columns: new[] { "UserId", "Phone", "NickName", "TenantId" });
            

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
                migrationBuilder.DropIndex(
                    name: "IX_Student_UserId_Phone_NickName_TenantId",
                    table: GetShardingTableName<Student>());
            


            
                migrationBuilder.DropColumn(
                    name: "TenantName",
                    table: GetShardingTableName<Student>());
            

        }
    }
}
