using Girvs.Refit;
using HaoKao.Common.RemoteService;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace HaoKao.ProductService.Infrastructure.RemoteService;

[RefitService(RefitServiceNames.PaperService)]
public interface IRemotePaperService : IGirvsRefit
{
    [Get($"{URLPrefixManager.PaperManagementPrefix}/PaperDetailInfo/{{id}}")]
    Task<dynamic> GetPaperDetailInfo([FromHeader] Guid id);
}