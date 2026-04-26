using Girvs.AuthorizePermission.Enumerations;
using Girvs.Infrastructure;
using HaoKao.AnsweringQuestionService.Application.ViewModels;
using HaoKao.AnsweringQuestionService.Application.ViewModels.AnsweringQuestion;
using HaoKao.AnsweringQuestionService.Domain.Commands.AnsweringQuestion;
using HaoKao.AnsweringQuestionService.Domain.Commands.AnsweringQuestionReply;
using HaoKao.AnsweringQuestionService.Domain.Entities;
using HaoKao.AnsweringQuestionService.Domain.Queries.EntityQuery;
using HaoKao.AnsweringQuestionService.Domain.Repositories;
using HaoKao.Common;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace HaoKao.AnsweringQuestionService.Application.AppService.ManageMent;

/// <summary>
/// 答疑接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "答疑接口服务",
    "da8ec485-ba90-4fa4-a3f0-16e725a3cba6",
    "512",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class AnsweringQuestionService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IAnsweringQuestionRepository repository,
    IAnsweringQuestionReplyRepository replyrepository
) : IAnsweringQuestionService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IAnsweringQuestionRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IAnsweringQuestionReplyRepository _replyrepository = replyrepository ?? throw new ArgumentNullException(nameof(replyrepository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseAnsweringQuestionViewModel> Get(Guid id)
    {
        var answeringQuestion = await _repository.GetByIdAsync(id)
                             ?? throw new GirvsException("对应的答疑不存在", StatusCodes.Status404NotFound);
        return answeringQuestion.MapToDto<BrowseAnsweringQuestionViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<AnsweringQuestionQueryViewModel> Get([FromQuery] AnsweringQuestionQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<AnsweringQuestionQuery>();
        await _repository.GetByQueryAsync(query);
        var result = query.MapToQueryDto<AnsweringQuestionQueryViewModel, AnsweringQuestion>();
        result.Result.ForEach(x =>
        {
            var replyInfo = _replyrepository.GetAnsweringQuestionReplyList(x.Id).Result.FirstOrDefault();
            if (replyInfo != null)
            {
                x.ReplyDateTime = replyInfo.CreateTime;
                x.ReplyUserName = replyInfo.ReplyUserName;
                //1:判断老师是否回复了学员的追问，
                var result = _repository.GetChildQuestion(x.Id);
                if (result.Result?.Id != null)
                {
                    // 2:没有回复 显示有   回复了显示无,
                    var replycount = _replyrepository.GetAnsweringQuestionReplyCount(result.Result.Id);
                    x.IsNewRepley = replycount.Result > 0;
                }
            }
        });
        return result;
    }

    /// <summary>
    /// 根据主键删除指定答疑回复
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteAnsweringQuestionCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}