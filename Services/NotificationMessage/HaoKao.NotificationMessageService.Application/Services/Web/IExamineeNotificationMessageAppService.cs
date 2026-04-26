namespace HaoKao.NotificationMessageService.Application.Services.Web;

public interface IExamineeNotificationMessageAppService : IAppWebApiService
{
    /// <summary>
    /// 获取考生的站内消息列表
    /// </summary>
    /// <returns></returns>
    Task<ExamineeNotificationMessageQueryViewModel> GetByQueryAsync(
        ExamineeNotificationMessageQueryViewModel queryModel);

    /// <summary>
    /// 考生阅读指定消息
    /// </summary>
    /// <returns></returns>
    Task ReadMessage(Guid id);

    // /// <summary>
    // /// 设置考生全部已读
    // /// </summary>
    // /// <returns></returns>
    // Task ReadAll(string cardNo);

    /// <summary>
    /// 获取考生未读条数
    /// </summary>
    /// <returns></returns>
    Task<int> GetUnreadCount(string idCard, Guid tenantAccessId);

    Task<bool> FollowWeChat(string openId);

    Task<string> GetOpenIdByCode(string code);
}