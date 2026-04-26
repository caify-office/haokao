using HaoKao.PaperTempleteService.Domain.Entities;
using System.Collections.Generic;

namespace HaoKao.PaperTempleteService.Domain.Repositories;

public interface IPaperTempleteRepository : IRepository<PaperTemplete>
{
    Task<List<PaperTemplete>> GetPaperTempleteByQueryAsync(string subjectId);
}