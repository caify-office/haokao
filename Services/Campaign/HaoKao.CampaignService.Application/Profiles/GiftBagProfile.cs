using HaoKao.CampaignService.Domain.Commands;
using HaoKao.CampaignService.Domain.Entities;

namespace HaoKao.CampaignService.Application.Profiles;

public class GiftBagProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public GiftBagProfile()
    {
        CreateMap<CreateGiftBagCommand, GiftBag>();
        CreateMap<UpdateGiftBagCommand, GiftBag>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}