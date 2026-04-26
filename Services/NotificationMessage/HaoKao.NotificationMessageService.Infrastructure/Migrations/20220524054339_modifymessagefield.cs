using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.NotificationMessageService.Infrastructure.Migrations
{
    public partial class modifymessagefield : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<NotificationMessage>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_NotificationMessage_CreateTime_Receiver_ParameterContent_Tit~",
                    table: GetShardingTableName<NotificationMessage>());
            }


            if (IsCreateShardingTable<NotificationMessage>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "IdCard",
                    table: GetShardingTableName<NotificationMessage>(),
                    type: "varchar(18)",
                    nullable: true,
                    comment: "身份证号码")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<NotificationMessage>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_NotificationMessage_CreateTime_Receiver_ParameterContent_Tit~",
                    table: GetShardingTableName<NotificationMessage>(),
                    columns: new[] { "CreateTime", "Receiver", "ParameterContent", "Title", "IdCard", "NotificationMessageType", "ReceivingChannel", "SendState", "IsRead" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<NotificationMessage>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_NotificationMessage_CreateTime_Receiver_ParameterContent_Tit~",
                    table: GetShardingTableName<NotificationMessage>());
            }


            if (IsCreateShardingTable<NotificationMessage>())
            {
                migrationBuilder.DropColumn(
                    name: "IdCard",
                    table: GetShardingTableName<NotificationMessage>());
            }


            if (IsCreateShardingTable<NotificationMessage>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_NotificationMessage_CreateTime_Receiver_ParameterContent_Tit~",
                    table: GetShardingTableName<NotificationMessage>(),
                    columns: new[] { "CreateTime", "Receiver", "ParameterContent", "Title", "NotificationMessageType", "ReceivingChannel", "SendState", "IsRead" });
            }

        }
    }
}
