using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HaoKao.ErrorCorrectingService.Application.ViewModels.ErrorCorrecting;


[AutoMapTo(typeof(Domain.Entities.ErrorCorrecting))]
public class CreateErrorCorrectingViewModel : IDto
{

    /// <summary>
    /// 问题id
    /// </summary>
    [DisplayName("问题id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid QuestionId{ get; set; }


    /// <summary>
    /// 描述
    /// </summary>
    [DisplayName("描述")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Description{ get; set; }

    /// <summary>
    /// 问题类型
    /// </summary>
    [DisplayName("问题类型")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string QuestionTypes{ get; set; }


    [DisplayName("科目ID")]
    public Guid SubjectId { get; set; }//科目ID
    [DisplayName("科目名称")]
    public string SubjectName { get; set; }//科目名称
    [DisplayName("题型类别id")]
    public Guid QuestionTypeId { get; set; }//题型类别id
    [DisplayName("题型类别名称")]
    public string QuestionTypeName { get; set; }//题型类别名称
    [DisplayName("题干")]
    public string QuestionText { get; set; }//题干
    [DisplayName("昵称")]
    public string NickName { get; set; }//昵称
    [DisplayName("手机号码")]
    public string Phone { get; set; }//手机号码
    [DisplayName("分类id")]
    public Guid CategoryId { get; set; }//分类id
    [DisplayName("分类名称")]
    public string CategoryName { get; set; }//分类名称
}