using AutoMapper;
using Girvs.AutoMapper;
using HaoKao.WebsiteConfigurationService.Domain.Commands.TemplateStyle;

namespace HaoKao.WebsiteConfigurationService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class TemplateStyleProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public TemplateStyleProfile()
    {
        CreateMap<CreateTemplateStyleCommand, TemplateStyle>();
        CreateMap<UpdateTemplateStyleCommand, TemplateStyle>();
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}