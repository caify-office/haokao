using HaoKao.ProductService.Application.ViewModels.SupervisorStudent;
using HaoKao.ProductService.Domain.Queries;
using System.Text.Json.Serialization;

namespace HaoKao.ProductService.Application.ViewModels.SupervisorClass;


[AutoMapFrom(typeof(SupervisorClassQuery))]
[AutoMapTo(typeof(SupervisorClassQuery))]
public class SupervisorClassQueryViewModel : QueryDtoBase<SupervisorClassQueryListViewModel>
{
    /// <summary>
    /// 班级名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// 产品id
    /// </summary>
    public Guid? ProductId { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.SupervisorClass))]
[AutoMapTo(typeof(Domain.Entities.SupervisorClass))]
public class SupervisorClassQueryListViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 班级名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// 产品包id
    /// </summary>
    public Guid ProductPackageId { get; set; }

    /// <summary>
    /// 产品包名称
    /// </summary>
    public string ProductPackageName { get; set; }

    /// <summary>
    /// 产品id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// 营销人员Id
    /// </summary>
    public Guid SalespersonId { get; set; }

    /// <summary>
    /// 营销人员名称
    /// </summary>
    public string SalespersonName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonIgnore]
    public List<BrowseSupervisorStudentViewModel> SupervisorStudents { get; set; }


    public int SupervisorStudentCount
    {
        get
        {
            return SupervisorStudents.Count;
        }
    }

}