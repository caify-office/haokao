namespace HaoKao.CourseService.Domain.CourseChapterModule;

public class CourseChapterQuery : QueryBase<CourseChapter>
{
    public override Expression<Func<CourseChapter, bool>> GetQueryWhere()
    {
        Expression<Func<CourseChapter, bool>> expression = x => true;
        return expression;
    }
}