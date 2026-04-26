using HaoKao.DataDictionaryService.Domain.Entities;

namespace HaoKao.DataDictionaryService.Domain.EventHandlers;

public class DictionariesEventHandler(IDictionariesRepository dictionariesRepository, IMediatorHandler bus, IUnitOfWork<Dictionaries> unitOfWork) : CommandHandler(unitOfWork, bus), INotificationHandler<DictionariesUpdateEvent>
{
    private readonly IDictionariesRepository _dictionariesRepository = dictionariesRepository ?? throw new ArgumentNullException(nameof(dictionariesRepository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public Task Handle(DictionariesUpdateEvent model, CancellationToken cancellationToken)
    {
        Expression<Func<Dictionaries, bool>> predicate = dictionaries => dictionaries.Pid == model.Id;
        List<Dictionaries> informationItems = _dictionariesRepository.GetWhereAsync(predicate).Result;
        informationItems.ForEach(delegate (Dictionaries dictionaries)
        {
            dictionaries.PName = model.Name;
        });
        _dictionariesRepository.UpdateRangeAsync(informationItems);
        if (Commit().Result)
        {

        }
        return Task.CompletedTask;
    }
}