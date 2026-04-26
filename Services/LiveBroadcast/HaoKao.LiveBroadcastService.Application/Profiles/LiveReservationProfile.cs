using AutoMapper;
using Girvs.AutoMapper;
using HaoKao.LiveBroadcastService.Domain.Commands.LiveReservation;
using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Application.Profiles;

public class LiveReservationProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public LiveReservationProfile()
    {
        CreateMap<CreateLiveReservationCommand, LiveReservation>();
    }

    public int Order => 201;
}