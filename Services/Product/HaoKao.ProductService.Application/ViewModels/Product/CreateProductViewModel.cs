using HaoKao.Common.Enums;
using HaoKao.Common.RemoteModel;
using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Application.ViewModels.Product;

[AutoMapTo(typeof(Domain.Commands.Product.CreateProductCommand))]
public class CreateProductViewModel : IDto
{
    /// <summary>
    /// 产品名称
    /// </summary>
    [DisplayName("产品名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(100, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; set; }

    /// <summary>
    /// 显示名称
    /// </summary>
    [DisplayName("显示名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(100, ErrorMessage = "{0}长度不能大于{1}")]
    public string DisplayName { get; set; }

    /// <summary>
    /// 产品类别
    /// </summary>
    [DisplayName("产品类别")]
    [Required(ErrorMessage = "{0}不能为空")]
    public ProductType ProductType { get; set; }

    /// <summary>
    /// 是否上架
    /// </summary>
    [DisplayName("是否上架")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool IsShelves { get; set; }

    /// <summary>
    /// 产品描述
    /// </summary>
    [DisplayName("产品描述")]
    [MaxLength(2000, ErrorMessage = "{0}长度不能大于{1}")]
    public string Description { get; set; }

    /// <summary>
    /// 产品图片
    /// </summary>
    [DisplayName("产品图片")]
    [MaxLength(1000, ErrorMessage = "{0}长度不能大于{1}")]
    public string Icon { get; set; }

    /// <summary>
    /// 产品详情图片
    /// </summary>
    [DisplayName("产品详情图片")]
    [MaxLength(1000, ErrorMessage = "{0}长度不能大于{1}")]
    public string DetailImage { get; set; }

    /// <summary>
    /// 0-按日期 1-按天数
    /// </summary>
    [DisplayName("有效期类型")]
    [Required(ErrorMessage = "{0}不能为空")]
    public ExpiryTimeTypeEnum ExpiryTimeTypeEnum { get; set; }

    /// <summary>
    /// 按天数
    /// </summary>
    [DisplayName("按天数")]
    public int Days { get; set; }

    /// <summary>
    /// 到期时间
    /// </summary>
    [DisplayName("到期时间")]
    public DateTime ExpiryTime { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    [DisplayName("年份")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(4, ErrorMessage = "{0}长度不能大于{1}")]
    public string Year { get; set; }

    /// <summary>
    /// 价格
    /// </summary>
    [DisplayName("价格")]
    [Required(ErrorMessage = "{0}不能为空")]
    public double Price { get; set; }

    /// <summary>
    /// 优惠价格
    /// </summary>
    [DisplayName("优惠价格")]
    [Required(ErrorMessage = "{0}不能为空")]
    public double DiscountedPrice { get; set; }

    /// <summary>
    /// 苹果内购产品ID
    /// </summary>
    [DisplayName("苹果内购产品ID")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(36, ErrorMessage = "{0}长度不能大于{1}")]
    public string AppleProductId { get; set; }

    /// <summary>
    /// 答疑
    /// </summary>
    [DisplayName("答疑")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool Answering { get; set; }

    /// <summary>
    /// 对应的权限列表
    /// </summary>
    [DisplayName("对应的权限列表")]
    public ICollection<ProductPermissionViewModel> ProductPermissions { get; set; } = new List<ProductPermissionViewModel>();

    /// <summary>
    /// 对应的智辅权限列表
    /// </summary>
    [DisplayName("对应的权限列表")]
    public ICollection<AssistantProductPermissionViewModel> AssistantProductPermissions { get; set; } =
        new List<AssistantProductPermissionViewModel>();

    /// <summary>
    /// 产品协议
    /// </summary>
    [DisplayName("产品协议")]
    public Guid? Agreement { get; set; }

    /// <summary>
    /// 赠送列表
    /// </summary>
    [DisplayName("赠送列表")]
    public Dictionary<Guid, string> GiveAwayAList { get; set; } = new();

    /// <summary>
    /// 是否体验产品
    /// </summary>
    [DisplayName("是否体验产品")]
    public bool IsExperience { get; set; }
}

[AutoMapTo(typeof(Domain.Commands.ProductPermission.ProductPermissionCommand))]
public class ProductPermissionViewModel : IDto
{
    /// <summary>
    /// 对应的科目Id
    /// </summary>
    [DisplayName("对应的科目Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 对应科目名称
    /// </summary>
    [DisplayName("对应科目名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(150, ErrorMessage = "{0}长度不能大于{1}")]
    public string SubjectName { get; set; }

    /// <summary>
    /// 权限名称(课程名称、题库分类名称、资料名称)
    /// </summary>
    [DisplayName("权限名称(课程名称、题库分类名称、资料名称)")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(150, ErrorMessage = "{0}长度不能大于{1}")]
    public string PermissionName { get; set; }

    /// <summary>
    /// 权限Id(课程ID、题库分类ID、资料ID)
    /// </summary>
    [DisplayName("权限Id(课程ID、题库分类ID、资料ID)")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid PermissionId { get; set; }

    /// <summary>
    /// 所属分类(为课程时是阶段分类)
    /// </summary>
    [DisplayName("所属分类(为课程时是阶段分类)")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(150, ErrorMessage = "{0}长度不能大于{1}")]
    public string Category { get; set; }
}

[AutoMapTo(typeof(Domain.Commands.ProductPermission.AssistantProductPermissionCommand))]
public class AssistantProductPermissionViewModel : IDto
{
    /// <summary>
    /// 对应的科目Id
    /// </summary>
    [DisplayName("对应的科目Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 对应科目名称
    /// </summary>
    [DisplayName("对应科目名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string SubjectName { get; set; }

    /// <summary>
    /// 课程阶段Id
    /// </summary>
    [DisplayName("权限Id(课程ID、题库分类ID、资料ID)")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid CourseStageId { get; set; }

    /// <summary>
    /// 课程阶段名称
    /// </summary>
    [DisplayName("课程阶段名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string CourseStageName { get; set; }


    /// <summary>
    /// 开始时间
    /// </summary>
    [DisplayName("开始时间)")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    [DisplayName("结束时间)")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 智辅产品权限内容
    /// </summary>
    public ICollection<AssistantProductPermissionContent> AssistantProductPermissionContents { get; set; } = new List<AssistantProductPermissionContent>();
}


[AutoMapTo(typeof(Domain.Entities.AssistantProductPermission))]
public class AssistantProductPermissionContentViewModel : IDto
{
    /// <summary>
    /// 对应的科目Id
    /// </summary>
    [DisplayName("对应的科目Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 对应科目名称
    /// </summary>
    [DisplayName("对应科目名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string SubjectName { get; set; }

    /// <summary>
    /// 课程阶段Id
    /// </summary>
    [DisplayName("权限Id(课程ID、题库分类ID、资料ID)")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid CourseStageId { get; set; }

    /// <summary>
    /// 课程阶段名称
    /// </summary>
    [DisplayName("课程阶段名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string CourseStageName { get; set; }


    /// <summary>
    /// 开始时间
    /// </summary>
    [DisplayName("开始时间)")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    [DisplayName("结束时间)")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 智辅产品权限内容
    /// </summary>
    public ICollection<AssistantProductPermissionContent> AssistantProductPermissionContents { get; set; } = new List<AssistantProductPermissionContent>();
}