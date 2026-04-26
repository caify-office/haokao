using HaoKao.GroupBookingService.Domain.Entities;
using HaoKao.GroupBookingService.Infrastructure.EntityConfigurations;

namespace HaoKao.GroupBookingService.Infrastructure;

[GirvsDbConfig("HaoKao_GroupBooking")]
public class GroupBookingDbContext(DbContextOptions<GroupBookingDbContext> options) : GirvsDbContext(options)
{
    public DbSet<GroupData> GroupDatas { get; set; } 

    public DbSet<GroupSituation> GroupSituations { get; set; }

    public DbSet<GroupMember> GroupMembers { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new GroupDataEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new GroupSituationEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new GroupMemberEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}