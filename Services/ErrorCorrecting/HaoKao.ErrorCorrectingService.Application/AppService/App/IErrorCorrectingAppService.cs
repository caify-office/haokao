using HaoKao.ErrorCorrectingService.Application.ViewModels.ErrorCorrecting;

namespace HaoKao.ErrorCorrectingService.Application.AppService.App;

public interface IErrorCorrectingAppService : IAppWebApiService, IManager
{
    /// <summary>
    /// 创建题库类别
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateErrorCorrectingViewModel model);
}