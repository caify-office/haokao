using Girvs.Infrastructure;
using HaoKao.Common.RemoteModel;
using HaoKao.ProductService.Domain.RemoteRepositories;
using HaoKao.ProductService.Infrastructure.RemoteService;
using Refit;

namespace HaoKao.ProductService.Infrastructure.RemoteRepositories
{
    public class RemotePaperRepository : IRemotePaperRepository
    {
        /// <summary>
        /// 获取试卷基本信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BrowsePaperInfo> GetPaperDetailInfo(Guid id)
        {
            var remot = EngineContext.Current.Resolve<IRemotePaperService>();
            dynamic reponse;
            try
            {
                reponse = await remot.GetPaperDetailInfo(id);
            }
            catch (ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return null;
            }
            var result = JsonConvert.DeserializeObject<BrowsePaperInfo>(reponse.GetRawText());
            return result;
        }
    }
}