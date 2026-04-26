using HaoKao.BasicService.Domain.Entities;
using HaoKao.BasicService.Domain.Enumerations;
using HaoKao.BasicService.Domain.Extensions;

namespace HaoKao.BasicService.Domain.Queries;

public class UserQuery : QueryBase<User>
{
    [QueryCacheKey]
    public string UserName { get; set; }

    [QueryCacheKey]
    public string UserAccount { get; set; }

    [QueryCacheKey]
    public DataState? DataState { get; set; }

    public override Expression<Func<User, bool>> GetQueryWhere()
    {
        Expression<Func<User, bool>> expression = user => true;
        var userId = EngineContext.Current.ClaimManager.IdentityClaim.UserId;
        expression = expression.And(x => x.Id.ToString() != userId);

        if (DataState.HasValue)
        {
            expression = expression.And(user => user.State == DataState.Value);
        }

        if (!string.IsNullOrEmpty(UserName))
        {
            expression = expression.And(user => user.UserName.Contains(UserName));
        }

        if (!string.IsNullOrEmpty(UserAccount))
        {
            expression = expression.And(user => user.UserAccount.Contains(UserAccount));
        }

        //如果当前用户是普通用户，则列表只显示当前用户自己创建的用户列表
        var httpContext = EngineContext.Current.HttpContext;
        if (httpContext?.User.Identity is { IsAuthenticated: true })
        {
            var currentUser = EngineContext.Current.GetCurrentUser();
            if (currentUser != null && currentUser.UserType == UserType.GeneralUser)
            {
                expression = expression.And(x => x.CreatorId == currentUser.Id);
            }
        }

        return expression;
    }
}