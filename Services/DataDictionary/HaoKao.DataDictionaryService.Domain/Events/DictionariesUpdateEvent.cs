namespace HaoKao.DataDictionaryService.Domain.Events;

public record DictionariesUpdateEvent(Guid Id, string Name) : Event;