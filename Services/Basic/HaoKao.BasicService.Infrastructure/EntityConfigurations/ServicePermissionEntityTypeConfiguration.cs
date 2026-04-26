using HaoKao.BasicService.Domain.Entities;

namespace HaoKao.BasicService.Infrastructure.EntityConfigurations;

public class ServicePermissionEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<ServicePermission>
{
    public override void Configure(EntityTypeBuilder<ServicePermission> builder)
    {
        var permissionConverter = new ValueConverter<Dictionary<string, string>, string>(
            v => JsonSerializerPermissionsString(v),
            v => JsonSerializerDeserializePermissions(v));


        var operationPermissionConverter = new ValueConverter<List<OperationPermissionModel>, string>(
            v => JsonSerializerOperationPermissionString(v),
            v => JsonSerializerDeserializeOperationPermission(v));


        var otherParamsConverter = new ValueConverter<string[], string>(
            v => JsonSerializerOtherParamsString(v),
            v => JsonSerializerDeserializeOtherParams(v));

        OnModelCreatingBaseEntityAndTableKey<ServicePermission, Guid>(builder);
        builder.Property(x => x.ServiceId).HasColumnType("varchar(36)");
        builder.Property(x => x.Permissions)
               .HasColumnType("text")
               .HasConversion(permissionConverter);
        builder.Property(x => x.OperationPermissions)
               .HasColumnType("text")
               .HasConversion(operationPermissionConverter);
        builder.Property(x => x.OtherParams)
               .HasColumnType("text")
               .HasConversion(otherParamsConverter);
        builder.Property(x => x.ServiceName).HasColumnType("varchar(255)");
        builder.Property(x => x.Tag).HasColumnType("varchar(50)");
        builder.Property(x => x.Order).HasColumnType("int");
    }

    private string JsonSerializerOtherParamsString(string[] v)
    {
        return JsonSerializer.Serialize(v);
    }

    private string[] JsonSerializerDeserializeOtherParams(string str)
    {
        return JsonSerializer.Deserialize<string[]>(str);
    }

    private string JsonSerializerPermissionsString(Dictionary<string, string> v)
    {
        return JsonSerializer.Serialize(v);
    }

    private Dictionary<string, string> JsonSerializerDeserializePermissions(string str)
    {
        return JsonSerializer.Deserialize<Dictionary<string, string>>(str);
    }


    private string JsonSerializerOperationPermissionString(List<OperationPermissionModel> v)
    {
        return JsonSerializer.Serialize(v);
    }

    private List<OperationPermissionModel> JsonSerializerDeserializeOperationPermission(string str)
    {
        return JsonSerializer.Deserialize<List<OperationPermissionModel>>(str);
    }
}