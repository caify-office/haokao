namespace HaoKao.OpenPlatformService.Domain.Queries;

public class RegisterUserQuery : QueryBase<RegisterUser>
{
    /// <summary>
    /// 手机号码
    /// </summary>
    [QueryCacheKey]
    public string Phone { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    [QueryCacheKey]
    public string NickName { get; set; }

    /// <summary>
    /// 用户状态
    /// </summary>
    [QueryCacheKey]
    public UserState? UserState { get; set; }

    /// <summary>
    /// 开始注册时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? StartRegisterDateTime { get; set; }

    /// <summary>
    /// 结束注册时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? EndRegisterDateTime { get; set; }

    public override Expression<Func<RegisterUser, bool>> GetQueryWhere()
    {
        Expression<Func<RegisterUser, bool>> expression = x => true;

        if (!Phone.IsNullOrEmpty())
        {
            expression = expression.And(x => x.Phone.Contains(Phone));
        }

        if (!NickName.IsNullOrEmpty())
        {
            expression = expression.And(x => x.NickName.Contains(NickName));
        }

        if (UserState.HasValue)
        {
            expression = expression.And(x => x.UserState == UserState);
        }

        if (StartRegisterDateTime.HasValue)
        {
            expression = expression.And(x => x.CreateTime >= StartRegisterDateTime);
        }

        if (EndRegisterDateTime.HasValue)
        {
            expression = expression.And(x => x.CreateTime <= EndRegisterDateTime);
        }

        return expression;
    }
}