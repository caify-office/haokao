using Girvs.BusinessBasis;

namespace HaoKao.ProductService.Domain.RemoteRepositories
{
    public interface IRemotePaperRepository : IManager
    {
        /// <summary>
        /// 获取试卷基本信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BrowsePaperInfo> GetPaperDetailInfo(Guid id);
    }
}
