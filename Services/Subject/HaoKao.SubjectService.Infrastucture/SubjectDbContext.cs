using HaoKao.SubjectService.Domain.SubjectModule;
using HaoKao.SubjectService.Infrastructure.Mappings;

namespace HaoKao.SubjectService.Infrastructure;

[GirvsDbConfig("HaoKao_Subject")]
public class SubjectDbContext(DbContextOptions<SubjectDbContext> options) : GirvsDbContext(options)
{
    public DbSet<Subject> Subjects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SubjectEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}