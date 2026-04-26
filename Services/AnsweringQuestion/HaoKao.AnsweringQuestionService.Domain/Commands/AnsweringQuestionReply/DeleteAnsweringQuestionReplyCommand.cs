using System;

namespace HaoKao.AnsweringQuestionService.Domain.Commands.AnsweringQuestionReply;
/// <summary>
/// 删除答疑回复命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteAnsweringQuestionReplyCommand(
    Guid Id
) : Command("删除答疑回复");