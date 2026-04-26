using Girvs.AutoMapper;
using HaoKao.ProductService.Domain.Commands.ProductPackage;
using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class PackageProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public PackageProfile()
    {
        CreateMap<CreateProductPackageCommand, ProductPackage>();
        CreateMap<UpdateProductPackageCommand, ProductPackage>();
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}