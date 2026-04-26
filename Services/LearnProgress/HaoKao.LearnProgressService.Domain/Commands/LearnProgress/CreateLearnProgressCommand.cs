using FluentValidation;
using System;

namespace HaoKao.LearnProgressService.Domain.Commands.LearnProgress;
/// <summary>
/// 创建命令
/// </summary>
/// <param name="ProductId">产品Id</param>
/// <param name="SubjectId">对应的科目Id</param>
/// <param name="ChapterId">章节id</param>
/// <param name="CourseId">课程id</param>
/// <param name="VideoId">视频id</param>
/// <param name="Identifier">当前视频标识符,</param>
/// <param name="Progress">学习时长(单位:s)</param>
/// <param name="TotalProgress">视频总长度(单位:s)--冗余,用于进度百分比计算</param>
/// <param name="MaxProgress">观看视频最大长度(单位:s)</param>
public record CreateLearnProgressCommand(
    Guid ProductId,
    Guid SubjectId,
    Guid ChapterId,
    Guid CourseId,
    Guid VideoId,
    string Identifier,
    float Progress,
    float TotalProgress,
    float MaxProgress
) : Command("创建")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Identifier)
            .NotEmpty().WithMessage("当前视频标识符,不能为空");

    }
}