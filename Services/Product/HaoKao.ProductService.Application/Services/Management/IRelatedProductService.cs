using HaoKao.ProductService.Application.ViewModels.RelatedProduct;

namespace HaoKao.ProductService.Application.Services.Management;

public interface IRelatedProductService : IAppWebApiService, IManager
{
     /// <summary>
     /// 根据主键获取指定
     /// </summary>
     /// <param name="id">主键</param>
     Task<BrowseRelatedProductViewModel> Get(Guid id);

     /// <summary>
     /// 根据查询获取列表，用于分页
     /// </summary>
     /// <param name="queryViewModel">查询对象</param>
     Task<RelatedProductQueryViewModel> Get(RelatedProductQueryViewModel queryViewModel);

     /// <summary>
     /// 创建关联产品
     /// </summary>
     /// <param name="model">新增模型</param>
     Task Create(IList<CreateRelatedProductViewModel> model);

     /// <summary>
     /// 根据主键删除指定关联产品
     /// </summary>
     /// <param name="ids">主键</param>
     Task Delete(Guid[] ids);
}