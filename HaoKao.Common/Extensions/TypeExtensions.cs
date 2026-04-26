using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HaoKao.Common.Extensions;

public static class TypeExtensions
{
    public static dynamic GetTypeFields(this Type type)
    {
        var result = type
                     .GetProperties()
                     .Select(x =>
                     {
                         var title = x.GetCustomAttribute<DescriptionAttribute>();
                         var required = x.GetCustomAttribute<RequiredAttribute>();
                         var multipleField = x.GetCustomAttribute<MultipleFieldAttribute>();
                         return new
                         {
                             Title = title?.Description,
                             Key = x.Name,
                             Must = required != null,
                             MultipleField = multipleField != null,
                             Type = x.PropertyType.Name,
                         };
                     });
        return result;
    }

    public static dynamic GetEnumTypeKeyValue(this Type type)
    {
        var result = type.GetFields(BindingFlags.Public | BindingFlags.Static)
                         .Select(x =>
                         {
                             var title = x.GetCustomAttribute<DescriptionAttribute>();
                             return new
                             {
                                 Value = title?.Description,
                                 Key = x.GetValue(null),
                             };
                         });
        return result;
    }

    public static dynamic GetTypeFieldsSimple(this Type type)
    {
        var result = type
                     .GetProperties()
                     .Select(x =>
                     {
                         var title = x.GetCustomAttribute<DescriptionAttribute>();
                         return new
                         {
                             Title = title?.Description,
                             Key = x.Name,
                             Type = x.PropertyType.Name,
                         };
                     });
        return result;
    }
}