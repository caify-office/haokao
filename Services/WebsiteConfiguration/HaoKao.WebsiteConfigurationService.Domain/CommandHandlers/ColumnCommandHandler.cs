using AutoMapper;

namespace HaoKao.WebsiteConfigurationService.Domain.CommandHandlers;

public class ColumnCommandHandler(
    IUnitOfWork<Column> uow,
    IColumnRepository repository,
    IMediatorHandler bus,
    IWebsiteTemplateRepository websiteTemplateRepository,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateColumnCommand, bool>,
    IRequestHandler<UpdateColumnCommand, bool>,
    IRequestHandler<DeleteColumnCommand, bool>
{
    private readonly IColumnRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IWebsiteTemplateRepository _websiteTemplateRepository = websiteTemplateRepository ?? throw new ArgumentNullException(nameof(websiteTemplateRepository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateColumnCommand request, CancellationToken cancellationToken)
    {
        var exist = await _repository.ExistEntityAsync(c => c.DomainName == request.DomainName && c.EnglishName == request.EnglishName);
        if (exist)
        {
            await _bus.RaiseEvent(new DomainNotification(request.EnglishName, "当前域名下已存在此英文名称", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }
        var column = mapper.Map<Column>(request);


        await _repository.AddAsync(column);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<Column>.ByIdCacheKey.Create(column.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(column, key, key.CacheTime), cancellationToken);
           await _bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<Column>.TenantListCacheKey.Create()), cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateColumnCommand request, CancellationToken cancellationToken)
    {
        var column = await _repository.GetByIdAsync(request.Id);
        if (column == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应栏目的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }

        var columnExist = await _repository.GetAsync(c => c.DomainName == request.DomainName && c.EnglishName == request.EnglishName);
        if (columnExist != null && columnExist.Id != request.Id)
        {
            await _bus.RaiseEvent(new DomainNotification(request.EnglishName, "当前域名下已存在此英文名称", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }
        column = mapper.Map(request, column);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<Column>.ByIdCacheKey.Create(column.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(column, key, key.CacheTime), cancellationToken);
           await _bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<Column>.TenantListCacheKey.Create()),
                                  cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteColumnCommand request, CancellationToken cancellationToken)
    {
        var column = await _repository.GetByIdAsync(request.Id);
        if (column == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应栏目的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        var existChildren = await _repository.ExistEntityAsync(w => w.ParentId.Equals(request.Id));
        if (existChildren)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "请先删除子节点", StatusCodes.Status400BadRequest),
                cancellationToken);
            return false;
        }
        var existTemplate = await _websiteTemplateRepository.ExistEntityAsync(x => x.ColumnId == request.Id);
        if (existTemplate)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "请先删除下属模板", StatusCodes.Status400BadRequest),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(column);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<Column>.ByIdCacheKey.Create(column.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
           await _bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<Column>.TenantListCacheKey.Create()),
                                  cancellationToken);
        }

        return true;
    }
}