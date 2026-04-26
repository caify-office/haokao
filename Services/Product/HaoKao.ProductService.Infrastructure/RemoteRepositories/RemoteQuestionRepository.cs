using Girvs.Infrastructure;
using HaoKao.ProductService.Domain.RemoteRepositories;
using HaoKao.ProductService.Infrastructure.RemoteService;

namespace HaoKao.ProductService.Infrastructure.RemoteRepositories
{
    public class RemoteQuestionRepository : IRemoteQuestionRepository
    {
        /// <summary>
        /// 获取试卷基本信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> GetChaperCategorieQuestionCount(Guid chaperId, Guid questionCategoryId)
        {
            var remot = EngineContext.Current.Resolve<IRemoteQuestionService>();
            var count = await remot.GetChaperCategorieQuestionCount(chaperId, questionCategoryId);
            return count;
        }
    }
}