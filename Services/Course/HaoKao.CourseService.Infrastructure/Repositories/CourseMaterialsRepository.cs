using HaoKao.CourseService.Domain.CourseMaterialsModule;

namespace HaoKao.CourseService.Infrastructure.Repositories;

public class CourseMaterialsRepository(CourseDbContext context) : Repository<CourseMaterials>, ICourseMaterialsRepository
{
    public async Task<int> UpdateSort(Guid id, int sort, Guid compareId, int compareSort)
    {
        var result = await context.CourseMaterials.Where(x => x.Id == id)
                                  .ExecuteUpdateAsync(x => x.SetProperty(y => y.Sort, compareSort));

        result += await context.CourseMaterials.Where(x => x.Id == compareId)
                               .ExecuteUpdateAsync(x => x.SetProperty(y => y.Sort, sort));

        return result;
    }

    /// <summary>
    /// 判断课程下面是否存在讲义
    /// </summary>
    /// <param name="courseId"></param>
    /// <returns></returns>
    public Task<int> MaterialsCount(Guid courseId)
    {
        var query = from cv in context.CourseVideos
                    join cc in context.CourseChapters
                        on cv.CourseChapterId equals cc.Id into CourseChapter
                    from cc in CourseChapter.DefaultIfEmpty()
                    where cc.CourseId == courseId
                    select cv;
        return query.CountAsync();
    }

    /// <summary>
    /// 根据章节id查询章节下面的讲义集合
    /// </summary>
    /// <param name="chapterId"></param>
    /// <returns></returns>
    public async Task<List<CourseMaterials>> GetByQueryByChapterIdsAsync(Guid chapterId)
    {
        return await Queryable.Where(x => x.CourseChapterId == chapterId).ToListAsync();
    }

    public override async Task<List<CourseMaterials>> GetByQueryAsync(QueryBase<CourseMaterials> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).CountAsync();

        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result =
                await Queryable.Where(query.GetQueryWhere())
                               .SelectProperties(query.QueryFields)
                               .OrderBy(x => x.Sort)
                               .Skip(query.PageStart)
                               .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }
}