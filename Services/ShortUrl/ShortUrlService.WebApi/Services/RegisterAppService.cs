using ShortUrlService.Domain.Commands;
using ShortUrlService.Domain.Repositories;
using ShortUrlService.WebApi.Models;

namespace ShortUrlService.WebApi.Services;

public interface IRegisterAppService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据Id获取注册应用
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<RegisterAppDto> Get(long id);

    /// <summary>
    /// 获取注册应用列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<PagingResult<RegisterAppDto>> Get(PagingRequest request);

    /// <summary>
    /// 创建注册应用并返回应用密钥
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<string> CreateRegisterApp(CreateRegisterAppRequest request);
}

[DynamicWebApi]
[Route("apiShortUrl/RegisterAppService")]
public class RegisterAppService(
    IMediatorHandler bus,
    IMapper mapper,
    IRegisterAppRepository repository
) : IRegisterAppService
{
    /// <summary>
    /// 根据Id获取注册应用
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:long}")]
    public async Task<RegisterAppDto> Get(long id)
    {
        var entity = await repository.GetByIdAsync(id);
        return mapper.Map<RegisterAppDto>(entity);
    }

    /// <summary>
    /// 获取注册应用列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagingResult<RegisterAppDto>> Get([FromQuery] PagingRequest request)
    {
        var (totalCount, data) = await repository.GetPagedListAsync(request.PageIndex, request.PageSize);
        return new PagingResult<RegisterAppDto>(totalCount, mapper.Map<IReadOnlyList<RegisterAppDto>>(data));
    }

    /// <summary>
    /// 创建注册应用并返回应用密钥
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<string> CreateRegisterApp([FromBody] CreateRegisterAppRequest request)
    {
        if (await repository.ExistForCreate(request.AppName, request.AppCode))
        {
            throw new GirvsException("App already exists.");
        }

        var command = mapper.Map<CreateRegisterAppCommand>(request);
        var result = (string)await bus.SendCommand(command);
        return result;
    }
}