using HaoKao.OpenPlatformService.Domain.Entities;
using HaoKao.OpenPlatformService.Domain.Queries;

namespace HaoKao.OpenPlatformService.Application.ViewModels.ClientViewModel;

[AutoMapFrom(typeof(AccessClientQuery))]
[AutoMapTo(typeof(AccessClientQuery))]
public class ClientQueryListViewModel : QueryDtoBase<BrowseClientListViewModel>
{
    /// <summary>
    /// 客户端标识
    /// </summary>
    public string ClientId { get; set; }
    /// <summary>
    /// 客户端名称
    /// </summary>
    public string ClientName { get; set; }
}

[AutoMapFrom(typeof(AccessClient))]
public class BrowseClientListViewModel : IDto
{

    public Guid Id { get; set; }
    /// <summary>
    /// 客户端名称
    /// </summary>
    public string ClientName { get; set; }
    /// <summary>
    /// 客户端 Uri 
    /// </summary>
    public string ClientUri { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// 微标 Uri 
    /// </summary>
    public string LogoUri { get; set; }
    /// <summary>
    /// 客户端标识
    /// </summary>
    public string ClientId { get; set; }
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }
    //public bool ShowInApplist { get; set; }
    //public bool ShowInWellknown { get; set; }

    //public string WellknowServiceKey { get; set; }

    //public int DisplayOrder { get; set; }

    //public string ApiBaseUri { get; set; }
}