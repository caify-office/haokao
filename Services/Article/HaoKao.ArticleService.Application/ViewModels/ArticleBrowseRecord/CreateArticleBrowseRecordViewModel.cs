namespace HaoKao.ArticleService.Application.ViewModels.ArticleBrowseRecord;


[AutoMapTo(typeof(Domain.Entities.ArticleBrowseRecord))]
public class CreateArticleBrowseRecordViewModel : IDto
{
    /// <summary>
    /// 客户端唯一识别号
    /// </summary>
    [DisplayName(" 客户端唯一识别号")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid ClientUniqueId { get; set; }

    /// <summary>
    /// 文章Id
    /// </summary>
    [DisplayName("文章Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid ArticleId{ get; set; }
}