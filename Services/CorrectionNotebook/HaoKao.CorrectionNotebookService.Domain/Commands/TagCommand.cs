
using HaoKao.CorrectionNotebookService.Domain.Entities;
using HaoKao.CorrectionNotebookService.Domain.Repositories;

namespace HaoKao.CorrectionNotebookService.Domain.Commands;

public record CreateTagCommand(string Name, Guid UserId) : Command("创建标签命令")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name).NotEmpty().WithMessage("标签名称不能为空");
        validator.RuleFor(x => Name).MaximumLength(10).WithMessage("标签名称不能超过10个字符");
    }
}

public record DeleteTagCommand(Guid Id) : Command("删除标签命令");


public class TagCommandHandler(
    IUnitOfWork<Tag> uow,
    IMediatorHandler bus,
    ITagRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateTagCommand, bool>,
    IRequestHandler<DeleteTagCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly ITagRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<bool> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var tag = Tag.Create(request.Name, request.UserId);
        await _repository.AddAsync(tag);
        return await Commit();
    }

    public async Task<bool> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _repository.GetByIdAsync(request.Id);
        if (tag == null)
        {
            await _bus.RaiseBadRequestEvent(nameof(DeleteTagCommand), "标签不存在", cancellationToken);
            return false;
        }

        if (tag.IsBuiltIn)
        {
            await _bus.RaiseBadRequestEvent(nameof(DeleteTagCommand), "内置标签不能删除", cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(tag);

        return await Commit();
    }
}