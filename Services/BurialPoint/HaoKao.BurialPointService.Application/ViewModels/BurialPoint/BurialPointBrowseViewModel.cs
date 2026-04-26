using HaoKao.BurialPointService.Application.ViewModels.BrowseRecord;
using HaoKao.BurialPointService.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace HaoKao.BurialPointService.Application.ViewModels.BurialPoint;


[AutoMapFrom(typeof(Domain.Entities.BurialPoint))]
public class BrowseBurialPointViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 所属端
    /// </summary>
    public BelongingPortType BelongingPortType { get; set; }

    /// <summary>
    /// 浏览记录
    /// </summary>
    [JsonIgnore]
    public List<BrowseBrowseRecordViewModel> BrowseRecords { get; set; }
    /// <summary>
    /// 总浏览量
    /// </summary>
    public int TotalViews
    {
        get
        { 
         return BrowseRecords.Count;
        }
    }
    /// <summary>
    /// 总浏览人数
    /// </summary>
    public int TotalPeoples
    {
        get
        {
            return BrowseRecords.GroupBy(x=>x.CreatorId).Count();
        }
    }
    /// <summary>
    /// 今日浏览量
    /// </summary>
    public int TodayTotalViews
    {
        get
        {
            return BrowseRecords.Where(x => x.CreateTime.Date == DateTime.Now.Date).Count();
        }
    }
    /// <summary>
    /// 今日浏览人数
    /// </summary>
    public int TodayTotalPeoples
    {
        get
        {
            return BrowseRecords.Where(x => x.CreateTime.Date == DateTime.Now.Date).GroupBy(x => x.CreatorId).Count();
        }
    }
}
