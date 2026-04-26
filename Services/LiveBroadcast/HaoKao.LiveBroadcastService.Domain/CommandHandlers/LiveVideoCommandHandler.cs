using AutoMapper;
using Girvs.Configuration;
using Girvs.Infrastructure;
using HaoKao.Common.Extensions;
using HaoKao.LiveBroadcastService.Domain.Config;
using HaoKao.LiveBroadcastService.Domain.Entities;
using HaoKao.LiveBroadcastService.Domain.Enums;
using HaoKao.LiveBroadcastService.Domain.Extensions;
using System.Linq;
using System.Web;

namespace HaoKao.LiveBroadcastService.Domain.CommandHandlers;

public class LiveVideoCommandHandler(
    IUnitOfWork<LiveVideo> uow,
    ILiveVideoRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateLiveVideoCommand, bool>,
    IRequestHandler<UpdateLiveVideoCommand, bool>,
    IRequestHandler<SetLiveVideoStatusCommand, bool>,
    IRequestHandler<DeleteLiveVideoCommand, bool>
{
    private readonly ILiveVideoRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateLiveVideoCommand request, CancellationToken cancellationToken)
    {
        var liveVideo = _mapper.Map<LiveVideo>(request);
        liveVideo.SubjectIdsStr = string.Join("_", liveVideo.SubjectIds);
        await _repository.AddAsync(liveVideo);

        var appName = "AppNewHaoKao";
        var streamName = request.SubjectNames.FirstOrDefault()?.GetChinesePinyinFirstLetters() ?? "default";
        var expireTime = 9999 * 60;
        //过滤url不支持的特殊字符
        streamName = streamName.SanitizeForUrl();
        //生成推流地址和播流地址
        liveVideo.StreamingAddress =string.IsNullOrEmpty(request.StreamingAddress)?CreatePushUrl(new CreateLiveVideoUrlViewModel
        {
            appName = appName,
            streamName = streamName,
            expireTime = expireTime,
            LiveVideoId = liveVideo.Id,
        }):request.StreamingAddress;
        liveVideo.LiveAddress = CreatePullUrl(request.LiveAddress, appName, streamName, expireTime);
        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<LiveVideo>.ByIdCacheKey.Create(liveVideo.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(liveVideo, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<LiveVideo>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateLiveVideoCommand request, CancellationToken cancellationToken)
    {
        var liveVideo = await _repository.GetByIdAsync(request.Id);
        if (liveVideo == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应视频直播的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }

        liveVideo = _mapper.Map(request, liveVideo);
        liveVideo.SubjectIdsStr = string.Join("_", liveVideo.SubjectIds);

        var appName = "AppNewHaoKao";
        var streamName = request.SubjectNames.FirstOrDefault()?.GetChinesePinyinFirstLetters() ?? "default";
        var expireTime = 9999 * 60;
        //过滤url不支持的特殊字符
        streamName = streamName.SanitizeForUrl();
        //生成推流地址和播流地址
        liveVideo.StreamingAddress = string.IsNullOrEmpty(request.StreamingAddress) ? CreatePushUrl(new CreateLiveVideoUrlViewModel
        {
            appName = appName,
            streamName = streamName,
            expireTime = expireTime,
            LiveVideoId = liveVideo.Id,
        }) : request.StreamingAddress;
        liveVideo.LiveAddress = CreatePullUrl(request.LiveAddress,appName, streamName, expireTime);
        await _repository.UpdateAsync(liveVideo);
        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<LiveVideo>.ByIdCacheKey.Create(liveVideo.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(liveVideo, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<LiveVideo>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(SetLiveVideoStatusCommand request, CancellationToken cancellationToken)
    {
        var liveVideo = await _repository.GetByIdAsync(request.Id);
        if (liveVideo == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应视频直播的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }

        liveVideo = _mapper.Map(request, liveVideo);
        await _repository.UpdateAsync(liveVideo);
        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<LiveVideo>.ByIdCacheKey.Create(liveVideo.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(liveVideo, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<LiveVideo>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteLiveVideoCommand request, CancellationToken cancellationToken)
    {
        var liveVideo = await _repository.GetByIdAsync(request.Id);
        if (liveVideo == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应视频直播的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(liveVideo);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<LiveVideo>.ByIdCacheKey.Create(liveVideo.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
           await _bus.RemoveListCacheEvent<LiveVideo>(cancellationToken);
        }

        return true;
    }

    /// <summary>
    /// 生成播流地址
    /// </summary>
    /// <param name="requestDic"></param>
    /// <param name="appName">app名称</param>
    /// <param name="streamName">流名称</param>
    /// <param name="expireTime">过期时间（单位是秒）</param>
    public Dictionary<string, string> CreatePullUrl(Dictionary<string, string> requestDic, string appName, string streamName, long expireTime)
    {
        var aliLiveConfig = Singleton<AppSettings>.Instance.ModuleConfigurations[nameof(AliLiveConfig)] as
            AliLiveConfig;

        // 获取当前时间
        var currentTime = DateTime.Now;
        // 计算自午夜以来的秒数
        var totalSeconds = (long)(currentTime - DateTime.MinValue).TotalSeconds;
        var timeStamp = totalSeconds + expireTime;

        //小程序：RTMP 
        var rtmpToMd5 = $"/{appName}/{streamName}-{timeStamp}-0-0-{aliLiveConfig.PullKey}";
        var rtmpAuthKey = rtmpToMd5.ToMd5().ToLower();
        var rtmpUrl = $"rtmp://{aliLiveConfig.PullDomain}/{appName}/{streamName}?auth_key={timeStamp}-0-0-{rtmpAuthKey}";


        // PC端：ARTC
        var rtsToMd5 = $"/{appName}/{streamName}_bq-RTS-{timeStamp}-0-0-{aliLiveConfig.PullKey}";
        var rtsAuthKey = rtsToMd5.ToMd5().ToLower();
        var rtsUrl = $"artc://{aliLiveConfig.PullDomain}/{appName}/{streamName}_bq-RTS?auth_key={timeStamp}-0-0-{rtsAuthKey}";
        if (requestDic is null)
        { 
         requestDic = new Dictionary<string, string>();
        }
        if (!requestDic.ContainsKey(LiveUrlType.RTMP.ToString()))
        {
            requestDic.Add(LiveUrlType.RTMP.ToString(), rtmpUrl);
        }
        else
        {
            if (string.IsNullOrEmpty(requestDic[LiveUrlType.RTMP.ToString()]))
            {
                requestDic[LiveUrlType.RTMP.ToString()]= rtmpUrl;
            }
        }
        if (!requestDic.ContainsKey(LiveUrlType.RTS.ToString()))
        {
            requestDic.Add(LiveUrlType.RTS.ToString(), rtsUrl);
        }
        else
        {
            if (string.IsNullOrEmpty(requestDic[LiveUrlType.RTS.ToString()]))
            {
                requestDic[LiveUrlType.RTS.ToString()] = rtsUrl;
            }
        }
        return requestDic;
    }

    /// <summary>
    /// 生成推流地址
    /// </summary>
    /// <param name="model"></param>
    public string CreatePushUrl(CreateLiveVideoUrlViewModel model)
    {
        //tenantId=08db5bf2-afae-4d40-8896-18e7e86b6b37&liveVideoId=08dc4b04-3123-4799-881e-267d6e247dc8
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId();
        var usrargs = $"tenantId={tenantId}&liveVideoId={model.LiveVideoId}";
        usrargs = HttpUtility.UrlEncode(usrargs);
        var aliLiveConfig = Singleton<AppSettings>.Instance.ModuleConfigurations[nameof(AliLiveConfig)] as
            AliLiveConfig;

        // 获取当前时间
        var currentTime = DateTime.Now;
        // 计算自午夜以来的秒数
        var totalSeconds = (long)(currentTime - DateTime.MinValue).TotalSeconds;

        var timeStamp = totalSeconds + model.expireTime;
        var stringToMd5 = $"/{model.appName}/{model.streamName}-{timeStamp}-0-0-{aliLiveConfig.PushKey}";
        var authKey = stringToMd5.ToMd5().ToLower();
        //var pushUrl = @$"rtmp://{aliLiveConfig.PushDomain}/{model.appName}/{model.streamName}?auth_key={timeStamp}-0-0-{authKey}&usrargs={usrargs}";
        var pushUrl = $"rtmp://{aliLiveConfig.PushDomain}/{model.appName}/{model.streamName}?auth_key={timeStamp}-0-0-{authKey}";
        return pushUrl;
    }
}