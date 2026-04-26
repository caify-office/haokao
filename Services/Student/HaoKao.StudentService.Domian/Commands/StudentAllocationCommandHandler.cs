using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Domain.Repositories;

namespace HaoKao.StudentService.Domain.Commands;

public class StudentAllocationCommandHandler(
    IUnitOfWork<StudentAllocation> uow,
    IMediatorHandler bus,
    IStudentAllocationRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<UpdateAllocateToCommand, bool>,
    IRequestHandler<UpdateRemarkCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IStudentAllocationRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<bool> Handle(UpdateAllocateToCommand request, CancellationToken cancellationToken)
    {
        var list = await _repository.GetWhereAsync(x => request.Ids.Contains(x.Id));
        if (list.Count == 0)
        {
            await _bus.RaiseEvent(new DomainNotification(nameof(UpdateAllocateToCommand), "学员不存在"), cancellationToken);
            return false;
        }

        await Task.WhenAll(list.Select(x =>
        {
            x.SalespersonId = request.SalespersonId;
            x.SalespersonName = request.SalespersonName;
            x.AllocationTime = DateTime.Now;
            return _repository.UpdateAsync(x);
        }));

        if (await Commit())
        {
            await _bus.RemoveListCache<StudentAllocation>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateRemarkCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            await _bus.RaiseEvent(new DomainNotification(nameof(UpdateRemarkCommand), "学员不存在"), cancellationToken);
            return false;
        }

        entity.Remark = request.Remark;
        await _repository.UpdateAsync(entity);

        if (await Commit())
        {
            await _bus.RemoveListCache<StudentAllocation>(cancellationToken);
        }

        return true;
    }
}