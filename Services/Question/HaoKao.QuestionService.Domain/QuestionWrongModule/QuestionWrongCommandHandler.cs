using Girvs.Infrastructure;
using HaoKao.QuestionService.Domain.CacheExtensions;
using HaoKao.QuestionService.Domain.QuestionModule;

namespace HaoKao.QuestionService.Domain.QuestionWrongModule;

public class QuestionWrongCommandHandler(
    IUnitOfWork<QuestionWrong> uow,
    IMediatorHandler bus,
    IRepository<QuestionWrong> repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateQuestionWrongCommand, bool>,
    IRequestHandler<CleanQuestionWrongCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IRepository<QuestionWrong> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<bool> Handle(CreateQuestionWrongCommand request, CancellationToken cancellationToken)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();

        // 处理已存在的错题，找到已存在的错题，并且进行激活
        var wrongs = await _repository.GetWhereAsync(x => request.QuestionIds.Contains(x.QuestionId) && x.CreatorId == userId);

        // 激活错题
        foreach (var t in wrongs) t.IsActive = true;

        var questionRepository = EngineContext.Current.Resolve<IRepository<Question>>();
        var questions = await questionRepository.GetWhereAsync(x => request.QuestionIds.Contains(x.Id));
        if (questions.Any(x => x.ParentId.HasValue))
        {
            var parentIds = questions.Where(x => x.ParentId.HasValue).Select(x => x.ParentId.Value).Distinct().ToList();
            var parentQuestions = await questionRepository.GetWhereAsync(x => parentIds.Contains(x.Id));
            questions.AddRange(parentQuestions);
        }

        var list = request.QuestionIds
                          .Where(x => !wrongs.Select(i => i.QuestionId).Contains(x))
                          .Select(x => new QuestionWrong
                          {
                              QuestionId = x,
                              IsActive = true,
                              CreatorId = userId,
                              CreateTime = DateTime.Now
                          }).ToList();

        foreach (var q in list)
        {
            var question = questions.FirstOrDefault(x => x.Id == q.QuestionId);
            q.QuestionTypeId = question.QuestionTypeId;
            q.Sort = Array.IndexOf(QuestionType.All, question.QuestionTypeId);
            if (question.ParentId.HasValue)
            {
                var parentQuestion = questions.FirstOrDefault(x => x.Id == question.ParentId);
                q.ParentQuestionId = parentQuestion.Id;
                q.ParentQuestionTypeId = parentQuestion.QuestionTypeId;
            }
        }

        await _repository.AddRangeAsync(list);

        if (await Commit())
        {
            // 删除当前用户的列表缓存
            await _bus.RaiseEvent(new RemoveCacheListEvent(QuestionWrongCacheManager.ListPrefix.Create()), cancellationToken);
            return true;
        }
        return false;
    }

    public async Task<bool> Handle(CleanQuestionWrongCommand request, CancellationToken cancellationToken)
    {
        // var userId = new Guid("08db6b1b-1013-4a2f-84b7-2335204560b8");
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var list = await _repository.GetWhereAsync(x => request.QuestionIds.Contains(x.QuestionId) && x.CreatorId == userId);
        foreach (var t in list) t.IsActive = false;

        if (await Commit())
        {
            // 删除当前用户的列表缓存
            await _bus.RaiseEvent(new RemoveCacheListEvent(QuestionWrongCacheManager.ListPrefix.Create()), cancellationToken);
            return true;
        }
        return false;
    }
}