namespace HaoKao.WebsiteConfigurationService.Application.ViewModels.Column;


[AutoMapTo(typeof(Domain.Commands.Column.UpdateColumnCommand))]
public class UpdateColumnViewModel : IDto
{
    /// <summary>
    /// 域名
    /// </summary>
    [DisplayName("域名")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(500, ErrorMessage = "{0}长度不能大于{1}")]
    public string DomainName { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    [DisplayName("名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; set; }

    /// <summary>
    /// 别名
    /// </summary>
    [DisplayName("别名")]
    public string Alias { get; set; }

    /// <summary>
    /// 英文名
    /// </summary>
    [DisplayName("英文名")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string EnglishName { get; set; }

    /// <summary>
    /// 父节点id
    /// </summary>
    [DisplayName("父节点id")]
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    [DisplayName("图标")]
    public string Icon { get; set; }

    /// <summary>
    /// 当前图标
    /// </summary>
    [DisplayName("当前图标")]
    public string ActiveIcon { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [DisplayName("排序")]
    public int Sort { get; set; }
    public Guid Id { get; internal set; }
}