using FluentValidation;
using System;

namespace HaoKao.AnsweringQuestionService.Domain.Commands.AnsweringQuestionReply;
/// <summary>
/// 更新答疑回复命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="ReplyContent">答疑回复内容</param>
/// <param name="AnsweringQuestionId">关联的题目id</param>
public record UpdateAnsweringQuestionReplyCommand(
   Guid Id,
   string ReplyContent,
   Guid AnsweringQuestionId

) : Command("更新答疑回复")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {

        validator.RuleFor(x => ReplyContent)
            .NotEmpty().WithMessage("答疑回复内容不能为空");




    }
}