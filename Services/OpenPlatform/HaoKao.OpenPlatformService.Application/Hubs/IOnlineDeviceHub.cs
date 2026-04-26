namespace HaoKao.OpenPlatformService.Application.Hubs;

public interface IOnlineDeviceHub
{
    /// <summary>
    /// 登出设备
    /// </summary>
    /// <returns></returns>
    Task NotifyMultiDeviceLogin();
}