using Girvs.BusinessBasis.Dto;
using HaoKao.LiveBroadcastService.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveVideo;

/// <summary>
/// 创建视频直播命令
/// </summary>
/// <param name="Name">名称</param>
/// <param name="CardUrl">卡片Url</param>
/// <param name="LiveType">直播类型</param>
/// <param name="SubjectIds">科目Id</param>
/// <param name="SubjectNames">科目名称</param>
/// <param name="LecturerId">主讲老师Id</param>
/// <param name="LecturerName">主讲老师名称</param>
/// <param name="StartTime">直播开始时间</param>
/// <param name="EndTime">直播结束时间</param>
/// <param name="LiveStatus">直播状态</param>
/// <param name="TargetProductId">购买产品挑战对象</param>
/// <param name="Desc">详情介绍</param>
/// <param name="LiveAddress">播流地址</param>
/// <param name="StreamingAddress">推流地址</param>
/// <param name="LiveAnnouncementId">直播公告ID</param>
public record CreateLiveVideoCommand(
    string Name,
    string CardUrl,
    LiveType LiveType,
    List<Guid> SubjectIds,
    List<string> SubjectNames,
    List<Guid> LecturerId,
    List<string> LecturerName,
    DateTime StartTime,
    DateTime EndTime,
    LiveStatus LiveStatus,
    Guid? TargetProductId,
    string Desc,
    Dictionary<string,string> LiveAddress,
    string StreamingAddress,
    Guid? LiveAnnouncementId
) : Command("创建视频直播")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("名称不能为空")
                 .MaximumLength(50).WithMessage("名称长度不能大于50")
                 .MinimumLength(2).WithMessage("名称长度不能小于2");

        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("卡片不能为空")
                 .MaximumLength(300).WithMessage("名称长度不能大于300")
                 .MinimumLength(2).WithMessage("名称长度不能小于2");
    }
}

public class CreateLiveVideoUrlViewModel : IDto
{
    /// <summary>
    /// app名称
    /// </summary>
    [DisplayName("app名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string appName { get; set; }

    /// <summary>
    /// 流名称
    /// </summary>

    [DisplayName("流名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string streamName { get; set; }

    /// <summary>
    /// 过期时间（单位是秒)
    /// </summary>
    [DisplayName("过期时间（单位是秒)")]
    [Required(ErrorMessage = "{0}不能为空")]
    public long expireTime { get; set; }

    /// <summary>
    /// 直播Id
    /// </summary>
    [DisplayName("直播Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid LiveVideoId { get; set; }
}