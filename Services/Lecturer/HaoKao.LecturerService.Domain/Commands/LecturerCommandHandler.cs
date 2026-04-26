using AutoMapper;
using HaoKao.Common.Extensions;
using HaoKao.LecturerService.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace HaoKao.LecturerService.Domain.Commands;

/// <summary>
/// 讲师命令处理器
/// </summary>
public class LecturerCommandHandler(
    IUnitOfWork<Lecturer> uow,
    ILecturerRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateLecturerCommand, bool>,
    IRequestHandler<UpdateLecturerCommand, bool>,
    IRequestHandler<DeleteLecturerCommand, bool>
{
    private readonly ILecturerRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateLecturerCommand request, CancellationToken cancellationToken)
    {
        var lecturer = _mapper.Map<Lecturer>(request);

        await _repository.AddAsync(lecturer);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(lecturer, lecturer.Id.ToString(), cancellationToken);
            await _bus.RemoveTenantListCacheEvent<Lecturer>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateLecturerCommand request, CancellationToken cancellationToken)
    {
        var lecturer = await _repository.GetByIdAsync(request.Id);
        if (lecturer == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应讲师的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }
        lecturer.SubjectIds?.Clear();
        lecturer.SubjectNames?.Clear();
        lecturer.ProductPackageIds?.Clear();
        lecturer = _mapper.Map(request, lecturer);
        await _repository.UpdateAsync(lecturer);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(lecturer, lecturer.Id.ToString(), cancellationToken);
            await _bus.RemoveTenantListCacheEvent<Lecturer>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteLecturerCommand request, CancellationToken cancellationToken)
    {
        var lecturer = await _repository.GetByIdAsync(request.Id);
        if (lecturer == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应讲师的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(lecturer);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<Lecturer>(lecturer.Id.ToString(), cancellationToken);
            await _bus.RemoveTenantListCacheEvent<Lecturer>(cancellationToken);
        }

        return true;
    }
}