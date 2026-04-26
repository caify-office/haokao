using System.Text.Json;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.Common.Extensions;
using HaoKao.DataDictionaryService.Domain.Entities;
using HaoKao.DataDictionaryService.Domain.Extensions;

namespace HaoKao.DataDictionaryService.Application.Services.Management;

[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "字典管理",
    "0f58c6de-a315-449b-babc-0321001954a8",
    "1",
    SystemModule.All,
    1
)]
public class DictionariesAppService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IDictionariesRepository repository
) : IDictionariesAppService
{
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IDictionariesRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    /// <summary>
    /// 根据Id获取数据字典
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser | UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<DictionariesDetailViewModel> GetAsync(Guid id)
    {
        var dictionary = await _cacheManager.GetAsync(
                             GirvsEntityCacheDefaults<Dictionaries>.ByIdCacheKey.Create(id.ToString()),
                             () => _repository.GetByIdAsync(id))
                      ?? throw new GirvsException("未找到对应的字典", StatusCodes.Status404NotFound);

        return dictionary.MapToDto<DictionariesDetailViewModel>();
    }

    /// <summary>
    /// 根据id和名称补充树形直接子集字典数据
    /// </summary>
    /// <param name="id">字典id</param>
    /// <param name="name">字典名称</param>
    /// <returns></returns>
    [HttpGet("{id:guid}/{name}/SupplementaryChildren")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser | UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<string> SupplementaryChildrenDictionaryDataAsync(Guid id, string name)
    {
        var result = new List<DictionariesSupplyViewModel>();
        var dictionaries = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Dictionaries>.QueryCacheKey.Create($"Pid={id}"),
            () => _repository.GetByIncludeOneLevelChildrenIdAsync(id));
        var dictChildren = dictionaries?.Children;
        if (dictChildren != null && dictChildren.Any())
        {
            foreach (var dic in dictChildren)
            {
                var cache = GirvsEntityCacheDefaults<Dictionaries>.ListCacheKey.Create("", $"pid={dic.Id}".ToMd5());
                var children = await _cacheManager.GetAsync(cache, () => _repository.GetWhereAsync(x => x.Pid == dic.Id));
                result.Add(new DictionariesSupplyViewModel
                {
                    Value = dic.Id.ToString(),
                    Label = dic.Name,
                    IsLeaf = !children.Any()
                });
            }
        }
        return JsonConvert.SerializeObject(result);
    }

    /// <summary>
    /// 通过用户信息项保存的值中的字典id补充字典的名称
    /// </summary>
    /// <param name="modelJsonStr"></param>
    /// <returns></returns>
    [HttpPost("SupplementaryDictionaryName")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser | UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<List<UserInformationListViewModel>> SupplementaryDictionaryName([FromBody] string modelJsonStr)
    {
        var informItemList = JsonConvert.DeserializeObject<List<UserInformationListViewModel>>(modelJsonStr);

        var infoList = informItemList.Where(w => !string.IsNullOrWhiteSpace(w.Value)).ToList();

        foreach (var item in infoList)
        {
            var dicIds = item.Value.Split("/", StringSplitOptions.RemoveEmptyEntries);
            if (dicIds.Length > 0 && Guid.TryParse(dicIds[0], out _))
            {
                var ids = dicIds.Where(a => !string.IsNullOrWhiteSpace(a)).Select(Guid.Parse);
                var dictionaries = await GetDictionariesByIds(ids);
                var dicNames = dictionaries.Select(s => s.Name).ToArray();
                item.Display = dicNames.Any() ? string.Join("/", dicNames) : item.Value;
            }
            else
            {
                item.Display = item.Value;
            }
        }

        return informItemList;
    }

    [NonAction]
    private async Task<List<Dictionaries>> GetDictionariesByIds(IEnumerable<Guid> dicIds)
    {
        var cache = GirvsEntityCacheDefaults<Dictionaries>.ByIdsCacheKey.Create(string.Join("/", dicIds).ToMd5());
        var dictionaries = await _cacheManager.GetAsync(cache,
                                                        async () => await _repository.GetWhereAsync(x => dicIds.Contains(x.Id)));
        return dictionaries;
    }

    /// <summary>
    /// 补充字典数据
    /// </summary>
    /// <param name="models">json字符串</param>
    /// <returns></returns>
    [HttpPost("NewSupplementary")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser | UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<List<NewTemplateViewModel>> NewSupplementary([FromBody] List<NewTemplateViewModel> models)
    {
        if (models != null && models.Any())
        {
            var result = models.SelectMany(s => s.List).Where(w => !string.IsNullOrWhiteSpace(w.DictionaryId));

            foreach (var model in result)
            {
                var dictionaryId = model.DictionaryId.ToGuid();
                var cache = GirvsEntityCacheDefaults<Dictionaries>.ListCacheKey.Create("", $"pid={dictionaryId}HasChildren".ToMd5());
                var children = await _cacheManager.GetAsync(cache, () => _repository.GetWhereIncludeChildrenAsync(x => x.Pid == dictionaryId));

                if (children != null && children.Any())
                {
                    foreach (var dic in children)
                    {
                        model.DataSource.Add(new DictionariesNewSupplyViewModel
                        {
                            Id = dic.Id,
                            Name = dic.Name,
                            IsLeaf = dic.Children.Count == 0
                        });
                    }
                }
            }
        }
        return models;
    }

    private Task<List<Dictionaries>> GetOnLevelChildrens(Guid dictionaryId)
    {
        var cache = GirvsEntityCacheDefaults<Dictionaries>.ListCacheKey.Create("", $"pid={dictionaryId}".ToMd5());
        return _cacheManager.GetAsync(cache, () => _repository.GetWhereAsync(x => x.Pid == dictionaryId));
    }

    /// <summary>
    /// 补充字典数据
    /// </summary>
    /// <param name="modelJsonStr">json字符串</param>
    /// <returns></returns>
    [HttpPost("Supplementary")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser | UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<string> SupplementaryDictionaryDataAsync([FromBody] string modelJsonStr)
    {
        var templateList = JsonConvert.DeserializeObject<List<TemplateViewModel>>(modelJsonStr);

        foreach (var template in templateList)
        {
            foreach (var item in template.List)
            {
                if (string.IsNullOrEmpty(item.DictionaryId) || string.IsNullOrEmpty(item.DictionaryName)) continue;
                var dicId = Guid.Parse(item.DictionaryId);
                var dictionary = await GetTreeByIdAsync(dicId);
                if (dictionary == null) continue;
                var dataSource = CreateDataSource(dictionary.Children);
                item.DataSource = dataSource;
            }
        }

        return JsonConvert.SerializeObject(templateList);
    }

    /// <summary>
    /// 生成数据
    /// </summary>
    /// <param name="dictionariesTreeViewModel"></param>
    /// <returns></returns>
    [NonAction]
    private List<DictionariesSupplyViewModel> CreateDataSource(List<DictionariesTreeViewModel> dictionariesTreeViewModel)
    {
        var dataSource = new List<DictionariesSupplyViewModel>();
        if (dictionariesTreeViewModel == null) return dataSource;

        foreach (var dictionary in dictionariesTreeViewModel)
        {
            var supply = new DictionariesSupplyViewModel
            {
                Label = dictionary.Name,
                Value = dictionary.Id.ToString(),
                IsLeaf = dictionary.IsLeaf
            };
            dataSource.Add(supply);
        }

        return dataSource;
    }

    /// <summary>
    /// 创建数据字典类型
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.AdminUser | UserType.SpecialUser | UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<DictionariesAddViewModel> CreateAsync([FromBody] DictionariesAddViewModel model)
    {
        var command = new CreateDictionariesCommand(model.Sort, model.Code, model.Name, model.Pid, model.PName, model.State);
        var (id, code) = await _bus.SendCommand<CreateDictionariesCommand, (Guid?, string)>(command);
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        model.Id = id;
        model.Code = code;
        return model;
    }

    /// <summary>
    /// 更新数据字典类型
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [ServiceMethodPermissionDescriptor("修改", Permission.Edit, UserType.AdminUser | UserType.SpecialUser | UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<DictionariesEditViewModel> UpdateAsync(Guid id, [FromBody] DictionariesEditViewModel model)
    {
        var command = new UpdateDictionariesCommand(id, model.Sort, model.Code, model.Name, model.Pid, model.PName, model.State);
        await _bus.SendCommand(command);
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return model;
    }

    /// <summary>
    /// 删除数据字典类型
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpDelete("batch")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.AdminUser | UserType.SpecialUser | UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task DeleteAsync([FromQuery] Guid[] ids)
    {
        var command = new DeleteDictionariesCommand(ids);
        await _bus.SendCommand(command);
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 获取数据字典树列表
    /// </summary>
    /// <param name="id">展开的节点</param>
    /// <param name="name">根据名称搜索</param>
    /// <returns></returns>
    [HttpGet("tree")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser | UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<List<DictionariesTreeViewModel>> GetTreeByQueryAsync(Guid? id, string name)
    {
        var cacheKey = HashHelper.CreateHash(Encoding.UTF8.GetBytes($"id={id} & name={name}"));

        var dictionaries = await _cacheManager.GetAsync(
            cacheKey.CreateDictionariesQueryCacheKey(),
            () => _repository.GetDictionariesTreeByQueryAsync(id, name));
        var resultTrees = dictionaries.MapTo<List<DictionariesTreeViewModel>>();
        return resultTrees;
    }

    /// <summary>
    /// 获取自身和所有子字典
    /// </summary>
    /// <param name="id">展开的节点</param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser | UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<List<DictionariesTreeViewModel>> GetAllAsync(Guid id)
    {
        var cacheKey = HashHelper.CreateHash(Encoding.UTF8.GetBytes($"id={id}All "));

        var dictionaries = await _cacheManager.GetAsync(
            cacheKey.CreateDictionariesQueryCacheKey(),
            () => _repository.GetAllAsync(id));
        var resultTrees = dictionaries.MapTo<List<DictionariesTreeViewModel>>();
        //遍历子类塞入平级list中
        FindChildren(dictionaries, resultTrees);
        return resultTrees;
    }

    private void FindChildren(List<Dictionaries> dictionaries, List<DictionariesTreeViewModel> resultTrees)
    {
        foreach (var item in dictionaries)
        {
            if (!item.Children.Any()) continue;
            //直接子字典加入list中
            var childrenList = item.Children.MapTo<List<DictionariesTreeViewModel>>();
            resultTrees.AddRange(childrenList);
            //递归子字典List
            FindChildren(item.Children, resultTrees);
        }
    }

    /// <summary>
    /// 通过获取字典ID获取数据字典树列表
    /// </summary>
    /// <param name="id">展开的节点</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DictionariesTreeViewModel> GetTreeByIdAsync(Guid id)
    {
        var dictionaries = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Dictionaries>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIncludeChildrenIdAsync(id));
        var resultTrees = dictionaries.MapToDto<DictionariesTreeViewModel>();
        return resultTrees;
    }

    /// <summary>
    /// 通过获取字典ID数组获取数据字典和直接子字典列表
    /// </summary>
    /// <param name="ids">展开的节点</param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser | UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<List<DictionariesTreeHaveChildrenViewModel>> GetTreeByIdsAsync([FromBody] Guid[] ids)
    {
        var dictionariesList = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Dictionaries>.ByIdsCacheKey.Create(string.Join("-", ids).ToMd5()),
            () => _repository.GetByIncludeChildrenIdsAsync(ids));
        var resultTrees = dictionariesList.MapTo<List<DictionariesTreeHaveChildrenViewModel>>();
        return resultTrees;
    }

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="queryModel"></param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser | UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<DictionariesQueryViewModel> GetByQueryAsync([FromQuery] DictionariesQueryViewModel queryModel)
    {
        var query = queryModel.MapToQuery<DictionariesQuery>();

        query.QueryFields = typeof(DictionariesQueryListViewModel).GetTypeQueryFields();

        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Dictionaries>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<DictionariesQueryViewModel, Dictionaries>();
    }

    /// <summary>
    /// 获取字典导入格式
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetDictionariesFormat")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View)]
    public dynamic GetDictionariesFormat()
    {
        var cacheKey = GirvsEntityCacheDefaults<Dictionaries>.BuideCustomize("Dictionaries:{0}").Create("DictionariesDataCommandModel");
        var result = _cacheManager.Get<string>(cacheKey, () =>
        {
            var r = typeof(DictionariesDataCommandModel).GetTypeFields();
            return System.Text.Json.JsonSerializer.Serialize(r, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        });
        return System.Text.Json.JsonSerializer.Deserialize<dynamic>(result);
    }

    /// <summary>
    /// 导出全部字典数据
    /// </summary>
    /// <param name="queryModel"></param>
    /// <returns></returns>
    [HttpGet("simple")]
    [ServiceMethodPermissionDescriptor("导出Excel", Permission.Publish, UserType.AdminUser | UserType.SpecialUser | UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<SimpleDictionariesQueryViewModel> GetBySimpleQueryAsync([FromQuery] SimpleDictionariesQueryViewModel queryModel)
    {
        var query = queryModel.MapToQuery<SimpleDictionariesQuery>();

        query.QueryFields = typeof(DictionariesQueryListViewModel).GetTypeQueryFields();

        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Dictionaries>.QueryCacheKey.Create(query.GetCacheKey() + "_simple"),
            async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<SimpleDictionariesQueryViewModel, Dictionaries>();
    }

    /// <summary>
    /// 获取字典分类列表
    /// </summary>
    /// <param name="query">名称或者编码</param>
    /// <returns></returns>
    [HttpGet("categorys")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser | UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<List<DictionariesCategoryListViewModel>> GetCategorysByQueryAsync(string query)
    {
        var cacheKey = HashHelper.CreateHash(Encoding.UTF8.GetBytes($"query={query}"));

        var queryFields = typeof(DictionariesCategoryListViewModel).GetTypeQueryFields();

        var categoryList = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Dictionaries>.QueryCacheKey.Create(cacheKey),
            () => _repository.GetDictionariesCategoryListByQueryAsync(query, queryFields));
        return categoryList.MapTo<List<DictionariesCategoryListViewModel>>();
    }

    /// <summary>
    /// 批量添加字典数据
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("导入", Permission.Merge)]
    public async Task<dynamic> DictionariesDataBatchAdd([FromBody] List<DictionariesDataCommandModel> list)
    {
        var command = new BatchCreateDictionariesCommand(list);
        await _bus.SendCommand(command);
        if (_notifications.HasNotifications())
        {
            var notifications = _notifications.GetNotifications().ToList();
            return new
            {
                Detail = notifications.Select(x => new
                {
                    Index = x.Key,
                    Reason = new[] { x.Value }
                }),
                FailNum = notifications.Count,
                SuccNum = list.Count - notifications.Count,
                Total = list.Count
            };
        }

        return new
        {
            FailNum = 0,
            SuccNum = list.Count,
            Total = list.Count
        };
    }

    /// <summary>
    /// 根据Ids获取数据字典
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPost("Ids")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser | UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<Dictionary<string, string>> GetByIdsAsync([FromBody] List<string> ids)
    {
        if (!ids.Any()) return new Dictionary<string, string>();
        var idStr = string.Join('/', ids).Trim('/');
        var dictionaries = await _repository.GetWhereAsync(x => idStr.Contains(x.Id.ToString()));
        var result = new Dictionary<string, string>();
        foreach (var id in ids)
        {
            if (id.Contains('/'))
            {
                var dict = dictionaries.Where(f => id.Contains(f.Id.ToString())).Select(s => s.Name);
                if (dict.Any())
                {
                    result.Add(id, string.Join('/', dict));
                }
            }
            else
            {
                result.Add(id, dictionaries.FirstOrDefault(f => f.Id.ToString() == id)?.Name);
            }
        }

        return result;
    }
}