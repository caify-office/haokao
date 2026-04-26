using HaoKao.ErrorCorrectingService.Domain.Entities;
using System;

namespace HaoKao.ErrorCorrectingService.Infrastructure.EntityConfigurations;

public class ErrorCorrectingEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<ErrorCorrecting>
{
    public override void Configure(EntityTypeBuilder<ErrorCorrecting> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<ErrorCorrecting, Guid>(builder);

        builder.Property(x => x.Description)
               .HasColumnType("varchar")
               .HasMaxLength(2000)
               .HasComment("描述");

        builder.Property(x => x.QuestionTypes)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("问题类型");

        builder.HasIndex(x => new { x.SubjectId, x.CategoryId, x.QuestionTypeId });
    }
}