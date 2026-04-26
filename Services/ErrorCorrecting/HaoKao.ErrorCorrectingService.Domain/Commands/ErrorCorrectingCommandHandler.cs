using HaoKao.Common.Extensions;
using HaoKao.ErrorCorrectingService.Domain.Entities;
using HaoKao.ErrorCorrectingService.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HaoKao.ErrorCorrectingService.Domain.Commands;

public class ErrorCorrectingCommandHandler(
    IUnitOfWork<ErrorCorrecting> uow,
    IErrorCorrectingRepository repository,
    IMediatorHandler bus
) : CommandHandler(uow, bus),
    IRequestHandler<CreateErrorCorrectingCommand, bool>,
    IRequestHandler<UpdateErrorCorrectingCommand, bool>,
    IRequestHandler<DeleteErrorCorrectingCommand, bool>
{
    private readonly IErrorCorrectingRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateErrorCorrectingCommand request, CancellationToken cancellationToken)
    {
        var errorCorrecting = new ErrorCorrecting
        {
            QuestionId = request.QuestionId,
            UserId = request.UserId,
            Description = request.Description,
            QuestionTypes = request.QuestionTypes,
            SubjectId = request.SubjectId,
            SubjectName = request.SubjectName,
            QuestionTypeId = request.QuestionTypeId,
            QuestionTypeName = request.QuestionTypeName,
            QuestionText = request.QuestionText,
            NickName = request.Nickname,
            Phone = request.Phone,
            CategoryId = request.CategoryId,
            CategoryName = request.CategoryName,
            Status = (int)StatusEnum.NoHandle,
        };

        await _repository.AddAsync(errorCorrecting);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(errorCorrecting, errorCorrecting.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<ErrorCorrecting>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateErrorCorrectingCommand request, CancellationToken cancellationToken)
    {
        var errorCorrecting = await _repository.GetByIdAsync(request.Id);
        if (errorCorrecting == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应题库类别的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        errorCorrecting.Status = (int)request.Status;

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(errorCorrecting, errorCorrecting.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<ErrorCorrecting>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteErrorCorrectingCommand request, CancellationToken cancellationToken)
    {
        var errorCorrecting = await _repository.GetByIdAsync(request.Id);
        if (errorCorrecting == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应题库类别的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(errorCorrecting);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<ErrorCorrecting>(errorCorrecting.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<ErrorCorrecting>(cancellationToken);
        }

        return true;
    }
}