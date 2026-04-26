using HaoKao.OpenPlatformService.Domain.Commands.UserDailyActivityLog;

namespace HaoKao.OpenPlatformService.Domain.CommandHandlers;

public class DailyActiveUserLogCommandHandler(
    IUnitOfWork<DailyActiveUserLog> uow,
    IDailyActiveUserLogRepository repository,
    IMediatorHandler bus
) : CommandHandler(uow, bus),
    IRequestHandler<CreateDailyActiveUserLogCommand, bool>
{
    private readonly IDailyActiveUserLogRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateDailyActiveUserLogCommand request, CancellationToken cancellationToken)
    {
        var date = DateTime.Now.ToString("yyyy-MM-dd");
        var isExists = await _repository.ExistEntityAsync(x => x.UserId == request.UserId &&
                                                               x.ClientId == request.ClientId &&
                                                               x.CreateDate == date);
        if (!isExists && !string.IsNullOrEmpty(request.ClientId))
        {
            var entity = new DailyActiveUserLog
            {
                UserId = request.UserId,
                ClientId = request.ClientId,
                CreateDate = date,
                CreateTime = DateTime.Now,
            };
            await _repository.AddAsync(entity);
            if (await Commit())
            {
                return true;
            }
        }

        return true;
    }
}