using HaoKao.LecturerService.Domain.Entities;

namespace HaoKao.LecturerService.Infrastructure;

[GirvsDbConfig("HaoKao_Lecturer")]
public class LecturerDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<Lecturer> Lecturers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new LecturerEntityTypeConfiguration());
    }
}