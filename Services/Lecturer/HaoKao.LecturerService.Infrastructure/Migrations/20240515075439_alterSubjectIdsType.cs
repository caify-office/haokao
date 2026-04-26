using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.LecturerService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LecturerService.Infrastructure.Migrations
{
    public partial class alterSubjectIdsType : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Lecturer>())
            {
                migrationBuilder.Sql(@$"UPDATE {GetShardingTableName<Lecturer>()} SET {nameof(Lecturer.SubjectIds)}= JSON_ARRAY(SUBSTRING_INDEX({nameof(Lecturer.SubjectIds)}, ',', 1)) ");
                migrationBuilder.Sql(@$"UPDATE {GetShardingTableName<Lecturer>()} SET {nameof(Lecturer.SubjectNames)}= JSON_ARRAY(SUBSTRING_INDEX({nameof(Lecturer.SubjectNames)}, ',', 1)) ");
                migrationBuilder.AlterColumn<string>(
                    name: "SubjectNames",
                    table: GetShardingTableName<Lecturer>(),
                    type: "json",
                    nullable: true,
                    comment: "科目名称数组",
                    oldClrType: typeof(string),
                    oldType: "text",
                    oldNullable: true,
                    oldComment: "科目名称数组")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Lecturer>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "SubjectIds",
                    table: GetShardingTableName<Lecturer>(),
                    type: "json",
                    nullable: true,
                    comment: "科目Id数组",
                    oldClrType: typeof(string),
                    oldType: "text",
                    oldNullable: true,
                    oldComment: "科目Id数组")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Lecturer>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "SubjectNames",
                    table: GetShardingTableName<Lecturer>(),
                    type: "text",
                    nullable: true,
                    comment: "科目名称数组",
                    oldClrType: typeof(string),
                    oldType: "json",
                    oldNullable: true,
                    oldComment: "科目名称数组")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Lecturer>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "SubjectIds",
                    table: GetShardingTableName<Lecturer>(),
                    type: "text",
                    nullable: true,
                    comment: "科目Id数组",
                    oldClrType: typeof(string),
                    oldType: "json",
                    oldNullable: true,
                    oldComment: "科目Id数组")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}
