using HaoKao.CorrectionNotebookService.Application.Interfaces;
using HaoKao.CorrectionNotebookService.Application.Options;
using HaoKao.CorrectionNotebookService.Domain.Services;

namespace HaoKao.CorrectionNotebookService.Application.Services;

/// <summary>
/// 图片服务接口
/// </summary>
[DynamicWebApi]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class ImageService : IImageService
{
    private readonly EnabledServiceOptions _options = Singleton<AppSettings>.Instance.Get<EnabledServiceOptions>();

    /// <summary>
    /// 图片处理接口
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ImageProcessResponse> Process([FromBody] ImageProcessRequest request)
    {
        // https://arcaify-bucket.oss-cn-guangzhou.aliyuncs.com/WechatIMG2030.jpg
        var path = await DownloadImage(request.ImageUrl);
        var buffer = await GetImageDataAsync(path);
        var imageBase64 = Convert.ToBase64String(buffer);
        var demoireImage = await DemoireImage(imageBase64);
        var dewarpedImage = await DewarpImage(demoireImage);
        var enhancedImage = await EnhanceImage(dewarpedImage);
        var data = await RecognizeImage(enhancedImage);
        File.Delete(path);
        return new ImageProcessResponse(enhancedImage, dewarpedImage, data);
    }

    /// <summary>
    /// 图片处理接口(文件上传)
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<ImageProcessResponse> ProcessFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentNullException(nameof(file));
        }

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var buffer = ms.ToArray();
        var imageBase64 = Convert.ToBase64String(buffer);
        var demoireImage = await DemoireImage(imageBase64);
        var dewarpedImage = await DewarpImage(demoireImage);
        var enhancedImage = await EnhanceImage(dewarpedImage);
        var data = await RecognizeImage(enhancedImage);
        return new ImageProcessResponse(enhancedImage, dewarpedImage, data);
    }

    /// <summary>
    /// 图片去摩尔纹
    /// </summary>
    /// <param name="imageBase64"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<string> DemoireImage([FromBody] string imageBase64)
    {
        var id = _options.ImageDemoireService.Id;
        var factory = EngineContext.Current.Resolve<IServiceFactory<IImageDemoire>>();
        var instance = factory.Create(id);
        return instance.DemoireImage(imageBase64);
    }

    /// <summary>
    /// 图片畸变矫正
    /// </summary>
    /// <param name="imageBase64"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<string> DewarpImage([FromBody] string imageBase64)
    {
        var id = _options.ImageDewarpService.Id;
        var factory = EngineContext.Current.Resolve<IServiceFactory<IImageDewarp>>();
        var instance = factory.Create(id);
        return instance.DewarpImage(imageBase64);
    }

    /// <summary>
    /// 图片增强接口
    /// </summary>
    /// <param name="imageBase64"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<string> EnhanceImage([FromBody] string imageBase64)
    {
        var id = _options.ImageEnhanceService.Id;
        var factory = EngineContext.Current.Resolve<IServiceFactory<IImageEnhancement>>();
        var instance = factory.Create(id);
        return await instance.EnhanceImage(imageBase64);
    }

    /// <summary>
    /// 图片识别接口
    /// </summary>
    /// <param name="imageBase64"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<string> RecognizeImage([FromBody] string imageBase64)
    {
        var id = _options.ImageRecognitionService.Id;
        var factory = EngineContext.Current.Resolve<IServiceFactory<IImageRecognition>>();
        var instance = factory.Create(id);
        return instance.RecognizeImage(imageBase64);
    }

    public static async Task<string> DownloadImage(Uri uri)
    {
        var filename = Path.GetFileName(uri.LocalPath);
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);

        using var client = new HttpClient();
        using var response = await client.GetAsync(uri);
        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync();
        await using var fileStream = File.Create(path);
        await stream.CopyToAsync(fileStream);

        return path;
    }

    public static async Task<byte[]> GetImageDataAsync(string imagePath)
    {
        await using var fs = new FileStream(imagePath, FileMode.Open);
        var buffer = new byte[fs.Length];
        await fs.ReadAsync(buffer);
        return buffer;
    }
}

public record ImageProcessRequest(Uri ImageUrl);

public record ImageProcessResponse(string EnhancedImage, string DewarpedImage, string ImageRecognitionResult);