using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.GroupBookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.GroupBookingService.Infrastructure.Migrations
{
    public partial class initDatabase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<GroupData>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<GroupData>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        DataName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "资料名")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        SuitableSubjects = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "适用科目")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        PeopleNumber = table.Column<int>(type: "int", nullable: false, comment: "成团人数"),
                        LimitTime = table.Column<int>(type: "int", nullable: false, comment: "限制时间"),
                        Document = table.Column<string>(type: "text", nullable: true, comment: "拼团资料")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        State = table.Column<ulong>(type: "bit", nullable: true, comment: "状态"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_GroupData", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<GroupSituation>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<GroupSituation>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        GroupDataId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "组团资料Id", collation: "ascii_general_ci"),
                        DataName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "资料名")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        SuitableSubjects = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "适用科目")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        PeopleNumber = table.Column<int>(type: "int", nullable: false, comment: "成团人数"),
                        LimitTime = table.Column<int>(type: "int", nullable: false, comment: "限制时间"),
                        Document = table.Column<string>(type: "text", nullable: true, comment: "拼团资料")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        SuccessTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "组团成功时间"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_GroupSituation", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<GroupMember>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<GroupMember>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        GroupSituationId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "拼团情况Id", collation: "ascii_general_ci"),
                        GroupDataId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "拼团资料Id", collation: "ascii_general_ci"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "用户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "昵称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ImageUrl = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "用户头像Url")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        IsLeader = table.Column<ulong>(type: "bit", nullable: false, comment: "是否团长"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ExpirationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "组团过期时间"),
                        SuccessTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "组团成功时间")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_GroupMember", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<GroupMember>("FK_GroupMember_GroupSituation_GroupSituationId"),
                            column: x => x.GroupSituationId,
                            principalTable: GetShardingTableName<GroupSituation>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<GroupData>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_GroupData_DataName_SuitableSubjects_TenantId",
                    table: GetShardingTableName<GroupData>(),
                    columns: new[] { "DataName", "SuitableSubjects", "TenantId" });
            }


            if (IsCreateShardingTable<GroupMember>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_GroupMember_CreatorId_GroupDataId_TenantId",
                    table: GetShardingTableName<GroupMember>(),
                    columns: new[] { "CreatorId", "GroupDataId", "TenantId" });
            }


            if (IsCreateShardingTable<GroupMember>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_GroupMember_GroupSituationId",
                    table: GetShardingTableName<GroupMember>(),
                    column: "GroupSituationId");
            }


            if (IsCreateShardingTable<GroupSituation>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_GroupSituation_DataName_SuitableSubjects_TenantId",
                    table: GetShardingTableName<GroupSituation>(),
                    columns: new[] { "DataName", "SuitableSubjects", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<GroupData>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<GroupData>());
            }


            if (IsCreateShardingTable<GroupMember>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<GroupMember>());
            }


            if (IsCreateShardingTable<GroupSituation>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<GroupSituation>());
            }

        }
    }
}
