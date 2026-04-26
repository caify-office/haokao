namespace HaoKao.CourseService.Domain.CourseVideoNoteModule;

public class CourseVideoNoteQuery : QueryBase<CourseVideoNote>
{
    /// <summary>
    /// 视频id
    /// </summary>
    [QueryCacheKey]
    public string VideoId { get; set; }

    public override Expression<Func<CourseVideoNote, bool>> GetQueryWhere()
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();

        Expression<Func<CourseVideoNote, bool>> expression = x => x.CreatorId == userId;

        if (!string.IsNullOrEmpty(VideoId))
        {
            expression = expression.And(x => x.VideoId == VideoId);
        }

        return expression;
    }
}