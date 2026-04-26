using HaoKao.CorrectionNotebookService.Domain.Entities;
using HaoKao.CorrectionNotebookService.Domain.Queries;
using HaoKao.CorrectionNotebookService.Domain.Repositories;

namespace HaoKao.CorrectionNotebookService.Domain.Commands;

public record CreateExamLevelCommand(string Name, Guid ParentId, Guid UserId) : Command("创建考试级别命令")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name).NotEmpty().WithMessage("考试级别名称不能为空");
        validator.RuleFor(x => Name).MaximumLength(20).WithMessage("考试级别名称不能超过20个字符");
    }
}

public record EditExamLevelNameCommand(Guid Id, string Name) : Command("编辑考试级别名称命令")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name).NotEmpty().WithMessage("考试级别名称不能为空");
        validator.RuleFor(x => Name).MaximumLength(20).WithMessage("考试级别名称不能超过20个字符");
    }
}

public record DeleteExamLevelCommand(Guid Id, Guid UserId) : Command("删除考试级别命令");

public class ExamLevelCommandHandler(
    IUnitOfWork<ExamLevel> uow,
    IMediatorHandler bus,
    IExamLevelRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateExamLevelCommand, bool>,
    IRequestHandler<EditExamLevelNameCommand, bool>,
    IRequestHandler<DeleteExamLevelCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IExamLevelRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<bool> Handle(CreateExamLevelCommand request, CancellationToken cancellationToken)
    {
        var list = await _repository.GetListByUserAsync(new ExamLevelQuery(request.UserId));
        if (request.ParentId == Guid.Empty)
        {
            if (list.Count(x => x.ParentId == Guid.Empty) >= 10)
            {
                await _bus.RaiseBadRequestEvent(nameof(CreateExamLevelCommand), "考试级别数量不能超过10个", cancellationToken);
                return false;
            }
        }
        else
        {
            var parent = list.FirstOrDefault(x => x.Id == request.ParentId);
            if (parent == null)
            {
                await _bus.RaiseBadRequestEvent(nameof(CreateExamLevelCommand), "父级考试级别不存在", cancellationToken);
                return false;
            }

            if (list.Count(x => x.ParentId == parent.Id) >= 15)
            {
                await _bus.RaiseBadRequestEvent(nameof(CreateExamLevelCommand), "子考试级别数量不能超过15个", cancellationToken);
                return false;
            }

            if (parent.ParentId != Guid.Empty)
            {
                await _bus.RaiseBadRequestEvent(nameof(CreateExamLevelCommand), "考试级别只能有一级嵌套", cancellationToken);
                return false;
            }
        }

        var entity = ExamLevel.Create(request.Name, request.ParentId, request.UserId);

        await _repository.AddAsync(entity);

        return await Commit();
    }

    public async Task<bool> Handle(EditExamLevelNameCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            await _bus.RaiseBadRequestEvent(nameof(EditExamLevelNameCommand), "考试级别不存在", cancellationToken);
            return false;
        }

        if (entity.IsBuiltIn)
        {
            await _bus.RaiseBadRequestEvent(nameof(EditExamLevelNameCommand), "内置考试级别不能修改", cancellationToken);
            return false;
        }

        entity.Name = request.Name;

        await _repository.UpdateAsync(entity);

        return await Commit();
    }

    public async Task<bool> Handle(DeleteExamLevelCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetWithSubjectByUserAsync(request.Id, request.UserId);
        if (entity == null)
        {
            await _bus.RaiseBadRequestEvent(nameof(DeleteExamLevelCommand), "考试级别不存在", cancellationToken);
            return false;
        }

        if (entity.IsBuiltIn)
        {
            await _bus.RaiseBadRequestEvent(nameof(DeleteExamLevelCommand), "内置考试级别不能删除", cancellationToken);
            return false;
        }

        if (entity.ParentId != Guid.Empty && entity.Subjects.Any(x => x.QuestionCount > 0))
        {
            await _bus.RaiseBadRequestEvent(nameof(DeleteExamLevelCommand), "考试级别下存在题目，不能删除", cancellationToken);
            return false;
        }

        if (entity.ParentId == Guid.Empty && await _repository.ExistEntityAsync(x => x.ParentId == entity.Id))
        {
            await _bus.RaiseBadRequestEvent(nameof(DeleteExamLevelCommand), "存在子考试级别，不能删除", cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(entity);

        return await Commit();
    }
}