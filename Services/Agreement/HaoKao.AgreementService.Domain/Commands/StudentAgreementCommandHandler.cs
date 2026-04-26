using HaoKao.AgreementService.Domain.Entities;
using HaoKao.AgreementService.Domain.Repositories;
using HaoKao.Common.Extensions;

namespace HaoKao.AgreementService.Domain.Commands;

public class StudentAgreementCommandHandler(
    IUnitOfWork<StudentAgreement> uow,
    IStudentAgreementRepository repository,
    IMediatorHandler bus
) : CommandHandler(uow, bus),
    IRequestHandler<CreateStudentAgreementCommand, bool>,
    IRequestHandler<UpdateStudentAgreementCommand, bool>
{
    private readonly IStudentAgreementRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateStudentAgreementCommand request, CancellationToken cancellationToken)
    {
        // 判断是否已经签署
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        if (await _repository.ExistEntityAsync(x => x.ProductId == request.ProductId && x.AgreementId == request.AgreementId && x.CreatorId == userId))
        {
            var notification = new DomainNotification(request.ToString(), "用户已签署过学员协议", StatusCodes.Status400BadRequest);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        var entity = new StudentAgreement
        {
            ProductId = request.ProductId,
            ProductName = request.ProductName,
            AgreementId = request.AgreementId,
            AgreementName = request.AgreementName,
            StudentName = request.StudentName,
            IdCard = request.IdCard,
            Contact = request.Contact,
            Address = request.Address,
            Email = request.Email
        };

        await _repository.AddAsync(entity);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(entity, entity.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<StudentAgreement>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateStudentAgreementCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应学员协议的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        entity.StudentName = request.StudentName;
        entity.IdCard = request.IdCard;
        entity.Contact = request.Contact;
        entity.Address = request.Address;
        entity.Email = request.Email;

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(entity, entity.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<StudentAgreement>(cancellationToken);
        }

        return true;
    }
}