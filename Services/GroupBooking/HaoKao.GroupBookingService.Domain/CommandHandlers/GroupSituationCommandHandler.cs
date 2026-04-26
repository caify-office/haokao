using Girvs.Infrastructure;
using HaoKao.Common.Extensions;
using HaoKao.GroupBookingService.Domain.Commands.GroupSituation;
using HaoKao.GroupBookingService.Domain.Entities;
using HaoKao.GroupBookingService.Domain.Extensions;
using HaoKao.GroupBookingService.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HaoKao.GroupBookingService.Domain.CommandHandlers;

public class GroupSituationCommandHandler(
    IUnitOfWork<GroupData> uow,
    IGroupSituationRepository repository,
    IGroupMemberRepository groupMemberRepository,
    IMediatorHandler bus,
    ILogger<GroupSituationCommandHandler> logger
) : CommandHandler(uow, bus),
    IRequestHandler<CreateGroupSituationCommand, Guid>,
    IRequestHandler<JoinGroupSituationCommand, bool>
{
    private readonly IGroupSituationRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IGroupMemberRepository _groupMemberRepository = groupMemberRepository ?? throw new ArgumentNullException(nameof(groupMemberRepository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly ILogger<GroupSituationCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// 发起拼团
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Guid> Handle(CreateGroupSituationCommand request, CancellationToken cancellationToken)
    {
        var userId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        var isGroupSuccess = await _groupMemberRepository.IsGroupSuccess(request.GroupDataId, userId);
        if (isGroupSuccess)
        {
            await _bus.RaiseEvent(
                new DomainNotification("", "您已拼团成功，请勿重复发起团！", StatusCodes.Status400BadRequest),
                cancellationToken);
            return Guid.Empty;
        }
        var isInGroup = await _groupMemberRepository.IsInGroup(request.GroupDataId, userId);
        if (isInGroup)
        {
            await _bus.RaiseEvent(
                new DomainNotification("", "拼团进行中，请勿重复操作", StatusCodes.Status400BadRequest),
                cancellationToken);
            return Guid.Empty;
        }

        var groupSituation = new GroupSituation
        {
            GroupDataId = request.GroupDataId,
            DataName = request.DataName,
            SuitableSubjects = request.SuitableSubjects,
            PeopleNumber = request.PeopleNumber,
            LimitTime = request.LimitTime,
            Document = request.Document,
            CreateTime = DateTime.Now,
            GroupMembers =
            [
                new()
                {
                    GroupDataId = request.GroupDataId,
                    Name = request.Name,
                    ImageUrl = request.ImageUrl,
                    IsLeader = true,
                    ExpirationTime = DateTime.Now.AddHours(request.LimitTime),
                },
            ],
        };

        await _repository.AddAsync(groupSituation);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(groupSituation, groupSituation.Id.ToString(), cancellationToken);
        }

        return groupSituation.Id;
    }

    /// <summary>
    /// 加入拼团
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> Handle(JoinGroupSituationCommand request, CancellationToken cancellationToken)
    {
        var userId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        var groupSituation = await _repository.GetByIdIncludeAllAsync(request.GroupSituationId);
        var isGroupSuccess = await _groupMemberRepository.IsGroupSuccess(request.GroupDataId, userId);
        if (isGroupSuccess)
        {
            await _bus.RaiseEvent(
                new DomainNotification("", "已成功领取过该资料，请勿重复拼团", StatusCodes.Status400BadRequest),
                cancellationToken);
            return false;
        }
        var isInGroup = await _groupMemberRepository.IsInGroup(request.GroupDataId, userId);
        if (isInGroup)
        {
            await _bus.RaiseEvent(
                new DomainNotification("", "当前资料正在拼团中，请勿重复拼团", StatusCodes.Status400BadRequest),
                cancellationToken);
            return false;
        }
        if (groupSituation == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification("", "未找到对应的拼团数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        if (groupSituation.GroupMembers.Any(t => t.CreatorId == userId))
        {
            await _bus.RaiseEvent(
                new DomainNotification("", "不能重复拼团", StatusCodes.Status400BadRequest),
                cancellationToken);
            return false;
        }

        if (DateTime.Now > groupSituation.CreateTime.AddHours(groupSituation.LimitTime)) //判断拼团时间是否失效
        {
            await _bus.RaiseEvent(
                new DomainNotification("", "拼团时间已过期", StatusCodes.Status400BadRequest),
                cancellationToken);
            return false;
        }

        if (groupSituation.PeopleNumber == groupSituation.GroupMembers.Count) //拼团人数等于团成员数量判断
        {
            await _bus.RaiseEvent(
                new DomainNotification("", "该团当前状态已满员", StatusCodes.Status400BadRequest),
                cancellationToken);
            return false;
        }

        var groupMember = new GroupMember
        {
            GroupDataId = request.GroupDataId,
            GroupSituationId = request.GroupSituationId,
            Name = request.Name,
            ImageUrl = request.ImageUrl,
            IsLeader = false,
            ExpirationTime = groupSituation.CreateTime.AddHours(groupSituation.LimitTime),
        };

        groupSituation.GroupMembers.Add(groupMember);

        //当前拼团人员是最后一个名额时更新拼团成功时间
        if (groupSituation.GroupMembers.Count == groupSituation.PeopleNumber)
        {
            groupSituation.SuccessTime = DateTime.Now;
            groupSituation.GroupMembers.ForEach(x => { x.SuccessTime = DateTime.Now; });

            _logger.LogInformation("拼团成功Id=>{GroupSituationId}", groupSituation.Id);
        }

        await _repository.UpdateAsync(groupSituation);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(groupSituation, groupSituation.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<GroupSituation>(cancellationToken);

            //删除我的资料缓存
            foreach (var x in groupSituation.GroupMembers)
            {
                var cacheKey = GroupDataCacheManager.MyGroupDataCacheKey.Create(x.CreatorId.ToString());
                _logger.LogInformation("cacheKey.Key=>{CacheKeyKey}", cacheKey.Key);
                await _bus.RaiseEvent(new RemoveCacheEvent(cacheKey), cancellationToken);
            }

            //删除我的成功拼团缓存
            foreach (var x in groupSituation.GroupMembers)
            {
                var cacheKey = GroupSituationCacheManager.MySuccessGroupSituationCacheKey.Create(x.CreatorId.ToString());
                _logger.LogInformation("cacheKey.Key=>{CacheKeyKey}", cacheKey.Key);
                await _bus.RaiseEvent(new RemoveCacheEvent(cacheKey), cancellationToken);
            }
        }

        return true;
    }
}