using HaoKao.NoticeService.Domain.Models;
using HaoKao.NoticeService.Infrastructure.Mappings;

namespace HaoKao.NoticeService.Infrastructure;

[GirvsDbConfig("HaoKao_Notice")]
public class NoticeDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<Notice> Notices { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new NoticeEntityTypeConfiguration());
        base.OnModelCreating(builder);
    }
}