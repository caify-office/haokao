using System.Net.Http.Json;
using HaoKao.CorrectionNotebookService.Domain.Services;

namespace HaoKao.CorrectionNotebookService.Infrastructure.Services;

public record TextInOptions : IAppModuleConfig
{
    public string AppId { get; init; } = "f0e0eb69d62a2ce0336acda7ec44ded7";

    public string SecretCode { get; init; } = "d2ff5288c84b9d156fb3a13d593a26af";

    public void Init() { }
}

/// <summary>
/// TextIn图像服务
/// <see cref="https://www.textin.com/document/crop_enhance_image"/>
/// </summary>
/// <param name="options"></param>
public class TextInImageService : IImageEnhancement, IImageDemoire, IImageDewarp
{
    public Guid Id => new("01a53a77-c2b3-4ca5-97fa-57c90f991343");

    private readonly TextInOptions _options = Singleton<AppSettings>.Instance.Get<TextInOptions>();

    public async Task<string> EnhanceImage(string imageBase64)
    {
        var parameter = new EnhanceImageRequestParameter
        {
            EnhanceMode = 5,
            CropImage = 0,
            CorpScene = 0,
            CorrectDirection = 1,
        };

        var buffer = Convert.FromBase64String(imageBase64);
        var url = $"https://api.textin.com/ai/service/v1/crop_enhance_image?{parameter}";
        using var client = CreateClient();
        using var response = await client.PostAsync(url, new ByteArrayContent(buffer));
        response.EnsureSuccessStatusCode();
        try
        {
            var result = await response.Content.ReadFromJsonAsync<EnhanceImageResponse>();
            return result?.Result.ImageList[0].Image;
        }
        catch
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new GirvsException(message);
        }
    }

    public async Task<string> DemoireImage(string imageBase64)
    {
        var buffer = Convert.FromBase64String(imageBase64);
        var url = "https://api.textin.com/ai/service/v1/demoire";
        using var client = CreateClient();
        using var response = await client.PostAsync(url, new ByteArrayContent(buffer));
        response.EnsureSuccessStatusCode();
        try
        {
            var result = await response.Content.ReadFromJsonAsync<DemoireImageResponse>();
            return result.Result.Image;
        }
        catch
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new GirvsException(message);
        }
    }

    public async Task<string> DewarpImage(string imageBase64)
    {
        var parameter = new DewarpImageRequestParameter
        {
            Crop = 1,
            Inpainting = 1,
        };

        var buffer = Convert.FromBase64String(imageBase64);
        var url = $"https://api.textin.com/ai/service/v1/dewarp?{parameter}";
        using var client = CreateClient();
        using var response = await client.PostAsync(url, new ByteArrayContent(buffer));
        response.EnsureSuccessStatusCode();
        try
        {
            var result = await response.Content.ReadFromJsonAsync<DewarpImageResponse>();
            return result.Result.Image;
        }
        catch
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new GirvsException(message);
        }
    }

    private HttpClient CreateClient()
    {
        var client = HttpClientFactory.Create();
        client.DefaultRequestHeaders.Add("x-ti-app-id", _options.AppId);
        client.DefaultRequestHeaders.Add("x-ti-secret-code", _options.SecretCode);
        return client;
    }
}

