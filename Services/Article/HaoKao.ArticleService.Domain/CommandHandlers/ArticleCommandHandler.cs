using AutoMapper;
using Girvs.Driven.Extensions;
using HaoKao.ArticleService.Domain.Commands.Article;
using HaoKao.ArticleService.Domain.Entities;
using HaoKao.Common.Extensions;

namespace HaoKao.ArticleService.Domain.CommandHandlers;

public class ArticleCommandHandler(
    IUnitOfWork<Article> uow,
    IArticleRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateArticleCommand, bool>,
    IRequestHandler<UpdateArticleCommand, bool>,
    IRequestHandler<SetArticleIsToppingCommand, bool>,
    IRequestHandler<DeleteArticleCommand, bool>
{
    private readonly IArticleRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        var article = request.MapToEntity<Article>();

        await _repository.AddAsync(article);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(article, article.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Article>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
    {
        var article = await _repository.GetByIdAsync(request.Id);
        if (article == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应文章的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        article = _mapper.Map(request, article);

        //刷新更新时间
        await _repository.UpdateAsync(article);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(article, article.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Article>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(SetArticleIsToppingCommand request, CancellationToken cancellationToken)
    {
        var article = await _repository.GetByIdAsync(request.Id);
        if (article == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应文章的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }
        article = _mapper.Map(request, article);

        //刷新更新时间
        await _repository.UpdateAsync(article);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(article, article.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Article>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        var article = await _repository.GetByIdAsync(request.Id);
        if (article == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应文章的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(article);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<Article>(article.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Article>(cancellationToken);
        }

        return true;
    }
}