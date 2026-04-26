using AutoMapper;
using Girvs.Extensions;
using HaoKao.DrawPrizeService.Application.Services.Management;
using HaoKao.DrawPrizeService.Domain.Entities;
using System.Threading;

namespace HaoKao.DrawPrizeService.Application.Services.WeChat;


/// <summary>
/// 奖品服务-Web端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class PrizeWeChatService(IPrizeService prizeService
        , IDrawPrizeService prizeWebService
        , IDrawPrizeRecordService prizeRecordService
        , ILocker locker
        , IMediatorHandler bus
        , INotificationHandler<DomainNotification> notifications
        , IDrawPrizeRecordRepository drawRecordRepository
        , IMapper mapper) : IPrizeWeChatService
{
    #region 初始参数
    private readonly IDrawPrizeService _drawPrizeService = prizeWebService ?? throw new ArgumentNullException(nameof(prizeWebService));
    private readonly IPrizeService _prizeService = prizeService ?? throw new ArgumentNullException(nameof(prizeService));
    private readonly IDrawPrizeRecordService _drawPrizeRecordService = prizeRecordService ?? throw new ArgumentNullException(nameof(prizeRecordService));
    private readonly ILocker _locker = locker ?? throw new ArgumentNullException(nameof(locker));
    private readonly IMediatorHandler _bus = bus;
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IDrawPrizeRecordRepository _drawRecordRepository = drawRecordRepository ?? throw new ArgumentNullException(nameof(drawRecordRepository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    public Task<BrowsePrizeViewModel> Get(Guid id)
    {
        return _prizeService.Get(id);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public Task<PrizeQueryViewModel> Get([FromQuery] PrizeQueryViewModel queryViewModel)
    {
        return _prizeService.Get(queryViewModel);
    }

    /// <summary>
    /// 抽奖
    /// </summary>
    [HttpPost]
    public async Task<dynamic> Draw([FromBody] DrawPrizeViewModel model)
    {
        //检查信息
        await CheckInfo();
        //执行抽奖
        return await DrawPrizeExecute();

        async Task CheckInfo()
        {
            //检查数据是否损坏
            var dataKey = $"{model.DrawPrizeId}-{model.Phone}-{model.IsPaidStudents.ToString().ToLower()}-zhuofan168";
            var dateKeyMd5 = dataKey.ToMd5().ToLower();
            if (model.DataKey.ToLower() != dateKeyMd5)
            {
                throw new GirvsException("请求数据异常", StatusCodes.Status400BadRequest);
            }
            var drawPrize = await _drawPrizeService.Get(model.DrawPrizeId);
            if (drawPrize == null || !drawPrize.Enable)
            {
                throw new GirvsException("当前抽奖活动已关闭", StatusCodes.Status400BadRequest);
            }
            var now = DateTime.Now;
            if (now > drawPrize.EndTime)
            {
                throw new GirvsException("当前抽奖活动已结束", StatusCodes.Status400BadRequest);
            }
            if (now < drawPrize.StartTime)
            {
                throw new GirvsException("当前抽奖活动未开始", StatusCodes.Status400BadRequest);
            }


            if (drawPrize.DrawPrizeRange == DrawPrizeRange.PaidStudents && !model.IsPaidStudents)
            {
                throw new GirvsException("当前抽奖活动仅限付费学员参与", StatusCodes.Status400BadRequest);
            }

            if (drawPrize.DrawPrizeRange == DrawPrizeRange.NonPaidStudents && model.IsPaidStudents)
            {
                throw new GirvsException("当前抽奖活动仅限非付费学员参与", StatusCodes.Status400BadRequest);
            }

           
        }

        async Task<dynamic> DrawPrizeExecute()
        {
            dynamic result = "没抢到锁";
            bool lockResult = false;
            var expirationTime = TimeSpan.FromSeconds(5);
            var spinWait = new SpinWait();
            while (!lockResult)
            {
                //加锁
                lockResult = await _locker.PerformActionWithLockAsync(
                    model.DrawPrizeId.ToString(), expirationTime,
                    async () =>
                    {
                        //判定当前活动是否已抽过奖
                        var userId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
                        var exist = await _drawRecordRepository.ExistEntityAsync(x => x.CreatorId == userId && x.DrawPrizeId == model.DrawPrizeId);
                        if (exist)
                        {
                            throw new GirvsException("您已抽过奖", StatusCodes.Status400BadRequest);
                        }
                        var command = _mapper.Map<DrawPrizeCommand>(model);
                        var prize = await _bus.SendCommand(command) as Prize;
                        result = new
                        {
                            PrizeId = prize.Id,
                            Message = prize.Name
                        };
                    });
                if (!lockResult)
                {
                    spinWait.SpinOnce();
                }
            }

            return result;
        }
    }

    #endregion
}