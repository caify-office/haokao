using HaoKao.CorrectionNotebookService.Domain.Entities;
using HaoKao.CorrectionNotebookService.Domain.Enums;
using HaoKao.CorrectionNotebookService.Domain.Repositories;

namespace HaoKao.CorrectionNotebookService.Domain.Commands;

public record CreateQuestionCommand(Guid SubjectId, Guid UserId, IReadOnlyCollection<Uri> ImageUrls, IReadOnlyList<Guid> TagIds) : Command<IReadOnlyList<Question>>("创建题目命令");

public record SaveAnswerAndAnalysisCommand(Question Question) : Command("保存答案和解析命令");

public record EditQuestionMasteryDegreeCommand(IReadOnlyList<Guid> Ids, MasteryDegree MasteryDegree) : Command("修改掌握程度命令");

public record EditQuestionTagCommand(Guid Id, IReadOnlyList<Guid> TagIds) : Command("修改题目标签命令");

public record DeleteQuestionCommand(Guid SubjectId, IReadOnlyList<Guid> Ids) : Command("删除题目命令");

public class QuestionCommandHandler(
    IUnitOfWork<Question> uow,
    IMediatorHandler bus,
    IExamLevelRepository examLevelRepository,
    ISubjectRepository subjectRepository,
    IQuestionRepository questionRepository,
    ITagRepository tagRepository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateQuestionCommand, IReadOnlyList<Question>>,
    IRequestHandler<SaveAnswerAndAnalysisCommand, bool>,
    IRequestHandler<EditQuestionMasteryDegreeCommand, bool>,
    IRequestHandler<EditQuestionTagCommand, bool>,
    IRequestHandler<DeleteQuestionCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IExamLevelRepository _examLevelRepository = examLevelRepository ?? throw new ArgumentNullException(nameof(examLevelRepository));
    private readonly ISubjectRepository _subjectRepository = subjectRepository ?? throw new ArgumentNullException(nameof(subjectRepository));
    private readonly IQuestionRepository _questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
    private readonly ITagRepository _tagRepository = tagRepository ?? throw new ArgumentNullException(nameof(tagRepository));

    public async Task<IReadOnlyList<Question>> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var subject = await _subjectRepository.GetByIdAsync(request.SubjectId);
        if (subject == null)
        {
            await _bus.RaiseBadRequestEvent(nameof(CreateQuestionCommand), "科目不存在", cancellationToken);
            return [];
        }

        var tags = await _tagRepository.GetByIdsAsync(request.TagIds);
        if (tags.Where(x => x.IsBuiltIn).GroupBy(x => x.Category).Any(x => x.Count() > 1))
        {
            await _bus.RaiseBadRequestEvent(nameof(CreateQuestionCommand), "同一分类下的内置标签只能选择一个", cancellationToken);
            return [];
        }

        foreach (var imageUrl in request.ImageUrls)
        {
            subject.AddQuestion(imageUrl, request.UserId, tags);
        }

        await _questionRepository.AddRangeAsync(subject.Questions);
        await _subjectRepository.UpdateAsync(subject);

        await Commit();
        return subject.Questions;
    }

    public async Task<bool> Handle(SaveAnswerAndAnalysisCommand request, CancellationToken cancellationToken)
    {
        await _questionRepository.UpdateAsync(request.Question);
        return await Commit();
    }

    public async Task<bool> Handle(EditQuestionMasteryDegreeCommand request, CancellationToken cancellationToken)
    {
        await _questionRepository.UpdateMasteryDegreeAsync(request.Ids, request.MasteryDegree);
        return true;
    }

    public async Task<bool> Handle(EditQuestionTagCommand request, CancellationToken cancellationToken)
    {
        var entity = await _questionRepository.GetWithTagsAsync(request.Id);
        if (entity == null)
        {
            await _bus.RaiseBadRequestEvent(nameof(DeleteQuestionCommand), "题目不存在", cancellationToken);
            return false;
        }

        var tags = await _tagRepository.GetByIdsAsync(request.TagIds);
        if (tags.Where(x => x.IsBuiltIn).GroupBy(x => x.Category).Any(x => x.Count() > 1))
        {
            await _bus.RaiseBadRequestEvent(nameof(EditQuestionTagCommand), "同一分类下的内置标签只能选择一个", cancellationToken);
            return false;
        }

        _questionRepository.DeleteQuestionTags(entity.Tags);
        _questionRepository.AddQuestionTags(tags.Select(x => new QuestionTag
        {
            TagId = x.Id,
            QuestionId = entity.Id
        }).ToList());

        return await Commit();
    }

    public async Task<bool> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var subject = await _subjectRepository.GetByIdAsync(request.SubjectId);
        if (subject == null)
        {
            await _bus.RaiseBadRequestEvent(nameof(DeleteQuestionCommand), "科目不存在", cancellationToken);
            return false;
        }

        await _questionRepository.DeleteAsync(request.Ids);

        return await Commit();
    }
}