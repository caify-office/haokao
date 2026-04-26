using HaoKao.Common.Enums;
using HaoKao.Common.RemoteModel;
using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Application.ViewModels.Product;

[AutoMapFrom(typeof(Domain.Entities.Product))]
[AutoMapTo(typeof(Domain.Entities.Product))]
public class BrowseProductViewModel : IDto
{

    /// <summary>
    /// 验证当前用户是否买过这个产品
    /// </summary>

    public bool IsBuy { get; set; }
    public Guid Id { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 显示名称
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// 预约起始人数（只有直播类型的产品需要用到）
    /// </summary>
    public int ReservationBaseNumber { get; set; }

    /// <summary>
    /// 产品类别
    /// </summary>
    public ProductType ProductType { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; }

    /// <summary>
    /// 是否上架
    /// </summary>
    public bool IsShelves { get; set; }

    /// <summary>
    /// 产品描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 产品图片
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 产品详情图片
    /// </summary>
    public string DetailImage { get; set; }

    /// <summary>
    /// 0-按日期 1-按天数
    /// </summary>
    public ExpiryTimeTypeEnum ExpiryTimeTypeEnum { get; set; }

    /// <summary>
    /// 按天数
    /// </summary>
    public int Days { get; set; }

    /// <summary>
    /// 到期时间
    /// </summary>
    public DateTime ExpiryTime { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    public string Year { get; set; }

    /// <summary>
    /// 价格
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// 优惠价格
    /// </summary>
    public double DiscountedPrice { get; set; }

    /// <summary>
    /// 苹果内购产品ID
    /// </summary>
    public string AppleProductId { get; set; }

    /// <summary>
    /// 答疑
    /// </summary>
    public bool Answering { get; set; }

    /// <summary>
    /// 对应的权限列表
    /// </summary>
    public ICollection<BrowseProductPermissionViewModel> ProductPermissions { get; set; } =
        new List<BrowseProductPermissionViewModel>();

    /// <summary>
    /// 对应的智辅权限列表
    /// </summary>
    public ICollection<BrowseAssistantProductPermissionViewModel> AssistantProductPermissions { get; set; } =
        new List<BrowseAssistantProductPermissionViewModel>();

    /// <summary>
    /// 产品协议
    /// </summary>
    public Guid? Agreement { get; set; }

    /// <summary>
    /// 赠送列表
    /// </summary>
    public Dictionary<Guid, string> GiveAwayAList { get; set; }

    /// <summary>
    /// 是否体验产品
    /// </summary>
    public bool IsExperience { get; set; }


}

[AutoMapFrom(typeof(Domain.Entities.ProductPermission))]
[AutoMapTo(typeof(Domain.Entities.ProductPermission))]
public class BrowseProductPermissionViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 对应的科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 对应科目名称
    /// </summary>
    public string SubjectName { get; set; }

    /// <summary>
    /// 权限名称(课程名称、题库分类名称、资料名称)
    /// </summary>
    public string PermissionName { get; set; }

    /// <summary>
    /// 权限Id(课程ID、题库分类ID、资料ID)
    /// </summary>
    public Guid PermissionId { get; set; }

    /// <summary>
    /// 所属分类(为课程时是阶段分类)
    /// </summary>
    public string Category { get; set; }
}


[AutoMapFrom(typeof(Domain.Entities.AssistantProductPermission))]
[AutoMapTo(typeof(Domain.Entities.AssistantProductPermission))]
public class BrowseAssistantProductPermissionViewModel : IDto
{
    public Guid Id { get; set; }
    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// 对应的科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 对应科目名称
    /// </summary>
    public string SubjectName { get; set; }

    /// <summary>
    /// 课程阶段Id
    /// </summary>
    public Guid CourseStageId { get; set; }

    /// <summary>
    /// 课程阶段名称
    /// </summary>
    public string CourseStageName { get; set; }
    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; }
    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; set; }
    /// <summary>
    /// 智辅产品权限内容
    /// </summary>
    public ICollection<AssistantProductPermissionContent> AssistantProductPermissionContents { get; set; } = new List<AssistantProductPermissionContent>();

    /// <summary>
    /// 租户
    /// </summary>
    public Guid TenantId { get; set; }
}