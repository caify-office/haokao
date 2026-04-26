using HaoKao.Common.Enums;
using HaoKao.Common.Events.UserAnswerRecord;
using HaoKao.QuestionService.Application.QuestionHandlers;
using HaoKao.QuestionService.Application.QuestionModule.Interfaces;
using HaoKao.QuestionService.Application.QuestionModule.ViewModels;
using HaoKao.QuestionService.Domain.CacheExtensions;
using HaoKao.QuestionService.Domain.QuestionCollectionModule;
using HaoKao.QuestionService.Domain.QuestionModule;
using HaoKao.QuestionService.Domain.QuestionWrongModule;

namespace HaoKao.QuestionService.Application.QuestionModule.Services;

/// <summary>
/// 试题App接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.App)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class QuestionAppService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    IEventBus eventBus,
    IMapper mapper,
    INotificationHandler<DomainNotification> notifications,
    IQuestionService service,
    IQuestionRepository repository,
    IQuestionCollectionRepository questionCollectionRepository
) : IQuestionAppService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;

    private readonly IQuestionService _service = service ?? throw new ArgumentNullException(nameof(service));
    private readonly IQuestionRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IQuestionCollectionRepository _questionCollectionRepository = questionCollectionRepository ?? throw new ArgumentNullException(nameof(questionCollectionRepository));

    #endregion

    #region 服务方法

    /// <inheritdoc />
    [HttpGet("{id:guid}")]
    public async Task<BrowseQuestionAppViewModel> Get(Guid id)
    {
        var question = await _service.GetByIdAsync(id);
        var model = _mapper.Map<BrowseQuestionAppViewModel>(question);
        model.IsCollected = await _cacheManager.GetAsync(
            QuestionCollectionCacheManager.ListQuery.Create($"CollectionCount:questionId={id}"),
            () => _questionCollectionRepository.IsCollected(id)
        );
        return model;
    }

    /// <inheritdoc />
    [HttpPost]
    public Task<int> GetSubjectQuestionCount([FromBody] QuerySubjectQuestionCountViewModel input)
    {
        var cacheKey = $"{nameof(GetSubjectQuestionCount)}:{JsonSerializer.Serialize(input).ToMd5()}";
        return _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Question>.QueryCacheKey.Create(cacheKey),
            () => _repository.GetSubjectQuestionCount(input.SubjectId, input.QuestionCategoryIds)
        );
    }

    /// <summary>
    /// 按试题章节和试题分类查询试题数量
    /// </summary>
    /// <param name="chapterId"></param>
    /// <param name="questionCategoryId"></param>
    /// <returns></returns>
    [HttpGet("{chapterId:guid}/{questionCategoryId:guid}")]
    public Task<int> GetChaperCategorieQuestionCount(Guid chapterId, Guid questionCategoryId)
    {
        return service.GetChaperCategorieQuestionCount(chapterId, questionCategoryId);
    }

    /// <inheritdoc />
    [HttpGet]
    public async Task<IReadOnlyList<ChapterViewModel>> GetChapterList([FromQuery] QueryChapterViewModel input)
    {
        var cacheKey = $"{nameof(GetChapterList)}:{JsonSerializer.Serialize(input).ToMd5()}";
        var list = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Question>.QueryCacheKey.Create(cacheKey),
            async () =>
            {
                var result = await _repository.GetChapterList(input.SubjectId, input.QuestionCategoryId, input.FreeState);
                return result.Select(x => new ChapterViewModel(x.Id, x.Name, x.Count)).ToList();
            });

        return list.Where(x => x.Count > 0).ToList();
        // var result = await _repository.GetChapterList(input.SubjectId, input.QuestionCategoryId, input.FreeState);
        // return result.Select(x => new ChapterViewModel(x.Id, x.Name, x.Count)).Where(x => x.Count > 0).ToList();
    }

    /// <inheritdoc />
    [HttpGet]
    public async Task<IReadOnlyList<ChapterViewModel>> GetSectionList([FromQuery] QuerySectionViewModel input)
    {
        var cacheKey = $"{nameof(GetChapterList)}:{JsonSerializer.Serialize(input).ToMd5()}";
        var list = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Question>.QueryCacheKey.Create(cacheKey),
            async () =>
            {
                var result = await _repository.GetSectionList(input.ChapterId, input.QuestionCategoryId, input.FreeState);
                return result.Select(x => new ChapterViewModel(x.Id, x.Name, x.Count)).ToList();
            });

        return list.Where(x => x.Count > 0).ToList();
        // var result = await _repository.GetSectionList(input.ChapterId, input.QuestionCategoryId, input.FreeState);
        // return result.Select(x => new ChapterViewModel(x.Id, x.Name, x.Count)).Where(x => x.Count > 0).ToList();
    }

    /// <inheritdoc />
    [HttpGet]
    public async Task<IReadOnlyList<ChapterViewModel>> GetKnowledgePointList([FromQuery] QueryKnowledgePointViewModel input)
    {
        var cacheKey = $"{nameof(GetChapterList)}:{JsonSerializer.Serialize(input).ToMd5()}";
        var list = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Question>.QueryCacheKey.Create(cacheKey),
            async () =>
            {
                var result = await _repository.GetKnowledgePointList(input.SectionId, input.QuestionCategoryId, input.FreeState);
                return result.Select(x => new ChapterViewModel(x.Id, x.Name, x.Count)).ToList();
            });

        return list.Where(x => x.Count > 0).ToList();
        // var result = await _repository.GetKnowledgePointList(input.SectionId, input.QuestionCategoryId, input.FreeState);
        // return result.Select(x => new ChapterViewModel(x.Id, x.Name, x.Count)).Where(x => x.Count > 0).ToList();
    }

    /// <inheritdoc />
    [HttpGet]
    public async Task<Dictionary<Guid, List<QueryQuestionListAppViewModel>>> GetChapterQuestionIdGroup
        ([FromQuery] QueryChapterQuestionViewModel input)
    {
        var questions = await GetChapterQuestionList(input);
        var list = _mapper.Map<List<QueryQuestionListAppViewModel>>(questions);
        return GroupByQuestionType(list);
    }

    /// <inheritdoc />
    [HttpGet]
    public async Task<Dictionary<Guid, List<QueryQuestionListAppViewModel>>> GetSectionQuestionIdGroup
        ([FromQuery] QuerySectionQuestionViewModel input)
    {
        var questions = await GetSectionQuestionList(input);
        var list = _mapper.Map<List<QueryQuestionListAppViewModel>>(questions);
        return GroupByQuestionType(list);
    }

    /// <inheritdoc />
    [HttpGet]
    public async Task<Dictionary<Guid, List<QueryQuestionListAppViewModel>>> GetKnowledgePointQuestionIdGroup
        ([FromQuery] QueryKnowledgePointQuestionViewModel input)
    {
        var questions = await GetKnowledgePointQuestionList(input);
        var list = _mapper.Map<List<QueryQuestionListAppViewModel>>(questions);
        return GroupByQuestionType(list);
    }

    /// <inheritdoc />
    [HttpPost]
    public async Task<Dictionary<Guid, List<QueryQuestionListAppViewModel>>> GetPaperQuestionIdGroup
        ([FromBody] IReadOnlyList<Guid> ids)
    {
        var questions = await GetQuestionListByIds(ids);
        var list = _mapper.Map<List<QueryQuestionListAppViewModel>>(questions);
        return GroupByQuestionType(list);
    }

    /// <inheritdoc />
    [HttpGet]
    public async Task<Dictionary<Guid, List<QueryQuestionListAppViewModel>>> GetSpecialPromotionQuestionIdGroup
        ([FromQuery] QuerySpecialPromotionQuestionViewModel input)
    {
        var ids = await GetSpecialPromotionQuestionIds(input);
        var questions = await GetQuestionListByIds(ids);
        var list = _mapper.Map<List<QueryQuestionListAppViewModel>>(questions);
        return GroupByQuestionType(list);
    }

    /// <inheritdoc />
    [HttpGet]
    public Task<bool> ExistsFreeQuestion(Guid subjectId, Guid chapterId, Guid categoryId)
    {
        return _repository.ExistEntityAsync(x => x.SubjectId == subjectId
                                              && x.ChapterId == chapterId
                                              && x.QuestionCategoryId == categoryId
                                              && x.FreeState == FreeState.Yes);
    }

    #endregion

    #region NonAction

    /// <inheritdoc />
    [NonAction]
    public Dictionary<Guid, List<QueryQuestionListAppViewModel>> GroupByQuestionType
        (IReadOnlyList<QueryQuestionListAppViewModel> list)
    {
        var array = list as QueryQuestionListAppViewModel[] ?? list.ToArray();

        var dict = array.Where(x => x.ParentId == null)
                        .GroupBy(x => x.QuestionTypeId)
                        .ToDictionary(k => k.Key, v => v.OrderByDescending(x => x.FreeState).ToList());

        if (dict.TryGetValue(QuestionType.CaseAnalysis, out var questions))
        {
            dict[QuestionType.CaseAnalysis] = questions.SelectMany(x => array.Where(i => i.ParentId == x.Id).OrderBy(i => i.Sort)).ToList();
        }

        return dict;
    }

    /// <inheritdoc />
    [NonAction]
    public Dictionary<Guid, List<BrowseQuestionAppViewModel>> GroupByQuestionType
        (IReadOnlyList<BrowseQuestionAppViewModel> list)
    {
        var dict = list.Where(x => x.ParentId == null)
                       .GroupBy(x => x.QuestionTypeId)
                       .ToDictionary(k => k.Key, v => v.OrderByDescending(y => y.FreeState).ToList());

        var children = list.Where(x => x.ParentId != null).ToList();
        if (children.Count == 0) return dict;

        // 按父题目Id分组并排序
        var subDict = children.GroupBy(x => x.ParentId!.Value).ToDictionary(k => k.Key, v => v.OrderBy(x => x.Sort).ToList());
        var parentTypeIds = list.Where(x => subDict.ContainsKey(x.Id)).Select(x => x.QuestionTypeId).ToHashSet();
        foreach (var typeId in parentTypeIds)
        {
            // 按顺序组装题目, 示例: [父题A, 子题A1, 子题A2, 父题B, 子题B1, 子题B2]
            var questions = new List<BrowseQuestionAppViewModel>();
            foreach (var parent in dict[typeId])
            {
                questions.Add(parent);
                questions.AddRange(subDict[parent.Id]);
            }
            dict[typeId] = questions;
        }
        return dict;
    }

    /// <inheritdoc />
    [NonAction]
    public async Task<IReadOnlyList<Question>> GetQuestionListByIds(IReadOnlyList<Guid> ids)
    {
        var cacheKey = $"{nameof(GetQuestionListByIds)}:{string.Join(',', ids).ToMd5()}";
        var list = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Question>.QueryCacheKey.Create(cacheKey),
            () => _repository.GetQuestionListByIds(ids)
        );

        // var list = await _repository.GetQuestionListByIds(ids);

        // 按照ids的顺序构造list
        var set = new HashSet<Question>();
        foreach (var id in ids)
        {
            var question = list.FirstOrDefault(x => x.Id == id);
            if (question == null) continue;

            var parents = list.Where(x => x.Id == question.ParentId);
            foreach (var parent in parents)
            {
                set.Add(parent);
            }

            set.Add(question);

            var children = list.Where(x => x.ParentId == question.Id);
            foreach (var child in children)
            {
                set.Add(child);
            }
        }
        return set.ToList();
    }

    /// <inheritdoc />
    [NonAction]
    public Task<IReadOnlyList<Question>> GetChapterQuestionList(QueryChapterQuestionViewModel input)
    {
        var cacheKey = $"{nameof(GetChapterQuestionList)}:{JsonSerializer.Serialize(input).ToMd5()}";
        return _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Question>.QueryCacheKey.Create(cacheKey),
            () => _repository.GetChapterQuestions(
                input.ChapterId,
                input.QuestionCategoryId,
                input.FreeState
            )
        );
    }

    /// <inheritdoc />
    [NonAction]
    public Task<IReadOnlyList<Question>> GetSectionQuestionList(QuerySectionQuestionViewModel input)
    {
        var cacheKey = $"{nameof(GetSectionQuestionList)}:{JsonSerializer.Serialize(input).ToMd5()}";
        return _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Question>.QueryCacheKey.Create(cacheKey),
            () => _repository.GetSectionQuestions(
                input.SectionId,
                input.QuestionCategoryId,
                input.FreeState
            )
        );
    }

    /// <inheritdoc />
    [NonAction]
    public Task<IReadOnlyList<Question>> GetKnowledgePointQuestionList(QueryKnowledgePointQuestionViewModel input)
    {
        var cacheKey = $"{nameof(GetKnowledgePointQuestionList)}:{JsonSerializer.Serialize(input).ToMd5()}";
        return _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Question>.QueryCacheKey.Create(cacheKey),
            () => _repository.GetKnowledgePointQuestions(
                input.KnowledgePointId,
                input.QuestionCategoryId,
                input.FreeState
            )
        );
    }

    /// <inheritdoc />
    [NonAction]
    public Task<IReadOnlyList<Guid>> GetSpecialPromotionQuestionIds(QuerySpecialPromotionQuestionViewModel input)
    {
        return _repository.GetSpecialPromotionQuestionIds(input.SubjectId, input.Ability, input.Count, input.Trial);
    }

    #endregion

    #region 提交作答和判题

    /// <inheritdoc />
    [HttpPost]
    public async Task<SubmitAnswerReturnViewModel> SubmitUserAnswers([FromBody] SubmitAnswersViewModel input)
    {
        if (input.QuestionGroups == null || !input.QuestionGroups.Any())
        {
            throw new GirvsException("没有数据");
        }

        // 判题
        var questions = await ScoringUserAnswerQuestions(input);
        input.QuestionCount = questions.Count;

        if (questions.Count != 0)
        {
            await PublishAfterSubmitQuestionEvent(input, questions);
        }

        // 平铺试题
        foreach (var q in questions)
        {
            var group = input.QuestionGroups.First(x => x.UserAnswerQuestions.Any(y => y.QuestionId == q.QuestionId));
            q.QuestionTypeId = group.QuestionTypeId;
        }

        return new SubmitAnswerReturnViewModel
        {
            RecordIdentifierName = input.RecordIdentifierName,
            ElapsedTime = input.ElapsedTime,
            UserScore = questions.Sum(x => x.ScoringResult.Score),
            PassingScore = input.PassingScore,
            TotalScore = input.TotalScore,
            QuestionCount = input.QuestionCount,
            CorrectCount = input.CorrectCount,
            RecordQuestions = questions.Select(x => new SubmitQuestionViewModel
            {
                QuestionId = x.QuestionId,
                ParentId = x.ParentId,
                UserScore = x.ScoringResult.Score,
                JudgeResult = x.ScoringResult.ScoringType,
                WhetherMark = x.WhetherMark,
                QuestionTypeId = x.QuestionTypeId,
                QuestionOptions = x.UserAnswer.Select(u => new SubmitQuestionOptionViewModel
                {
                    OptionId = u.Id,
                    OptionContent = u.Content,
                }).ToList(),
            }).ToList(),
        };
    }

    /// <summary>
    /// 判题
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private async Task<List<UserAnswerQuestionModel>> ScoringUserAnswerQuestions(SubmitAnswersViewModel input)
    {
        // 查询试题
        var questionIds = input.QuestionGroups.SelectMany(x => x.UserAnswerQuestions)
                               .Select(x => x.QuestionId)
                               .ToList();

        // 获取试题走缓存
        var questionDict = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Question>.ByIdsCacheKey.Create(string.Join(',', questionIds).ToMd5()),
            () => _repository.GetQuestionDictByIds(questionIds)
        );

        if (!questionDict.Any())
        {
            return [];
        }

        var userAnswerQuestions = new List<UserAnswerQuestionModel>(questionDict.Count);

        // 先处理题型
        foreach (var questionGroup in input.QuestionGroups)
        {
            // 获取判题处理程序
            IQuestion questionHandler = null;

            // 案例分析题特殊处理
            if (questionGroup.QuestionTypeId != QuestionType.CaseAnalysis)
            {
                questionHandler = QuestionManager.GetByQuestionId(questionGroup.QuestionTypeId);
            }

            // 处理题型中的每一道题
            foreach (var questionAnswer in questionGroup.UserAnswerQuestions)
            {
                // 获取到相对应的试题
                if (!questionDict.TryGetValue(questionAnswer.QuestionId, out var question))
                {
                    continue; // 序列化试题选项
                }

                // 取小题的类型判分
                questionHandler ??= QuestionManager.GetByQuestionId(question.QuestionTypeId);

                // 设置判分规则
                var scoringRules = SetScoringRules(questionAnswer, questionGroup, questionHandler);

                // 设置试题的选项内容
                questionHandler.SetQuestionOption(question.QuestionOptions);

                // 判断得分
                questionAnswer.ScoringResult = questionHandler.HandleScoring(scoringRules, questionAnswer.UserAnswer.ToArray());

                // 计算题型得分
                questionGroup.Score += questionAnswer.ScoringResult.Score;

                // 添加到该题型答对题的总数
                questionGroup.CorrectTotal += Convert.ToInt32(questionAnswer.ScoringResult.ScoringType == ScoringRuleType.Correct);

                // 如果判题结果不是未作答 用户作答统计累加
                input.AnswerCount += Convert.ToInt32(questionAnswer.ScoringResult.ScoringType != ScoringRuleType.Missing);
                input.CorrectCount += Convert.ToInt32(questionAnswer.ScoringResult.ScoringType == ScoringRuleType.Correct);

                // 平铺试题
                // questionAnswer.QuestionTypeId = questionGroup.QuestionTypeId;

                userAnswerQuestions.Add(questionAnswer);
            }
        }

        return userAnswerQuestions;

        static Dictionary<ScoringRuleType, decimal> SetScoringRules(
            UserAnswerQuestionModel questionAnswer,
            UserAnswerQuestionGroupModel questionGroup,
            IQuestion questionHandler
        )
        {
            // 优先使用单个试题的判分规则
            if (questionAnswer.ScoringRules?.Any() == true)
            {
                return questionAnswer.ScoringRules;
            }

            // 如果不存在则使用题型的判分规则
            if (questionGroup.ScoringRules?.Any() == true)
            {
                return questionGroup.ScoringRules;
            }

            // 如果都不存在则使用判题处理程序的的规则
            return questionHandler.ScoringRules;
        }
    }

    private async Task PublishAfterSubmitQuestionEvent(SubmitAnswersViewModel input, IEnumerable<UserAnswerQuestionModel> questions)
    {
        // 消灭错题
        if (input.AnswerType == SubmitAnswerType.ClearError)
        {
            await PublishCleanQuestionWrong(questions);
        }
        // 今日任务
        else if (input.AnswerType == SubmitAnswerType.TodayTask)
        {
            await PublishAnswerRecord(input, questions);
            await PublishCleanQuestionWrong(questions);
        }
        // 专项提升
        else if (input.AnswerType == SubmitAnswerType.SpecialPromotion)
        {
            // 专项提升中被消灭的错题应进入“已消灭错题”的池子
            await PublishCleanQuestionWrong(questions);
        }
        else if (input.AnswerType == SubmitAnswerType.ProductChapter)
        {
            await PublishProductChapterAnswerRecord(input, questions);
        }
        else if (input.AnswerType == SubmitAnswerType.ProductPaper)
        {
            await PublishProductPaperAnswerRecord(input, questions);
        }
        else if (input.AnswerType == SubmitAnswerType.ProductKnowledge)
        {
            await PublishProductKnowledgeAnswerRecord(input, questions);
        }
        else // 每日一题, 章节, 试卷
        {
            // 发布答题记录事件
            await PublishAnswerRecord(input, questions);
            // 发布错题记事件
            await PublishWrongAnswerRecord(questions);
        }

        await PublishElapsedTimeRecord(input, questions);
    }

    /// <summary>
    /// 发布错题事件
    /// </summary>
    /// <param name="questions"></param>
    private async Task PublishWrongAnswerRecord(IEnumerable<UserAnswerQuestionModel> questions)
    {
        var ids = questions.Where(x => x.ScoringResult.ScoringType != ScoringRuleType.Correct
                                    && x.ScoringResult.ScoringType != ScoringRuleType.Missing
                                    && x.ScoringResult.ScoringType != ScoringRuleType.Subjective)
                           .Select(x => x.QuestionId)
                           .ToList();

        if (ids.Count != 0)
        {
            var command = new CreateQuestionWrongCommand(ids);

            await _bus.SendCommand(command);

            if (_notifications.HasNotifications())
            {
                var errorMessage = _notifications.GetNotificationMessage();
                throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
            }
        }
    }

    /// <summary>
    /// 消灭错题
    /// </summary>
    /// <param name="questions"></param>
    private async Task PublishCleanQuestionWrong(IEnumerable<UserAnswerQuestionModel> questions)
    {
        // 获取回答正确的所有的试题Id
        var ids = questions
                  .Where(x => x.ScoringResult.ScoringType == ScoringRuleType.Correct)
                  .Select(x => x.QuestionId).ToList();
        if (ids.Any())
        {
            var command = new CleanQuestionWrongCommand(ids);

            await _bus.SendCommand(command);

            if (_notifications.HasNotifications())
            {
                var errorMessage = _notifications.GetNotificationMessage();
                throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
            }
        }
    }

    private Task PublishElapsedTimeRecord(SubmitAnswersViewModel vm, IEnumerable<UserAnswerQuestionModel> questions)
    {
        var targetId = vm.AnswerType switch
        {
            SubmitAnswerType.Paper or SubmitAnswerType.ProductPaper => vm.PaperId,
            SubmitAnswerType.Chapter or SubmitAnswerType.ProductChapter => questions.FirstOrDefault()?.ChapterId,
            SubmitAnswerType.KnowledgePoint or SubmitAnswerType.ProductKnowledge => questions.FirstOrDefault()?.KnowledgePointId,
            SubmitAnswerType.Section => questions.FirstOrDefault()?.SectionId,
            _ => Guid.Empty
        };

        var domainEvent = new CreateElapsedTimeRecordEvent(
            vm.SubjectId,
            targetId ?? Guid.Empty,
            vm.ProductId ?? Guid.Empty,
            vm.AnswerType,
            vm.QuestionCount,
            vm.AnswerCount,
            vm.CorrectCount,
            vm.ElapsedTime,
            vm.CreateTime,
            _userId
        );
        return _eventBus.PublishAsync(domainEvent);
    }

    /// <summary>
    /// 发布答题记录事件
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="questions"></param>
    private Task PublishAnswerRecord(SubmitAnswersViewModel vm, IEnumerable<UserAnswerQuestionModel> questions)
    {
        if (vm.AnswerType == SubmitAnswerType.Chapter) return PublishChapterAnswerRecord(vm, questions);
        if (vm.AnswerType == SubmitAnswerType.Paper) return PublishPaperAnswerRecord(vm, questions);
        if (vm.AnswerType is SubmitAnswerType.Daily or SubmitAnswerType.TodayTask) return PublishDateAnswerRecord(vm, questions);
        return Task.CompletedTask;
    }

    private Task PublishChapterAnswerRecord(SubmitAnswersViewModel vm, IEnumerable<UserAnswerQuestionModel> questions)
    {
        var domainEvents = questions.GroupBy(x => new { x.ChapterId, x.SectionId, x.KnowledgePointId })
                                    .Select(g => new CreateChapterAnswerRecordEvent(
                                                vm.SubjectId,
                                                vm.CategoryId,
                                                g.Key.ChapterId,
                                                g.Key.SectionId,
                                                g.Key.KnowledgePointId,
                                                _userId,
                                                vm.CreateTime,
                                                new CreateAnswerRecordEvent(
                                                    g.Count(),
                                                    g.Count(x => x.ScoringResult.ScoringType != ScoringRuleType.Missing),
                                                    g.Count(x => x.ScoringResult.ScoringType == ScoringRuleType.Correct),
                                                    vm.CreateTime,
                                                    _userId,
                                                    vm.AnswerType,
                                                    questions
                                                        .Where(x => x.ChapterId == g.Key.ChapterId
                                                                 && x.SectionId == g.Key.SectionId
                                                                 && x.KnowledgePointId == g.Key.KnowledgePointId)
                                                        .Select(x => new CreateAnswerQuestionEvent(
                                                                    x.QuestionId,
                                                                    x.QuestionTypeId,
                                                                    x.ParentId,
                                                                    x.ScoringResult.ScoringType,
                                                                    x.WhetherMark,
                                                                    x.UserAnswer.Select(a => new CreateUserAnswerEvent(a.Id == Guid.Empty ? a.Content : a.Id.ToString())).ToList()
                                                                )).ToList()))).ToList();

        //发布答题记录事件
        return Task.WhenAll(domainEvents.Select(x => _eventBus.PublishAsync(x)));
    }

    private Task PublishPaperAnswerRecord(SubmitAnswersViewModel vm, IEnumerable<UserAnswerQuestionModel> questions)
    {
        var domainEvent = new CreatePaperAnswerRecordEvent(
            vm.SubjectId,
            vm.CategoryId,
            vm.PaperId.Value,
            _userId,
            vm.CreateTime,
            questions.Sum(q => q.ScoringResult.Score),
            vm.PassingScore,
            vm.TotalScore,
            vm.ElapsedTime,
            new CreateAnswerRecordEvent(
                vm.QuestionCount,
                vm.AnswerCount,
                vm.CorrectCount,
                vm.CreateTime,
                _userId,
                vm.AnswerType,
                questions.Select(x => new CreateAnswerQuestionEvent(
                                     x.QuestionId,
                                     x.QuestionTypeId,
                                     x.ParentId,
                                     x.ScoringResult.ScoringType,
                                     x.WhetherMark,
                                     x.UserAnswer.Select(a => new CreateUserAnswerEvent(a.Id == Guid.Empty ? a.Content : a.Id.ToString())).ToList()
                                 )).ToList()));
        return _eventBus.PublishAsync(domainEvent);
    }

    private Task PublishDateAnswerRecord(SubmitAnswersViewModel vm, IEnumerable<UserAnswerQuestionModel> questions)
    {
        var domainEvent = new CreateDateAnswerRecordEvent(
            vm.SubjectId,
            _userId,
            vm.CreateTime,
            DateOnly.FromDateTime(vm.CreateTime),
            vm.AnswerType,
            new CreateAnswerRecordEvent(
                vm.QuestionCount,
                vm.AnswerCount,
                vm.CorrectCount,
                vm.CreateTime,
                _userId,
                vm.AnswerType,
                questions.Select(x => new CreateAnswerQuestionEvent(
                                     x.QuestionId,
                                     x.QuestionTypeId,
                                     x.ParentId,
                                     x.ScoringResult.ScoringType,
                                     x.WhetherMark,
                                     x.UserAnswer.Select(a => new CreateUserAnswerEvent(a.Id == Guid.Empty ? a.Content : a.Id.ToString())).ToList()
                                 )).ToList()));
        return _eventBus.PublishAsync(domainEvent);
    }

    private Task PublishProductChapterAnswerRecord(SubmitAnswersViewModel vm, IEnumerable<UserAnswerQuestionModel> questions)
    {
        var q = questions.First();
        var domainEvent = new CreateProductChapterAnswerRecordEvent(
            vm.ProductId.Value,
            vm.SubjectId,
            q.ChapterId,
            q.SectionId,
            q.KnowledgePointId,
            _userId,
            vm.CreateTime,
            new CreateAnswerRecordEvent(
                vm.QuestionCount,
                vm.AnswerCount,
                vm.CorrectCount,
                vm.CreateTime,
                _userId,
                vm.AnswerType,
                questions.Select(x => new CreateAnswerQuestionEvent(
                                     x.QuestionId,
                                     x.QuestionTypeId,
                                     x.ParentId,
                                     x.ScoringResult.ScoringType,
                                     x.WhetherMark,
                                     x.UserAnswer.Select(a => new CreateUserAnswerEvent(a.Id == Guid.Empty ? a.Content : a.Id.ToString())).ToList()
                                 )).ToList()));
        return _eventBus.PublishAsync(domainEvent);
    }

    private Task PublishProductPaperAnswerRecord(SubmitAnswersViewModel vm, IEnumerable<UserAnswerQuestionModel> questions)
    {
        var domainEvent = new CreateProductPaperAnswerRecordEvent(
            vm.ProductId.Value,
            vm.SubjectId,
            vm.PaperId.Value,
            questions.Sum(q => q.ScoringResult.Score),
            vm.PassingScore,
            vm.TotalScore,
            _userId,
            vm.CreateTime,
            new CreateAnswerRecordEvent(
                vm.QuestionCount,
                vm.AnswerCount,
                vm.CorrectCount,
                vm.CreateTime,
                _userId,
                vm.AnswerType,
                questions.Select(x => new CreateAnswerQuestionEvent(
                                     x.QuestionId,
                                     x.QuestionTypeId,
                                     x.ParentId,
                                     x.ScoringResult.ScoringType,
                                     x.WhetherMark,
                                     x.UserAnswer.Select(a => new CreateUserAnswerEvent(a.Id == Guid.Empty ? a.Content : a.Id.ToString())).ToList()
                                 )).ToList()));
        return _eventBus.PublishAsync(domainEvent);
    }

    private Task PublishProductKnowledgeAnswerRecord(SubmitAnswersViewModel vm, IEnumerable<UserAnswerQuestionModel> questions)
    {
        var domainEvents = questions.GroupBy(x => new { x.ChapterId, x.SectionId, x.KnowledgePointId })
                                    .Select(g => new CreateProductKnowledgeAnswerRecordEvent(
                                                vm.ProductId.Value,
                                                vm.SubjectId,
                                                g.Key.ChapterId,
                                                g.Key.SectionId,
                                                g.Key.KnowledgePointId,
                                                _userId,
                                                vm.CreateTime,
                                                vm.ExamFrequency.Value,
                                                new CreateAnswerRecordEvent(
                                                    g.Count(),
                                                    g.Count(x => x.ScoringResult.ScoringType != ScoringRuleType.Missing),
                                                    g.Count(x => x.ScoringResult.ScoringType == ScoringRuleType.Correct),
                                                    vm.CreateTime,
                                                    _userId,
                                                    vm.AnswerType,
                                                    questions
                                                        .Where(x => x.ChapterId == g.Key.ChapterId
                                                                 && x.SectionId == g.Key.SectionId
                                                                 && x.KnowledgePointId == g.Key.KnowledgePointId)
                                                        .Select(x => new CreateAnswerQuestionEvent(
                                                                    x.QuestionId,
                                                                    x.QuestionTypeId,
                                                                    x.ParentId,
                                                                    x.ScoringResult.ScoringType,
                                                                    x.WhetherMark,
                                                                    x.UserAnswer.Select(a => new CreateUserAnswerEvent(a.Id == Guid.Empty ? a.Content : a.Id.ToString())).ToList()
                                                                )).ToList()))).ToList();

        //发布答题记录事件
        return Task.WhenAll(domainEvents.Select(x => _eventBus.PublishAsync(x)));
    }

    private readonly Guid _userId = EngineContext.Current.IsAuthenticated
        ? EngineContext.Current.ClaimManager.GetUserId().To<Guid>()
        : Guid.Empty;

    #endregion
}