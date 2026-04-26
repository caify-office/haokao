using HaoKao.ProductService.Application.ViewModels.Product;
using HaoKao.ProductService.Application.ViewModels.RelatedProduct;

namespace HaoKao.ProductService.Application.Services.Web;

public interface IRelatedProductWebService : IAppWebApiService, IManager
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
    /// 通过产品id数组获取关联了这些产品的产品信息
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<List<ProductListViewModel>> GetRelatedProductByIds([FromBody] Guid[] ids);



}