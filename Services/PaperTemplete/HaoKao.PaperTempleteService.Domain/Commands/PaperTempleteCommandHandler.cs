using HaoKao.Common.Extensions;
using HaoKao.PaperTempleteService.Domain.Repositories;

namespace HaoKao.PaperTempleteService.Domain.Commands;

public class PaperTempleteCommandHandler(
    IUnitOfWork<Entities.PaperTemplete> uow,
    IPaperTempleteRepository repository,
    IMediatorHandler bus
) : CommandHandler(uow, bus),
    IRequestHandler<CreatePaperTempleteCommand, bool>,
    IRequestHandler<UpdatePaperTempleteCommand, bool>,
    IRequestHandler<DeletePaperTempleteCommand, bool>,
    IRequestHandler<UpdateTempleteStructCommand, bool>
{
    private readonly IPaperTempleteRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreatePaperTempleteCommand request, CancellationToken cancellationToken)
    {
        var paperTemplete = new Entities.PaperTemplete
        {
            TempleteName = request.TempleteName,
            Remark = request.Remark,
            SuitableSubjects = request.SuitableSubjects,
        };

        await _repository.AddAsync(paperTemplete);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<Entities.PaperTemplete>.ByIdCacheKey.Create(paperTemplete.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(paperTemplete, key, key.CacheTime), cancellationToken);
            //删除列表缓存
            await _bus.RemoveListCacheEvent<Entities.PaperTemplete>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdatePaperTempleteCommand request, CancellationToken cancellationToken)
    {
        var paperTemplete = await _repository.GetByIdAsync(request.Id);
        if (paperTemplete == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        paperTemplete.TempleteName = request.TempleteName;
        paperTemplete.Remark = request.Remark;
        paperTemplete.SuitableSubjects = request.SuitableSubjects;

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<Entities.PaperTemplete>.ByIdCacheKey.Create(paperTemplete.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(paperTemplete, key, key.CacheTime), cancellationToken);
            //删除列表缓存
            await _bus.RemoveListCacheEvent<Entities.PaperTemplete>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateTempleteStructCommand request, CancellationToken cancellationToken)
    {
        var paperTemplete = await _repository.GetByIdAsync(request.Id);
        if (paperTemplete == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        paperTemplete.TempleteStructDatas = request.TemplateStructDatas;

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<Entities.PaperTemplete>.ByIdCacheKey.Create(paperTemplete.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(paperTemplete, key, key.CacheTime), cancellationToken);
            //删除列表缓存
            await _bus.RemoveListCacheEvent<Entities.PaperTemplete>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeletePaperTempleteCommand request, CancellationToken cancellationToken)
    {
        var paperTemplete = await _repository.GetByIdAsync(request.Id);
        if (paperTemplete == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(paperTemplete);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<Entities.PaperTemplete>.ByIdCacheKey.Create(paperTemplete.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
            //删除列表缓存
            await _bus.RemoveListCacheEvent<Entities.PaperTemplete>(cancellationToken);
        }

        return true;
    }
}