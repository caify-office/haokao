using System;

namespace HaoKao.AnsweringQuestionService.Domain.Commands.AnsweringQuestion;
/// <summary>
/// 删除答疑命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteAnsweringQuestionCommand(
    Guid Id
) : Command("删除答疑");