file class EnhanceImageRequestParameter
{
    /// <summary>
    /// 1 增亮
    /// 2 增亮并锐化
    /// 3 黑白
    /// 4 灰度
    /// 5 去阴影增强
    /// 6 点阵图
    /// -1 禁用增强。
    /// </summary>
    [JsonPropertyName("enhance_mode")]
    public int? EnhanceMode
    {
        get => _enhanceMode;
        init
        {
            if (value is < -1 or > 6)
            {
                throw new ArgumentOutOfRangeException(nameof(EnhanceMode), "enhance_mode must be between -1 and 6");
            }
            _enhanceMode = value.Value;
        }
    }
    private readonly int? _enhanceMode;

    /// <summary>
    /// 0 不执行切边操作
    /// 1 执行切边操作，默认为1
    /// </summary>
    [JsonPropertyName("crop_image")]
    public int? CropImage
    {
        get => _cropImage;
        init
        {
            if (value != 0 && value != 1)
            {
                throw new ArgumentOutOfRangeException(nameof(CropImage), "crop_image must be 0 or 1");
            }
            _cropImage = value.Value;
        }
    }
    private readonly int _cropImage = 1;

    /// <summary>
    /// 0 通用场景
    /// 1 名片场景
    /// </summary>
    [JsonPropertyName("corp_scene")]
    public int? CorpScene
    {
        get => _corpScene;
        init
        {
            if (value != 0 && value != 1)
            {
                throw new ArgumentOutOfRangeException(nameof(CorpScene), "corp_scene must be 0 or 1");
            }
            _corpScene = value.Value;
        }
    }
    private readonly int _corpScene;

    /// <summary>
    /// 0 返回角点坐标及处理图
    /// 1 仅返回切边角点，不返回切边结果图
    /// </summary>
    [JsonPropertyName("corp_mode")]
    public int? OnlyPosition
    {
        get => _onlyPosition;
        init
        {
            if (value != 0 && value != 1)
            {
                throw new ArgumentOutOfRangeException(nameof(OnlyPosition), "corp_mode must be 0 or 1");
            }
            _onlyPosition = value.Value;
        }
    }
    private readonly int _onlyPosition;

    /// <summary>
    /// 0 不返回圆角切边，默认为0
    /// 1 返回圆角切边结果
    /// </summary>
    [JsonPropertyName("round_image")]
    public int? RoundImage
    {
        get => _roundImage;
        init
        {
            if (value != 0 && value != 1)
            {
                throw new ArgumentOutOfRangeException(nameof(RoundImage), "round_image must be 0 or 1");
            }
            _roundImage = value.Value;
        }
    }
    private readonly int _roundImage;

    /// <summary>
    /// 0 不校正图片方向，默认为0
    /// 1 校正图片方向
    /// </summary>
    [JsonPropertyName("correct_direction")]
    public int? CorrectDirection
    {
        get => _correctDirection;
        init
        {
            if (value != 0 && value != 1)
            {
                throw new ArgumentOutOfRangeException(nameof(CorrectDirection), "correct_direction must be 0 or 1");
            }
            _correctDirection = value.Value;
        }
    }
    private readonly int _correctDirection;

    /// <summary>
    /// 支持切边图片压缩率设置，可设置范围0~100；不设置则默认为95
    /// </summary>
    [JsonPropertyName("jpeg_quality")]
    public int? JpegQuality
    {
        get => _jpegQuality;
        init
        {
            if (value is < 0 or > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(JpegQuality), "jpeg_quality must be between 0 and 100");
            }
            _jpegQuality = value.Value;
        }
    }
    private readonly int _jpegQuality = 95;

    /// <summary>
    /// 支持客户端传入宽高及座标,根据传的切图宽高和座标点来切边 格式 width,height,x1,y1,x2,y2,x3,y3,x4,y4
    /// width：图像宽，height：图像高
    /// (x1, y1) 左上角坐标
    /// (x2, y2) 右上角坐标
    /// (x3, y3) 右下角坐标
    /// (x4, y4) 左下角坐标
    /// </summary>
    [JsonPropertyName("size_and_position")]
    public string SizeAndPosition { get; init; }

    public override string ToString()
    {
        var parameters = new Dictionary<string, object>();

        if (EnhanceMode.HasValue)
        {
            parameters["enhance_mode"] = EnhanceMode;
        }
        if (CropImage.HasValue)
        {
            parameters["crop_image"] = CropImage;
        }
        if (CorpScene.HasValue)
        {
            parameters["corp_scene"] = CorpScene;
        }
        if (OnlyPosition.HasValue)
        {
            parameters["corp_mode"] = OnlyPosition;
        }
        if (RoundImage.HasValue)
        {
            parameters["round_image"] = RoundImage;
        }
        if (CorrectDirection.HasValue)
        {
            parameters["correct_direction"] = CorrectDirection;
        }
        if (JpegQuality.HasValue)
        {
            parameters["jpeg_quality"] = JpegQuality;
        }
        if (!string.IsNullOrEmpty(SizeAndPosition))
        {
            parameters["size_and_position"] = SizeAndPosition;
        }

        return string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));
    }
}

file class DewarpImageRequestParameter
{
    /// <summary>
    /// 是否切边（即文档提取）；0不切边；1切边
    /// </summary>
    public int Crop
    {
        get => _crop;
        init
        {
            if (value != 0 && value != 1)
            {
                throw new ArgumentOutOfRangeException(nameof(Crop), "crop must be 0 or 1");
            }
            _crop = value;
        }
    }
    private readonly int _crop = 1;

    /// <summary>
    /// 是否边缘填充； 0不填充；1填充
    /// </summary>
    public int Inpainting
    {
        get => _inpainting;
        init
        {
            if (value != 0 && value != 1)
            {
                throw new ArgumentOutOfRangeException(nameof(Inpainting), "inpainting must be 0 or 1");
            }
            _inpainting = value;
        }
    }
    private readonly int _inpainting = 1;

    public override string ToString()
    {
        return $"crop={Crop}&inpainting={Inpainting}";
    }
}

file record TextInImageResponse
{
    [JsonPropertyName("code")]
    public int Code { get; init; }

    [JsonPropertyName("message")]
    public string Message { get; init; }

    [JsonPropertyName("version")]
    public string Version { get; init; }

    [JsonPropertyName("duration")]
    public long Duration { get; init; }
}

file record EnhanceImageResponse : TextInImageResponse
{
    [JsonPropertyName("result")]
    public EnhanceImageResponseResult Result { get; init; }
}

file record EnhanceImageResponseResult
{
    [JsonPropertyName("image_list")]
    public EnhanceImageResponseImageListItem[] ImageList { get; init; }
}

file record EnhanceImageResponseImageListItem
{
    [JsonPropertyName("position")]
    public int[] Position { get; init; }

    [JsonPropertyName("origin_width")]
    public int OriginWidth { get; init; }

    [JsonPropertyName("origin_height")]
    public int OriginHeight { get; init; }

    [JsonPropertyName("cropped_height")]
    public int CroppedHeight { get; init; }

    [JsonPropertyName("cropped_width")]
    public int CroppedWidth { get; init; }

    [JsonPropertyName("angle")]
    public int Angle { get; init; }

    [JsonPropertyName("image")]
    public string Image { get; init; }
}

file record DemoireImageResponse : TextInImageResponse
{
    [JsonPropertyName("result")]
    public TextInImageResponseResult Result { get; init; }
}

file record DewarpImageResponse : TextInImageResponse
{
    [JsonPropertyName("result")]
    public TextInImageResponseResult Result { get; init; }
}

file record TextInImageResponseResult
{
    [JsonPropertyName("image")]
    public string Image { get; init; }
}