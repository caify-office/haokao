using System;
using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.NotificationMessageService.Infrastructure.Migrations
{
    public partial class addMessageSeedData : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<MobileMessageSetting>())
            {
                migrationBuilder.UpdateData(
                    table: GetShardingTableName<MobileMessageSetting>(),
                    keyColumn: "Id",
                    keyValue: new Guid("d26043f7-c92f-4265-b859-44fcce01212c"),
                    column: "Templates",
                    value: "[{\"NotificationMessageType\":0,\"TemplateId\":\"1073230\",\"Desc\":\"您正在申请手机注册，验证码为：{1}，{2}分钟内有效！\"},{\"NotificationMessageType\":1,\"TemplateId\":\"1074912\",\"Desc\":\"{1}为您的登录验证码，请于{2}分钟内填写。\"},{\"NotificationMessageType\":2,\"TemplateId\":\"1076233\",\"Desc\":\"{1}为您的找回密码验证码，请于{2}分钟内填写。\"},{\"NotificationMessageType\":3,\"TemplateId\":\"1253909\",\"Desc\":\"验证码：{1}，您正在进行变更手机号操作，验证码{2}分钟内有效。请勿将验证码告知他人。\"},{\"NotificationMessageType\":4,\"TemplateId\":\"1403274\",\"Desc\":\"您好，您本次报名的{1}，完成{2}环节执行，状态为：{3}，如非本人操作，请忽略本短信。\"},{\"NotificationMessageType\":5,\"TemplateId\":\"1220437\",\"Desc\":\"您好，恭喜您{1} 报名成功，请留意考试时间。\"},{\"NotificationMessageType\":6,\"TemplateId\":\"2121845\",\"Desc\":\"您预约的“{1}”还有5分钟（{2}）就要开播啦，记得来听课哟！\"},{\"NotificationMessageType\":8,\"TemplateId\":\"2271962\",\"Desc\":\"下午好，好慧考小助手查询到您几日未开展学习，马上要临考了，请抓紧时间观看课程哦 \"},{\"NotificationMessageType\":9,\"TemplateId\":\"2271964\",\"Desc\":\"忙完了，请记得每日学习 \"},{\"NotificationMessageType\":10,\"TemplateId\":\"2271969\",\"Desc\":\"每天学习两小时，早日上岸，加油 \"},{\"NotificationMessageType\":11,\"TemplateId\":\"2271971\",\"Desc\":\"学习是每天的头等大事！！\"},{\"NotificationMessageType\":12,\"TemplateId\":\"2271972\",\"Desc\":\"好慧考温馨提醒：课程学完了，可进入题库作答巩固知识点哦~ \"}]");
            }


            if (IsCreateShardingTable<WechatMessageSetting>())
            {
                migrationBuilder.UpdateData(
                    table: GetShardingTableName<WechatMessageSetting>(),
                    keyColumn: "Id",
                    keyValue: new Guid("c3032e7b-477f-4922-8054-e61a45610a49"),
                    column: "Templates",
                    value: "[{\"NotificationMessageType\":6,\"TemplateId\":\"Gtqw_tKWsvH07P7o-Fvkzgfg78opmDJs-7d696daWOM\",\"Desc\":\"直播开播提醒\\n直播间名称: {{thing6.DATA}}\\n开播时间: {{date7.DATA}}\"},{\"NotificationMessageType\":7,\"TemplateId\":\"v3NjkJkYissNTiBO4SDJwrC1A_bQNjuT2zqsXBdXuuM\",\"Desc\":\"温馨提示: 卡券即将到期，点击查看详情使用\\n卡券名称: {{thing2.DATA}}\\n过期时间: {{time3.DATA}}\"}]");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<MobileMessageSetting>())
            {
                migrationBuilder.UpdateData(
                    table: GetShardingTableName<MobileMessageSetting>(),
                    keyColumn: "Id",
                    keyValue: new Guid("d26043f7-c92f-4265-b859-44fcce01212c"),
                    column: "Templates",
                    value: "[{\"NotificationMessageType\":0,\"TemplateId\":\"1073230\",\"Desc\":\"您正在申请手机注册，验证码为：{1}，{2}分钟内有效！\"},{\"NotificationMessageType\":1,\"TemplateId\":\"1074912\",\"Desc\":\"{1}为您的登录验证码，请于{2}分钟内填写。\"},{\"NotificationMessageType\":2,\"TemplateId\":\"1076233\",\"Desc\":\"{1}为您的找回密码验证码，请于{2}分钟内填写。\"},{\"NotificationMessageType\":3,\"TemplateId\":\"1253909\",\"Desc\":\"验证码：{1}，您正在进行变更手机号操作，验证码{2}分钟内有效。请勿将验证码告知他人。\"},{\"NotificationMessageType\":4,\"TemplateId\":\"1403274\",\"Desc\":\"您好，您本次报名的{1}，完成{2}环节执行，状态为：{3}，如非本人操作，请忽略本短信。\"},{\"NotificationMessageType\":5,\"TemplateId\":\"1220437\",\"Desc\":\"您好，恭喜您{1} 报名成功，请留意考试时间。\"},{\"NotificationMessageType\":6,\"TemplateId\":\"2121845 \",\"Desc\":\"您预约的“{1}”还有5分钟（{2}）就要开播啦，记得来听课哟！\"}]");
            }


            if (IsCreateShardingTable<WechatMessageSetting>())
            {
                migrationBuilder.UpdateData(
                    table: GetShardingTableName<WechatMessageSetting>(),
                    keyColumn: "Id",
                    keyValue: new Guid("c3032e7b-477f-4922-8054-e61a45610a49"),
                    column: "Templates",
                    value: "[{\"NotificationMessageType\":6,\"TemplateId\":\"Gtqw_tKWsvH07P7o-Fvkzgfg78opmDJs-7d696daWOM\",\"Desc\":\"直播开播提醒\\n直播间名称: {thing6.DATA}\\n开播时间: {date&.DATA}\"},{\"NotificationMessageType\":7,\"TemplateId\":\"v3NjkJkYissNTiBO4SDJwrC1A_bQNjuT2zqsXBdXuuM\",\"Desc\":\"温馨提示: 卡券即将到期，点击查看详情使用\\n卡券名称: {thing2.DATA}\\n过期时间: {time3.DATA}\"}]");
            }

        }
    }
}
