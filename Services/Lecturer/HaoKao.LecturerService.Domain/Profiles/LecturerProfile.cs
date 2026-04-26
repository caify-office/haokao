using AutoMapper;
using Girvs.AutoMapper;
using HaoKao.LecturerService.Domain.Commands;
using HaoKao.LecturerService.Domain.Entities;

namespace HaoKao.LecturerService.Domain.Profiles;

/// <summary>
/// 文书模型映射文件
/// </summary>
public class LecturerProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public LecturerProfile()
    {
        CreateMap<CreateLecturerCommand, Lecturer>();
        CreateMap<UpdateLecturerCommand, Lecturer>();
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}