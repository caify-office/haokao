using HaoKao.Common.Extensions;

namespace HaoKao.QuestionService.Domain.QuestionModule;

public class QuestionCommandHandler(
    IUnitOfWork<Question> uow,
    IMediatorHandler bus,
    IMapper mapper,
    IQuestionRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateQuestionCommand, bool>,
    IRequestHandler<UpdateQuestionCommand, bool>,
    IRequestHandler<UpdateFreeStateCommand, bool>,
    IRequestHandler<UpdateEnableStateCommand, bool>,
    IRequestHandler<UpdateSortCommand, bool>,
    IRequestHandler<DeleteQuestionCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IQuestionRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<bool> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = mapper.Map<Question>(request);

        question.AbilityIds = string.Join(',', request.AbilityIds);
        question.QuestionOptions = JsonSerializer.Serialize(request.QuestionOptions);

        if (question.ParentId.HasValue)
        {
            question.QuestionCount = 0;
            var parent = await _repository.GetByIdAsync(question.ParentId.Value);
            parent.QuestionCount++;
            await _repository.UpdateAsync(parent);
        }

        await _repository.AddAsync(question);

        if (await Commit())
        {
            if (question.QuestionTypeId == QuestionType.CaseAnalysis)
            {
                question.QuestionCount = 0;
                await Commit();
            }

            await UpdateEntityCache(question, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _repository.GetByIdAsync(request.Id);
        if (question == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应试题实体的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        mapper.Map(request, question);
        question.AbilityIds = string.Join(',', request.AbilityIds);
        question.QuestionOptions = JsonSerializer.Serialize(request.QuestionOptions);

        if (await Commit())
        {
            await UpdateEntityCache(question, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateFreeStateCommand request, CancellationToken cancellationToken)
    {
        await repository.ExecuteUpdateAsync(x => request.Ids.Contains(x.Id) || (x.ParentId.HasValue && request.Ids.Contains(x.ParentId.Value)), s => s.SetProperty(x => x.FreeState, request.FreeState));


        await Task.WhenAll(request.Ids.Select(x => RemoveEntityCache(x, cancellationToken)));
        await RemoveListCache(cancellationToken);

        return true;
    }

    public async Task<bool> Handle(UpdateEnableStateCommand request, CancellationToken cancellationToken)
    {
        await repository.ExecuteUpdateAsync(x => request.Ids.Contains(x.Id) || (x.ParentId.HasValue && request.Ids.Contains(x.ParentId.Value)), s => s.SetProperty(x => x.EnableState, request.EnableState));

        await Task.WhenAll(request.Ids.Select(x => RemoveEntityCache(x, cancellationToken)));
        await RemoveListCache(cancellationToken);

        return true;
    }

    public async Task<bool> Handle(UpdateSortCommand request, CancellationToken cancellationToken)
    {
        var question = await _repository.GetByIdAsync(request.Id);
        if (question == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应试题实体的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        question.Sort = request.Sort;

        if (await Commit())
        {
            await UpdateEntityCache(question, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var questions = await _repository.GetWhereAsync(x => request.Ids.Contains(x.Id) || (x.ParentId.HasValue && request.Ids.Contains(x.ParentId.Value)));
        await Task.WhenAll(questions.Select(x => _repository.DeleteAsync(x)));

        // 如果删除的是子试题，则需要更新父试题的试题数量
        if (questions.All(x => x.ParentId.HasValue))
        {
            var parent = await _repository.GetByIdAsync(questions.First().ParentId.Value);
            parent.QuestionCount -= questions.Count;
        }

        if (await Commit())
        {
            await Task.WhenAll(request.Ids.Select(x => RemoveEntityCache(x, cancellationToken)));
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    #region 缓存操作

    private Task UpdateEntityCache(Question entity, CancellationToken cancellationToken)
    {
        return _bus.UpdateIdCacheEvent(entity, entity.Id.ToString(), cancellationToken);
    }

    private Task RemoveEntityCache(Guid id, CancellationToken cancellationToken)
    {
        return _bus.RemoveIdCacheEvent<Question>(id.ToString(), cancellationToken);
    }

    private Task RemoveListCache(CancellationToken cancellationToken)
    {
        return _bus.RemoveTenantListCacheEvent<Question>(cancellationToken);
    }

    #endregion
}