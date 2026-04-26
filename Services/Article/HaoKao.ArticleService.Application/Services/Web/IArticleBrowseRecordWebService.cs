using HaoKao.ArticleService.Application.ViewModels.ArticleBrowseRecord;

namespace HaoKao.ArticleService.Application.Services.Web;

public interface IArticleBrowseRecordWebService : IAppWebApiService, IManager
{
     /// <summary>
     /// 创建文章浏览记录
     /// </summary>
     /// <param name="model">新增模型</param>
     Task Create(CreateArticleBrowseRecordViewModel model);

}