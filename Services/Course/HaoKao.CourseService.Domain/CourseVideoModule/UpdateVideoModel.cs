namespace HaoKao.CourseService.Domain.CourseVideoModule;

public class UpdateVideoModel
{
    public string videoname { get; set; }

    public string createtime { get; set; }

    public string coursename { get; set; }

    public Guid courseid { get; set; }
}