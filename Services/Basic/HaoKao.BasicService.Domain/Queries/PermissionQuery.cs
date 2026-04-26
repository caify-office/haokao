using HaoKao.BasicService.Domain.Entities;

namespace HaoKao.BasicService.Domain.Queries;

public class PermissionQuery : QueryBase<BasalPermission>
{
    /// <summary>
    /// 功能模块的ID
    /// </summary>
    public Guid? AppliedObjectId { get; set; }

    /// <summary>
    /// 接受权限类型(用户或者角色)
    /// </summary>
    public PermissionAppliedObjectType AppliedType = PermissionAppliedObjectType.Role;

    /// <summary>
    /// 用户或角色ID
    /// </summary>
    public Guid? AppliedId { get; set; }

    /// <summary>
    /// 权限分类(比如说功能菜单,数据记录等)
    /// </summary>
    public PermissionValidateObjectType ValidateObjectType = PermissionValidateObjectType.FunctionMenu;

    public override Expression<Func<BasalPermission, bool>> GetQueryWhere()
    {
        Expression<Func<BasalPermission, bool>> expression = x => x.ValidateObjectType == ValidateObjectType && x.AppliedObjectType == AppliedType;

        if (AppliedId.HasValue)
        {
            expression = expression.And(x => x.AppliedId == AppliedId.Value);
        }

        if (AppliedObjectId.HasValue)
        {
            expression = expression.And(x => x.AppliedObjectId == AppliedId.Value);
        }

        return expression;
    }
}