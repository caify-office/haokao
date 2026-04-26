using ShortUrlService.Domain.Entities;
using ShortUrlService.Domain.Repositories;

namespace ShortUrlService.Domain.Commands;

public class ShortUrlCommandHandler(
    IUnitOfWork<ShortUrl> uow,
    IMediatorHandler bus,
    IShortUrlRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateShortUrlCommand, ShortUrl>,
    IRequestHandler<UpdateShortUrlCommand, bool>,
    IRequestHandler<UpdateShortKeyCommand, bool>,
    IRequestHandler<DeleteShortUrlCommand, bool>
{
    private readonly IShortUrlRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<ShortUrl> Handle(CreateShortUrlCommand request, CancellationToken cancellationToken)
    {
        var entity = new ShortUrl
        {
            RegisterAppId = request.RegisterAppId,
            ShortKey = request.ShortKey,
            OriginUrl = request.OriginUrl,
            AccessLimit = request.AccessLimit,
            ExpiredTime = request.ExpiredTime,
            CreateTime = DateTime.Now,
        };

        await _repository.AddAsync(entity);

        if (await Commit())
        {
            return entity;
        }

        throw new GirvsException(StatusCodes.Status500InternalServerError, "保存短链接失败");
    }

    public async Task<bool> Handle(UpdateShortUrlCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            throw new GirvsException("Not Found: " + request.Id, StatusCodes.Status404NotFound);
        }

        entity.AccessLimit = request.AccessLimit;
        entity.ExpiredTime = request.ExpiredTime;
        await _repository.UpdateAsync(entity);

        return await Commit();
    }

    public async Task<bool> Handle(UpdateShortKeyCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            throw new GirvsException("Not Found: " + request.Id, StatusCodes.Status404NotFound);
        }

        entity.ShortKey = request.ShortKey;
        await _repository.UpdateAsync(entity);

        return await Commit();
    }

    public async Task<bool> Handle(DeleteShortUrlCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id) ?? throw new GirvsException(StatusCodes.Status404NotFound, "未找到对应的数据");

        entity.IsDelete = true;

        await _repository.UpdateAsync(entity);

        if (await Commit())
        {
            return true;
        }

        throw new GirvsException(StatusCodes.Status500InternalServerError, "删除短链接失败");
    }
}