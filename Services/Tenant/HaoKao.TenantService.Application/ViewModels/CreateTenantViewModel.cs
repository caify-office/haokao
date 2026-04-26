using HaoKao.TenantService.Domain.Entities;

namespace HaoKao.TenantService.Application.ViewModels;

/// <summary>
/// 添加视图模型
/// </summary>
[AutoMapFrom(typeof(Tenant))]
[AutoMapTo(typeof(Tenant))]
public class CreateTenantViewModel : IDto
{
    /// <summary>
    /// 其它名称ID，一般主要是数据字典中的Id
    /// </summary>
    public Guid? OtherId { get; set; }

    /// <summary>
    /// 其它名称
    /// </summary>
    public string OtherName { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [DisplayName("名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(1, ErrorMessage = "{0}不能小于1")]
    [MaxLength(500, ErrorMessage = "{0}不能大于500")]
    public string TenantName { get; set; }

    /// <summary>
    /// 代码
    /// </summary>
    [DisplayName("代码")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(1, ErrorMessage = "{0}不能小于1")]
    [MaxLength(20, ErrorMessage = "{0}不能大于20")]
    public string TenantNo { get; set; }

    /// <summary>
    /// 管理员账户
    /// </summary>
    [DisplayName("管理员账户")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(1, ErrorMessage = "{0}不能小于1")]
    [MaxLength(36, ErrorMessage = "{0}不能大于36")]
    public string AdminUserAcount { get; set; }

    /// <summary>
    /// 管理员姓名
    /// </summary>
    [DisplayName("管理员姓名")]
    [MaxLength(36, ErrorMessage = "{0}不能大于36")]
    public string AdminUserName { get; set; }

    /// <summary>
    /// 管理员手机号
    /// </summary>
    [DisplayName("管理员手机号")]
    [MaxLength(36, ErrorMessage = "{0}不能大于36")]
    public string AdminPhone { get; set; }

    /// <summary>
    /// 管理员密码
    /// </summary>
    [DisplayName("管理员密码")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(1, ErrorMessage = "{0}不能小于1")]
    [MaxLength(36, ErrorMessage = "{0}不能大于36")]
    public string AdminPassWord { get; set; }

    /// <summary>
    /// 所包含的功能模块
    /// </summary>
    [DisplayName("所包含的功能模块")]
    [Required(ErrorMessage = "{0}不能为空")]
    public SystemModule SystemModule { get; set; }

    /// <summary>
    /// 年度考试时间
    /// </summary>
    [DisplayName("年度考试时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime AnnualExamTime { get; set; }
}