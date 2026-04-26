using FluentValidation;
using System;

namespace HaoKao.AnsweringQuestionService.Domain.Commands.AnsweringQuestion;
/// <summary>
/// 更新答疑命令
/// </summary>
/// <param name="Id">主键</param>

public record UpdateAnsweringQuestionCommand(
   Guid Id
  

) : Command("更新答疑")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {

        


    }
}