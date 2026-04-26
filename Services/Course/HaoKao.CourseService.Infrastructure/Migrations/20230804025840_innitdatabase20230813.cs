using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CourseVideoModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class innitdatabase20230813 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.AlterColumn<decimal>(
                    name: "Duration",
                    table: GetShardingTableName<CourseVideo>(),
                    type: "decimal(65,30)",
                    nullable: false,
                    oldClrType: typeof(long),
                    oldType: "bigint");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.AlterColumn<long>(
                    name: "Duration",
                    table: GetShardingTableName<CourseVideo>(),
                    type: "bigint",
                    nullable: false,
                    oldClrType: typeof(decimal),
                    oldType: "decimal(65,30)");
            }

        }
    }
}
