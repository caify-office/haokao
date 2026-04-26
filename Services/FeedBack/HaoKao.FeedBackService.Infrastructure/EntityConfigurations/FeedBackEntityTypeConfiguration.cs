using HaoKao.FeedBackService.Domain.Entities;
using System;

namespace HaoKao.FeedBackService.Infrastructure.EntityConfigurations;

public class FeedBackEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<FeedBack>
{
    public override void Configure(EntityTypeBuilder<FeedBack> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<FeedBack, Guid>(builder);

        builder.Property(x => x.Contract)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("联系人");

        builder.Property(x => x.Description)
               .HasColumnType("varchar")
               .HasMaxLength(2000)
               .HasComment("描述");

        builder.Property(x => x.FileUrls)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("图片");

        builder.Property(x => x.CreatorName).HasColumnType("varchar(40)");
    }
}