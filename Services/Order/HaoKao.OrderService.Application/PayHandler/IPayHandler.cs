using HaoKao.OrderService.Application.PayHandler.Config;
using HaoKao.OrderService.Application.Services.Management;
using HaoKao.OrderService.Application.ViewModels.Order;
using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace HaoKao.OrderService.Application.PayHandler;

public interface IPayHandler
{
    /// <summary>
    /// 支付者类型名称
    /// </summary>
    string PayHandlerName { get; }

    /// <summary>
    /// 使用场景
    /// </summary>
    PlatformPayerScenes Scenes { get; }

    /// <summary>
    /// 支付者的标识
    /// </summary>
    Guid PayHandlerId { get; }

    /// <summary>
    /// 读取所需要的参数类型
    /// </summary>
    /// <returns></returns>
    Type GetPayerNeedParameterType();

    /// <summary>
    /// 设置配置
    /// </summary>
    /// <param name="config"></param>
    void SetConfig(Dictionary<string, string> config);

    /// <summary>
    /// 创建订单，也就是通常所讲的下单,返回预支付交易会话ID
    /// </summary>
    /// <returns></returns>
    Task<IOrderReturn> CreateOrder(Order order, Dictionary<string, string> extendParams);

    /// <summary>
    /// 支付回调
    /// </summary>
    /// <returns></returns>
    Task<dynamic> ReceivePayNotifyMessage(Guid orderId);

    /// <summary>
    /// 当前回调（如App支付后，在客户端就已经返回支付成功的消息，对订单进行修改），或者扫码支付成功，前端页面回调成功
    /// </summary>
    /// <returns></returns>
    Task<bool> CurrentPayNotify(CurrentPayNotifyModel currentPayNotifyModel);

    /// <summary>
    /// 创建订单号
    /// </summary>
    /// <returns></returns>
    Task<string> GenerateOrderNumber();
}

public abstract class PayHandlerBase<TConfig> : IPayHandler where TConfig : IPayHandlerConfig, new()
{
    /// <summary>
    /// 配置
    /// </summary>
    protected TConfig Config { get; } = new();

    public virtual void SetConfig(Dictionary<string, string> config)
    {
        Config.ReadFromDictionary(config);
    }

    protected virtual string BuildNotifyUrl(Guid platformPayerId, Guid orderId)
    {
        return $"/{platformPayerId}/{orderId}";
    }

    public abstract Guid PayHandlerId { get; }

    public abstract string PayHandlerName { get; }

    public abstract PlatformPayerScenes Scenes { get; }

    public Type GetPayerNeedParameterType()
    {
        return typeof(TConfig);
    }

    /// <summary>
    /// 创建订单
    /// </summary>
    /// <param name="order"></param>
    /// <param name="extendParams"></param>
    /// <returns></returns>
    public abstract Task<IOrderReturn> CreateOrder(Order order, Dictionary<string, string> extendParams);

    /// <summary>
    /// 接收支付回调通知
    /// </summary>
    /// <returns></returns>
    public abstract Task<dynamic> ReceivePayNotifyMessage(Guid orderId);

    /// <summary>
    /// 生成订单号
    /// </summary>
    /// <returns></returns>
    public Task<string> GenerateOrderNumber()
    {
        var orderNumber = OrderNumberGenerator.Generate(Config.OrderPrefix);
        return Task.FromResult(orderNumber);
    }

    public Task<string> GenerateOrderNumber(Order order)
    {
        var orderNumber = Config.OrderPrefix + order.OrderSerialNumber;
        return Task.FromResult(orderNumber);
    }

    /// <summary>
    /// 根据订单生成当前返回的签名
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    public virtual string BuildCurrentPayNotifySign(Order order)
    {
        return $"{order.Id}{order.OrderSerialNumber}{order.CreatorId}".ToLower().ToMd5();
    }

    /// <summary>
    /// 较验当前签名
    /// </summary>
    /// <param name="order"></param>
    /// <param name="currentPaynotifySign"></param>
    /// <returns></returns>
    public virtual bool VerifyCurrentPayNotifySign(Order order, string currentPaynotifySign)
    {
        var signContent = BuildCurrentPayNotifySign(order);
        return signContent.Equals(currentPaynotifySign, StringComparison.CurrentCultureIgnoreCase);
    }

    /// <summary>
    /// 支付回调成功后，修改本地订单
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="orderSerialNumber">订单流水号</param>
    /// <param name="orderNumber">订单号,主要是和第三方平台支付时产生的交易号</param>
    /// <returns></returns>
    protected virtual async Task<bool> PaySucceedUpdateLocalOrder(Guid orderId, string orderSerialNumber, string orderNumber)
    {
        await using var scope = EngineContext.Current.Resolve<IServiceProvider>().CreateAsyncScope();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
        await orderService.FinishPayOrder(new FinishPayOrderViewModel
        {
            Id = orderId,
            PlatformPayerId = PayHandlerId,
            PlatformPayerName = PayHandlerName,
            OrderSerialNumber = orderSerialNumber,
            OrderNumber = orderNumber,
            OrderState = OrderState.PaymentSuccessful
        });

        return true;
    }

    /// <summary>
    /// 当前回调（如App支付后，在客户端就已经返回支付成功的消息，对订单进行修改），或者扫码支付成功，前端页面回调成功
    /// </summary>
    /// <returns></returns>
    public virtual async Task<bool> CurrentPayNotify(CurrentPayNotifyModel model)
    {
        // 说明第三方支付平台还未回调成功
        if (model.Order.OrderState != OrderState.PaymentSuccessful)
        {
            if (model.Order == null || !VerifyCurrentPayNotifySign(model.Order, model.CurrentPayNotifySign))
            {
                throw new GirvsException("签名认证失败");
            }

            await PaySucceedUpdateLocalOrder(model.Order.Id, model.OrderSerialNumber, "");
        }

        return true;
    }
}