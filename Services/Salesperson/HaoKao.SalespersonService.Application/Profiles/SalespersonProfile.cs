using HaoKao.SalespersonService.Domain.Commands;
using HaoKao.SalespersonService.Domain.Entities;

namespace HaoKao.SalespersonService.Application.Profiles;

public class SalespersonProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public SalespersonProfile()
    {
        CreateMap<CreateSalespersonCommand, Salesperson>();
        CreateMap<UpdateSalespersonCommand, Salesperson>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}