using HaoKao.BasicService.Domain.Entities;

namespace HaoKao.BasicService.Infrastructure.EntityConfigurations;

public class ServiceDataRuleEntityTypeConfiguration: GirvsAbstractEntityTypeConfiguration<ServiceDataRule>
{
    public override void Configure(EntityTypeBuilder<ServiceDataRule> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<ServiceDataRule,Guid>(builder);
            
        builder.Property(x => x.EntityTypeName).HasColumnType("varchar(200)");
        builder.Property(x => x.EntityDesc).HasColumnType("varchar(200)");
        builder.Property(x => x.FieldDesc).HasColumnType("varchar(50)");
        builder.Property(x => x.FieldName).HasColumnType("varchar(100)");
        builder.Property(x => x.FieldType).HasColumnType("varchar(100)");
        builder.Property(x => x.FieldValue).HasColumnType("varchar(10)");
        builder.Property(x => x.Tag).HasColumnType("varchar(50)");
        builder.Property(x => x.Order).HasColumnType("int");
    }
}