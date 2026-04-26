namespace HaoKao.DataDictionaryService.Application.ViewModels.Dictionaries;

[AutoMapFrom(typeof(SimpleDictionariesQuery))]
[AutoMapTo(typeof(SimpleDictionariesQuery))]
public class SimpleDictionariesQueryViewModel : QueryDtoBase<DictionariesQueryListViewModel>;