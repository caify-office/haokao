using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveVideo;

/// <summary>
/// 更新视频直播命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Name">名称</param>
/// <param name="CardUrl">卡片</param>
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
/// <param name="LiveAnnouncementId">直播公告Id</param>
public record UpdateLiveVideoCommand(
    Guid Id,
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
    Dictionary<string, string> LiveAddress,
    string StreamingAddress,
    Guid? LiveAnnouncementId
) : Command("更新视频直播")
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