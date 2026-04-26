using HaoKao.ProductService.Domain.Commands.Product;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Enums;
using HaoKao.ProductService.Domain.RemoteRepositories;
using HaoKao.ProductService.Domain.Repositories;

namespace HaoKao.ProductService.Domain.CommandHandlers;

public class ProductCommandHandler(
    IUnitOfWork<Product> uow,
    IProductRepository productRepository,
    IProductPackageRepository productPackageRepository,
    IRemoteCourseRepository remoteCourseRepository,
    IRemotePaperRepository remotePaperRepository,
    IRemoteQuestionRepository remoteQuestionRepository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateProductCommand, bool>,
    IRequestHandler<UpdateProductCommand, bool>,
    IRequestHandler<DeleteProductCommand, bool>,
    IRequestHandler<SetProductAnsweringCommand, bool>,
    IRequestHandler<SetProductEnableCommand, bool>,
    IRequestHandler<SetProductShelvesCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var p = mapper.Map<Product>(request);
        p.Enable = true;
        p.ReservationBaseNumber = request.ProductType == ProductType.Live ? new Random().Next(500, 800) : 0;
        await productRepository.AddAsync(p);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(p, p.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Product>(cancellationToken);
            await ExecuteStatisticsTaskCountAndDurations(p.AssistantProductPermissions);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var p = await productRepository.GetByIdInclude(request.Id);
        if (p == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应产品的数据", StatusCodes.Status404NotFound), cancellationToken);
            return false;
        }
        if (p.IsExperience != request.IsExperience)
        {
            //是否体验产品属性被修改
            var productIds = await productPackageRepository.GetAllProductIds();
            if (productIds.Any(x => x == p.Id))
            {
                await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "该产品已被添加到产品包，请先前往“产品包-设置产品”中解除添加”", StatusCodes.Status404NotFound), cancellationToken);
                return false;
            }
        }

        p = mapper.Map(request, p);

        p.ProductPermissions.Clear();
        foreach (var permissionCommand in request.ProductPermissions)
        {
            p.ProductPermissions.Add(new ProductPermission
            {
                SubjectId = permissionCommand.SubjectId,
                SubjectName = permissionCommand.SubjectName,
                PermissionName = permissionCommand.PermissionName,
                PermissionId = permissionCommand.PermissionId,
                Category = permissionCommand.Category
            });
        }

        p.AssistantProductPermissions.Clear();
        foreach (var permissionCommand in request.AssistantProductPermissions)
        {
            p.AssistantProductPermissions.Add(new AssistantProductPermission
            {
                SubjectId = permissionCommand.SubjectId,
                SubjectName = permissionCommand.SubjectName,
                CourseStageId = permissionCommand.CourseStageId,
                CourseStageName = permissionCommand.CourseStageName,
                StartTime = permissionCommand.StartTime,
                EndTime = permissionCommand.EndTime,
                AssistantProductPermissionContents = permissionCommand.AssistantProductPermissionContents,
            });
        }

        await productRepository.UpdateAsync(p);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(p, p.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Product>(cancellationToken);
            await ExecuteStatisticsTaskCountAndDurations(p.AssistantProductPermissions);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await productRepository.DeleteByIds(request.Ids);

        foreach (var id in request.Ids)
        {
            await _bus.RemoveIdCacheEvent<Product>(id.ToString(), cancellationToken);
        }

        await _bus.RemoveListCacheEvent<Product>(cancellationToken);

        return true;
    }

    public async Task<bool> Handle(SetProductAnsweringCommand request, CancellationToken cancellationToken)
    {
        await productRepository.UpdateAnsweringByIds(request.Ids, request.Answering);

        foreach (var id in request.Ids)
        {
            await _bus.RemoveIdCacheEvent<Product>(id.ToString(), cancellationToken);
        }

        await _bus.RemoveListCacheEvent<Product>(cancellationToken);

        return true;
    }

    public async Task<bool> Handle(SetProductEnableCommand request, CancellationToken cancellationToken)
    {
        await productRepository.UpdateEnableByIds(request.Ids, request.Enable);

        foreach (var id in request.Ids)
        {
            await _bus.RemoveIdCacheEvent<Product>(id.ToString(), cancellationToken);
        }

        await _bus.RemoveListCacheEvent<Product>(cancellationToken);

        return true;
    }

    public async Task<bool> Handle(SetProductShelvesCommand request, CancellationToken cancellationToken)
    {
        var existDisEnableProduct = await productRepository.ExistEntityAsync(x => request.Ids.Contains(x.Id) && x.Enable == false);
        if (existDisEnableProduct && request.IsShelves)
        {
            await _bus.RaiseEvent(new DomainNotification("", "请先启用产品再上架", StatusCodes.Status400BadRequest),
                                  cancellationToken);
            return false;
        }
        await productRepository.UpdateIsShelvesByIds(request.Ids, request.IsShelves);

        foreach (var id in request.Ids)
        {
            await _bus.RemoveIdCacheEvent<Product>(id.ToString(), cancellationToken);
        }

        await _bus.RemoveListCacheEvent<Product>(cancellationToken);

        return true;
    }

    #region 私有方法

    private async Task ExecuteStatisticsTaskCountAndDurations(ICollection<AssistantProductPermission> assistantProductPermissions)
    {
        if (assistantProductPermissions.Count == 0) return;

        var p = assistantProductPermissions.Select(x => new AssistantProductPermissionModel(x.ProductId, x.SubjectId, x.AssistantProductPermissionContents)).ToList();
        //统计当前产品权限课程和试卷的总任务和总时长
        await StatisticsTaskCountAndDurations(p);
    }

    /// <summary>
    /// 统计任务总数和总时长存入缓存
    /// </summary>
    /// <param name="assistantProductPermissions"></param>
    /// <returns></returns>
    private async Task StatisticsTaskCountAndDurations(ICollection<AssistantProductPermissionModel> assistantProductPermissions)
    {
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId();
        var permissionDic = assistantProductPermissions
                            .GroupBy(x => new { x.ProductId, x.SubjectId })
                            .ToDictionary(x => x.Key, x => x.ToList().SelectMany(y => y.AssistantProductPermissionContents).ToList());

        foreach (var item in permissionDic)
        {
            var cacheKey = StatisticsAssistantProductPermissionManager.BuildCacheKey(tenantId, item.Key.ProductId, item.Key.SubjectId).Create("", "", 365 * 24 * 60);
            //统计当前产品当前科目的任务总数和任务总时长
            var result = await StatisticsTaskCountAndDurations(item.Value);
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(result, cacheKey, cacheKey.CacheTime));
        }
    }

    private async Task<StatisticsTaskCountDurationsModel> StatisticsTaskCountAndDurations(List<AssistantProductPermissionContent> value)
    {
        var model = new StatisticsTaskCountDurationsModel();
        foreach (var assistantProductPermissionContent in value)
        {
            if (assistantProductPermissionContent.AssistanPermissionType == AssistanPermissionType.Course)
            {
                //通过自身章节id找到根章节id，已根章节id为键，组成List键值对
                var allCourseChapter = await remoteCourseRepository.GetAllCourseChapterAsync(assistantProductPermissionContent.PermissionId);
                //通过课程id查课程章节id，没有查到课程章节 ，可能是后台把智辅课程配置在阶段课程中了
                if (!allCourseChapter.Any()) continue;
                var baseCourseChapterList = allCourseChapter.Where(x => x.ParentId == Guid.Empty).ToList();

                var baseCourseChapterValueTupleList = baseCourseChapterList.Select(x => new ValueTuple<Guid, List<CourseVideoQueryListInfo>>(x.Id, [])).ToList();

                //查询课程视频，将视频插入已章节id为键组成的List键值对中
                var result = await remoteCourseRepository.GetVideoIdsByCourseIds(assistantProductPermissionContent.PermissionId.ToString());
                if (result.Any())
                {
                    var resultGroup = result.GroupBy(x => x.CourseChapterId)
                                            .Select(g => new KeyValuePair<Guid, List<CourseVideoQueryListInfo>>(g.Key, g.ToList()))
                                            .ToList();

                    foreach (var keyValue in resultGroup)
                    {
                        var baseCourseChapter = GetParentCourseChapter(keyValue.Key, allCourseChapter);
                        if (baseCourseChapter == null) continue;
                        var index = baseCourseChapterValueTupleList.FindIndex(x => x.Item1 == baseCourseChapter.Id);
                        if (index != -1)
                        {
                            baseCourseChapterValueTupleList[index].Item2.AddRange(keyValue.Value);
                        }
                    }
                }
                //遍历根章节字段依次将视频和章节练习排入学习任务中
                foreach (var baseCourseChapter in baseCourseChapterValueTupleList)
                {
                    foreach (var courseVideoInfo in baseCourseChapter.Item2)
                    {
                        model.TaskCount += 1;
                        model.TaskTotalDurations += (int)(courseVideoInfo.Duration / 60);
                    }

                    //获取章节练习
                    var ChapterPractice = await remoteCourseRepository.GetCoursePractice(baseCourseChapter.Item1);
                    if (ChapterPractice is null) continue;

                    //整理智辅课程章节练习任务，放入章节练习任务组中（先通过课程id和章节Id）
                    //第一步  通过课程id和章节id获取配置试题章节id和试题分类id
                    var count = await remoteQuestionRepository.GetChaperCategorieQuestionCount(ChapterPractice.ChapterNodeId.Value, ChapterPractice.QuestionCategoryId.Value);
                    if (count == 0) continue;
                    model.TaskCount += 1;
                    model.TaskTotalDurations += count;
                }
            }
            else if (assistantProductPermissionContent.AssistanPermissionType == AssistanPermissionType.Paper)
            {
                //通过试卷id，获取试卷详情
                var paper = await remotePaperRepository.GetPaperDetailInfo(assistantProductPermissionContent.PermissionId);
                if (paper is null) continue;
                model.TaskCount += 1;
                model.TaskTotalDurations += paper.Time;
            }
        }
        return model;

        static BrowseCourseChapterViewModel GetParentCourseChapter(Guid key, List<BrowseCourseChapterViewModel> allCourseChaper)
        {
            var courseChapter = allCourseChaper.FirstOrDefault(x => x.Id == key);
            if (courseChapter != null && courseChapter.ParentId != Guid.Empty)
            {
                return GetParentCourseChapter(courseChapter.ParentId, allCourseChaper);
            }
            return courseChapter;
        }
    }

    #endregion
}

public class StatisticsTaskCountDurationsModel
{
    public int TaskCount { get; set; }

    public int TaskTotalDurations { get; set; }
}