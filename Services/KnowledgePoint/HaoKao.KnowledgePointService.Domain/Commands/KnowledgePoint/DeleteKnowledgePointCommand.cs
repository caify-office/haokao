using System;

namespace HaoKao.KnowledgePointService.Domain.Commands.KnowledgePoint;

/// <summary>
/// 删除知识点命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteKnowledgePointCommand(Guid Id) : Command("删除知识点");