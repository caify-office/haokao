using Girvs.AutoMapper;
using HaoKao.OrderService.Domain.Commands.OrderSqInvoice;
using HaoKao.OrderService.Domain.Entities;

namespace HaoKao.OrderService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class OrderInvoiceProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public OrderInvoiceProfile()
    {
        CreateMap<CreateOrderInvoiceCommand, OrderInvoice>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}