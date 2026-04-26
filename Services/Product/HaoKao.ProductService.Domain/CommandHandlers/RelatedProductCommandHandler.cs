using HaoKao.ProductService.Domain.Commands.RelatedProduct;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Repositories;

namespace HaoKao.ProductService.Domain.CommandHandlers;

public class RelatedProductCommandHandler(
    IUnitOfWork<RelatedProduct> uow,
    IRelatedProductRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateRelatedProductCommand, bool>,
    IRequestHandler<DeleteRelatedProductCommand, bool>
{
    private readonly IRelatedProductRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateRelatedProductCommand request, CancellationToken cancellationToken)
    {
        var models = _mapper.Map<List<RelatedProduct>>(request.Models);
        await _repository.AddRangeAsync(models);

        if (await Commit())
        {
           await _bus.RemoveListCacheEvent<RelatedProduct>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteRelatedProductCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteRangeAsync(x => request.Ids.Contains(x.Id));
        // 批量删除影响行数一直显示0：暂时去除if判断
        await Commit();
        // 删除查询相关的缓存
        await _bus.RemoveListCacheEvent<RelatedProduct>(cancellationToken);
        return true;
    }
}