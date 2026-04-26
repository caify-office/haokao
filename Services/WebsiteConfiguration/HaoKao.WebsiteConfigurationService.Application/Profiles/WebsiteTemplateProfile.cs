using AutoMapper;
using Girvs.AutoMapper;
using HaoKao.WebsiteConfigurationService.Domain.Commands.WebsiteTemplate;

namespace HaoKao.WebsiteConfigurationService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class WebsiteTemplateProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public WebsiteTemplateProfile()
    {
        CreateMap<CreateWebsiteTemplateCommand, WebsiteTemplate>();
        CreateMap<UpdateWebsiteTemplateCommand, WebsiteTemplate>();
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}