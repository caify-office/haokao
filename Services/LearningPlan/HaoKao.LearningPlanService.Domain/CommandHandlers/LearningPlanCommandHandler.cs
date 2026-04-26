using AutoMapper;
using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common.Extensions;
using HaoKao.Common.RemoteModel;
using HaoKao.LearningPlanService.Domain.Commands.LearningPlan;
using HaoKao.LearningPlanService.Domain.Enumerations;
using HaoKao.LearningPlanService.Domain.Records;
using HaoKao.LearningPlanService.Domain.RemoteRepositories;
using System.Linq;
using System.Text.Json;

namespace HaoKao.LearningPlanService.Domain.CommandHandlers;

public class LearningPlanCommandHandler(
    IRemoteCourseRepository remoteCourseRepository,
    IRemotePaperRepository remotePaperRepository,
    IRemoteQuestionRepository remoteQuestionRepository,
    IUnitOfWork<LearningPlan> uow,
    ILearningPlanRepository repository,
    IMapper mapper,
    IMediatorHandler bus
) : CommandHandler(uow, bus),
    IRequestHandler<CreateLearningPlanCommand, bool>,
    IRequestHandler<DeleteLearningPlanCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateLearningPlanCommand request, CancellationToken cancellationToken)
    {
        //计划学习该视频的时间点()
        var scheduledTime = DateOnly.FromDateTime(DateTime.Now);
        //判定是否执行新增逻辑
        var needAdd = false;
        //暂存任务标识，任务是否已排期的判定依据
        string taskIdentity;
        //任务排序字段
        var sort = 0;
        //当前天已安排任务时长(分钟)
        var arrangeTaskDuration = 0;
        //已存在任务计划的主键id
        Guid? id = null;

        LearningPlan learningPlan = null;
        if (request.Id.HasValue)
        {
            learningPlan = await repository.GetIncludeByIdAsync(request.Id.Value);
        }

        if (learningPlan is null)
        {
            var userId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
            learningPlan = await repository.GetIncludeByCreatorIdAsync(userId, request.SubjectId, request.ProductId);
            id = learningPlan?.Id;
        }

        if (learningPlan is null)
        {
            learningPlan = mapper.Map<LearningPlan>(request);
            needAdd = true;
        }
        else
        {
            learningPlan = mapper.Map(request, learningPlan);
            if (id != null) learningPlan.Id = id.Value;
            //从明日开始排期
            scheduledTime = scheduledTime.AddDays(1);
        }
        //保留以前和今天的任务排期
        learningPlan.LearningTasks = learningPlan.LearningTasks.Where(x => x.ScheduledTime < scheduledTime).ToList();
        if (learningPlan.LearningTasks.Any())
        {
            sort = learningPlan.LearningTasks.Max(x => x.Sort);
        }

        var learningTaskHashSet = new HashSet<string>(learningPlan.LearningTasks.Select(x => x.TaskIdentity));

        //将知识点任务整理成学习任务
        foreach (var knowledgePointTask in request.KnowledgePointTasks)
        {
            foreach (var videoInfo in knowledgePointTask.VideoInfoViewModels)
            {
                if (videoInfo.VideoDurationSeconds == 0) continue;
                taskIdentity = $"{knowledgePointTask.CourseId}_{videoInfo.KnowledgePointId}".ToMd5();
                //通过CourseId ChapterId 检查当前任务是否已存在于过期任务中，如果存在就过滤掉 continue;
                if (learningTaskHashSet.Contains(taskIdentity)) continue;

                CalculateSchedulingTime(request, ref scheduledTime, ref arrangeTaskDuration, videoInfo.VideoDurationSeconds / 60);
                //整理成视频任务，放入任务数组中
                sort = OrganizeVideoTask(
                    learningPlan
                  , scheduledTime
                  , taskIdentity
                  , sort
                  , knowledgePointTask.CourseId
                  , knowledgePointTask.CourseName
                  , knowledgePointTask.ChapterId
                  , knowledgePointTask.ChapterName
                  , videoInfo.KnowledgePointId
                  , videoInfo.KnowledgePointName
                  , videoInfo.KnowledgePointExamFrequency
                  , videoInfo.VideoName
                  , videoInfo.VideoDurationSeconds
                  , videoInfo.VideoId
                  , CourseType.IntelligenceCourse
                );
            }
            //通过CourseId ChapterId 检查当前任务是否已存在于过期任务中，如果存在就过滤掉
            taskIdentity = $"{knowledgePointTask.CourseId}_{knowledgePointTask.ChapterId}".ToMd5();
            if (learningTaskHashSet.Contains(taskIdentity)) continue;
            //整理智辅课程章节练习任务，放入章节练习任务组中（先通过课程id和章节Id）
            var chapterPractice = await remoteCourseRepository.GetChapterPractice(knowledgePointTask.CourseId, knowledgePointTask.ChapterId);
            if (chapterPractice?.ChapterNodeId == null || !chapterPractice.QuestionCategoryId.HasValue) continue;
            //第一步  通过课程id和章节id获取配置试题章节id和试题分类id
            var count = await remoteQuestionRepository.GetChapterCategorieQuestionCount(chapterPractice.ChapterNodeId.Value, chapterPractice.QuestionCategoryId.Value);
            if (count == 0) continue;
            CalculateSchedulingTime(request, ref scheduledTime, ref arrangeTaskDuration, count);
            //整理成章节练习任务，放入任务数组中
            sort = OrganizeExerciseTask(
                learningPlan
              , scheduledTime
              , taskIdentity
              , sort
              , chapterPractice.CourseId
              , chapterPractice.CourseChapterId
              , chapterPractice.CourseChapterName
              , chapterPractice.ChapterNodeId.Value
              , chapterPractice.ChapterNodeName
              , chapterPractice.QuestionCategoryId.Value
              , chapterPractice.QuestionCategoryName
              , count
              , CourseType.IntelligenceCourse
            );
        }

        //将阶段任务转为具体的学习任务并排期
        foreach (var assistantProductPermissionContent in request.StateTasks)
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
                    var resultGroup = result.GroupBy(x => x.CourseChapterId).Select(g => new KeyValuePair<Guid, List<CourseVideoQueryListInfo>>(g.Key, g.ToList()))
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
                        taskIdentity = $"{courseVideoInfo.CourseChapterId}_{courseVideoInfo.VideoId}".ToMd5();
                        //通过ChapterId VideoId(同一课程章节下有多个视频) 检查当前任务是否已存在于过期任务中，如果存在就过滤掉
                        if (learningTaskHashSet.Contains(taskIdentity)) continue;

                        CalculateSchedulingTime(request, ref scheduledTime, ref arrangeTaskDuration, courseVideoInfo.Duration / 60);
                        //整理成视频任务，放入任务数组中
                        sort = OrganizeVideoTask(
                            learningPlan
                          , scheduledTime
                          , taskIdentity
                          , sort
                          , assistantProductPermissionContent.PermissionId
                          , assistantProductPermissionContent.PermissionName
                          , courseVideoInfo.CourseChapterId
                          , courseVideoInfo.CourseChapterName
                          , Guid.Empty
                          , string.Empty
                          , default
                          , courseVideoInfo.DisplayName
                          , courseVideoInfo.Duration
                          , courseVideoInfo.Id.ToString()
                          , CourseType.StageCourse
                        );
                    }

                    taskIdentity = baseCourseChapter.Item1.ToString().ToMd5();
                    if (learningTaskHashSet.Contains(taskIdentity)) continue;
                    //获取章节练习
                    var ChapterPractice = await remoteCourseRepository.GetCoursePractice(baseCourseChapter.Item1);
                    if (ChapterPractice is null) continue;

                    //整理智辅课程章节练习任务，放入章节练习任务组中（先通过课程id和章节Id）
                    //第一步  通过课程id和章节id获取配置试题章节id和试题分类id
                    var count = await remoteQuestionRepository.GetChapterCategorieQuestionCount(ChapterPractice.ChapterNodeId.Value, ChapterPractice.QuestionCategoryId.Value);
                    if (count == 0) continue;
                    CalculateSchedulingTime(request, ref scheduledTime, ref arrangeTaskDuration, count);
                    //整理成章节练习任务，放入任务数组中
                    sort = OrganizeExerciseTask(
                        learningPlan
                      , scheduledTime
                      , taskIdentity
                      , sort
                      , assistantProductPermissionContent.PermissionId
                      , ChapterPractice.CourseChapterId
                      , ChapterPractice.CourseChapterName
                      , ChapterPractice.ChapterNodeId.Value
                      , ChapterPractice.ChapterNodeName
                      , ChapterPractice.QuestionCategoryId.Value
                      , ChapterPractice.QuestionCategoryName
                      , count
                      , CourseType.StageCourse
                    );
                }
            }
            else if (assistantProductPermissionContent.AssistanPermissionType == AssistanPermissionType.Paper)
            {
                //通过PaperId检查当前任务是否已存在于过期任务中，如果存在就过滤掉
                taskIdentity = assistantProductPermissionContent.PermissionId.ToString().ToMd5();
                if (learningTaskHashSet.Contains(taskIdentity)) continue;
                //通过试卷id，获取试卷详情
                var paper = await remotePaperRepository.GetPaperDetailInfo(assistantProductPermissionContent.PermissionId);
                if (paper is null) continue;
                CalculateSchedulingTime(
                    request
                  , ref scheduledTime
                  , ref arrangeTaskDuration
                  , paper.Time
                );
                //整理成试卷任务，放入任务数组中
                sort = OrganizePaperTask(
                    learningPlan
                  , scheduledTime
                  , taskIdentity
                  , sort
                  , paper.Id
                  , paper.Name
                  , paper.Time
                );
            }
        }


        if (needAdd)
        {
            await repository.AddAsync(learningPlan);
        }
        else
        {
            await repository.UpdateAsync(learningPlan);
        }

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(learningPlan, learningPlan.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<LearningPlan>(cancellationToken);
        }

        return true;

        static void CalculateSchedulingTime(
            CreateLearningPlanCommand request
          , ref DateOnly ScheduledTime
          , ref int arrangeTaskDuration
          , decimal durationMinute)
        {
            if (ScheduledTime > request.EndDate) return;
            var index = (int)ScheduledTime.DayOfWeek;
            //判定规则，如果当前任务时长一半与已安排任务相加大于当天计划安排时长，则往后一天排期
            if (arrangeTaskDuration + durationMinute / 2 >= request.DayLearningTimes[index])
            {
                //当前天已经不能排进任务了，需要往后排一天
                ScheduledTime = ScheduledTime.AddDays(1);
                arrangeTaskDuration = 0;
            }
            else
            {
                arrangeTaskDuration += (int)durationMinute;
                return;
            }

            CalculateSchedulingTime(request, ref ScheduledTime, ref arrangeTaskDuration, durationMinute);
        }

        static int OrganizeVideoTask(
            LearningPlan learningPlan
          , DateOnly ScheduledTime
          , string taskIdentity
          , int sort
          , Guid courseId
          , string courseName
          , Guid chapterId
          , string chapterName
          , Guid knowledgePointId
          , string knowledgePointName
          , ExamFrequency knowledgePointExamFrequency
          , string videoName
          , decimal videoDurationSeconds
          , string videoId
          , CourseType courseType)
        {
            var videoTask = new LearningTask
            {
                TaskName = courseType == CourseType.IntelligenceCourse ? knowledgePointName : videoName,
                ScheduledTime = ScheduledTime,
                DurationSeconds = videoDurationSeconds,
                TaskType = TaskType.VideoTask,
                TaskIdentity = taskIdentity,
                Sort = ++sort,
                TaskContent = JsonSerializer.Serialize(new VideoTaskContent
                {
                    CourseId = courseId,
                    CourseName = courseName,
                    CourseType = courseType,
                    ChapterId = chapterId,
                    ChapterName = chapterName,
                    KnowledgePointId = knowledgePointId,
                    KnowledgePointName = knowledgePointName,
                    KnowledgePointExamFrequency = knowledgePointExamFrequency,
                    VideoId = videoId,
                    VideoName = videoName
                })
            };
            learningPlan.LearningTasks.Add(videoTask);
            return sort;
        }

        static int OrganizeExerciseTask(
            LearningPlan learningPlan
          , DateOnly ScheduledTime
          , string taskIdentity
          , int sort
          , Guid courseId
          , Guid courseChapterId
          , string courseChapterName
          , Guid questionChapterId
          , string questionChapterName
          , Guid questionCategoryId
          , string questionCategoryName
          , int count
          , CourseType courseType)
        {
            var task = new LearningTask
            {
                TaskName = courseChapterName,
                ScheduledTime = ScheduledTime,
                DurationSeconds = count * 60,
                TaskType = TaskType.ExerciseTask,
                TaskIdentity = taskIdentity,
                Sort = ++sort,
                TaskContent = JsonSerializer.Serialize(new ExerciseTaskContent
                {
                    CourseId = courseId,
                    CourseType = courseType,
                    ChapterId = courseChapterId,
                    ChapterName = courseChapterName,
                    QuestionChapterId = questionChapterId,
                    QuestionChapterName = questionChapterName,
                    QuestionCategoryId = questionCategoryId,
                    QuestionCategoryName = questionCategoryName,
                    QuestionCount = count,
                })
            };
            learningPlan.LearningTasks.Add(task);
            return sort;
        }


        static int OrganizePaperTask(
            LearningPlan learningPlan
          , DateOnly scheduledTime
          , string taskIdentity
          , int sort
          , Guid paperId
          , string paperName
          , decimal paperTime)
        {
            var videoTask = new LearningTask
            {
                TaskName = paperName,
                ScheduledTime = scheduledTime,
                DurationSeconds = paperTime * 60,
                TaskType = TaskType.PaperTask,
                TaskIdentity = taskIdentity,
                Sort = ++sort,
                TaskContent = JsonSerializer.Serialize(new PaperTaskContent
                {
                    PaperId = paperId,
                    PaperName = paperName
                })
            };
            learningPlan.LearningTasks.Add(videoTask);
            return sort;
        }

        static BrowseCourseChapterViewModel GetParentCourseChapter(
            Guid key
          , List<BrowseCourseChapterViewModel> allCourseChapter)
        {
            var courseChapter = allCourseChapter.FirstOrDefault(x => x.Id == key);
            if (courseChapter != null && courseChapter.ParentId != Guid.Empty)
            {
                return GetParentCourseChapter(courseChapter.ParentId, allCourseChapter);
            }
            return courseChapter;
        }
    }

    public async Task<bool> Handle(DeleteLearningPlanCommand request, CancellationToken cancellationToken)
    {
        var learningPlan = await repository.GetByIdAsync(request.Id);
        if (learningPlan == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应学习计划主类，用于组织和管理一系列学习任务的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await repository.DeleteAsync(learningPlan);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<LearningPlan>(learningPlan.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<LearningPlan>(cancellationToken);
        }

        return true;
    }
}