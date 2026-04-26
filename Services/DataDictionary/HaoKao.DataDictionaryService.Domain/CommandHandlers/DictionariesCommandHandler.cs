using HaoKao.Common.Extensions;
using HaoKao.DataDictionaryService.Domain.Entities;

namespace HaoKao.DataDictionaryService.Domain.CommandHandlers;

public class DictionariesCommandHandler(
    IDictionariesRepository repository,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IUnitOfWork<Dictionaries> unitOfWork
) : CommandHandler(unitOfWork, bus),
    IRequestHandler<CreateDictionariesCommand, (Guid?, string)>,
    IRequestHandler<BatchCreateDictionariesCommand, bool>,
    IRequestHandler<UpdateDictionariesCommand, bool>,
    IRequestHandler<DeleteDictionariesCommand, bool>
{
    private readonly IDictionariesRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications ?? throw new ArgumentNullException(nameof(notifications));

    public async Task<(Guid?, string)> Handle(CreateDictionariesCommand request, CancellationToken cancellationToken)
    {
        var dictionaries = new Dictionaries
        {
            Sort = request.Sort,
            Name = request.Name,
            Code = string.IsNullOrEmpty(request.Code) ? request.Name.PinYinCode() : request.Code,
            Pid = request.Pid,
            PName = request.PName,
            State = request.State,
            TenantId = EngineContext.Current.ClaimManager.IdentityClaim.GetTenantId<Guid>()
        };

        var exist = await _repository.ExistEntityAsync(x => x.Code == dictionaries.Code);

        if (exist)
        {
            await _bus.RaiseEvent(new DomainNotification("", $"字典编码{dictionaries.Code}已存在"), cancellationToken);
            return (null, string.Empty);
        }

        if (request.Pid.HasValue)
        {
            var parent = await _repository.GetByIdAsync(request.Pid.Value);
            if (parent != null)
            {
                if (parent.TenantId != dictionaries.TenantId)
                {
                    await _bus.RaiseEvent(new DomainNotification("", "共享数据无法修改!"), cancellationToken);
                    return (null, string.Empty);
                }
                dictionaries.Path = $"{parent.Path}=>{dictionaries.Name}";
            }
        }
        else
        {
            dictionaries.Path = $"{dictionaries.Name}";
        }

        await _repository.AddAsync(dictionaries);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(dictionaries, dictionaries.Id.ToString(), cancellationToken);
            await _bus.RemoveTenantListCacheEvent<Dictionaries>(cancellationToken);

            if (dictionaries.TenantId == Guid.Empty)
            {
                await RemoveEntityAllCache(cancellationToken);
            }
        }

        return (dictionaries.Id, dictionaries.Code);
    }

    private async Task RemoveEntityAllCache(CancellationToken cancellationToken)
    {
        //删除当前实体所有缓存
        var entityCacheKey = GirvsEntityCacheDefaults<Dictionaries>.BuideCustomize(nameof(Dictionaries).ToLowerInvariant()).Create();
        await _bus.RaiseEvent(new RemoveCacheByPrefixEvent(entityCacheKey), cancellationToken);
    }

    public async Task<bool> Handle(BatchCreateDictionariesCommand request, CancellationToken cancellationToken)
    {
        //符合条件的数据
        var importList = new List<Dictionaries>();
        foreach (var sourceObj in request.DictionariesDataCommandModels)
        {
            var dictionaries = new Dictionaries
            {
                Code = sourceObj.Code,
                Name = sourceObj.Name,
                State = sourceObj.State != 0,
                Sort = sourceObj.Sort,
                TenantId = EngineContext.Current.ClaimManager.IdentityClaim.GetTenantId<Guid>(),
            };

            //判断文本是否符合规范

            if (sourceObj.Code?.Length > 30)
            {
                await _bus.RaiseEvent(
                    new DomainNotification(sourceObj.Index.ToString(), "字典编码度不能大于30!", StatusCodes.Status404NotFound),
                    cancellationToken);
                continue;
            }

            if (sourceObj.Name?.Length > 225)
            {
                await _bus.RaiseEvent(
                    new DomainNotification(sourceObj.Index.ToString(), "字典名称长度不能大于225!", StatusCodes.Status404NotFound),
                    cancellationToken);
                continue;
            }

            if (sourceObj.PCode?.Length > 30)
            {
                await _bus.RaiseEvent(
                    new DomainNotification(sourceObj.Index.ToString(), "父字典编码度不能大于30!", StatusCodes.Status404NotFound),
                    cancellationToken);
                continue;
            }

            if (sourceObj.PName?.Length > 225)
            {
                await _bus.RaiseEvent(
                    new DomainNotification(sourceObj.Index.ToString(), "父字典名称长度不能大于225!", StatusCodes.Status404NotFound),
                    cancellationToken);
                continue;
            }

            var obj = sourceObj;
            var target = await _repository.ExistEntityAsync(o => o.Code == obj.Code && o.Name == obj.Name);

            if (target)
            {
                await _bus.RaiseEvent(
                    new DomainNotification(sourceObj.Index.ToString(), $"字典编码为{sourceObj.Code}字典名称为{sourceObj.Name}的字典已存在", StatusCodes.Status404NotFound),
                    cancellationToken);
                continue;
            }

            var dictionary = importList.Find(d => d.Code == sourceObj.PCode && d.Name == sourceObj.PName);
            if (dictionary is not null)
            {
                //父节点跟当前要添加到数据库的子节点在同一批次
                await _repository.AddAsync(dictionary);
                if (await Commit())
                {
                    importList.Remove(dictionary);
                }
            }

            if (string.IsNullOrEmpty(sourceObj.PCode) || string.IsNullOrEmpty(sourceObj.PName))
            {
                //顶级节点
                dictionaries.Path = $"{sourceObj.Name}";
                dictionaries.PName = sourceObj.PName;
            }
            else
            {
                var parent = await _repository.GetAsync(o => o.Code == sourceObj.PCode && o.Name == sourceObj.PName);

                if (parent == null)
                {
                    await _bus.RaiseEvent(
                        new DomainNotification(sourceObj.Index.ToString(), $"未找到编号为{sourceObj.PCode}名称为{sourceObj.PName}的父类", StatusCodes.Status404NotFound),
                        cancellationToken);
                    continue;
                }
                dictionaries.PName = parent.Name;
                dictionaries.Pid = parent.Id;
                dictionaries.Path = $"{parent.Path}=>{sourceObj.Name}";
            }

            importList.Add(dictionaries);
        }

        await _repository.AddRangeAsync(importList);

        if (await Commit())
        {
            await _bus.RemoveTenantListCacheEvent<Dictionaries>(cancellationToken);

            if (importList.FirstOrDefault().TenantId == Guid.Empty)
            {
                await RemoveEntityAllCache(cancellationToken);
            }
        }
        return true;
    }

    public async Task<bool> Handle(UpdateDictionariesCommand request, CancellationToken cancellationToken)
    {
        var dictionaries = await _repository.GetByIdAsync(request.Id);
        if (dictionaries == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应的数据"), cancellationToken);
            return false;
        }
        // 更新子节点相关Path字段
        if (!dictionaries.Name.Equals(request.Name))
        {
            var parentDics = await _repository.GetWhereAsync(w => w.Path.Contains($"=>{dictionaries.Name}"));
            parentDics.ForEach(f => f.Path = f.Path.Replace($"=>{dictionaries.Name}", $"=>{request.Name}"));
            await _repository.UpdateRangeAsync(parentDics);
        }

        dictionaries.Sort = request.Sort;
        dictionaries.Name = request.Name;
        dictionaries.Pid = request.Pid;
        dictionaries.PName = request.PName;
        dictionaries.State = request.State;
        dictionaries.Code = !string.IsNullOrEmpty(request.Code)
            ? request.Code
            : request.Name == dictionaries.Name
                ? request.Name.PinYinCode()
                : dictionaries.Code;

        await _repository.UpdateAsync(dictionaries);

        if (await Commit())
        {
            await _bus.RaiseEvent(new DictionariesUpdateEvent(dictionaries.Id, dictionaries.Name), cancellationToken);

            await _bus.UpdateIdCacheEvent(dictionaries, dictionaries.Id.ToString(), cancellationToken);
            await _bus.RemoveTenantListCacheEvent<Dictionaries>(cancellationToken);

            if (dictionaries.TenantId == Guid.Empty)
            {
                await RemoveEntityAllCache(cancellationToken);
            }
        }

        return true;
    }

    public async Task<bool> Handle(DeleteDictionariesCommand request, CancellationToken cancellationToken)
    {
        // 批量删除
        if (request.Ids != null && request.Ids.Any())
        {
            var dictionaries = await _repository.GetNoTenantIdWhereAsync(w => w.Pid.HasValue && request.Ids.Contains(w.Pid.Value));
            // 判断是否能删除
            foreach (var item in request.Ids)
            {
                var children = dictionaries.FirstOrDefault(w => w.Pid.Equals(item));
                if (children != null)
                {
                    await _bus.RaiseEvent(
                        new DomainNotification(children.PName, "请先删除子级字典!", StatusCodes.Status400BadRequest),
                        cancellationToken);
                }
            }
            if (_notifications.HasNotifications()) return false;

            // 判断哪些找不到数据
            dictionaries = await _repository.GetWhereAsync(w => request.Ids.Contains(w.Id));
            foreach (var item in request.Ids)
            {
                var exists = dictionaries.Any(w => w.Id.Equals(item));
                if (!exists)
                {
                    await _bus.RaiseEvent(
                        new DomainNotification(item.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                        cancellationToken);
                }
            }

            await _repository.DeleteRangeAsync(dictionaries);

            if (await Commit())
            {
                await _bus.RemoveTenantListCacheEvent<Dictionaries>(cancellationToken);

                if (dictionaries.FirstOrDefault().TenantId == Guid.Empty)
                {
                    await RemoveEntityAllCache(cancellationToken);
                }
            }
        }

        return true;
    }
}