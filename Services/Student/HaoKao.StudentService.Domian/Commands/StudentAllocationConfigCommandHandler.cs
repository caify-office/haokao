using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Domain.Repositories;

namespace HaoKao.StudentService.Domain.Commands;

public class StudentAllocationConfigCommandHandler(
    IUnitOfWork<StudentAllocationConfig> uow,
    IMediatorHandler bus,
    IStudentAllocationConfigRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<SaveStudentAllocationConfigCommand, bool>
{
    private readonly IStudentAllocationConfigRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<bool> Handle(SaveStudentAllocationConfigCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(x => x.TenantId == request.TenantId);
        if (entity == null)
        {
            await _repository.AddAsync(new StudentAllocationConfig
            {
                Data = request.Data,
                WaysOfAllocation = request.WaysOfAllocation,
                TenantId = request.TenantId,
            });
        }
        else
        {
            entity.Data = request.Data;
            entity.WaysOfAllocation = request.WaysOfAllocation;
            await _repository.UpdateAsync(entity);
        }

        return await Commit();
    }
}