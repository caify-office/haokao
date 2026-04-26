using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Domain.Repositories;

namespace HaoKao.StudentService.Domain.Commands;

public class StudentCommandHandler(
    IUnitOfWork<Student> uow,
    IMediatorHandler bus,
    IStudentRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateStudentCommand, bool>,
    IRequestHandler<UpdateStudentCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IStudentRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<bool> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.ExistEntityAsync(x => x.RegisterUserId == request.RegisterUserId))
        {
            //await _bus.RaiseEvent(new DomainNotification(request.RegisterUserId.ToString(), "该用户已经是学员"), cancellationToken);
            return false;
        }

        var entity = new Student { RegisterUserId = request.RegisterUserId };

        await _repository.AddAsync(entity);

        if (await Commit())
        {
            await _bus.UpdateEntityCache(entity, cancellationToken);
            await _bus.RemoveListCache<Student>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(x => x.RegisterUserId == request.RegisterUserId);
        if (entity == null)
        {
            entity = new Student { RegisterUserId = request.RegisterUserId, IsPaidStudent = request.IsPaidStudent };
            await _repository.AddAsync(entity);
        }
        else
        {
            entity.IsPaidStudent = request.IsPaidStudent;
            await _repository.UpdateAsync(entity);
        }

        if (await Commit())
        {
            await _bus.UpdateEntityCache(entity, cancellationToken);
            await _bus.RemoveListCache<Student>(cancellationToken);
        }

        return true;
    }
}