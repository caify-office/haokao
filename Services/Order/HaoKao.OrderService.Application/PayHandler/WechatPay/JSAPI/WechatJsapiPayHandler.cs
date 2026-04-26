using HaoKao.OrderService.Application.PayHandler.WechatPay.Base;
using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;
using Microsoft.AspNetCore.WebUtilities;
using SKIT.FlurlHttpClient.Wechat.TenpayV3.Utilities;
using System.Net.Http;

namespace HaoKao.OrderService.Application.PayHandler.WechatPay.JSAPI;

public class WechatJsapiPayHandler : WechatPayHandler<WechatJsapiPayHandlerConfig>
{
    public override Guid PayHandlerId => new("e50b6510-4a24-4272-b28b-d5bf9c4684c6");

    public override string PayHandlerName => "微信JSAPI支付";

    public override PlatformPayerScenes Scenes => PlatformPayerScenes.WechatMiniProgram;

    public override async Task<IOrderReturn> CreateOrder(Order order, Dictionary<string, string> extendParams)
    {
        //获取请求头信息
        var attachment = PayTokenHandler.GetAttachment();

        var orderNumber = await GenerateOrderNumber(order);

        if (!EngineContext.Current.HttpContext.Request.Query.TryGetValue("code", out var code))
        {
            throw new GirvsException("下单失败: 缺少code参数");
        }

        var openId = await GetOpenId(code);
        var request = new CreatePayTransactionJsapiRequest
        {
            AppId = Config.AppId,
            MerchantId = Config.MerchantId,
            Description = order.PurchaseName,
            OutTradeNumber = $"{orderNumber}_{DateTime.Now.Millisecond}" ,
            NotifyUrl = BuildNotifyUrl(order.PlatformPayerId, order.Id),
            ExpireTime = DateTimeOffset.Now.AddHours(2),
            Attachment = attachment,
            Payer = new CreatePayTransactionJsapiRequest.Types.Payer { OpenId = openId },
            Amount = new CreatePayTransactionJsapiRequest.Types.Amount { Total = (int)(order.ActualAmount * 100) },
        };

        var response = await WechatTenpayClient.ExecuteCreatePayTransactionJsapiAsync(request);
        if (!response.IsSuccessful())
        {
            throw new GirvsException($"下单失败（状态码：{response.RawStatus}，错误代码：{response.ErrorCode}，错误描述：{response.ErrorMessage}）。");
        }

        var result = new WechatJsapiPayCreateOrderReturn
        {
            AppId = Config.AppId,
            TimeStamp = response.WechatpayTimestamp,
            NonceStr = response.WechatpayNonce,
            Package = $"prepay_id={response.PrepayId}",
            SignType = "RSA",
            CurrentPayNotifySign = BuildCurrentPayNotifySign(order),
            OrderId = order.Id,
            OrderNumber = order.OrderSerialNumber
        };

        var message = $"{result.AppId}\n{result.TimeStamp}\n{result.NonceStr}\n{result.Package}\n";
        result.PaySign = RSAUtility.SignWithSHA256(Config.CertificatePrivateKey, message);
        return result;
    }

    /// <summary>
    /// 读取openid
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    private async Task<string> GetOpenId(string code)
    {
        const string uri = "https://api.weixin.qq.com/sns/jscode2session";
        var url = QueryHelpers.AddQueryString(uri, new Dictionary<string, string>
        {
            { "appid", Config.AppId },
            { "secret", Config.AppSecret },
            { "js_code", code },
            { "grant_type", "authorization_code" },
        });

        using var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var model = JsonConvert.DeserializeObject<WechatOpenIdViewModel>(json);
        if (model == null) throw new GirvsException(json);
        return model.openid;
    }
}

public class WechatOpenIdViewModel
{
    public string openid { get; set; }

    public string session_key { get; set; }

    public string unionid { get; set; }

    public int errcode { get; set; }

    public string errmsg { get; set; }
}