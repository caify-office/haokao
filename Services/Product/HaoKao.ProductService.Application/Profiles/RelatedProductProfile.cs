using Girvs.AutoMapper;
using HaoKao.ProductService.Domain.Commands.RelatedProduct;
using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Application.Profiles;

/// <summary>
/// 文书模型映射文件
/// </summary>
public class RelatedProductProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public RelatedProductProfile()
    {
        CreateMap<CreateRelatedProductModel, RelatedProduct>();
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}