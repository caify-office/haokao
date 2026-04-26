using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Domain.Queries;

public class StudentPermissionQuery : QueryBase<StudentPermission>
{
    /// <summary>
    /// 学员昵称(即用户昵称)
    /// </summary>
    [QueryCacheKey]
    public string StudentName { get; set; }

    /// <summary>
    /// 对应的订单号
    /// </summary>
    [QueryCacheKey]
    public string OrderNumber { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    [QueryCacheKey]
    public string ProductName { get; set; }

    /// <summary>
    /// 启用/禁用
    /// </summary>
    [QueryCacheKey]
    public bool? Enable { get; set; }

    /// <summary>
    /// 来源
    /// </summary>
    [QueryCacheKey]
    public SourceMode? SourceMode { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [QueryCacheKey]
    public string Phone { get; set; }

    public override Expression<Func<StudentPermission, bool>> GetQueryWhere()
    {
        Expression<Func<StudentPermission, bool>> expression = x => true;

        if (Enable.HasValue)
        {
            expression = expression.And(x => x.Enable == Enable);
        }

        if (!StudentName.IsNullOrEmpty())
        {
            expression = expression.And(x => x.StudentName.Contains(StudentName));
        }

        if (!OrderNumber.IsNullOrEmpty())
        {
            expression = expression.And(x => x.OrderNumber.Contains(OrderNumber));
        }

        if (!ProductName.IsNullOrEmpty())
        {
            expression = expression.And(x => x.ProductName.Contains(ProductName));
        }

        if (SourceMode.HasValue)
        {
            expression = expression.And(x => x.SourceMode == SourceMode.Value);
        }

        if (!Phone.IsNullOrEmpty())
        {
            expression = expression.And(x => x.RegisterUser.Phone.Contains(Phone));
        }

        return expression;
    }
}