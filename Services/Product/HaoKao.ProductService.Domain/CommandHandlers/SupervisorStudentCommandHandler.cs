using HaoKao.ProductService.Domain.Commands.SupervisorStudent;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Repositories;

namespace HaoKao.ProductService.Domain.CommandHandlers;

public class SupervisorStudentCommandHandler(
    IUnitOfWork<SupervisorStudent> uow,
    ISupervisorStudentRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateSupervisorStudentCommand, bool>,
    IRequestHandler<UpdateSupervisorStudentCommand, bool>,
    IRequestHandler<DeleteSupervisorStudentCommand, bool>,
    IRequestHandler<UpdateStudentStatisticsDataCommand, bool>
{
    private readonly ISupervisorStudentRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateSupervisorStudentCommand request, CancellationToken cancellationToken)
    {
        var student = _mapper.Map<SupervisorStudent>(request);

        await _repository.AddAsync(student);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(student, student.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<SupervisorStudent>(cancellationToken);
            await _bus.RemoveListCacheEvent<SupervisorClass>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateSupervisorStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _repository.GetByIdAsync(request.Id);
        if (student == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应督学学员的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        student = _mapper.Map(request, student);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(student, student.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<SupervisorStudent>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateStudentStatisticsDataCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.Now;

        var supervisorStudent = await _repository.GetWhereAsync(x => x.SupervisorClassId == request.SupervisorClassId
                                                                  && (x.StatisticsTime == null || x.StatisticsTime.Value.Date < now.Date));
        if (supervisorStudent.Count == 0)
        {
            return false;
        }

        var students = await _repository.GetStatisticsData(supervisorStudent, request.ProductId);

        students.ForEach(item => { item.StatisticsTime = now; });

        await _repository.UpdateRangeAsync(students);

        if (await Commit())
        {
            await _bus.RemoveListCacheEvent<SupervisorStudent>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteSupervisorStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _repository.GetByIdAsync(request.Id);
        if (student == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应督学学员的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(student);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<SupervisorStudent>(student.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<SupervisorStudent>(cancellationToken);
            await _bus.RemoveListCacheEvent<SupervisorClass>(cancellationToken);
        }

        return true;
    }
}