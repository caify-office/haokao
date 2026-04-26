using HaoKao.ProductService.Application.ViewModels.SupervisorStudent;

namespace HaoKao.ProductService.Application.Services.WeChat;

public interface ISupervisorStudentWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 创建督学学员
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateSupervisorStudentViewModel model);
}