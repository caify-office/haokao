using HaoKao.AgreementService.Domain.Entities;
using HaoKao.AgreementService.Infrastructure.Mappings;

namespace HaoKao.AgreementService.Infrastructure;

[GirvsDbConfig("HaoKao_Agreement")]
public class AgreementRecordDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<CourseAgreement> CourseAgreements { get; set; }

    public DbSet<StudentAgreement> StudentAgreements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CourseAgreementEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StudentAgreementEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}