using AutoMapper;
using Girvs.AutoMapper;
using HaoKao.GroupBookingService.Domain.Commands.GroupData;
using HaoKao.GroupBookingService.Domain.Entities;

namespace HaoKao.GroupBookingService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class GroupDataProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public GroupDataProfile()
    {
        CreateMap<CreateGroupDataCommand, GroupData>();
        CreateMap<UpdateGroupDataCommand, GroupData>();
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}