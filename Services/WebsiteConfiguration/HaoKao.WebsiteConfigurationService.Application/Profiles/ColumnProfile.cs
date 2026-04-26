using AutoMapper;
using Girvs.AutoMapper;

namespace HaoKao.WebsiteConfigurationService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class ColumnProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public ColumnProfile()
    {
        CreateMap<CreateColumnCommand, Column>();
        CreateMap<UpdateColumnCommand, Column>();
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}