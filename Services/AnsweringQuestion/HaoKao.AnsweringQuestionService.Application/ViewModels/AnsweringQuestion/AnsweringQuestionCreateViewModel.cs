using HaoKao.AnsweringQuestionService.Domain.Enumerations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HaoKao.AnsweringQuestionService.Application.ViewModels.AnsweringQuestion;


[AutoMapTo(typeof(Domain.Entities.AnsweringQuestion))]
public class CreateAnsweringQuestionViewModel : IDto
{

    /// <summary>
    /// 父级id
    /// </summary>
    [DisplayName("父级id")]
    public Guid? ParentId { get; set; }



    /// <summary>
    /// 科目id
    /// </summary>
    [DisplayName("科目id")]
    //[Required(ErrorMessage = "{0}不能为空")]
    public Guid SubjectId{ get; set; }

    /// <summary>
    /// 科目名称
    /// </summary>
    [DisplayName("科目名称")]
    //[Required(ErrorMessage = "{0}不能为空")]
    public string SubjectName{ get; set; }


    /// <summary>
    /// 课程id
    /// </summary>
    [DisplayName("课程id")]
    public Guid CourseId{ get; set; }

    /// <summary>
    /// 课程name
    /// </summary>
    [DisplayName("课程name")]
    public string CourseName { get; set; }
    /// <summary>
    /// 课程章节名称
    /// </summary>
    [DisplayName("课程章节名称")]
    public string CourseChapterName { get; set; }
    /// <summary>
    /// 课程视频名称
    /// </summary>
    [DisplayName("课程视频名称")]
    public string CourseVideoName { get; set; }

    /// <summary>
    /// 选择的课程章节id
    /// </summary>
    [DisplayName("选择的课程章节id")]
    public Guid CourseChapterId{ get; set; }

    /// <summary>
    /// 选择的课程视频id
    /// </summary>
    [DisplayName("选择的课程视频id")]
    public Guid CourseVideId{ get; set; }

    /// <summary>
    /// 书籍页码
    /// </summary>
    [DisplayName("书籍页码")]
    public string BookPageSize{ get; set; }

    /// <summary>
    /// 书籍名称以及相关描述
    /// </summary>
    [DisplayName("书籍名称以及相关描述")]
    public string BookName{ get; set; }

    /// <summary>
    /// 提问类型
    /// </summary>
    [DisplayName("提问类型")]
    [Required(ErrorMessage = "{0}不能为空")]
    public AnsweringQuestionEnum Type{ get; set; }

    /// <summary>
    /// 问题描述
    /// </summary>
    [DisplayName("问题描述")]
    public string Description{ get; set; }

    /// <summary>
    /// 详细描述
    /// </summary>
    [DisplayName("详细描述")]
    public string Remark{ get; set; }

    /// <summary>
    /// 上传的图片路劲
    /// </summary>
    [DisplayName("上传的图片路劲")]
    public string FileUrl{ get; set; }

    [DisplayName("产品id")]
    public Guid ProductId { get; set; }


}