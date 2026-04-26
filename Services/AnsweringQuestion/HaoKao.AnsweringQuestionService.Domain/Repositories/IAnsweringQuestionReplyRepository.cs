using HaoKao.AnsweringQuestionService.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace HaoKao.AnsweringQuestionService.Domain.Repositories;

public interface IAnsweringQuestionReplyRepository : IRepository<AnsweringQuestionReply>
{
    Task<List<AnsweringQuestionReply>> GetAnsweringQuestionReplyList(Guid AnsweringQuestionId);

    Task<int> GetAnsweringQuestionReplyCount(Guid AnsweringQuestionId);
}
