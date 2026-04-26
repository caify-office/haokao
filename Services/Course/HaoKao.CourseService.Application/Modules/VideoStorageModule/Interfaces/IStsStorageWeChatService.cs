namespace HaoKao.CourseService.Application.Modules.VideoStorageModule.Interfaces;

public interface IStsStorageWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 获取STS凭证
    /// </summary>
    /// <returns></returns>
    Task<dynamic> AssumeRole();
}