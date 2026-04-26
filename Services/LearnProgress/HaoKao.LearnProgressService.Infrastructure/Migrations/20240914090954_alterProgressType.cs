using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.LearnProgressService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LearnProgressService.Infrastructure.Migrations
{
    public partial class alterProgressType : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LearnProgress>())
            {
                migrationBuilder.AlterColumn<float>(
                    name: "TotalProgress",
                    table: GetShardingTableName<LearnProgress>(),
                    type: "float",
                    nullable: false,
                    comment: "视频总时长",
                    oldClrType: typeof(int),
                    oldType: "int");
            }


            if (IsCreateShardingTable<LearnProgress>())
            {
                migrationBuilder.AlterColumn<float>(
                    name: "Progress",
                    table: GetShardingTableName<LearnProgress>(),
                    type: "float",
                    nullable: false,
                    comment: "本次学习时长",
                    oldClrType: typeof(int),
                    oldType: "int");
            }


            if (IsCreateShardingTable<LearnProgress>())
            {
                migrationBuilder.AlterColumn<float>(
                    name: "MaxProgress",
                    table: GetShardingTableName<LearnProgress>(),
                    type: "float",
                    nullable: false,
                    comment: "观看视频最大时长",
                    oldClrType: typeof(int),
                    oldType: "int");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LearnProgress>())
            {
                migrationBuilder.AlterColumn<int>(
                    name: "TotalProgress",
                    table: GetShardingTableName<LearnProgress>(),
                    type: "int",
                    nullable: false,
                    oldClrType: typeof(float),
                    oldType: "float",
                    oldComment: "视频总时长");
            }


            if (IsCreateShardingTable<LearnProgress>())
            {
                migrationBuilder.AlterColumn<int>(
                    name: "Progress",
                    table: GetShardingTableName<LearnProgress>(),
                    type: "int",
                    nullable: false,
                    oldClrType: typeof(float),
                    oldType: "float",
                    oldComment: "本次学习时长");
            }


            if (IsCreateShardingTable<LearnProgress>())
            {
                migrationBuilder.AlterColumn<int>(
                    name: "MaxProgress",
                    table: GetShardingTableName<LearnProgress>(),
                    type: "int",
                    nullable: false,
                    oldClrType: typeof(float),
                    oldType: "float",
                    oldComment: "观看视频最大时长");
            }

        }
    }
}
