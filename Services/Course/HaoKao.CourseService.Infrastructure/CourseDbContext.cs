using HaoKao.CourseService.Domain.CourseChapterModule;
using HaoKao.CourseService.Domain.CourseMaterialsModule;
using HaoKao.CourseService.Domain.CourseModule;
using HaoKao.CourseService.Domain.CoursePracticeModule;
using HaoKao.CourseService.Domain.CourseVideoModule;
using HaoKao.CourseService.Domain.CourseVideoNoteModule;
using HaoKao.CourseService.Domain.VideoStorageModule;
using HaoKao.CourseService.Infrastructure.Mappings;

namespace HaoKao.CourseService.Infrastructure;

[GirvsDbConfig("HaoKao_Course")]
public class CourseDbContext(DbContextOptions<CourseDbContext> options) : GirvsDbContext(options)
{
    public DbSet<Course> Courses { get; set; }

    public DbSet<CourseVideo> CourseVideos { get; set; }

    public DbSet<CourseChapter> CourseChapters { get; set; }

    public DbSet<CourseMaterials> CourseMaterials { get; set; }

    public DbSet<CoursePractice> CoursePractice { get; set; }

    public DbSet<CourseVideoNote> CourseVideoNote { get; set; }

    public DbSet<VideoStorage> VideoStorages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CourseMaterialsEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CoursePracticeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CourseVideoNoteEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CourseVideoEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CourseEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CourseChapterEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new VideoStorageEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}