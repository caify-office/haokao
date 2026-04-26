namespace HaoKao.ProductService.Application.ViewModels.SupervisorStudent;


[AutoMapTo(typeof(Domain.Commands.SupervisorStudent.UpdateSupervisorStudentCommand))]
public class UpdateSupervisorStudentViewModel : IDto
{
    public Guid Id { get; set; }
    /// <summary>
    /// 督学班级id
    /// </summary>
    [DisplayName("督学班级id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid SupervisorClassId { get; set; }

    /// <summary>
    /// 注册用户id
    /// </summary>
    [DisplayName("注册用户id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid RegisterUserId { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [DisplayName("昵称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [DisplayName("手机号")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Phone { get; set; }

}