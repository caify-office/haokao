using AlibabaCloud.OpenApiClient.Models;
using AlibabaCloud.SDK.Ocr_api20210707;
using AlibabaCloud.SDK.Ocr_api20210707.Models;
using AlibabaCloud.TeaUtil.Models;
using HaoKao.CorrectionNotebookService.Domain.Services;

namespace HaoKao.CorrectionNotebookService.Infrastructure.Services;

public record AliyunOcrOptions : IAppModuleConfig
{
    public string AccessKeyId { get; init; } = "";

    public string AccessKeySecret { get; init; } = "";

    public string Endpoint { get; init; } = "ocr-api.cn-hangzhou.aliyuncs.com";

    public string CutType { get; init; } = "question";

    public string ImageType { get; init; } = "scan";

    public void Init() { }
}

/// <summary>
/// 阿里云OCR服务
/// <see cref="https://api.aliyun.com/document/ocr-api/2021-07-07/overview"/>
/// </summary>
/// <param name="options"></param>
public class AliyunOcrService : IOcrService, IImageRecognition
{
    public Guid Id => new("462c575b-047f-42dc-9e49-34e299137dfa");

    private readonly AliyunOcrOptions _options = Singleton<AppSettings>.Instance.Get<AliyunOcrOptions>();

    private Client _client;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public async Task<string> Scan(Uri imageUrl)
    {
        var client = CreateClient();
        var request = new RecognizeEduQuestionOcrRequest { Url = imageUrl.ToString(), };
        var timeout = TimeSpan.FromMinutes(5).Milliseconds;
        var runtime = new RuntimeOptions { ConnectTimeout = timeout, ReadTimeout = timeout, Autoretry = true };
        var response = await client.RecognizeEduQuestionOcrWithOptionsAsync(request, runtime);
        if (response.Body.Code != null && response.Body.Data != "200")
        {
            throw new GirvsException(response.Body.Message);
        }
        var data = JsonSerializer.Deserialize<RecognizeEduQuestionOcrResponseData>(response.Body.Data, _jsonSerializerOptions);
        return data.Content;
    }

    public async Task<string> RecognizeImage(string imageBase64)
    {
        var client = CreateClient();
        // 把base64的图片转换为stream
        using var imageStream = new MemoryStream(Convert.FromBase64String(imageBase64));
        var request = new RecognizeEduPaperCutRequest
        {
            Body = imageStream,
            CutType = _options.CutType,
            ImageType = _options.ImageType,
        };
        var timeout = TimeSpan.FromMinutes(5).Milliseconds;
        var runtime = new RuntimeOptions { ConnectTimeout = timeout, ReadTimeout = timeout, Autoretry = true };
        var response = await client.RecognizeEduPaperCutWithOptionsAsync(request, runtime);

        if (response.Body.Code != null && response.Body.Data != "200")
        {
            throw new GirvsException(response.Body.Message);
        }

        return response.Body.Data;
    }

    private Client CreateClient()
    {
        if (_client == null)
        {
            var config = new Config
            {
                // 必填，您的 AccessKey ID
                AccessKeyId = _options.AccessKeyId,
                // 必填，您的 AccessKey Secret
                AccessKeySecret = _options.AccessKeySecret,
                // Endpoint 请参考 https://api.aliyun.com/product/ocr-api
                Endpoint = _options.Endpoint,
            };
            _client = new Client(config);
        }
        return _client;
    }
}

file record RecognizeEduQuestionOcrResponseData
{
    /// <summary>
    /// 识别出图片的文字块汇总
    /// </summary>
    public string Content { get; init; }

    /// <summary>
    /// 图片中的图案信息
    /// </summary>
    public Figure[] Figure { get; init; }

    /// <summary>
    /// 图片中的图案信息
    /// </summary>
    public PrismWordsInfo[] PrismWordsInfo { get; init; }

    /// <summary>
    /// 识别的文字块的数量，prism_wordsInfo 数组的大小
    /// </summary>
    public int PrismWnum { get; init; }

    /// <summary>
    /// 算法矫正图片后的高度
    /// </summary>
    public int Height { get; init; }

    /// <summary>
    /// 算法矫正图片后的宽度
    /// </summary>
    public int Width { get; init; }

    /// <summary>
    /// 原图的高度
    /// </summary>
    public int OrgHeight { get; init; }

    /// <summary>
    /// 原图的宽度
    /// </summary>
    public int OrgWidth { get; init; }
}

file record Figure
{
    /// <summary>
    /// 图案类型
    /// </summary>
    public string Type { get; init; }

    /// <summary>
    /// 图案左上角横坐标
    /// </summary>
    public int X { get; init; }

    /// <summary>
    /// 图案左上角纵坐标
    /// </summary>
    public int Y { get; init; }

    /// <summary>
    /// 图案宽度
    /// </summary>
    public int W { get; init; }

    /// <summary>
    /// 图案高度
    /// </summary>
    public int H { get; init; }

    public FigureBox Box { get; init; }

    public FigurePoint[] Points { get; init; }
}

file record FigureBox
{
    /// <summary>
    /// 中心横坐标
    /// </summary>
    public int X { get; init; }

    /// <summary>
    /// 中心纵坐标
    /// </summary>
    public int Y { get; init; }

    /// <summary>
    /// 长
    /// </summary>
    public int W { get; init; }

    /// <summary>
    /// 宽
    /// </summary>
    public int H { get; init; }

    /// <summary>
    /// 顺时针旋转角度
    /// </summary>
    public int Angle { get; init; }
}

file record FigurePoint
{
    public int X { get; init; }

    public int Y { get; init; }
}

file record PrismWordsInfo
{
    /// <summary>
    /// 文字块的角度
    /// </summary>
    public int Angle { get; init; }

    /// <summary>
    /// 算法矫正图片后的高度
    /// </summary>
    public int Height { get; init; }

    /// <summary>
    /// 算法矫正图片后的宽度
    /// </summary>
    public int Width { get; init; }

    /// <summary>
    /// 文字块的外矩形四个点的坐标按顺时针排列（左上、右上、右下、左下）
    /// </summary>
    public int[] Pos { get; init; }

    /// <summary>
    /// 文字块的文字内容
    /// </summary>
    public string Word { get; init; }

    /// <summary>
    /// 单字信息
    /// </summary>
    public PrismCharInfo[] CharInfo { get; init; }

    /// <summary>
    /// 文字属性分类
    /// </summary>
    public int RecClassify { get; init; }
}

file record PrismCharInfo
{
    /// <summary>
    /// 单字文字
    /// </summary>
    public string Word { get; init; }

    /// <summary>
    /// 置信度
    /// </summary>
    public int Prob { get; init; }

    /// <summary>
    /// 单字左上角横坐标
    /// </summary>
    public int X { get; init; }

    /// <summary>
    /// 单字左上角纵坐标
    /// </summary>
    public int Y { get; init; }

    /// <summary>
    /// 单字宽度
    /// </summary>
    public int W { get; init; }

    /// <summary>
    /// 单字高度
    /// </summary>
    public int H { get; init; }
}