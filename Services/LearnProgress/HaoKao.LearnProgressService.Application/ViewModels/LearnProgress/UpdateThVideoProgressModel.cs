

namespace HaoKao.LearnProgressService.Application.ViewModels.LearnProgress;

public  class UpdateThVideoProgressModel:IDto
{
    /// <summary>
    /// 视频id
    /// </summary>
    public Guid VideoId { get; set; }

    /// <summary>
    /// 时长
    /// </summary>
    public double Duration { get; set; }
}