using HaoKao.BasicService.Domain.Entities;

namespace HaoKao.BasicService.Domain;

public static class PermissionHelper
{
    public static int ConvertStringToPermissionValue(List<string> permissionStrs)
    {
        var validateObjectID = Permission.Undefined;
        foreach (var permissionStr in permissionStrs)
        {
            var c = Enum.Parse<Permission>(permissionStr, true);
            validateObjectID |= c;
        }

        return (int)validateObjectID;
    }

    public static List<Permission> ConvertStringToPermission(List<string> permissions)
    {
        return permissions.Select(permissionStr => Enum.Parse<Permission>(permissionStr, true)).ToList();
    }

    public static List<string> ConvertPermissionToString(BasalPermission basalPermission)
    {
        var list = new List<string>();
        foreach (Permission value in typeof(Permission).GetEnumValues())
        {
            if (basalPermission.GetBit(value))
            {
                list.Add(value.ToString());
            }
        }

        return list;
    }

    /// <summary>
    /// 对权限进行合并
    /// </summary>
    /// <param name="ps"></param>
    /// <returns></returns>
    public static List<BasalPermission> MergeValidateObjectTypePermission(List<BasalPermission> ps)
    {
        var psGroup = ps.GroupBy(p => p.AppliedObjectId).ToList();
        var newPs = new List<BasalPermission>();
        foreach (var item in psGroup)
        {
            if (!item.Any()) continue;
            var allowMask = Permission.Undefined;
            var denyMask = Permission.Undefined;
            foreach (var p in item)
            {
                allowMask |= p.AllowMask;
                denyMask |= p.DenyMask;
            }

            var m = item.FirstOrDefault();
            m.AllowMask = allowMask;
            m.DenyMask = denyMask;
            newPs.Add(m);
        }

        return newPs;
    }

    public static List<AuthorizeDataRuleModel> ConvertAuthorizeDataRuleModels(List<UserRule> rulesList)
    {
        var authorizeDataRuleModels = new List<AuthorizeDataRuleModel>();
        foreach (var entityTypeItem in rulesList.GroupBy(x => x.EntityTypeName))
        {
            var entityType = new AuthorizeDataRuleModel(
                string.Empty,
                entityTypeItem.Key,
                string.Empty,
                0,
                []
            );

            foreach (var field in entityTypeItem)
            {
                var authorizeDataRuleFieldModel = new AuthorizeDataRuleFieldModel(
                    UserType.All,
                    field.FieldName,
                    field.FieldDesc,
                    field.FieldType,
                    field.FieldValue,
                    field.ExpressionType
                );
                entityType.AuthorizeDataRuleFieldModels.Add(authorizeDataRuleFieldModel);
            }

            authorizeDataRuleModels.Add(entityType);
        }

        return authorizeDataRuleModels;
    }
}