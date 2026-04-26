using HaoKao.QuestionService.Domain.CacheExtensions;

namespace HaoKao.QuestionService.Domain.QuestionWrongPaperMoudle;

public record CreateQuestionWrongPaperCommand(Guid SubjectId, string PaperName, Uri DownloadUrl, int QuestionCount) : Command("创建错题组卷命令");

public class QuestionWrongPaperCommandHandler(
    IUnitOfWork<QuestionWrongPaper> uow,
    IMediatorHandler bus,
    IQuestionWrongPaperRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateQuestionWrongPaperCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IQuestionWrongPaperRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<bool> Handle(CreateQuestionWrongPaperCommand request, CancellationToken cancellationToken)
    {
        var paper = new QuestionWrongPaper
        {
            SubjectId = request.SubjectId,
            PaperName = request.PaperName,
            DownloadUrl = request.DownloadUrl,
            QuestionCount = request.QuestionCount,
        };

        await _repository.AddAsync(paper);

        if (await Commit())
        {
            await _bus.RaiseEvent(new RemoveCacheListEvent(new GenericCacheManager(typeof(QuestionWrongPaper)).ListPrefix.Create()), cancellationToken);
        }

        return true;
    }
}