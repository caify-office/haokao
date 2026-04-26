using AutoMapper;
using HaoKao.WebsiteConfigurationService.Domain.Commands.WebsiteTemplate;

namespace HaoKao.WebsiteConfigurationService.Domain.CommandHandlers;

public class WebsiteTemplateCommandHandler(
    IUnitOfWork<WebsiteTemplate> uow,
    IWebsiteTemplateRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateWebsiteTemplateCommand, bool>,
    IRequestHandler<UpdateWebsiteTemplateCommand, bool>,
    IRequestHandler<SetWebsiteTemplateContentCommand, bool>,
    IRequestHandler<SetWebsiteTemplateDefaultCommand, bool>,
    IRequestHandler<DeleteWebsiteTemplateCommand, bool>
{
    private readonly IWebsiteTemplateRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateWebsiteTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = mapper.Map<WebsiteTemplate>(request);

        await _repository.AddAsync(template);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<WebsiteTemplate>.ByIdCacheKey.Create(template.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(template, key, key.CacheTime), cancellationToken);
           await _bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<WebsiteTemplate>.TenantListCacheKey.Create()),
                cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateWebsiteTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _repository.GetByIdAsync(request.Id);
        if (template == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应模板的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        template = mapper.Map(request, template);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<WebsiteTemplate>.ByIdCacheKey.Create(template.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(template, key, key.CacheTime), cancellationToken);
           await _bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<WebsiteTemplate>.TenantListCacheKey.Create()),
                cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(SetWebsiteTemplateContentCommand request, CancellationToken cancellationToken)
    {
        var template = await _repository.GetByIdAsync(request.Id);
        if (template == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应模板的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        template.Content = request.Content;

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<WebsiteTemplate>.ByIdCacheKey.Create(template.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(template, key, key.CacheTime), cancellationToken);
           await _bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<WebsiteTemplate>.TenantListCacheKey.Create()),
                cancellationToken);
        }

        return true;
    }

   
    public async Task<bool> Handle(SetWebsiteTemplateDefaultCommand request, CancellationToken cancellationToken)
    {
        var template = await _repository.GetByIdAsync(request.Id);
        if (template == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应栏目的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }
        //设为默认时，需要把同栏目下同类型的其他默认模板改为非默认
        if (request.isDefault)
        {
            var defaultTmplate = await _repository.GetWhereAsync(x => x.IsDefault == true&&x.ColumnId==template.ColumnId && x.WebsiteTemplateType == template.WebsiteTemplateType);
            defaultTmplate.ForEach(x =>
            {
                x.IsDefault = false;
            });
        }

        template.IsDefault =request.isDefault;

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<WebsiteTemplate>.ByIdCacheKey.Create(template.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(template, key, key.CacheTime), cancellationToken);
           await _bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<WebsiteTemplate>.TenantListCacheKey.Create()),
                cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteWebsiteTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _repository.GetByIdAsync(request.Id);
        if (template == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应模板的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(template);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<WebsiteTemplate>.ByIdCacheKey.Create(template.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
           await _bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<WebsiteTemplate>.TenantListCacheKey.Create()),
                cancellationToken);
        }

        return true;
    }
}
