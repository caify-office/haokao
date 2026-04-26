using HaoKao.Common.Extensions;
using HaoKao.QuestionCategoryService.Domain.Entities;
using HaoKao.QuestionCategoryService.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HaoKao.QuestionCategoryService.Domain.Commands;

public class QuestionCategoryCommandHandler(
    IUnitOfWork<QuestionCategory> uow,
    IQuestionCategoryRepository repository,
    IMediatorHandler bus
) : CommandHandler(uow, bus),
    IRequestHandler<CreateQuestionCategoryCommand, bool>,
    IRequestHandler<UpdateQuestionCategoryCommand, bool>,
    IRequestHandler<DeleteQuestionCategoryCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateQuestionCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = new QuestionCategory
        {
            Name = request.Name,
            Code = request.Code,
            AdaptPlace = request.AdaptPlace,
            DisplayCondition = request.DisplayCondition,
            ProductPackageId = request.ProductPackageId,
            ProductPackageType = request.ProductPackageType
        };

        await repository.AddAsync(entity);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<QuestionCategory>.ByIdCacheKey.Create(entity.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(entity, key, key.CacheTime), cancellationToken);
            //删除列表缓存
            await _bus.RemoveListCacheEvent<QuestionCategory>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateQuestionCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应题库类别的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }

        entity.Name = request.Name;
        entity.Code = request.Code;
        entity.AdaptPlace = request.AdaptPlace;
        entity.DisplayCondition = request.DisplayCondition;
        entity.ProductPackageId = request.ProductPackageId;
        entity.ProductPackageType = request.ProductPackageType;

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<QuestionCategory>.ByIdCacheKey.Create(entity.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(entity, key, key.CacheTime), cancellationToken);
            //删除列表缓存
            await _bus.RemoveListCacheEvent<QuestionCategory>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteQuestionCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应题库类别的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await repository.DeleteAsync(entity);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<QuestionCategory>.ByIdCacheKey.Create(entity.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
            //删除列表缓存
            await _bus.RemoveListCacheEvent<QuestionCategory>(cancellationToken);
        }

        return true;
    }
}