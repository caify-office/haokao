using HaoKao.AnsweringQuestionService.Domain.Entities;
using System.Threading.Tasks;
using System;

namespace HaoKao.AnsweringQuestionService.Domain.Repositories;

public interface IAnsweringQuestionRepository : IRepository<AnsweringQuestion>
{
    Task<int> UpdateIsReply(Guid id);

    Task<int> GetCountByProductId(Guid productId, Guid userId);

    /// <summary>
    /// 查询当前答疑是否存在追问
    /// </summary>
    /// <param name="answerQuestionId"></param>
    /// <returns></returns>
    Task<AnsweringQuestion> GetChildQuestion(Guid answerQuestionId);

    Task<int> DeleteChildQuestion(Guid id);

    Task<int> DeleteQuestion(Guid id);
}