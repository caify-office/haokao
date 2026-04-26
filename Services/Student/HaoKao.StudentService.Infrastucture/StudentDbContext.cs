using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Infrastructure.Mappings;

namespace HaoKao.StudentService.Infrastructure;

[GirvsDbConfig("HaoKao_Student_2023")]
public class StudentDbContext(DbContextOptions<StudentDbContext> options) : GirvsDbContext(options)
{
    public DbSet<Student> Students { get; init; }

    public DbSet<StudentParameterConfig> StudentParameterConfigs { get; init; }

    public DbSet<StudentAllocation> StudentAllocations { get; init; }

    public DbSet<StudentAllocationConfig> StudentAllocationConfigs { get; init; }

    public DbSet<StudentFollow> StudentFollows { get; init; }

    public DbSet<UnionStudent> UnionStudents { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new StudentEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StudentParameterConfigEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StudentAllocationEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StudentAllocationConfigEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StudentFollowEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UnionStudentEntityTypeConfiguration());

        modelBuilder.Entity<Student>().HasOne(x => x.RegisterUser).WithOne();

        base.OnModelCreating(modelBuilder);
    }
}