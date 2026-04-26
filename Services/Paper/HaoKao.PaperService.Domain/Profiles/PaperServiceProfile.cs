using HaoKao.PaperService.Domain.Commands;
using HaoKao.PaperService.Domain.Entities;

namespace HaoKao.PaperService.Domain.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class PaperServiceProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public PaperServiceProfile()
    {
        CreateMap<CreatePaperCommand, Paper>();
        CreateMap<UpdatePaperCommand, Paper>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}