namespace HaoKao.CourseService.Domain.CourseMaterialsModule;

public interface ICourseMaterialsRepository : IRepository<CourseMaterials>
{
    /// <summary>
    /// 查询课程下面的讲义数量
    /// </summary>
    /// <param name="courseId">课程id</param>
    /// <returns></returns>
    Task<int> MaterialsCount(Guid courseId);

    /// <summary>
    /// 更改排序
    /// </summary>
    /// <param name="id"></param>
    /// <param name="sort"></param>
    /// <param name="compareId"></param>
    /// <param name="compareSort"></param>
    /// <returns></returns>
    Task<int> UpdateSort(Guid id, int sort, Guid compareId, int compareSort);

    /// <summary>
    /// 根据章节id查询章节下面的讲义集合
    /// </summary>
    /// <param name="chapterId"></param>
    /// <returns></returns>
    Task<List<CourseMaterials>> GetByQueryByChapterIdsAsync(Guid chapterId);
}