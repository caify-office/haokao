using HaoKao.QuestionService.Domain.CacheExtensions;
using HaoKao.QuestionService.Domain.QuestionWrongPaperMoudle;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace HaoKao.QuestionService.Application.QuestionWrongPaperModule;

/// <summary>
/// 错题组卷微信服务
/// </summary>
/// <param name="cacheManager"></param>
/// <param name="bus"></param>
/// <param name="notifications"></param>
/// <param name="repository"></param>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class QuestionWrongPaperWeChatService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IQuestionWrongPaperRepository repository
) : IQuestionWrongPaperWeChatService
{
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IQuestionWrongPaperRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly GenericCacheManager _cacheKeyManager = new(typeof(QuestionWrongPaper));

    /// <summary>
    /// 获取错题组卷
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async Task<BrowseQuestionWrongPaperViewModel> Get(Guid id)
    {
        var entity = await _cacheManager.GetAsync(
            _cacheKeyManager.ById.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException(StatusCodes.Status404NotFound, "未找到错题组卷");

        return entity.MapToDto<BrowseQuestionWrongPaperViewModel>();
    }

    /// <summary>
    /// 获取错题组卷列表
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<QueryQuestionWrongPaperViewModel> Get([FromQuery] QueryQuestionWrongPaperViewModel viewModel)
    {
        var query = viewModel.MapToQuery<QuestionWrongPaperQuery>();
        query.OrderBy = nameof(QuestionWrongPaper.CreateTime);
        var tempQuery = await _cacheManager.GetAsync(
            _cacheKeyManager.ListQuery.Create(query.GetCacheKey()),
            async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            }
        );

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<QueryQuestionWrongPaperViewModel, QuestionWrongPaper>();
    }

    /// <summary>
    /// 创建错题组卷
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task Create([FromBody] CreateQuestionWrongPaperViewModel viewModel)
    {
        var command = new CreateQuestionWrongPaperCommand(
            viewModel.SubjectId,
            viewModel.PaperName,
            viewModel.DownloadUrl,
            viewModel.QuestionCount
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 暂存错题组卷的数据
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost, AllowAnonymous]
    public Guid HoldData([FromBody] dynamic data)
    {
        var id = Guid.NewGuid();
        _cacheManager.SetAsync(new CacheKey($"questionwrongpaper_{id}").Create(), data).Wait();
        return id;
    }

    /// <summary>
    /// 获取暂存的错题组卷数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet, AllowAnonymous]
    public List<dynamic> DropData(Guid id)
    {
        return _cacheManager.Get<List<dynamic>>(new CacheKey($"questionwrongpaper_{id}").Create(), () => []);
    }

    [HttpPost, AllowAnonymous]
    public Guid HoldHtml([FromBody] HoldHtmlRequest request)
    {
        var id = Guid.NewGuid();
        _cacheManager.SetAsync(new CacheKey($"html_{id}").Create(), request.Html).Wait();
        return id;
    }

    [HttpGet, AllowAnonymous]
    public IActionResult DropHtml(Guid id, [FromServices] IConverter converter)
    {
        var html = _cacheManager.Get(new CacheKey($"html_{id}").Create(), () => "");
        if (string.IsNullOrEmpty(html))
        {
            return new OkResult();
        }

        var doc = new HtmlToPdfDocument
        {
            GlobalSettings = { PaperSize = PaperKind.A4 },
            Objects = { new ObjectSettings { HtmlContent = html, } }
        };
        var pdf = converter.Convert(doc);
        return new FileContentResult(pdf, "application/pdf") { FileDownloadName = $"{Guid.NewGuid()}.pdf" };
    }

    /// <summary>
    /// html转pdf
    /// </summary>
    /// <param name="html"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    [HttpPost, AllowAnonymous]
    public IActionResult Html2Pdf([FromBody] string html, [FromServices] IConverter converter)
    {
        var doc = new HtmlToPdfDocument
        {
            GlobalSettings = { PaperSize = PaperKind.A4 },
            Objects = { new ObjectSettings { HtmlContent = html, } }
        };
        var pdf = converter.Convert(doc);
        return new FileContentResult(pdf, "application/pdf") { FileDownloadName = $"{Guid.NewGuid()}.pdf" };
    }
}

public record HoldHtmlRequest(string Html);