using HaoKao.CorrectionNotebookService.Domain.Entities;
using HaoKao.CorrectionNotebookService.Domain.Repositories;

namespace HaoKao.CorrectionNotebookService.Domain.Commands;

public record CreateSubjectCommand(string Name, Guid ExamLevelId, Guid UserId) : Command("创建科目命令")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name).NotEmpty().WithMessage("科目名称不能为空");
        validator.RuleFor(x => Name).MaximumLength(20).WithMessage("科目名称不能超过20个字符");
    }
}

public record ResortSubjectCommand(Guid ExamLevelId, Guid UserId, IReadOnlyList<SubjectSort> SubjectSorts) : Command("重新排序科目命令");

public record DeleteSubjectCommand(Guid Id) : Command<Guid>("删除科目命令");

public class SubjectCommandHandler(
    IUnitOfWork<Subject> uow,
    IMediatorHandler bus,
    IExamLevelRepository examLevelRepository,
    ISubjectRepository subjectRepository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateSubjectCommand, bool>,
    IRequestHandler<ResortSubjectCommand, bool>,
    IRequestHandler<DeleteSubjectCommand, Guid>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IExamLevelRepository _examLevelRepository = examLevelRepository ?? throw new ArgumentNullException(nameof(examLevelRepository));
    private readonly ISubjectRepository _subjectRepository = subjectRepository ?? throw new ArgumentNullException(nameof(subjectRepository));

    public async Task<bool> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
    {
        var examLevel = await _examLevelRepository.GetWithSubjectByUserAsync(request.ExamLevelId, request.UserId);
        if (examLevel == null)
        {
            await _bus.RaiseBadRequestEvent(nameof(CreateSubjectCommand), "考试级别不存在", cancellationToken);
            return false;
        }

        if (examLevel.Subjects.Count >= 20)
        {
            await _bus.RaiseBadRequestEvent(nameof(CreateSubjectCommand), "科目数量不能超过20个", cancellationToken);
            return false;
        }

        var sort = examLevel.Subjects.Count > 0 ? examLevel.Subjects.Max(x => x.Sort.Priority) : 0;
        examLevel.AddSubject(request.Name, request.UserId, sort + 1);

        await _examLevelRepository.UpdateAsync(examLevel);

        return await Commit();
    }

    public async Task<bool> Handle(ResortSubjectCommand request, CancellationToken cancellationToken)
    {
        var examLevel = await _examLevelRepository.GetWithSubjectByUserAsync(request.ExamLevelId, request.UserId);
        if (examLevel == null)
        {
            await _bus.RaiseBadRequestEvent(nameof(CreateSubjectCommand), "考试级别不存在", cancellationToken);
            return false;
        }

        foreach (var sort in request.SubjectSorts)
        {
            var subject = examLevel.Subjects.FirstOrDefault(x => x.Id == sort.SubjectId);
            if (subject == null)
            {
                await _bus.RaiseBadRequestEvent(nameof(ResortSubjectCommand), "科目不存在", cancellationToken);
                return false;
            }

            var subjectSort = subject.Sorts.FirstOrDefault(x => x.CreatorId == request.UserId);
            if (subjectSort == null)
            {
                _subjectRepository.AddSort(sort);
            }
            else
            {
                subjectSort.Priority = sort.Priority;
                _subjectRepository.UpdateSort(subjectSort);
            }
        }

        // 出于性能考虑 不从考试级别更新科目排序
        // await _examLevelRepository.UpdateAsync(examLevel);

        return await Commit();
    }

    public async Task<Guid> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
    {
        var entity = await _subjectRepository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            await _bus.RaiseBadRequestEvent(nameof(DeleteSubjectCommand), "科目不存在", cancellationToken);
            return Guid.Empty;
        }

        if (entity.IsBuiltIn)
        {
            await _bus.RaiseBadRequestEvent(nameof(DeleteSubjectCommand), "内置科目不能删除", cancellationToken);
            return Guid.Empty;
        }

        if (entity.QuestionCount > 0)
        {
            await _bus.RaiseBadRequestEvent(nameof(DeleteSubjectCommand), "科目下存在试题，不能删除", cancellationToken);
            return Guid.Empty;
        }

        await _subjectRepository.DeleteAsync(entity);

        await Commit();

        return entity.ExamLevelId;
    }
}