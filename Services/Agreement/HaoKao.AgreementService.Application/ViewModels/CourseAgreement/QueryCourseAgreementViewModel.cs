using HaoKao.AgreementService.Domain.Entities;
using HaoKao.AgreementService.Domain.Queries;

namespace HaoKao.AgreementService.Application.ViewModels.CourseAgreement;

[AutoMapFrom(typeof(CourseAgreementQuery))]
[AutoMapTo(typeof(CourseAgreementQuery))]
public class QueryCourseAgreementViewModel : QueryDtoBase<BrowseCourseAgreementViewModel>
{
    /// <summary>
    /// 协议名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 协议类型
    /// </summary>
    public AgreementType? AgreementType { get; set; }
}