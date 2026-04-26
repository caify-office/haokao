using Girvs.AutoMapper;
using HaoKao.ProductService.Domain.Commands.Product;
using HaoKao.ProductService.Domain.Commands.ProductPermission;
using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class ProductProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public ProductProfile()
    {
        CreateMap<CreateProductCommand, Product>();
        CreateMap<UpdateProductCommand, Product>();
        CreateMap<ProductPermissionCommand, ProductPermission>();
        CreateMap<AssistantProductPermissionCommand, AssistantProductPermission>();
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}