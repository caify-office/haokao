namespace HaoKao.Common.AuthorizeRepositories;

[RefitService("haokao-basicservice-webapi")]
public interface IAuthorityNetWorkRepository : IGirvsRefit
{
    [Get("/api/Management/UserAppService/CurrentUserAuthorization")]
    Task<AuthorizeModel> GetUserAuthorization();
}