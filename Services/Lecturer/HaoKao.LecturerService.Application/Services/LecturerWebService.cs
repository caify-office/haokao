using HaoKao.Common;
using HaoKao.LecturerService.Application.Interfaces;
using HaoKao.LecturerService.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace HaoKao.LecturerService.Application.Services;

/// <summary>
/// 讲师接口服务-Web端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[AllowAnonymous]
public class LecturerWebService(ILecturerService service) : ILecturerWebService
{
    private readonly ILecturerService _service = service ?? throw new ArgumentNullException(nameof(service));

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    public Task<BrowseLecturerViewModel> Get(Guid id)
    {
        return _service.Get(id);
    }

    /// <summary>
    /// 根据主键数组获取讲师列表
    /// </summary>
    /// <param name="ids">主键</param>
    [HttpPost]
    public Task<IReadOnlyList<BrowseLecturerViewModel>> GetByIds([FromBody] Guid[] ids)
    {
        return _service.GetByIds(ids);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public Task<LecturerQueryViewModel> Get([FromQuery] LecturerQueryViewModel queryViewModel)
    {
        return _service.Get(queryViewModel);
    }
}