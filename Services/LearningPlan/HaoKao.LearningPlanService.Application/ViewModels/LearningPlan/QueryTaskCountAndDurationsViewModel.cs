using HaoKao.LearningPlanService.Domain.Records;

namespace HaoKao.LearningPlanService.Application.ViewModels.LearningPlan;

public class QueryTaskCountAndDurationsViewModel : IDto
{
    /// <summary>
    /// 产品Id
    /// </summary>
    [DisplayName("产品Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid ProductId { get; set; }

    /// <summary>
    /// 对应的科目Id
    /// </summary>
    [DisplayName("对应的科目Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 选择的知识点任务
    /// </summary>
    public virtual ICollection<KnowledgePointTask> KnowledgePointTasks { get; set; } = [];
}