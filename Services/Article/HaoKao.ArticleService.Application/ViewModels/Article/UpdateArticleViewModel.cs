namespace HaoKao.ArticleService.Application.ViewModels.Article;


[AutoMapTo(typeof(UpdateArticleCommand))]
public class UpdateArticleViewModel : IDto
{
    public Guid Id { get; set; }
    /// <summary>
    /// 标题
    /// </summary>
    [DisplayName("标题")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Title { get; set; }

    /// <summary>
    /// 所属栏目(字典Id)
    /// </summary>
    [DisplayName("所属栏目")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid Column { get; set; }
    /// <summary>
    /// 所属类别(字典Id)
    /// </summary>
    [DisplayName("所属类别")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid Category { get; set; }

    /// <summary>
    /// 是否置顶
    /// </summary>
    [DisplayName("是否置顶")]
    public bool IsTopping { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    [DisplayName("排序号")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int Sort { get; set; }

    /// <summary>
    /// 是否首页展示
    /// </summary>
    [DisplayName("是否首页展示")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool IsDisplayedOnHomepage { get; set; }

    /// <summary>
    /// 是否发布
    /// </summary>
    [DisplayName("是否发布")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool IsPublish { get; set; }

    /// <summary>
    /// 预览图
    /// </summary>
    [DisplayName("是否发布")]
    public string PreviewUrl { get; set; }
    /// <summary>
    /// 内容
    /// </summary>
    [DisplayName("内容")]
    public string Content { get; set; }

    /// <summary>
    /// 是否外部链接
    /// </summary>
    [DisplayName("是否外部链接")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool IsExternalURL { get; set; }
}