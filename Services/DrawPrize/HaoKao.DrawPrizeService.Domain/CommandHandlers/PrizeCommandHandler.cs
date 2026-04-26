using AutoMapper;
using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common.Extensions;
using HaoKao.DrawPrizeService.Domain.Entities;
using System.Linq;

namespace HaoKao.DrawPrizeService.Domain.CommandHandlers;

public class PrizeCommandHandler(
    IUnitOfWork<Prize> uow,
    IPrizeRepository repository,
    IMediatorHandler bus,
    IMapper mapper,
    IDrawPrizeRecordRepository rawPrizeRecordRepository,
    IDrawPrizeRepository drawPrizeRepository
) : CommandHandler(uow, bus),
    IRequestHandler<CreatePrizeCommand, bool>,
    IRequestHandler<UpdatePrizeCommand, bool>,
    IRequestHandler<DrawPrizeCommand, Prize>,
    IRequestHandler<SetPrizeGuaranteedCommand, bool>,
    IRequestHandler<CancelPrizeGuaranteedCommand, bool>,
    IRequestHandler<DeletePrizeCommand, bool>
{
    private readonly IPrizeRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IDrawPrizeRecordRepository _drawPrizeRecordRepository = rawPrizeRecordRepository ?? throw new ArgumentNullException(nameof(rawPrizeRecordRepository));
    private readonly IDrawPrizeRepository _drawPrizeRepository = drawPrizeRepository ?? throw new ArgumentNullException(nameof(drawPrizeRepository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreatePrizeCommand request, CancellationToken cancellationToken)
    {
        //判定当前抽奖活动已存在的奖品数目
        var count = await _repository.GetPrizeCountAsync(request.DrawPrizeId);
        if (count >= 5)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.DrawPrizeId.ToString(), "奖品最多只能设置5个", StatusCodes.Status400BadRequest),
                cancellationToken);
            return false;
        }
        var prize = _mapper.Map<Prize>(request);

        await _repository.AddAsync(prize);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(prize, prize.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Prize>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdatePrizeCommand request, CancellationToken cancellationToken)
    {
        var prize = await _repository.GetByIdAsync(request.Id);
        if (prize == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应奖品的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        prize = _mapper.Map(request, prize);
        await _repository.UpdateAsync(prize);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<Prize>.ByIdCacheKey.Create(prize.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(prize, key, key.CacheTime), cancellationToken);
            await _bus.RemoveListCacheEvent<Prize>(cancellationToken);
            await _bus.RemoveListCacheEvent<DrawPrize>(cancellationToken);
        }

        return true;
    }

    public async Task<Prize> Handle(DrawPrizeCommand request, CancellationToken cancellationToken)
    {
        //第一步创建抽奖记录
        var drawPrizeRecord = new DrawPrizeRecord
        {
            Id = Guid.NewGuid(),
            DrawPrizeId = request.DrawPrizeId,
            Phone = request.Phone,
        };

        await _drawPrizeRecordRepository.AddAsync(drawPrizeRecord);

        await Commit();

        //判定是否中奖
        var drawPrize = await _drawPrizeRepository.GetByIdAsync(request.DrawPrizeId); //获取抽奖活动详情
        var random = new Random();

        //中奖了，更新奖品发放数（同步设置第一步中奖纪律的奖品名称，注意，这一步要事务处理，都成功或都失败）
        Prize prize;
        //先找指定学员奖
        var userId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
        var designatedStudentPrize = drawPrize.Prizes.Where(x => x.WinningRange == WinningRange.DesignatedStudent && x.AwardedQuantity < x.Count && x.DesignatedStudents.Any(y => y.UserId == userId));
        if (designatedStudentPrize.Any())
        {
            //存在指定学员奖
            var randomIndex = random.Next(0, designatedStudentPrize.Count());
            prize = designatedStudentPrize.ElementAt(randomIndex);
        }
        else
        {
            var drawResult = random.NextDouble();
            if (drawResult * 100 > drawPrize.Probability)
            {
                //未中奖
                return await RaiseNoWin();
            }
            Expression<Func<Prize, bool>> expression = x => true;
            if (request.IsPaidStudents)
            {
                //付费学员可以抽取全部和付费学员奖品
                expression = expression.And(x => x.WinningRange == WinningRange.All || x.WinningRange == WinningRange.PaidStudents);
            }
            else
            {
                //非付费学员可以抽取全部和非付费学员奖品
                expression = expression.And(x => x.WinningRange == WinningRange.All || x.WinningRange == WinningRange.NonPaidStudents);
            }
            //抽取随机奖品
            var randomPrizes = drawPrize.Prizes.AsQueryable().Where(expression).Where(x => x.AwardedQuantity < x.Count);
            if (randomPrizes.Any())
            {
                var randomIndex = random.Next(0, randomPrizes.Count());
                prize = randomPrizes.ElementAt(randomIndex);
            }
            else
            {
                //抽取保底奖品
                prize = drawPrize.Prizes.AsQueryable().Where(expression)?.FirstOrDefault(x => x.IsGuaranteed);
            }
        }

        if (prize == null)
        {
            return await RaiseNoWin();
        }

        prize.AwardedQuantity += 1;
        drawPrizeRecord.PrizeId = prize.Id;
        drawPrizeRecord.PrizeName = prize.Name;

        try
        {
            await Commit();
            await _bus.RemoveListCacheEvent<Prize>(cancellationToken);
            await _bus.RemoveListCacheEvent<DrawPrizeRecord>(cancellationToken);
            await _bus.RemoveListCacheEvent<DrawPrize>(cancellationToken);
            //更新数据库成功，中奖成功
            return prize;
        }
        catch
        {
            return await RaiseNoWin();
        }

        async Task<Prize> RaiseNoWin()
        {
            await _bus.RemoveListCacheEvent<DrawPrizeRecord>(cancellationToken);
            await _bus.RemoveListCacheEvent<DrawPrize>(cancellationToken);
            //并发导致更新数据库失败，中奖失败和未中奖同等效果
            return new Prize { Id = Guid.Empty, Name = string.Empty };
        }
    }

    public async Task<bool> Handle(SetPrizeGuaranteedCommand request, CancellationToken cancellationToken)
    {
        var prizes = await _repository.GetWhereAsync(w => w.DrawPrizeId == request.DrawPrizeId);
        if (prizes.Count == 0)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应奖品的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        prizes.ForEach(x => { x.IsGuaranteed = false; });
        prizes.Find(x => x.Id == request.Id).IsGuaranteed = true;
        await _repository.UpdateRangeAsync(prizes);

        if (await Commit())
        {
            foreach (var id in prizes)
            {
                // 创建缓存Key
                var key = GirvsEntityCacheDefaults<Prize>.ByIdCacheKey.Create(id.ToString());
                // 将新增的纪录放到缓存中
                await _bus.RaiseEvent(new SetCacheEvent(prizes, key, key.CacheTime), cancellationToken);
            }

            await _bus.RemoveListCacheEvent<Prize>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(CancelPrizeGuaranteedCommand request, CancellationToken cancellationToken)
    {
        var prize = await _repository.GetByIdAsync(request.Id);
        if (prize == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应奖品的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        prize.IsGuaranteed = false;

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<Prize>(prize.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Prize>(cancellationToken);
            await _bus.RemoveListCacheEvent<DrawPrize>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeletePrizeCommand request, CancellationToken cancellationToken)
    {
        var prize = await _repository.GetByIdAsync(request.Id);
        if (prize == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应奖品的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(prize);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<Prize>(prize.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Prize>(cancellationToken);
        }

        return true;
    }
}