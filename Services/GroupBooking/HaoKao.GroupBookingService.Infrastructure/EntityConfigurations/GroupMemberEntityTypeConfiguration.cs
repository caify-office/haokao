using HaoKao.GroupBookingService.Domain.Entities;
using System;

namespace HaoKao.GroupBookingService.Infrastructure.EntityConfigurations;

public class GroupMemberEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<GroupMember>
{
    public override void Configure(EntityTypeBuilder<GroupMember> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<GroupMember, Guid>(builder);

        builder.Property(x => x.GroupSituationId)
               .IsRequired()
               .HasComment("拼团情况Id");

        builder.Property(x => x.GroupDataId)
               .IsRequired()
               .HasComment("拼团资料Id");

        builder.Property(x => x.CreatorId)
               .IsRequired()
               .HasComment("用户Id");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .HasComment("昵称");

        builder.Property(x => x.ImageUrl)
           .IsRequired()
           .HasColumnType("varchar")
           .HasMaxLength(200)
           .HasComment("用户头像Url");

        builder.Property(x => x.IsLeader)
            .HasColumnType("bit")
            .HasComment("是否团长");

        builder.HasOne(x => x.GroupSituation)
       .WithMany(x => x.GroupMembers)
       .HasForeignKey(x => x.GroupSituationId);

        builder.Property(x => x.SuccessTime).HasComment("组团成功时间");

        builder.Property(x => x.ExpirationTime).HasComment("组团过期时间");

        builder.HasIndex(x => new { x.CreatorId, x.GroupDataId, x.TenantId });
    }
}
