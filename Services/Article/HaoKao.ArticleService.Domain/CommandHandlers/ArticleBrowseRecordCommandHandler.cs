using HaoKao.ArticleService.Domain.Commands.ArticleBrowseRecord;
using HaoKao.ArticleService.Domain.Entities;
using HaoKao.Common.Extensions;

namespace HaoKao.ArticleService.Domain.CommandHandlers;

public class ArticleBrowseRecordCommandHandler(
    IUnitOfWork<ArticleBrowseRecord> uow,
    IArticleBrowseRecordRepository repository,
    IMediatorHandler bus
) : CommandHandler(uow, bus),
    IRequestHandler<CreateArticleBrowseRecordCommand, bool>
{
    private readonly IArticleBrowseRecordRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateArticleBrowseRecordCommand request, CancellationToken cancellationToken)
    {
        //获取当前用户的浏览记录(浏览文章不需要登录，所以当前用户Id暂时使用前端传送的识别号代替)
        var starTime = DateTime.UtcNow.AddDays(-1);
        var isExist = await _repository.ExistEntityAsync(x => x.ClientUniqueId == request.ClientUniqueId
                                                           && x.ArticleId == request.ArticleId
                                                           && x.CreateTime > starTime);
        if (isExist)
        {
            return false;
        }

        var articleBrowseRecord = new ArticleBrowseRecord
        {
            ClientUniqueId = request.ClientUniqueId,
            ArticleId = request.ArticleId
        };

        await _repository.AddAsync(articleBrowseRecord);

        if (await Commit())
        {
            await _bus.RemoveListCacheEvent<ArticleBrowseRecord>(cancellationToken);
        }

        return true;
    }
}