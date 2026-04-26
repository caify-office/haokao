using HaoKao.CorrectionNotebookService.Domain.Services;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Ocr.V20181119;
using TencentCloud.Ocr.V20181119.Models;

namespace HaoKao.CorrectionNotebookService.Infrastructure.Services;

public record TencentCloundOptions : IAppModuleConfig
{
    public string SecretId { get; init; } = "";

    public string SecretKey { get; init; } = "";

    public string Region { get; init; } = "ap-guangzhou";

    public void Init() { }
}

/// <summary>
/// 腾讯云图像服务
/// <see cref="https://cloud.tencent.com/product/tie"/>
/// </summary>
/// <param name="options"></param>
public class TencentCloudImageService : IImageEnhancement
{
    public Guid Id => new("6dbc72ca-d212-4a99-93a2-52d4958ed7a3");

    /// <summary>
    /// 图像增强
    /// </summary>
    /// <param name="imageBase64"></param>
    /// <returns></returns>
    public Task<string> EnhanceImage(string imageBase64)
    {
        // 自动增强(去摩尔纹 去除阴影 去除模糊 去除过曝)
        var result = AutoEnhanceImage(imageBase64);

        return Task.FromResult(result);
    }

    private readonly TencentCloundOptions _options = Singleton<AppSettings>.Instance.Get<TencentCloundOptions>();

    private OcrClient CreateClient()
    {
        // 实例化一个client选项，可选的，没有特殊需求可以跳过
        var clientProfile = new ClientProfile
        {
            // 实例化一个http选项，可选的，没有特殊需求可以跳过
            HttpProfile = new HttpProfile
            {
                Endpoint = "ocr.tencentcloudapi.com",
            }
        };

        // 实例化一个认证对象，入参需要传入腾讯云账户 SecretId 和 SecretKey，此处还需注意密钥对的保密
        // 代码泄露可能会导致 SecretId 和 SecretKey 泄露，并威胁账号下所有资源的安全性。以下代码示例仅供参考，建议采用更安全的方式来使用密钥
        // 请参见：https://cloud.tencent.com/document/product/1278/85305
        // 密钥可前往官网控制台 https://console.cloud.tencent.com/cam/capi 进行获取
        var credential = new Credential { SecretId = _options.SecretId, SecretKey = _options.SecretKey };
        // 实例化要请求产品的client对象,clientProfile是可选的
        var client = new OcrClient(credential, _options.Region, clientProfile);
        return client;
    }

    private string AutoEnhanceImage(string imageBase64)
    {
        var request = new ImageEnhancementRequest
        {
            ImageBase64 = imageBase64,
            TaskType = 300,
            ReturnImage = "preprocess",
        };

        var response = CreateClient().ImageEnhancementSync(request);

        return response.Image;
    }
}