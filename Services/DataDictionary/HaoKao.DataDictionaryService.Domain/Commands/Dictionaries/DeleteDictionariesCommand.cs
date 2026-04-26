namespace HaoKao.DataDictionaryService.Domain.Commands.Dictionaries;

/// <summary>
/// 删除数据字典值
/// </summary>
/// <param name="Ids">数据字典Id集合</param>
public record DeleteDictionariesCommand(Guid[] Ids) : Command("删除数据字典值");