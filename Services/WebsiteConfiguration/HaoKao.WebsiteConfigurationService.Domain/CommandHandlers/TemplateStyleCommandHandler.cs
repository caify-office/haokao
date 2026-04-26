using AutoMapper;
using HaoKao.WebsiteConfigurationService.Domain.Commands.TemplateStyle;

namespace HaoKao.WebsiteConfigurationService.Domain.CommandHandlers;

public class TemplateStyleCommandHandler(
    IUnitOfWork<TemplateStyle> uow,
    ITemplateStyleRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateTemplateStyleCommand, bool>,
    IRequestHandler<UpdateTemplateStyleCommand, bool>,
    IRequestHandler<DeleteTemplateStyleCommand, bool>
{
    private readonly ITemplateStyleRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateTemplateStyleCommand request, CancellationToken cancellationToken)
    {
        var templateStyle = mapper.Map<TemplateStyle>(request);

        await _repository.AddAsync(templateStyle);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<TemplateStyle>.ByIdCacheKey.Create(templateStyle.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(templateStyle, key, key.CacheTime), cancellationToken);
           await _bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<TemplateStyle>.TenantListCacheKey.Create()),
                cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateTemplateStyleCommand request, CancellationToken cancellationToken)
    {
        var templateStyle = await _repository.GetByIdAsync(request.Id);
        if (templateStyle == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应模板的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        templateStyle = mapper.Map(request, templateStyle);
        
        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<TemplateStyle>.ByIdCacheKey.Create(templateStyle.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(templateStyle, key, key.CacheTime), cancellationToken);
           await _bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<TemplateStyle>.TenantListCacheKey.Create()),
                cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteTemplateStyleCommand request, CancellationToken cancellationToken)
    {
        var templateStyle = await _repository.GetByIdAsync(request.Id);
        if (templateStyle == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应模板的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(templateStyle);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<TemplateStyle>.ByIdCacheKey.Create(templateStyle.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
           await _bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<TemplateStyle>.TenantListCacheKey.Create()),
                cancellationToken);
        }

        return true;
    }
}
