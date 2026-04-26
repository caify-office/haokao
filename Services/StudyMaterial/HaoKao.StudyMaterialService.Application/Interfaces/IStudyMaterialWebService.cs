using HaoKao.StudyMaterialService.Domain.Entities;

namespace HaoKao.StudyMaterialService.Application.Interfaces;

public interface IStudyMaterialWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据ids获取资料列表
    /// </summary>
    /// <param name="ids">ids</param>
    /// <returns></returns>
    Task<List<Material>> Get(List<Guid> ids);
}